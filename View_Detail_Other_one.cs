using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class View_Detail_Other_one
    {
        /// <summary>
        /// 获取其他订单信息
        /// </summary>
        /// <returns></returns>
        public List<View_Detail_Other_one> GetOtherOneInfoByInOrderNo()
        {
            string sql = "select * from View_Detail_Other_one where inorderno =@inorderno";
            return DB<View_Detail_Other_one>.Query(sql, new{ inorderno = this.INORDERNO });
        }
    }
}
