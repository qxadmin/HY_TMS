using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 订单日志
    /// </summary>
    public partial class orderdel_log
    {
        /// <summary>
        /// 发运计划调整日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool AddorderdelLogs()
        {
            string sql = @"INSERT INTO [dbo].[orderdel_log]
                              ([orderno],[Remark],[type],[updatefield],[updateqian],[updatehou],[fromplant],[saporderno],[createName])
                              VALUES
                              (@orderno,@Remark,@type,@updatefield,@updateqian,@updatehou,@fromplant,@saporderno,@createName)";
            return DB<int>.Execute(sql, this) > 0;
        }
    }
}
