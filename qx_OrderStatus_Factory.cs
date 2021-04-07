using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_OrderStatus_Factory
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            string sql = "delete  from qx_OrderStatus_Factory  where ORDERNO =@ORDERNO";
            return DB<int>.Execute(sql, new { ORDERNO = this.ORDERNO }) > 0;
        }
    }
}
