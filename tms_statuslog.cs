using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class tms_statuslog
    {
        /// <summary>
        /// 记录该承运单所有的状态变化
        /// </summary>
        /// <param name="ordernum">订单编号</param>
        /// <param name="content">内容</param>
        /// <param name="statusname">状态名称</param>
        /// <param name="optime">操作时间</param>
        /// <param name="opuser">操作人</param>
        /// <returns>是否成功</returns>
        public static bool AddOrderStatusLog(string ordernum, string content, string statusname, string optime, string opuser)
        {
            string sql = string.Format(@"insert into tms_statuslog(orderstatuslogid,ordernum,opcontent,statusname,optime,opuser)
                         values(@orderstatuslogid,@ordernum,@opcontent,@statusname,@optime,@opuser)");
            return DB<int>.Execute(sql, new { orderstatuslogid = Guid.NewGuid().ToString(), ordernum = ordernum, opcontent = content, statusname = statusname, optime = optime, opuser = opuser }) > 0;

        }
    }
}
