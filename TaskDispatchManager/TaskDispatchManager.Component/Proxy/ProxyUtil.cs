using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using TaskDispatchManager.Common;
using TaskDispatchManager.ServiceModel;
using TaskDispatchManager.DBModels.Base;
namespace TaskDispatchManager.Component.Proxy
{
    public class ProxyUtil
    {
        public static HttpHelper HttpHelper = new HttpHelper();

        /// <summary>
        /// CPU数量
        /// </summary>
        private static readonly int CpuCount = 10;//Convert.ToInt32(Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS"));
        /// <summary>
        /// 获取总页数 // 从ip代理站点(http://www.xicidaili.com/nn/6)获取
        /// </summary>
        /// <returns>总页数</returns>
        public static int GetTotalPage(string ipurl, string proxyIp)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(GetHtml(ipurl, proxyIp));
            var res = doc.DocumentNode.SelectNodes(@"//div[@class='pagination']/a");
            if (res != null && res.Count > 2)
            {
                int page;
                if (int.TryParse(res[res.Count - 2].InnerText, out page))
                {
                    return page;
                }
            }
            return 1;
        }


        /// <summary>
        /// 获取页面html内容
        /// </summary>
        /// <param name="url"></param>
        /// <param name="proxyIp"></param>
        /// <returns></returns>
        public static string GetHtml(string url, string proxyIp)
        {
            try
            {
                //创建Httphelper参数对象
                HttpItem item = new HttpItem()
                {
                    URL = url,//URL     必需项    
                    Method = "get",//可选项 默认为Get   
                    ContentType = "text/html",//返回类型    可选项有默认值 
                    Timeout = 2000,
                    ReadWriteTimeout = 2500,
                    ProxyIp = proxyIp
                };
                //请求的返回值对象
                HttpHelper helper = new HttpHelper();
                HttpResult result = helper.GetHtml(item);
                return result.Html;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog($"url:{url},ip:{proxyIp}获取HTML内容出错", ex);
                return "<HTML></HTML>";
            }
        }

        /// <summary>
        /// 开始解析网站数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<DBModels.Base.Proxy> ParseProxy(ProxyParam param)
        {
            if (string.IsNullOrEmpty(param.IpUrl))
            {
                throw new ArgumentNullException("ParseProxy函数参数空异常");
            }

            //总页数
            int total = GetTotalPage(param.IpUrl, param.ProxyIp);
            if (total==1)
            {
                throw new ArgumentNullException("总页数信息异常");
            }

            //返回结果
            List<DBModels.Base.Proxy> list = new List<DBModels.Base.Proxy>();

            //多线程进行解析获取
            List<Thread> listThread = new List<Thread>();

            //每个线程需要解析的页面数量
            var threadPageCount = (total/CpuCount);
            int threadPqgeSize = threadPageCount == 0? 1 : threadPageCount;

            //为每个线程准备参数
            List<Hashtable> threadParams = new List<Hashtable>();
            int start=0, end = 0;
            Hashtable table =null;

            //平均分配到每个线程
            for (int i = 0; i < CpuCount; i++)
            {
                start = i * threadPqgeSize + 1;
                if (start == total || total <= CpuCount)
                {
                    i = CpuCount;
                    end = total;
                }
                else
                {
                    end = start + threadPqgeSize -1;
                    if (i==CpuCount-1 && end<total)//如果还有余数就都分配在最后的线程
                    {
                        end = total;
                    }
                }
                table = new Hashtable();
                table.Add("start", start);
                table.Add("end", end);
                table.Add("list", list);
                table.Add("param", param);
                threadParams.Add(table);

                Thread thread = new Thread(DoWork)
                {
                    IsBackground = true,
                    Name = "PageParse #" + i.ToString()
                };

                LogHelper.WriteInfoLog($"线程{thread.Name}已开启，Start：{start},End:{end}");
                listThread.Add(thread);
                thread.Start(threadParams[i]);
            }

            //for (int i = 0; i < CpuCount; i++)
            //{
            //    Thread thread = new Thread(DoWork)
            //    {
            //        IsBackground = true,
            //        Name = "PageParse #" + i.ToString()
            //    };
            //    listThread.Add(thread);
            //    thread.Start(threadParams[i]);
            //}

            // 为当前线程指派生成任务。
           // DoWork(threadParams[0]);

            // 等待所有的编译线程执行线束。
            foreach (Thread thread in listThread)
            {
                thread.Join();
            }
            if (list.Count == 0)
            {
                LogHelper.WriteInfoLog("爬虫-代理ip任务,没有获取到数据,可能当前ip(" + param.ProxyIp + ")已被服务器封锁");
            }
            return list;
        }

        /// <summary>
        /// 解析每一页数据
        /// </summary>
        /// <param name="param"></param>
        private static void DoWork(object param)
        {
            //参数还原
            Hashtable table = param as Hashtable;
            if (table == null) throw new ArgumentNullException(nameof(table));
            int start = Convert.ToInt32(table["start"]);
            int end = Convert.ToInt32(table["end"]);
            List<DBModels.Base.Proxy> list = table["list"] as List<DBModels.Base.Proxy>;
            if (list == null) throw new ArgumentNullException(nameof(list));
            ProxyParam proxyParam = table["param"] as ProxyParam;
            if (proxyParam == null) throw new ArgumentNullException(nameof(proxyParam));

            //页面地址
            string url = string.Empty;
            string ip = string.Empty;
            DBModels.Base.Proxy item = null;
            HtmlNodeCollection nodes = null;
            HtmlNode node = null;
            HtmlAttribute atr = null;
            for (int i = start; i <= end; i++)
            {
                LogHelper.WriteAsyncLog($"开始解析,页码{start}~{end},当前页码{i}");
                url = $"{proxyParam.IpUrl}/{i}";
                var doc = new HtmlDocument();
                //如果代理失效会带来问题
                doc.LoadHtml(GetHtml(url, proxyParam.ProxyIp));
                //获取所有数据节点tr 解析每一个ip
                int count = 0;
                var trs = doc.DocumentNode.SelectNodes(@"//table[@id='ip_list']/tr");
                if (trs != null && trs.Count > 1)
                {
                    LogHelper.WriteAsyncLog($"当前页码{i},请求地址{url},共{trs.Count}条数据");
                    for (int j = 1; j < trs.Count; j++)
                    {
                        nodes = trs[j].SelectNodes("td");
                        if (nodes != null && nodes.Count > 9)
                        {
                            ip = nodes[2].InnerText.Trim();
                            var port = nodes[3].InnerText.Trim();
                            //LogHelper.WriteAsyncLog($"开始验证IP：{ip}:{port}");
                            if (proxyParam.IsPingIp && !WebUtils.PingProxy(ip,port))//!WebUtils.Ping(ip) // GetTotalPage(proxyParam.IpUrl, $"{ip}:{port}") <=1
                            {
                                //LogHelper.WriteInfoLog($"验证IP不通过：{ip}:{port}");
                                continue;
                            }
                            count++;
                            LogHelper.WriteAsyncLog($"验证IP成功：{ip}:{port}");
                            //有效的IP才添加
                            item = new DBModels.Base.Proxy();

                            node = nodes[1].FirstChild;
                            if (node != null)
                            {
                                atr = node.Attributes["alt"];
                                if (atr != null)
                                {
                                    item.Country = atr.Value.Trim();
                                }
                            }

                            item.IP = ip;
                            item.Port = port;
                            item.ProxyIp = $"{item.IP}:{item.Port}";
                            item.Position = nodes[4].InnerText.Trim();
                            item.Anonymity = nodes[5].InnerText.Trim();
                            item.Type = nodes[6].InnerText.Trim();
                            item.Guid=Guid.NewGuid();
                            item.CreatedOn=DateTime.Now;
                            node = nodes[7].SelectSingleNode("div[@class='bar']");
                            if (node != null)
                            {
                                atr = node.Attributes["title"];
                                if (atr != null)
                                {
                                    item.Speed = atr.Value.Trim();
                                }
                            }

                            node = nodes[8].SelectSingleNode("div[@class='bar']");
                            if (node != null)
                            {
                                atr = node.Attributes["title"];
                                if (atr != null)
                                {
                                    item.ConnectTime = atr.Value.Trim();
                                }
                            }
                            item.VerifyTime = nodes[9].InnerText.Trim();
                            list.Add(item);
                        }
                    }
                    LogHelper.WriteAsyncLog($"当前页码{i},共{trs.Count}条数据，验证成功{count}条。");
                }
                LogHelper.WriteAsyncLog($"结束解析,页码{start}~{end},当前页码{i}");
            }
        }

    }
}
