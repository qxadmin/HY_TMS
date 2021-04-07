using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_Horizontally_fee
    {
        public float GetHorizontallyFee() 
        {
            string sql = string.Format("select Horizontally_proportion from qx_Horizontally_fee where factory =@factory and logistics =@logistics");
            return DB<float>.QueryFirstOrDefault(sql,new { factory=this.factory, logistics =this.logistics });
        }
    }
}
