using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections;
using System.IO;
using System.Drawing;

using System.Net;
using System.Configuration;
using Microsoft.VisualBasic;

namespace TaskDispatchManager.Common
{
    /// <summary>
    /// 字符串处理工具类
    /// </summary>
    /// <remarks>冯瑞 2011-09-29 17:44:41</remarks>
    public class StringUtils
    {

        /// <summary>
        /// 生成指定位数的随机数
        /// </summary>
        /// <param name="length">位数</param>
        /// <returns>返回指定长度的随机数</returns>
        /// <remarks>冯瑞 2011-07-27 14:40:42</remarks>
        public static string GetRandomNumber(int length)
        {
            Random randIndex = new Random();
            char[] arrChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + randIndex.Next(int.MinValue, int.MaxValue));
            for (int i = 0; i < length; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }

        #region 字符串替换

        /// <summary>
        /// 半角
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SbcToDbc(string str)
        {
            char[] chars = str.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(chars, i, 1);
                if ((bytes.Length == 2) && (bytes[1] == 0xff))
                {
                    bytes[0] = (byte)(bytes[0] + 0x20);
                    bytes[1] = 0;
                    chars[i] = Encoding.Unicode.GetChars(bytes)[0];
                }
            }
            return new string(chars);
        }

        #endregion

        #region 字符串分割、合并、截取、删除、替换

        /// <summary>
        /// 删除最后一个字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearLastChar(string str)
        {
            return (string.IsNullOrEmpty(str)) ? string.Empty : str.Substring(0, str.Length - 1);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">源字符串</param>
        /// <param name="strSplit">分隔符</param>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                {
                    return new string[] { strContent };
                }
                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
            {
                return new string[0] { };
            }
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">源字符串</param>
        /// <param name="strSplit">分隔符</param>
        /// <param name="count">前多少条</param>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                {
                    result[i] = splited[i];
                }
                else
                {
                    result[i] = string.Empty;
                }
            }

            return result;
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">被分割的字符串</param>
        /// <param name="strSplit">分割符</param>
        /// <param name="ignoreRepeatItem">忽略重复项</param>
        /// <param name="maxElementLength">单个元素最大长度</param>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem, int maxElementLength)
        {
            string[] result = SplitString(strContent, strSplit);

            return ignoreRepeatItem ? DistinctStringArray(result, maxElementLength) : result;
        }

        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem, int minElementLength, int maxElementLength)
        {
            string[] result = SplitString(strContent, strSplit);

            if (ignoreRepeatItem)
            {
                result = DistinctStringArray(result);
            }
            return PadStringArray(result, minElementLength, maxElementLength);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">被分割的字符串</param>
        /// <param name="strSplit">分割符</param>
        /// <param name="ignoreRepeatItem">忽略重复项</param>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem)
        {
            return SplitString(strContent, strSplit, ignoreRepeatItem, 0);
        }

        /// <summary>
        /// 过滤字符串数组中每个元素为合适的大小
        /// 当长度小于minLength时，忽略掉,-1为不限制最小长度
        /// 当长度大于maxLength时，取其前maxLength位
        /// 如果数组中有null元素，会被忽略掉
        /// </summary>
        /// <param name="minLength">单个元素最小长度</param>
        /// <param name="maxLength">单个元素最大长度</param>
        /// <returns></returns>
        public static string[] PadStringArray(string[] strArray, int minLength, int maxLength)
        {
            if (minLength > maxLength)
            {
                int t = maxLength;
                maxLength = minLength;
                minLength = t;
            }

            int iMiniStringCount = 0;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (minLength > -1 && strArray[i].Length < minLength)
                {
                    strArray[i] = null;
                    continue;
                }
                if (strArray[i].Length > maxLength)
                {
                    strArray[i] = strArray[i].Substring(0, maxLength);
                }

                iMiniStringCount++;
            }

            string[] result = new string[iMiniStringCount];
            for (int i = 0, j = 0; i < strArray.Length && j < result.Length; i++)
            {
                if (strArray[i] != null && strArray[i] != string.Empty)
                {
                    result[j] = strArray[i];
                    j++;
                }
            }
            return result;
        }

        /// <summary>
        /// 字符串截断
        /// </summary>
        /// <param name="unicode">字符串</param>
        /// <param name="limitnum">截取长度</param>
        /// <returns></returns>
        public static string SubUnicode(string unicode, int limitnum)
        {
            if (string.IsNullOrEmpty(unicode))
            {
                return string.Empty;
            }
            unicode = unicode.Replace("'", "＇");
            if (limitnum == -1) return unicode;

            byte[] asciiBytes = Encoding.Default.GetBytes(unicode);
            if (asciiBytes.Length <= limitnum)
            {
                limitnum = asciiBytes.Length;
            }
            //return strUnicode;

            List<byte> tmpbytes = new List<byte>();
            for (int i = 0; i < limitnum; i++)
            {
                if (asciiBytes[i] >= (byte)128 && (i + 1) < limitnum) //中文
                {
                    tmpbytes.Add(asciiBytes[i]);
                    tmpbytes.Add(asciiBytes[i + 1]);
                    i++;
                }
                else if (asciiBytes[i] < (byte)128)
                {
                    tmpbytes.Add(asciiBytes[i]);
                }
                //&& asciiBytes[i] != "?"
            }

            asciiBytes = tmpbytes.ToArray();

            char[] asciiChars = new char[Encoding.Default.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            Encoding.Default.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            string asciiString = new string(asciiChars).Replace("\0", "");// +"...";

            return asciiString;
        }

        /// <summary>
        /// 截取字符串中位于起始标记至结束标记之间的字符串
        /// </summary>
        /// <param name="sources">源字符串</param>
        /// <param name="startFlag">起始标记</param>
        /// <param name="endFlag">结束标记</param>
        /// <returns>Ralf 2013-09-14</returns>
        public static string SubStringByFlag(string sources, string startFlag, string endFlag)
        {
            int startIndex = sources.IndexOf(startFlag);
            int endIndex = sources.IndexOf(endFlag);
            string temp = sources.Substring(startIndex + startFlag.Length, endIndex - startIndex - endFlag.Length + 2);
            return temp;
        }

        /// <summary>
        /// 清除UBB标签
        /// </summary>
        /// <param name="sDetail">帖子内容</param>
        /// <returns>帖子内容</returns>
        public static string ClearUBB(string sDetail)
        {
            return Regex.Replace(sDetail, @"\[[^\]]*?\]", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 合并字符
        /// </summary>
        /// <param name="source">要合并的源字符串</param>
        /// <param name="target">要被合并到的目的字符串</param>
        /// <param name="mergechar">合并符</param>
        /// <returns>合并到的目的字符串</returns>
        public static string MergeString(string source, string target)
        {
            return MergeString(source, target, ",");
        }

        /// <summary>
        /// 合并字符
        /// </summary>
        /// <param name="source">要合并的源字符串</param>
        /// <param name="target">要被合并到的目的字符串</param>
        /// <param name="mergeChar">合并符</param>
        /// <returns>并到字符串</returns>
        public static string MergeString(string source, string target, string mergeChar)
        {
            if (string.IsNullOrEmpty(target))
            {
                target = source;
            }
            else
            {
                target += mergeChar + source;
            }
            return target;
        }

        /// <summary>
        /// 去除字符串末尾的字符串
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="tail">要去除的末尾字符串</param>
        /// <returns></returns>
        /// <remarks>Ralf 2012-06-27</remarks>
        public static string TrimEndString(string source, string tail)
        {
            try
            {
                char[] t = new char[tail.Length];
                for (int i = 0; i < tail.Length; i++)
                {
                    t[i] = tail[i];
                }
                return source.TrimEnd(t);
            }
            catch
            {
                return source;
            }
        }

        #endregion

        #region 字符串信息

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        public static bool IsCompriseStr(string str, string stringarray, string strsplit)
        {
            if (string.IsNullOrEmpty(stringarray))
            {
                return false;
            }

            str = str.ToLower();
            string[] stringArray = StringUtils.SplitString(stringarray.ToLower(), strsplit);
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (str.IndexOf(stringArray[i]) > -1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                    {
                        return i;
                    }
                }
                else if (strSearch == stringArray[i])
                {
                    return i;
                }
            }
            return -1;
        }


        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
        public static int GetInArrayID(string strSearch, string[] stringArray)
        {
            return GetInArrayID(strSearch, stringArray, true);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string[] stringArray)
        {
            return InArray(str, stringArray, false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringArray">内部以逗号分割单词的字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringArray)
        {
            return InArray(str, SplitString(stringArray, ","), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringArray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringArray, string strsplit)
        {
            return InArray(str, SplitString(stringArray, strsplit), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringArray">内部以逗号分割单词的字符串</param>
        /// <param name="strSplit">分割字符串</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringArray, string strSplit, bool caseInsensetive)
        {
            return InArray(str, SplitString(stringArray, strSplit), caseInsensetive);
        }


        #endregion

        #region 字符串操作

        /// <summary>
        /// 根据字符串获取枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">字符串枚举值</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static T GetEnum<T>(string value, T defValue)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch (ArgumentException)
            {
                return defValue;
            }
        }

        /// <summary>
        /// 清理字符串
        /// </summary>
        public static string CleanInput(string strIn)
        {
            return Regex.Replace(strIn.Trim(), @"[^\w\.@-]", "");
        }

        /// <summary>
        /// 自定义的替换字符串函数
        /// </summary>
        public static string ReplaceString(string sourceString, string searchString, string replaceString, bool isCaseInsensetive)
        {
            return Regex.Replace(sourceString, Regex.Escape(searchString), replaceString, isCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        public static string GetUnicodeSubString(string str, int len, string tailString)
        {
            str = str.TrimEnd();
            string result = string.Empty;// 最终返回的结果
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);// 单字节字符长度
            int charLen = str.Length;// 把字符平等对待时的字符串长度
            int byteCount = 0;// 记录读取进度
            int pos = 0;// 记录截取位置
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
                    {
                        byteCount += 2;
                    }
                    else// 按英文字符计算加1
                    {
                        byteCount += 1;
                    }

                    if (byteCount > len)// 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == len)// 记下当前位置
                    {
                        pos = i + 1;
                        break;
                    }
                }

                if (pos >= 0)
                {
                    result = str.Substring(0, pos) + tailString;
                }
            }
            else
            {
                result = str;
            }

            return result;
        }

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="srcString">要检查的字符串</param>
        /// <param name="length">指定长度</param>
        /// <param name="tailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string srcString, int length, string tailString)
        {
            return GetSubString(srcString, 0, length, tailString);
        }

        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="srcString">要检查的字符串</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">指定长度</param>
        /// <param name="tailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string srcString, int startIndex, int length, string tailString)
        {
            string myResult = srcString;

            Byte[] bComments = Encoding.UTF8.GetBytes(srcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (startIndex >= srcString.Length)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return srcString.Substring(startIndex, ((length + startIndex) > srcString.Length) ? (srcString.Length - startIndex) : length);
                    }
                }
            }

            if (length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(srcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > startIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (startIndex + length))
                    {
                        p_EndIndex = length + startIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        length = bsSrcString.Length - startIndex;
                        tailString = "";
                    }

                    int nRealLength = length;
                    int[] anResultFlag = new int[length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = startIndex; i < p_EndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }
                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[length - 1] == 1))
                    {
                        nRealLength = length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, startIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + tailString;
                }
            }

            return myResult;
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }

                if (startIndex > str.Length)
                {
                    return string.Empty;
                }
            }
            else
            {
                if (length < 0)
                {
                    return string.Empty;
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }

        /// <summary>
        /// 清除给定字符串中的回车及换行符
        /// </summary>
        /// <param name="str">要清除的字符串</param>
        /// <returns>清除后返回的字符串</returns>
        public static string ClearBR(string str)
        {
            Match m = null;

            for (m = new Regex(@"(\r\n)", RegexOptions.IgnoreCase).Match(str); m.Success; m = m.NextMatch())
            {
                str = str.Replace(m.Groups[0].ToString(), "");
            }
            return str;
        }

        /// <summary>
        /// 删除字符串尾部的回车/换行/空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RTrim(string str)
        {
            for (int i = str.Length; i >= 0; i--)
            {
                if (str[i].Equals(" ") || str[i].Equals("\r") || str[i].Equals("\n"))
                {
                    str.Remove(i, 1);
                }
            }
            return str;
        }

        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <param name="maxElementLength">字符串数组中单个元素的最大长度</param>
        /// <returns></returns>
        public static string[] DistinctStringArray(string[] strArray, int maxElementLength)
        {
            Hashtable h = new Hashtable();

            foreach (string s in strArray)
            {
                string k = s;
                if (maxElementLength > 0 && k.Length > maxElementLength)
                {
                    k = k.Substring(0, maxElementLength);
                }
                h[k.Trim()] = s;
            }

            string[] result = new string[h.Count];

            h.Keys.CopyTo(result, 0);

            return result;
        }

        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <returns></returns>
        public static string[] DistinctStringArray(string[] strArray)
        {
            return DistinctStringArray(strArray, 0);
        }

        /// <summary>
        /// 进行指定的替换(脏字过滤)
        /// </summary>
        public static string StrFilter(string str, string bantext)
        {
            string text1 = string.Empty, text2 = string.Empty;
            string[] textArray1 = SplitString(bantext, "\r\n");
            for (int num1 = 0; num1 < textArray1.Length; num1++)
            {
                text1 = textArray1[num1].Substring(0, textArray1[num1].IndexOf("="));
                text2 = textArray1[num1].Substring(textArray1[num1].IndexOf("=") + 1);
                str = str.Replace(text1, text2);
            }
            return str;
        }

        /// <summary>
        /// 获取身份证中的生日，否则返回DateTime.MinValue
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static DateTime GetIDCardBirthday(string Id)
        {
            if (!ValidatorUtils.IsIDCard(Id))
            {
                return DateTime.MinValue;
            }

            int intLen = Id.Length;

            if (intLen == 15)
            {
                string ts = string.Empty;
                if (Convert.ToInt32(Id.Substring(6, 2)) > 20)
                {
                    ts = "19";
                }
                else
                {
                    ts = "20";
                }
                string IDCode18 = Id.Substring(0, 6) + ts + Id.Substring(6);

                int[] wi = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
                string[] wf = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
                int s = 0;
                for (int i = 0; i < 17; i++)
                {
                    s += wi[i] * Convert.ToInt32(IDCode18.Substring(i, 1));
                }
                IDCode18 += wf[Convert.ToInt32(s) % 11];

                Id = IDCode18;
            }

            if (Id.Length == 18)
            {
                string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                DateTime time = new DateTime();
                if (DateTime.TryParse(birth, out time) == false)
                {
                    return DateTime.MinValue;//生日验证
                }
                return time;
            }
            return DateTime.MinValue;
        }


        #endregion

        #region 生成随机字符

        /// <summary>
        /// 生成制定位数的随机字符（0-9，A-Z，a-z，其中过滤数字0和1，字母o、l、O、L，因为0比较像字母O，1比较像字母l）
        /// </summary>
        /// <param name="length">位数</param>
        /// <returns>返回指定长度的随机字符</returns>
        /// <remarks>Ralf 2012-04-11</remarks>
        public static string GetRandomChar(int length)
        {
            Random randIndex = new Random();

            ArrayList charList = new ArrayList();

            //添加0-9
            for (int i = 0; i < 10; i++)
            {
                //过滤0和1，因为0比较像字母O，1比较像字母l
                if (i == 0 || i == 1)
                {
                    continue;
                }
                else
                {
                    charList.Add(i);
                }
            }

            //添加A-Z
            for (int i = 65; i <= 90; i++)
            {
                //过滤O和L，因为0比较像字母O，1比较像字母L
                if (((char)i) == 'L' || ((char)i) == 'O')
                {
                    continue;
                }
                else
                {
                    charList.Add((char)i);
                }
            }

            //添加a-z
            for (int i = 97; i <= 122; i++)
            {
                //过滤o和l，因为0比较像字母O，1比较像字母l
                if (((char)i) == 'l' || ((char)i) == 'o')
                {
                    continue;
                }
                else
                {
                    charList.Add((char)i);
                }
            }

            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + randIndex.Next(0, int.MaxValue));
            for (int i = 0; i < length; i++)
            {
                num.Append(charList[rnd.Next(0, charList.Count)].ToString());
            }
            return num.ToString();
        }

        #endregion

        #region GUID

        /// <summary>
        /// 去除Guid的“-”符号,返回字符串（小写）
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <remarks>Ralf 2012-05-09</remarks>
        public static string GuidToString(Guid guid)
        {
            return guid.ToString().Replace("-", "").ToLower();
        }
        /// <summary>
        /// 将已去除“-”的Guid字符串还原成Guid格式
        /// </summary>
        /// <param name="guid">去除“-”的Guid字符串</param>
        /// <remarks>Ralf 2012-05-09</remarks>
        public static Guid ReturnGuidByString(string guid)
        {
            string result = string.Empty;
            for (int i = 0; i < guid.Length; i++)
            {
                result += guid[i];
                //按照GUID格式(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)，在适当位置插入“-”
                if (i == 7 || i == 11 || i == 15 || i == 19)
                {
                    result += "-";
                }
            }
            return new Guid(result);
        }

        /// <summary>
        /// 生成一个不带横线的GUID字符串
        /// </summary>
        /// <returns></returns>
        public static string CreateGuidNoLine()
        {
            return Guid.NewGuid().ToString("N").ToLower();
        }

        #endregion

        #region 安全性

        /// <summary>
        /// 检测是否有危险的可能用于链接的字符串
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeUserInfoString(string str)
        {
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        }

        #endregion
    }
}
