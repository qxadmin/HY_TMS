using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model.Model
{
    public class WebApiRequest
    {
        /// <summary>
        /// 请求数据
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 其他参数
        /// </summary>
        public string OtherData { get; set; }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public qx_User currentUser { get; set; }
    }
}
