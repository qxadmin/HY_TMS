using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    ///状态日志
    /// </summary>
    public partial class qx_STATUSLOG
    {
        public bool InsertSingle()
        {
            string sql = @"insert into qx_STATUSLOG(TMSOrderNo,SAPORDERNO,OPCONTENT,STATUSNAME,OPTIME,OPUSER)values(@TMSOrderNo,@SAPORDERNO,@OPCONTENT,@STATUSNAME,getdate(),@OPUSER)";
            return DB<int>.Execute(sql, new { TMSOrderNo = this.TMSOrderNo, SAPORDERNO = this.SAPORDERNO, OPCONTENT = this.OPCONTENT, STATUSNAME = this.STATUSNAME, OPUSER = this.OPUSER }) > 0;
        }
    }
}
