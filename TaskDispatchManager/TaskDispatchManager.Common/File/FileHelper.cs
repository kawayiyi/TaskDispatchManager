using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TaskDispatchManager.Common
{
    /// <summary>
    /// 文件相关助手类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 获取文件的绝对路径,针对window程序和web程序都可使用
        /// </summary>
        /// <param name="relativePath">相对路径地址</param>
        /// <returns>绝对路径地址</returns>
        public static string GetAbsolutePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException("参数relativePath空异常！");
            }
            relativePath = relativePath.Replace("/", "\\");
            if (relativePath[0] == '\\')
            {
                relativePath = relativePath.Remove(0, 1);
            }
            //判断是Web程序还是window程序
            if (HttpContext.Current != null)
            {
                return Path.Combine(HttpRuntime.AppDomainAppPath, relativePath);
            }
            else
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            }
        }


        /// <summary>
        /// 建立目录（如果目录存在则忽略，不存在则创建该目录，可以直接指定子目录而不必关心父目录，如果父目录不存在则自动创建父目录。）
        /// </summary>
        /// <param name="path">路径（包括要创建的目录名）</param>
        /// <returns></returns>
        public static bool CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 给定一个目录，根据文件的创建时间，递归删除该目录下（包括子目录）的所有文件，但不删除该目录（包括子目录）
        /// </summary>
        /// <param name="dInfo">目录</param>
        /// <param name="start">要删除的文件时间段：开始时间（创建时间）</param>
        /// <param name="end">要删除的文件时间段：结束时间（创建时间）</param>
        /// <remarks>冯瑞 2012-04-21</remarks>
        public static void RecursionDelFile(DirectoryInfo dInfo, DateTime start, DateTime end)
        {
            try
            {
                if (dInfo != null && dInfo.Exists)
                {
                    FileInfo[] fInfo = dInfo.GetFiles();
                    if (fInfo.Length > 0)
                    {
                        foreach (FileInfo info in fInfo)
                        {
                            if (info.CreationTime > start && info.CreationTime < end)
                            {
                                info.Delete();
                            }
                        }
                    }

                    DirectoryInfo[] zDInfo = dInfo.GetDirectories();
                    if (zDInfo.Length > 0)
                    {
                        foreach (DirectoryInfo zInfo in zDInfo)
                        {
                            RecursionDelFile(zInfo, start, end);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }
        public static string GetMD5HashFromFile(FileInfo fileInfo)
        {
            try
            {
                if (!fileInfo.Exists)
                {
                    throw new Exception(string.Format("{0} is not exists!", fileInfo.FullName));
                }

                using (FileStream file = new FileStream(fileInfo.FullName, FileMode.Open))
                {
                    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    byte[] retVal = md5.ComputeHash(file);
                    file.Close();

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Method of GetMD5HashFromFile(FileInfo fileInfo) fail , error:" + ex.Message);
            }
        }


        #region 文件加密解密

        /// <summary>
        /// 计算文件MD5值
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string pathName)
        {
            string strResult = "";
            string strHashData = "";
            byte[] arrbytHashValue;

            System.IO.FileStream oFileStream = null;

            System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();

            oFileStream = new System.IO.FileStream(pathName.Replace("\"", ""), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);

            arrbytHashValue = oMD5Hasher.ComputeHash(oFileStream); //计算指定Stream 对象的哈希值

            oFileStream.Close();

            //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”

            strHashData = System.BitConverter.ToString(arrbytHashValue);

            //替换-
            strHashData = strHashData.Replace("-", "");
            strResult = strHashData;

            return strResult;
        }

        /// <summary>
        /// 计算文件SHA1值
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        public static string GetSHA1Hash(string pathName)
        {
            string strResult = "";
            string strHashData = "";
            byte[] arrbytHashValue;

            System.IO.FileStream oFileStream = null;

            System.Security.Cryptography.SHA1CryptoServiceProvider osha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();

            oFileStream = new System.IO.FileStream(pathName.Replace("\"", ""), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);

            arrbytHashValue = osha1.ComputeHash(oFileStream); //计算指定Stream 对象的哈希值

            oFileStream.Close();

            //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”

            strHashData = System.BitConverter.ToString(arrbytHashValue);

            //替换-
            strHashData = strHashData.Replace("-", "");
            strResult = strHashData;


            return strResult;
        }

        #endregion

        #region 文件操作

        /// <summary>
        /// 在磁盘上创建文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool CreateFile(string filePath, string content)
        {
            bool result = false;
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    try
                    {
                        sw.Write(content);
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                    finally
                    {
                        sw.Close();
                        sw.Dispose();

                        fs.Close();
                        fs.Dispose();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 得到文件信息Info
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static FileInfo GetFileInfo(string fullPath)
        {
            FileInfo fileInfo = null;
            if (File.Exists(fullPath))
            {
                fileInfo = new FileInfo(fullPath);
            }
            return fileInfo;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="outFileName">外部文件</param>
        /// <param name="outPath">外部文件目录</param>
        /// <param name="contentEncoding">编码格式</param>
        /// <param name="isCover">是否覆盖（true:覆盖，false:直接追加内容）</param>
        /// <returns></returns>
        /// <remarks>冯瑞 2011-12-13 15:21:27</remarks>
        public static bool WriteFile(string content, string outFileName, string outPath, Encoding contentEncoding, bool isCover)
        {
            StreamWriter Writer = null;

            DirectoryInfo Info;

            string AllPath = outFileName;

            string DirectoryName = Path.GetDirectoryName(AllPath);

            bool Result = false;

            if (outPath != string.Empty)
            {
                Info = Directory.CreateDirectory(outPath);
                AllPath = System.IO.Path.Combine(outPath, AllPath);
            }
            if (DirectoryName != string.Empty)
            {
                Info = Directory.CreateDirectory(DirectoryName);
            }

            try
            {
                try
                {
                    if (content.Length > 0)
                    {
                        if (File.Exists(AllPath))
                        {
                            File.SetAttributes(AllPath, FileAttributes.Normal);
                        }

                        if (isCover) //覆盖原内容
                        {
                            Writer = new StreamWriter(AllPath, false, contentEncoding);
                        }
                        else //直接追加内容
                        {
                            Writer = File.AppendText(AllPath);
                        }

                        Writer.Write(content);
                        Writer.Flush();
                        Result = true;
                    }
                }
                catch
                {
                    Result = false;
                }
            }
            finally
            {
                if (Writer != null)
                {
                    Writer.Close();
                }
            }
            return Result;
        }

        /// <summary>
        /// 读取文本文件（一次性读取所有内容）
        /// </summary>
        /// <param name="outPath">外部文件目录</param>
        /// <param name="outFileName">外部文件名（包括扩展名）</param>
        /// <returns></returns>
        /// <remarks>Ralf 2012-02-29</remarks>
        public static StringBuilder ReadFileAllText(string outPath, string outFileName)
        {
            //完整路径
            string allPath = System.IO.Path.Combine(outPath, outFileName);

            StringBuilder sb = new StringBuilder();

            //一次性读取所有内容
            sb = new StringBuilder(System.IO.File.ReadAllText(allPath));
            return sb;
        }

        /// <summary>
        /// 读取文本文件（一次性读取所有内容）
        /// </summary>
        /// <param name="outPath">外部文件目录</param>
        /// <param name="outFileName">外部文件名（包括扩展名）</param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        /// <remarks>Ralf 2012-02-29</remarks>
        public static StringBuilder ReadFileAllText(string outPath, string outFileName, Encoding encoding)
        {
            //完整路径
            string allPath = System.IO.Path.Combine(outPath, outFileName);

            StringBuilder sb = new StringBuilder();

            //一次性读取所有内容
            sb = new StringBuilder(System.IO.File.ReadAllText(allPath, encoding));
            return sb;
        }

        /// <summary>
        /// 读取文本文件（逐行读取）
        /// </summary>
        /// <param name="outPath">外部文件目录</param>
        /// <param name="outFileName">外部文件名（包括扩展名）</param>
        /// <returns></returns>
        /// <remarks>Ralf 2012-02-29</remarks>
        public static StringBuilder ReadFileByLines(string outPath, string outFileName)
        {
            //完整路径
            string allPath = System.IO.Path.Combine(outPath, outFileName);

            StringBuilder sb = new StringBuilder();

            string[] lines = System.IO.File.ReadAllLines(allPath);
            //逐行读取
            foreach (string line in lines)
            {
                sb.Append(line + "\r\n");
            }
            return sb;
        }

        /// <summary>
        /// 读取文本文件（逐行读取）
        /// </summary>
        /// <param name="outPath">外部文件目录</param>
        /// <param name="outFileName">外部文件名（包括扩展名）</param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        /// <remarks>Ralf 2012-02-29</remarks>
        public static StringBuilder ReadFileByLines(string outPath, string outFileName, Encoding encoding)
        {
            //完整路径
            string allPath = System.IO.Path.Combine(outPath, outFileName);

            StringBuilder sb = new StringBuilder();

            string[] lines = System.IO.File.ReadAllLines(allPath, encoding);
            //逐行读取
            foreach (string line in lines)
            {
                sb.Append(line + "\r\n");
            }
            return sb;
        }

        /// <summary>
        /// 读取文本文件（匹配关键字）[未经过测试]
        /// </summary>
        /// <param name="outPath">外部文件目录</param>
        /// <param name="outFileName">外部文件名（包括扩展名）</param>
        /// <param name="match">匹配关键字列表</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>返回文件内容中匹配关键字的行</returns>
        /// <remarks>Ralf 2012-02-29</remarks>
        public static ArrayList ReadFileByLinesForMatch(string outPath, string outFileName, string[] match, Encoding encoding)
        {
            //完整路径
            string allPath = System.IO.Path.Combine(outPath, outFileName);

            //返回值容器
            ArrayList sList = new ArrayList();

            //逐行读取
            foreach (string sLine in System.IO.File.ReadLines(allPath, encoding))
            {
                //逐行匹配
                for (int i = 0; i < match.Length; i++)
                {
                    if (sLine.IndexOf(match[i]) != -1) //匹配
                    {
                        sList.Add(sLine);
                    }
                }
            }
            return sList;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fromName"></param>
        /// <param name="toName"></param>
        public static void DownloadFile(string fromName, string toName)
        {
            DownloadFile(fromName, toName, false);
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fromName"></param>
        /// <param name="toName"></param>
        /// <param name="delete"></param>
        public static void DownloadFile(string fromName, string toName, bool delete)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(fromName);
            if (!file.Exists)
            {
                return;
            }

            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Buffer = false;
            System.Web.HttpContext.Current.Response.AddHeader("Connection", "Keep-Alive");
            System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(toName, System.Text.Encoding.UTF8));//避免中文出现乱码现象   

            System.IO.FileStream r = new System.IO.FileStream(fromName, System.IO.FileMode.Open);
            System.Web.HttpContext.Current.Response.AddHeader("Content-Length", r.Length.ToString());

            while (true)
            {
                byte[] buffer = new byte[1024];
                int leng = r.Read(buffer, 0, 1024);
                if (leng == 0)
                {
                    break;
                }
                if (leng == 1024)
                {
                    System.Web.HttpContext.Current.Response.BinaryWrite(buffer);
                }
                else
                {
                    byte[] b = new byte[leng];
                    for (int i = 0; i < leng; i++)
                    {
                        b[i] = buffer[i];
                    }
                    System.Web.HttpContext.Current.Response.BinaryWrite(b);
                }
            }
            r.Close();

            if (delete)
            {
                file.Delete();
            }

            System.Web.HttpContext.Current.Response.Filter.Close();
            System.Web.HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 返回文件是否存在
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }

        /// <summary>
        /// 以指定的ContentType输出指定文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">输出的文件名</param>
        /// <param name="fileType">将文件输出时设置的ContentType</param>
        public static void ResponseFile(string filePath, string fileName, string fileType)
        {
            Stream iStream = null;

            // 缓冲区为10k
            byte[] buffer = new Byte[10000];
            // 文件长度
            int length;
            // 需要读的数据长度
            long dataToRead;

            try
            {
                // 打开文件
                iStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                // 需要读的数据长度
                dataToRead = iStream.Length;

                HttpContext.Current.Response.ContentType = fileType;
                if (HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].IndexOf("MSIE") > -1)
                {
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + WebUtils.UrlEncode(fileName.Trim()).Replace("+", " "));
                }
                else
                {
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName.Trim());
                }

                while (dataToRead > 0)
                {
                    // 检查客户端是否还处于连接状态
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        length = iStream.Read(buffer, 0, 10000);
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        // 如果不再连接则跳出死循环
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    // 关闭文件
                    iStream.Close();
                }
            }
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 判断文件名是否为浏览器可以直接显示的图片文件名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>是否可以直接显示</returns>
        public static bool IsImgFilename(string fileName)
        {
            fileName = fileName.Trim();
            if (fileName.EndsWith(".") || fileName.IndexOf(".") == -1)
            {
                return false;
            }

            string extname = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();
            return (extname == "jpg" || extname == "jpeg" || extname == "png" || extname == "bmp" || extname == "gif");
        }

        /// <summary>
        /// 判断文件流是否为UTF8字符集
        /// </summary>
        /// <param name="sbInputStream">文件流</param>
        /// <returns>判断结果</returns>
        private static bool IsUTF8(FileStream sbInputStream)
        {
            int i;
            byte cOctets;  // octets to go in this UTF-8 encoded character 
            byte chr;
            bool bAllAscii = true;
            long iLen = sbInputStream.Length;

            cOctets = 0;
            for (i = 0; i < iLen; i++)
            {
                chr = (byte)sbInputStream.ReadByte();

                if ((chr & 0x80) != 0) bAllAscii = false;

                if (cOctets == 0)
                {
                    if (chr >= 0x80)
                    {
                        do
                        {
                            chr <<= 1;
                            cOctets++;
                        }
                        while ((chr & 0x80) != 0);

                        cOctets--;
                        if (cOctets == 0)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if ((chr & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    cOctets--;
                }
            }

            if (cOctets > 0)
            {
                return false;
            }

            if (bAllAscii)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 返回指定目录下的非 UTF8 字符集文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>文件名的字符串数组</returns>
        public static string[] FindNoUTF8File(string path)
        {
            StringBuilder filelist = new StringBuilder();
            DirectoryInfo Folder = new DirectoryInfo(path);
            FileInfo[] subFiles = Folder.GetFiles();

            for (int j = 0; j < subFiles.Length; j++)
            {
                if (subFiles[j].Extension.ToLower().Equals(".htm"))
                {
                    FileStream fs = new FileStream(subFiles[j].FullName, FileMode.Open, FileAccess.Read);
                    bool bUtf8 = FileHelper.IsUTF8(fs);
                    fs.Close();
                    if (!bUtf8)
                    {
                        filelist.Append(subFiles[j].FullName);
                        filelist.Append("\r\n");
                    }
                }
            }
            return StringUtils.SplitString(filelist.ToString(), "\r\n");
        }

        /// <summary>
        /// 格式化字节数字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytesStr(int bytes)
        {
            if (bytes > 1073741824)
            {
                return ((double)(bytes / 1073741824)).ToString("0") + "G";
            }

            if (bytes > 1048576)
            {
                return ((double)(bytes / 1048576)).ToString("0") + "M";
            }

            if (bytes > 1024)
            {
                return ((double)(bytes / 1024)).ToString("0") + "K";
            }

            return bytes.ToString() + "Bytes";
        }

        /// <summary>
        /// 备份文件
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <param name="overWrite">当目标文件存在时是否覆盖</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName, bool overWrite)
        {
            if (!System.IO.File.Exists(sourceFileName))
            {
                throw new FileNotFoundException(sourceFileName + "文件不存在！");
            }

            if (!overWrite && System.IO.File.Exists(destFileName))
            {
                return false;
            }

            System.IO.File.Copy(sourceFileName, destFileName, true);
            return true;
        }

        /// <summary>
        /// 备份文件,当目标文件存在时覆盖
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName)
        {
            return BackupFile(sourceFileName, destFileName, true);
        }

        /// <summary>
        /// 恢复文件
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <param name="targetFileName">要恢复的文件名</param>
        /// <param name="backupTargetFileName">要恢复文件再次备份的名称,如果为null,则不再备份恢复文件</param>
        /// <returns>操作是否成功</returns>
        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            if (!System.IO.File.Exists(backupFileName))
            {
                throw new FileNotFoundException(backupFileName + "文件不存在！");
            }

            if (backupTargetFileName != null)
            {
                if (!System.IO.File.Exists(targetFileName))
                {
                    throw new FileNotFoundException(targetFileName + "文件不存在！无法备份此文件！");
                }
                else
                {
                    System.IO.File.Copy(targetFileName, backupTargetFileName, true);
                }
            }
            System.IO.File.Delete(targetFileName);
            System.IO.File.Copy(backupFileName, targetFileName);

            return true;
        }

        public static bool RestoreFile(string backupFileName, string targetFileName)
        {
            return RestoreFile(backupFileName, targetFileName, null);
        }

        /// <summary>
        /// 获取指定文件的扩展名（比土办法更好的办法）
        /// </summary>
        /// <param name="fileName">指定文件名</param>
        /// <returns>扩展名</returns>
        public static string GetFileExtName(string fileName)
        {
            string fileExtName = string.Empty;
            fileExtName = Path.GetExtension(fileName.TrimStart('\"').TrimEnd('\"')).ToLower();
            return fileExtName;
        }

        /// <summary>
        /// 获取指定文件的扩展名 (土办法)
        /// </summary>
        /// <param name="fileName">指定文件名</param>
        /// <returns>扩展名</returns>
        public static string GetFileExtName2(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || fileName.IndexOf('.') <= 0)
            {
                return string.Empty;
            }

            fileName = fileName.ToLower().Trim();

            return fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.'));
        }

        /// <summary>
        /// 返回指定路径字符串的文件名和扩展名。
        /// </summary>
        /// <param name="path">文件全路径</param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            string fileName = string.Empty;
            fileName = Path.GetFileName(path.TrimStart('\"').TrimEnd('\"'));
            return fileName;
        }


        /// <summary>
        /// 转换长文件名为短文件名
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="repString"></param>
        /// <param name="leftNum"></param>
        /// <param name="rightNum"></param>
        /// <param name="charNum"></param>
        /// <returns></returns>
        public static string ConvertSimpleFileName(string fullName, string repString, int leftNum, int rightNum, int charNum)
        {
            string simplefilename = string.Empty, leftstring = string.Empty, rightstring = string.Empty, filename = string.Empty;
            string extname = FileHelper.GetFileExtName(fullName);

            if (string.IsNullOrEmpty(extname))
            {
                return fullName;
            }

            int filelength = 0, dotindex = 0;

            dotindex = fullName.LastIndexOf('.');
            filename = fullName.Substring(0, dotindex);
            filelength = filename.Length;
            if (dotindex > charNum)
            {
                leftstring = filename.Substring(0, leftNum);
                rightstring = filename.Substring(filelength - rightNum, rightNum);
                if (string.IsNullOrEmpty(repString))
                {
                    simplefilename = leftstring + rightstring + "." + extname;
                }
                else
                {
                    simplefilename = leftstring + repString + rightstring + "." + extname;
                }
            }
            else
            {
                simplefilename = fullName;
            }

            return simplefilename;
        }

        /// <summary>
        /// 删除文件到回收站
        /// </summary>
        /// <param name="fullName">文件名（路径+文件名）</param>
        private void DelFile(string fullName)
        {
            //为何不始用File.Delete()，是因为该方法不经过回收站，直接删除文件
            //要删除至回收站，可使用VisualBasic删除文件，需引用Microsoft.VisualBasic
            //删除确认对话框是根据电脑系统-回收站-显示删除确认对话框   是否打勾 自动添加的
            //为何不使用c#的File.Delete()方法？？？因为该方法是直接删除，而不是放入回收站
            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(fullName,
                        Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
                        Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin,
                        Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing);
        }

        /// <summary>
        /// 删除指定目录下，指定扩展名，指定时间段的文件
        /// </summary>
        /// <param name="directory">目录（e.g : @"C:\Users\ChinaBest\Desktop\新建文件夹\"）</param>
        /// <param name="extension">扩展名列表（e.g. : string[] e = { ".jpg",".jpeg",".png", ".gif" }; ）</param>
        /// <param name="fileCreateDateStart">要删除的文件创建日期之开始日期</param>
        /// <param name="fileCreateDateEnd">要删除的文件创建日期之结束日期</param>
        /// <param name="isContainsSubDirectories">是否包含子目录</param>
        /// <remarks>Ralf 2012-06-13</remarks>
        public static void DelDirectoryOfFiles(string directory, string[] extension, DateTime fileCreateDateStart, DateTime fileCreateDateEnd, bool isContainsSubDirectories)
        {
            //得到指定目录下所有文件
            DirectoryInfo path = new DirectoryInfo(directory);
            FileInfo[] files = path.GetFiles("*.*");

            //扩展名列表转字符串
            string sExtensionName = string.Empty;
            for (int i = 0; i < extension.Length; i++)
            {
                sExtensionName += extension[i];
            }

            //遍历该目录下的所有文件
            foreach (FileInfo fileInfo in files)
            {
                try
                {
                    //得到文件扩展名
                    string extensionName = fileInfo.Name.Substring(fileInfo.Name.LastIndexOf(".")).ToLower();

                    //如果不是指定类型的文件
                    if (sExtensionName.ToLower().IndexOf(extensionName) == -1)
                    {
                        continue;
                    }

                    //获取文件名并截取、转换成日期
                    DateTime fileDateTime = DateTime.MinValue;
                    fileDateTime = fileInfo.CreationTime;

                    //满足日期条件，删除文件
                    if (fileCreateDateStart < fileDateTime && fileDateTime < fileCreateDateEnd)
                    {
                        fileInfo.Delete();
                    }
                }
                catch
                {
                    continue;
                }
            }

            //操作子目录
            if (isContainsSubDirectories)
            {
                //得到所有子目录
                DirectoryInfo[] subDirectories = path.GetDirectories();
                //遍历该目录下的所有子目录
                foreach (DirectoryInfo subDirectorie in subDirectories)
                {
                    //递归
                    DelDirectoryOfFiles(subDirectorie.FullName, extension, fileCreateDateStart, fileCreateDateEnd, isContainsSubDirectories);
                }
            }
        }

        /// <summary>
        /// 删除指定目录下的指定后缀名的文件
        /// </summary>
        /// <param name="directory">要删除的文件所在的目录，是绝对目录，如d:\temp</param>
        /// <param name="masks">要删除的文件的后缀名的一个数组，比如masks中包含了.cs,.vb,.c这三个元素</param>
        /// <param name="searchSubdirectories">表示是否需要递归删除，即是否也要删除子目录中相应的文件</param>
        /// <param name="ignoreHidden">表示是否忽略隐藏文件</param>
        /// <param name="deletedFileCount">表示总共删除的文件数</param>
        public void DeleteFiles(string directory, string[] masks, bool searchSubdirectories, bool ignoreHidden, ref int deletedFileCount)
        {
            //先删除当前目录下指定后缀名的所有文件
            foreach (string file in Directory.GetFiles(directory, "*.*"))
            {
                if (!(ignoreHidden && (File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden))
                {
                    foreach (string mask in masks)
                    {
                        if (Path.GetExtension(file) == mask)
                        {
                            File.Delete(file);
                            deletedFileCount++;
                        }
                    }
                }
            }

            //如果需要对子目录进行处理，则对子目录也进行递归操作
            if (searchSubdirectories)
            {
                string[] childDirectories = Directory.GetDirectories(directory);
                foreach (string dir in childDirectories)
                {
                    if (!(ignoreHidden && (File.GetAttributes(dir) & FileAttributes.Hidden) == FileAttributes.Hidden))
                    {
                        DeleteFiles(dir, masks, searchSubdirectories, ignoreHidden, ref deletedFileCount);
                    }
                }
            }
        }

        #endregion

        #region 文件夹操作 Ralf 2011-12-13


        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>创建是否成功</returns>
        [DllImport("dbgHelp", SetLastError = true)]
        private static extern bool MakeSureDirectoryPathExists(string name);
        /// <summary>
        /// 新建文件夹
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CreateDir(string name)
        {
            return FileHelper.MakeSureDirectoryPathExists(name);
        }

        /// <summary>
        /// 移动目录
        /// </summary>
        /// <param name="sourceDirectoryName">源目录</param>
        /// <param name="destDirectoryName">新目录</param>
        /// <returns></returns>
        public static bool MoveDirectory(string sourceDirectoryName, string destDirectoryName)
        {
            try
            {
                Directory.Move(sourceDirectoryName, destDirectoryName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除目录（仅当目录为空时才会删除）
        /// </summary>
        /// <param name="directory">目录路径</param>
        /// <returns></returns>
        public static bool DelDirectory(string directory)
        {
            try
            {
                Directory.Delete(directory);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除目录（递归删除，包括子目录，子文件，全部删除）
        /// </summary>
        /// <param name="directory">目录（e.g : @"C:\Users\ChinaBest\Desktop\新建文件夹\"）</param>
        /// <param name="recursive">若为 true，则删除此目录、其子目录以及所有文件；否则为false </param>
        /// <remarks>Ralf 2012-06-14</remarks>
        public static bool DelDirectoryRecursive(string directory)
        {
            try
            {
                DirectoryInfo dInfo = new DirectoryInfo(directory);
                dInfo.Delete(true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 设置文件夹访问权限（以完全控制为例）
        /// </summary>
        /// <param name="filePath">目录</param>
        /// <param name="userName">添加可访问的用户名</param>
        public static void SetAccountForPath(string filePath, string userName)
        {
            //FileInfo fileInfo = new FileInfo(filePath);

            //FileSecurity fileSecurity = fileInfo.GetAccessControl();

            //dirsecurity.AddAccessRule(new FileSystemAccessRule(userName, FileSystemRights.FullControl, AccessControlType.Allow));     //以完全控制为例

            //dirinfo.SetAccessControl(fileSecurity);

        }

        #endregion

        #region 文件路径处理

        /// <summary>
        /// 返回该文件在被设置成“复制”属性后，最后在bin目录下的绝对路径
        /// </summary>
        /// <param name="path">目录名（e.g：Image\Image2）</param>
        /// <param name="fileName">文件名（e.g: img.jpg）</param>
        /// <returns></returns>
        public static string GetBinPath(string path, string fileName)
        {
            string result = string.Empty;
            result = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, path, fileName);
            return result;
        }

        #endregion
    }
}
