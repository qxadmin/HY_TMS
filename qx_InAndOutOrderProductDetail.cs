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
    /// 运单表行项目信息
    /// </summary>
    public partial class qx_InAndOutOrderProductDetail
    {

        //总重量
        private decimal _SumWEIGHT = 0;

        public decimal SumWEIGHT
        {
            get
            {
                if (_SumWEIGHT == 0)
                {
                    return this.REQ_QTY * this.WEIGHT / 1000;
                }
                return 0;
            }
        }

        /// <summary>
        /// 合计总毛重
        /// </summary>
        public decimal HJSumWEIGHT { get; set; }

        /// <summary>
        /// 获取运单表行项目信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<qx_InAndOutOrderProductDetail> GetOrderDetailByTMSOrderNo(string tmsOrderNo)
        {
            string sql = string.Format("select SAPORDERNO,ORDERNO,ORDERDETAILID,SAPORDERITEM,PRODUCTNO,PRODUCTNAME,REQ_QTY,ORDER_REQ_QTY,UNIT,UNIT_DESC,WEIGHT,REQ_QTY,ORDER_UNIT_DESC  FROM qx_inAndOutOrderProductDetail with(nolock) WHERE orderno=@INORDERNO order by SAPORDERITEM asc");
            return DB<qx_InAndOutOrderProductDetail>.Query(sql, new { INORDERNO = tmsOrderNo });
        }

        /// <summary>
        /// 创建拆分订单明细
        /// </summary>
        /// <returns></returns>
        public bool CreateOrderDetail(DbConnection connection, DbTransaction dbTransaction)
        {
            string sql = @"insert into qx_InAndOutOrderProductDetail(
                                   [ORDERNO]
                                  ,[PRODUCTNO]
                                  ,[PRODUCTNAME]
                                  ,[STANDARD]
                                  ,[REQ_QTY]
                                  ,[UNIT]
                                  ,[UNIT_DESC]
                                  ,[ORDER_REQ_QTY]
                                  ,[ORDER_UNIT]
                                  ,[ORDER_UNIT_DESC]
                                  ,[WH_QTY]
                                  ,[ORDERTYPE]
                                  ,[SAPORDERNO]
                                  ,[SAPORDERITEM]
                                  ,[DELIVERTIME]
                                  ,[MAXCATEGORY]
                                  ,[MINCATEGORY]
                                  ,[PRODUCTTYPE]
                                  ,[WEIGHT]
                                  ,[SPEMT]
                                  ,[ISITEMDEL]
                                  ,[ISLOAD]
                                  ,[ISLOCK])
                                  select 
                                   @NewOrderNo
                                  ,[PRODUCTNO]
                                  ,[PRODUCTNAME]
                                  ,[STANDARD]
                                  ,@REQ_QTY
                                  ,[UNIT]
                                  ,[UNIT_DESC]
                                  ,@ORDER_REQ_QTY
                                  ,[ORDER_UNIT]
                                  ,[ORDER_UNIT_DESC]
                                  , 0 [WH_QTY]
                                  ,[ORDERTYPE]
                                  ,[SAPORDERNO]
                                  ,[SAPORDERITEM]
                                  ,[DELIVERTIME]
                                  ,[MAXCATEGORY]
                                  ,[MINCATEGORY]
                                  ,[PRODUCTTYPE]
                                  ,[WEIGHT]
                                  ,[SPEMT]
                                  ,[ISITEMDEL]
                                  ,[ISLOAD]
                                  ,0 [ISLOCK] from qx_InAndOutOrderProductDetail where ORDERDETAILID=@ORDERDETAILID";
            return DB<int>.Execute(sql, new { NewOrderNo = this.ORDERNO, ORDERDETAILID=this.ORDERDETAILID, ORDER_REQ_QTY=this.ORDER_REQ_QTY, REQ_QTY=this.ORDER_REQ_QTY }
                                   , connection, dbTransaction) > 0;
        }


        /// <summary>
        /// 更新父级行项目数量
        /// </summary>
        /// <returns></returns>
        public bool UpdateParentItemCount(DbConnection connection, DbTransaction dbTransaction) 
        {
            string sql = @"update qx_InAndOutOrderProductDetail set REQ_QTY=REQ_QTY-@ORDER_REQ_QTY,ORDER_REQ_QTY=ORDER_REQ_QTY-@ORDER_REQ_QTY where ORDERDETAILID=@ORDERDETAILID";
            return DB<int>.Execute(sql, new { ORDERDETAILID = this.ORDERDETAILID, ORDER_REQ_QTY = this.ORDER_REQ_QTY}, connection, dbTransaction) > 0;
        }
    }
}
