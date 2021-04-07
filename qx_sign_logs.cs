using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_sign_logs
    {
        public bool Insert()
        {
            string sql = @"insert into qx_sign_logs ([ordertype]
                          ,[orderno]
                          ,[issign]
                          ,[signdesc]
                          ,[data]
                          ,[timer]
                          ,[itemno]
                          ,[pzno]
                          ,[sign]
                          ,[createtime]) values(@ordertype,@orderno,@issign,@signdesc,@data,@timer,@itemno,@pzno,@sign,getdate())";
            return DB<int>.Execute(sql, this) > 0;
        }
    }
}
