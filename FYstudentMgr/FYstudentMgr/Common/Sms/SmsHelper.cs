using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace FYstudentMgr.Common.Sms
{
  
    public static class SmsHelper
    {
        /// <summary>
        /// 推送数据 POST方式
        /// </summary>
        /// <param name="weburl">POST到的网址</param>
        /// <param name="data">POST的参数及参数值</param>
        /// <param name="encode">编码方式</param>
        /// <returns></returns>
        public static string PushToWeb(string weburl, string data, Encoding encode)
        {
            HttpWebRequest webRequest = null;
            HttpWebResponse response = null;
            StreamReader aspx = null;
            string result = string.Empty;
            try
            {
                byte[] byteArray = encode.GetBytes(data);

                webRequest = (HttpWebRequest)WebRequest.Create(new Uri(weburl));
                webRequest.Method = "POST";
                webRequest.KeepAlive = false;
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteArray.Length;

                Stream newStream = webRequest.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();

                //接收返回信息：
                response = (HttpWebResponse)webRequest.GetResponse();
                aspx = new StreamReader(response.GetResponseStream(), encode);
                result = aspx.ReadToEnd();
            }
            catch (Exception ex)
            {
                //记录错误日志
                result = "";
            }
            finally
            {
                if (aspx != null)
                {
                    aspx.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (webRequest != null)
                {
                    webRequest.Abort();
                }
            }

            return result;
        }

        #region 获取时间戳
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="serverTime">时间</param>
        /// <param name="isMillisecond">是否精确到毫秒</param>
        /// <returns></returns>
        public static string ToUnixStamp(DateTime serverTime)
        {
            var ts = serverTime - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        #endregion

        #region 转换时间戳
        /// <summary>
        /// 转换时间戳
        /// </summary>
        /// <param name="unixStamp">时间戳</param>
        /// <param name="isMillisecond">是否精确到毫秒</param>
        /// <returns></returns>
        public static DateTime FromUnixStamp(long unixStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(unixStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        #endregion
    }
}