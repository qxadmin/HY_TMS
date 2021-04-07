using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Common
{
    /// <summary>
    /// 云盘帮助类
    /// </summary>
    public static class YunPanHelper
    {
        #region 获取云盘接口钥匙
        public static string Yp_login()
        {

            Encoding myEncoding = Encoding.GetEncoding("utf-8");  //选择编码字符集
            string data = "userName=190003&passWord=190003ceshi&clientKey=0";  //要上传到网页系统里的数据（字段名=数值 ，用&符号连接起来）
            byte[] bytesToPost = System.Text.Encoding.Default.GetBytes(data); //转换为bytes数据

            string responseResult = String.Empty;
            HttpWebRequest req = (HttpWebRequest)
            HttpWebRequest.Create("http://yun.yuhong.com.cn/ucdisk/api/2.0/user/login");   //创建一个有效的httprequest请求，地址和端口和指定路径必须要和网页系统工程师确认正确，不然一直通讯不成功
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            req.ContentLength = bytesToPost.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bytesToPost, 0, bytesToPost.Length);     //把要上传网页系统的数据通过post发送
            }
            HttpWebResponse cnblogsRespone = (HttpWebResponse)req.GetResponse();
            if (cnblogsRespone != null && cnblogsRespone.StatusCode == HttpStatusCode.OK)
            {
                StreamReader sr;
                using (sr = new StreamReader(cnblogsRespone.GetResponseStream()))
                {
                    responseResult = sr.ReadToEnd();  //网页系统的json格式的返回值，在responseResult里，具体内容就是网页系统负责工程师跟你协议号的返回值协议内容
                }
                sr.Close();
            }
            cnblogsRespone.Close();

            string[] a = responseResult.Split('"');
            string token = "";

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].IndexOf("token") >= 0)
                {
                    token = a[i + 2];
                }
            }

            return token;
        }
        #endregion
    }
}
