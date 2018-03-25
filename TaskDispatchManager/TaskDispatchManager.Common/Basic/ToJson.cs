using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Data;
using Newtonsoft.Json.Linq;

namespace TaskDispatchManager.Common
{
    /// <summary>
    /// Newtonsoft.Json封装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>冯瑞 2011-02-20</remarks>
    public class ToJson<T>
    {
        public string ReturnMessage()
        {
            string json = string.Empty;
            return json;
        }
        /// <summary>
        /// 将类型实例转换为Json字符串
        /// </summary>
        /// <param name="t">类型实例</param>
        /// <returns></returns>
        public string ToJsonString(T t)
        {
            //string result = Newtonsoft.Json.JavaScriptConvert.SerializeObject(t);
            //JavaScriptSerializer ser = new JavaScriptSerializer();

            //新版本
            IsoDateTimeConverter a = new IsoDateTimeConverter();
            string result = JsonConvert.SerializeObject(t, a);
            return result;
        }

        /// <summary>
        /// 将Json字符串转换为类型实例
        /// </summary>
        /// <param name="jsonString">字符串</param>
        /// <returns>类型</returns>
        public T FromJsonString(string jsonString)
        {
            //return Newtonsoft.Json.JavaScriptConvert.DeserializeObject<T>(jsonstring);
            var jsonSetting = new JsonSerializerSettings();
            //jsonSetting.CheckAdditionalContent = true;

            //新版本            
            T t = JsonConvert.DeserializeObject<T>(jsonString, jsonSetting);
            return t;
        }

        /// <summary>
        /// 将List转换为字符串
        /// </summary>
        /// <param name="list">数据集</param>
        /// <returns>字符串</returns>
        public string ToJsonString(List<T> list)
        {
            //string result = Newtonsoft.Json.JavaScriptConvert.SerializeObject(tlist);
            //return result;

            //新版本
            IsoDateTimeConverter a = new IsoDateTimeConverter();
            string result = JsonConvert.SerializeObject(list, Formatting.Indented, a);
            return result;
        }

        /// <summary>
        /// 将Json字符串转换为类型List集合
        /// </summary>
        /// <param name="jsonString">字符串</param>
        /// <returns>类型集合</returns>
        public List<T> GetList(string jsonString)
        {
            //return Newtonsoft.Json.JavaScriptConvert.DeserializeObject<List<T>>(jsonstring);

            //新版本            
            List<T> list = JsonConvert.DeserializeObject<List<T>>(jsonString);
            return list;
        }

        /// <summary>   
        /// Datatable转换为Json   
        /// </summary>   
        /// <param name="dt">Datatable对象</param>   
        /// <returns>Json字符串</returns>   
        public static string ToJsonString(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = String.Format(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            jsonString = new StringBuilder(jsonString.ToString().TrimEnd(','));
            jsonString.Append("]");
            return jsonString.ToString();
        }
#pragma warning disable 693
        public string ToJsonString<T>(T t)
#pragma warning restore 693
        {
            //string result = Newtonsoft.Json.JavaScriptConvert.SerializeObject(t);
            //JavaScriptSerializer ser = new JavaScriptSerializer();

            //新版本
            IsoDateTimeConverter a = new IsoDateTimeConverter();
            string result = JsonConvert.SerializeObject(t, a);
            return result;
        }
    }
}
