using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// SAP订单信息
    /// </summary>
    public partial class qx_InAndOutBoundOrder_SAP
    {
        /// <summary>
        /// 锁定状态还原，数量还原
        /// </summary>
        /// <param name="sapOrderNo">sap订单号</param>
        /// <param name="sapItemNo">sap项目号</param>
        /// <param name="qty">还原数量</param>
        /// <returns></returns>
        public bool ReductionREADYQTYAndStatus(DbConnection connection, DbTransaction transaction)
        {
            string sql = "update qx_InAndOutBoundOrder_SAP set ISCLOSE=0,READYQTY=READYQTY-@READYQTY where SAPORDERNO=@SAPORDERNO and SAPITEMNO=@SAPITEMNO and READYQTY>=@READYQTY";//ISCANCEL 1 取消发运 0正常  取消发运状态99   
            return DB<int>.Execute(sql, new { SAPORDERNO = this.SAPORDERNO, SAPITEMNO = this.SAPITEMNO, READYQTY = this.REQ_QTY }, connection, transaction) > 0;
        }


        /// <summary>
        /// 获取签收详情
        /// </summary>
        /// <returns></returns>
        public List<qx_InAndOutBoundOrder_SAP> getSingDetail() 
        {
            string sql = @"select ZORDERTYPE,SAPORDERNO,ISCLOSE,READYQTY,EBELN,FROMPLANT from qx_InAndOutBoundOrder_SAP where 
                           ((saporderno=@saporderno and ISDEL is null and ISCLOSE=0)
                           or(EBELN=@saporderno and ISDEL is null and ISCLOSE=0))
                           and  exists
                           (
                             select factoryNo from DFYH_WMS_DB.dbo.qx_Factory  where  factoryNo=FROMPLANT
                           )";
            return DB<qx_InAndOutBoundOrder_SAP>.Query(sql,new { saporderno =this.SAPORDERNO});
        }
    }
}
