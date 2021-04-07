using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_Unloading_transshipment
    {
        public qx_Unloading_transshipment getDischargeFee()
        {
            string sql = @"select mixdischarge_fee,maxdischarge_fee from [dbo].[qx_Unloading_transshipment] where factory = @factory and logistics = @logistics";
            return DB<qx_Unloading_transshipment>.QueryFirstOrDefault(sql, new { factory = this.factory, logistics = this.logistics });
        }
    }
}
