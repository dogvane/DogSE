using System.Collections.Generic;

#region zh-CHS 2006 - 2010 DemoSoft 团队 | en 2006-2010 DemoSoft Team

//     NOTES
// ---------------
//
// This file is a part of the MMOSE(Massively Multiplayer Online Server Engine) for .NET.
//
//                              2006-2010 DemoSoft Team
//
//
// First Version : by H.Q.Cai - mailto:caihuanqing@hotmail.com

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU Lesser General Public License as published
 *   by the Free Software Foundation; either version 2.1 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

#region zh-CHS 包含名字空间 | en Include namespace

using System;
using System.Globalization;
using System.Net;
using System.Text;
#endregion

namespace DogSE.Library.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConvertString
    {
        #region zh-CHS 内部静态属性 | en Internal Static Properties
        #region zh-CHS 私有静态成员变量 | en Private Static Member Variables
        /// <summary>
        /// 
        /// </summary>
        private static Encoding s_UTF8 = new UTF8Encoding(false, false);
        #endregion
        /// <summary>
        /// Safe UTF8
        /// </summary>
        public static Encoding UTF8
        {
            get { return s_UTF8; }
        }

        #endregion

        #region zh-CHS To... | en To...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool ToBoolean(string strValue)
        {
            bool bReturn;

            bool.TryParse(strValue, out bReturn);

            return bReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool ConvertToBoolean(this string strValue)
        {
            return ToBoolean(strValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static float ToSingle(string strValue)
        {
            float fReturn;

            float.TryParse(strValue, out fReturn);

            return fReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static float ConvertToSingle(this string strValue)
        {
            return ToSingle(strValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static double ToDouble(string strValue)
        {
            double dReturn;

            double.TryParse(strValue, out dReturn);

            return dReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static double ConvertToDouble(this string strValue)
        {
            return ToDouble(strValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(string strValue)
        {
            TimeSpan timeSpan;

            TimeSpan.TryParse(strValue, out timeSpan);

            return timeSpan;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static TimeSpan ConvertToTimeSpan(this string strValue)
        {
            return ToTimeSpan(strValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(string strValue)
        {
            DateTime dateTime;

            DateTime.TryParse(strValue, out dateTime);

            return dateTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(this string strValue)
        {
            return ToDateTime(strValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static short ToInt16(string strValue)
        {
            short iReturn;

            if (strValue.StartsWith("0x"))
                short.TryParse(strValue.Substring(2), NumberStyles.HexNumber, null, out iReturn);
            else
                short.TryParse(strValue, out iReturn);

            return iReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static short ConvertToInt16(this string strValue)
        {
            return ToInt16(strValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static ushort ToUInt16(string strValue)
        {
            ushort iReturn;

            if (strValue.StartsWith("0x"))
                ushort.TryParse(strValue.Substring(2), NumberStyles.HexNumber, null, out iReturn);
            else
                ushort.TryParse(strValue, out iReturn);

            return iReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static ushort ConvertToUInt16(this string strValue)
        {
            return ToUInt16(strValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static int ToInt32(string strValue)
        {
            int iReturn;

            if (strValue.StartsWith("0x"))
                int.TryParse(strValue.Substring(2), NumberStyles.HexNumber, null, out iReturn);
            else
                int.TryParse(strValue, out iReturn);

            return iReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static int ConvertToInt32(this string strValue)
        {
            return ToInt32(strValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static uint ToUInt32(string strValue)
        {
            uint iReturn;

            if (strValue.StartsWith("0x"))
                uint.TryParse(strValue.Substring(2), NumberStyles.HexNumber, null, out iReturn);
            else
                uint.TryParse(strValue, out iReturn);

            return iReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static uint ConvertToUInt32(this string strValue)
        {
            return ToUInt32(strValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static long ToLong64(string strValue)
        {
            long lReturn;

            if (strValue.StartsWith("0x"))
                long.TryParse(strValue.Substring(2), NumberStyles.HexNumber, null, out lReturn);
            else
                long.TryParse(strValue, out lReturn);

            return lReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static long ConvertToLong64(this string strValue)
        {
            return ToLong64(strValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static ulong ToULong64(string strValue)
        {
            ulong lReturn;

            if (strValue.StartsWith("0x"))
                ulong.TryParse(strValue.Substring(2), NumberStyles.HexNumber, null, out lReturn);
            else
                ulong.TryParse(strValue, out lReturn);

            return lReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static ulong ConvertToULong64(this string strValue)
        {
            return ToULong64(strValue);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static IPAddress ToIPAddress(string strValue)
        {
            IPAddress ipAddress;

            IPAddress.TryParse(strValue, out ipAddress);

            return ipAddress;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static IPAddress ConvertToIPAddress(this string strValue)
        {
            return ToIPAddress(strValue);
        }

        /// <summary>
        /// 字符串转换成字节数组
        /// </summary>
        /// <param name="byteString"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(string byteString)
        {
            return s_UTF8.GetBytes(byteString);
        }


        /// <summary>
        /// 字符串转换成字节数组
        /// </summary>
        /// <param name="byteString"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(string byteString, Encoding encoding)
        {
            return encoding.GetBytes(byteString);
        }

        #endregion

        #region zh-CHS Concat... | en Concat...

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="strStringList"></param>
        public static void Concat(ref StringBuilder stringBuilder, params string[] strStringList)
        {
            for (int iIndex = 0; iIndex < strStringList.Length; iIndex++)
                stringBuilder.Append(strStringList[iIndex]);
        }


        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="stringBuilder"></param>
        /// <param name="strString2"></param>
        public static void Coalition(this string strString, ref StringBuilder stringBuilder, string strString2)
        {
            Concat(ref stringBuilder, strString, strString2);
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="stringBuilder"></param>
        /// <param name="strString2"></param>
        /// <param name="strString3"> </param>
        public static void Coalition(this string strString, ref StringBuilder stringBuilder, string strString2, string strString3)
        {
            Concat(ref stringBuilder, strString, strString2, strString3);
        }


        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="stringBuilder"></param>
        /// <param name="strString2"></param>
        /// <param name="strString3"></param>
        /// <param name="strString4"></param>
        public static void Coalition(this string strString, ref StringBuilder stringBuilder, string strString2, string strString3, string strString4)
        {
            Concat(ref stringBuilder, strString, strString2, strString3, strString4);
        }
        #endregion

        #region CSV


        private static readonly char[] CSVSplit = {','};

        /// <summary>
        /// 将 1,2,3 转换为字符串数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] ToCSVSplit(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return new string[0];

            return str.Split(CSVSplit, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 将 1,2,3 转换为字符串数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] ToCSVSplitToInt(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return new int[0];

            var arr = str.Split(CSVSplit, StringSplitOptions.RemoveEmptyEntries);
            var ret = new int[arr.Length];

            for (int i = 0; i < arr.Length; i++)
                ret[i] = int.Parse(arr[i]);

            return ret;
        }

        /// <summary>
        /// 将字符串列表转为csv格式数据
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ConvertToCSV(this List<string> array)
        {
            if (array == null || array.Count == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder(array.Count * 5);

            foreach (var str in array)
            {
                sb.Append(str);
                sb.Append(',');
            }

            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        /// <summary>
        /// 将字符串列表转为csv格式数据
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ConvertToCSV(this List<int> array)
        {
            if (array == null || array.Count == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder(array.Count * 5);

            foreach (var str in array)
            {
                sb.Append(str);
                sb.Append(',');
            }

            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        /// <summary>
        /// 将字符串列表转为csv格式数据
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ConvertToCSV(this int[] array)
        {
            if (array == null || array.Length == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder(array.Length * 5);

            foreach (var str in array)
            {
                sb.Append(str);
                sb.Append(',');
            }

            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        #endregion


        /// <summary>
        /// 字符串里是否包含4字节的字符
        /// </summary>
        /// <remarks>
        /// 代码来源：http://www.cnblogs.com/yangxudong/p/3737593.html
        /// </remarks>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasUtf8FourWord(string str)
        {
            int maxLen = 0;

            var bytes = Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < bytes.Length; )
            {
                int len = 1;
                byte b = bytes[i];
                if (b >= 0xFC)
                    len = 6;
                else if (b >= 0xF8)
                    len = 5;
                else if (b >= 0xF0)
                    len = 4;
                else if (b >= 0xE0)
                    len = 3;
                else if (b >= 0xC0)
                    len = 2;

                i += len;
                maxLen = Math.Max(maxLen, len);
            }

            return maxLen > 3;
        }

        /// <summary>
        /// 替换utf8不识别的超过4个字符的字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceUtf8FourWord(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var bytes = Encoding.UTF8.GetBytes(str);
            var retBytes = new byte[bytes.Length];
            int rIndex = 0;
            for (int i = 0; i < bytes.Length;)
            {
                int len = 1;
                byte b = bytes[i];
                if (b >= 0xFC)
                    len = 6;
                else if (b >= 0xF8)
                    len = 5;
                else if (b >= 0xF0)
                    len = 4;
                else if (b >= 0xE0)
                    len = 3;
                else if (b >= 0xC0)
                    len = 2;

                if (len > 3)
                {
                    retBytes[rIndex] = (byte) '?';
                    rIndex++;
                }
                else
                {
                    Buffer.BlockCopy(bytes, i, retBytes, rIndex, len);
                    rIndex += len;
                }

                i += len;
            }

            return Encoding.UTF8.GetString(retBytes, 0, rIndex);
        }

    }
}
#endregion