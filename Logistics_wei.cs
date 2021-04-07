using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 外加剂运输商
    /// </summary>
    public partial class Logistics_wei
    {
        /// <summary>
        /// 是否外加剂运输商
        /// </summary>
        /// <returns></returns>
        public bool IsLogisticsW()
        {
            bool b = false;
            string sql = "select * from Logistics_wei where factory = @factory and logistics = @logistics";
            Logistics_wei tansportPriceTable = DB<Logistics_wei>.QueryFirstOrDefault(sql, new { factory = this.Factory, logistics = this.Logistics });
            if (tansportPriceTable != null)
            {
                b = true;
            }
            return b;
        }
    }
}
