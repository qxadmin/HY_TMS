using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_OrderTime_Log
    {
        public bool UpdateOrderTimeLog(List<string> tmsordernoLst) 
        {
            string sql = "update qx_OrderTime_Log set SendcarsTime = getdate() where Tmsorderno in @Tmsorderno";
            return DB<int>.Execute(sql,new { Tmsorderno= tmsordernoLst })>0;
        }

        /// <summary>
        /// TMS订单状态全流程跟踪
        /// </summary>
        /// <param name="tmsordernoLst">订单编号集合</param>
        /// <param name="field">要修改字段</param>
        /// <param name="value">要修改的值</param>
        /// <returns></returns>
        public bool UpdateOrderTimeLog(List<string> tmsordernoLst,string field,string value)
        {
            string sql = "update qx_OrderTime_Log set " + field + " = '" + value + "' where Tmsorderno in @Tmsorderno";
            return DB<int>.Execute(sql, new { Tmsorderno = tmsordernoLst }) > 0;
        }
    }
}
