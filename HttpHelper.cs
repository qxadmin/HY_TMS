using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace HY.TMS.Common
{
    public class HttpHelper
    {
        public static string Post(string url, string body, string contentType)
        {
            string result = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                if (string.IsNullOrWhiteSpace(contentType))
                {
                    contentType = "application/x-www-form-urlencoded";
                }
                req.ContentType = contentType;
                byte[] data = Encoding.UTF8.GetBytes(body);//把字符串转换为字节
                req.ContentLength = data.Length; //请求长度
                using (Stream reqStream = req.GetRequestStream()) //获取
                {
                    reqStream.Write(data, 0, data.Length);//向当前流中写入字节
                    reqStream.Close(); //关闭当前流
                }
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse(); //响应结果
                Stream stream = resp.GetResponseStream();
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                //Log4netHelper.Log_PageLoad("POST请求异常：请求方法：" + url + "请求内容参数：" + body + "错误信息：\n" + e.StackTrace);
                return string.Empty;
            }
            return result;
        }


        /// <summary>
        /// GET请求与获取结果
        /// </summary>
        public static string HttpGet(string Url, string contentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET"; //设置请求方式
            if (string.IsNullOrWhiteSpace(contentType))
            {
                contentType = "text/html;charset=UTF-8";
            }
            request.ContentType = contentType; //设置内容类型

            HttpWebResponse response = (HttpWebResponse)request.GetResponse(); //返回响应

            Stream myResponseStream = response.GetResponseStream(); //获得响应流

            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);//以UTF8编码方式读取该流
            string retString = myStreamReader.ReadToEnd();//读取所有

            myStreamReader.Close();//关闭流
            myResponseStream.Close();
            return retString;
        }

        /// <summary>
        /// 生成最终URL
        /// </summary>
        /// <param name="baseUrl">基准URL（不含查询串）</param>
        /// <param name="dictParam">查询参数字典</param>
        /// <returns>最终URL</returns>
        public static string GetLastUrl_New(string baseUrl, Dictionary<string, string> dictParam)
        {
            var sbUrl = new StringBuilder(baseUrl);
            if (dictParam != null && dictParam.Count > 0)
            {
                sbUrl.Append("?");
                int index = 0;
                foreach (var item in dictParam)
                {
                    sbUrl.Append(string.Format("{0}={1}", item.Key, item.Value));
                    if (index < dictParam.Count - 1)
                    {
                        sbUrl.Append("&");
                    }
                    index++;
                }
            }
            var url = sbUrl.ToString();
            return url;
        }
    }
}