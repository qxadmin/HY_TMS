using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 共生接口响应实体
    /// </summary>
    public class addPlanResponseContent
    {
        /// <summary>
        /// 响应编码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 响应信息
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 响应数据
        /// </summary>
        public data data { get; set; }
    }

    public class data
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string no { get; set; }
    }
}
