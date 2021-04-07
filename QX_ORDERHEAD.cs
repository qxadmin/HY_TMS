using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 运单表
    /// </summary>
    public partial class QX_ORDERHEAD
    {
        /// <summary>
        /// 获取运单表详细信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public QX_ORDERHEAD GetOrderDetail(string tmsOrderNo)
        {
            string sql = string.Format("SELECT [WEIGHT]=(select sum([WEIGHT]) from dbo.View_Detail_Other_one where ORDERNO=a.INORDERNO group by ORDERNO),* FROM qx_InAndOutBoundOrder a WHERE INORDERNO=@INORDERNO");
            return DB<QX_ORDERHEAD>.QueryFirst(sql, new { INORDERNO = tmsOrderNo });
        }
    }
}
