using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 保温时间表
    /// </summary>
    public partial class qx_holding_time
    {

        /// <summary>
        /// 是否有保温时间
        /// </summary>
        /// <returns></returns>
        public bool IsHaveHoldingTime()
        {
            string sql = @"select 1 from qx_holding_time where FirstTime<=@FirstTime and LastTime>=@LastTime and factory=@factory";
            return DB<int>.QueryFirst(sql, new { FirstTime=this.FirstTime,LastTime=this.LastTime, factory=this.factory }) >0;
        }
    }
}
