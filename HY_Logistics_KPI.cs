using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 物流信息考核
    /// </summary>
    public partial class HY_Logistics_KPI
    {
        /// <summary>
        /// 新增物流考核数据
        /// </summary>
        /// <returns></returns>
        public bool InsertLogisticsKPI()
        {
            string sql = @"INSERT INTO [dbo].[HY_Logistics_KPI]
                           (
                            FreightCalculationTableId
                           ,OrderNo
                           ,LateArrivalOfGoodsAssessment
                           ,LateArrivalOfGoodsPrice
                           ,ReturnOrderAssessment
                           ,ReturnOrderPrice
                           ,ReceiptSigningNotMatch
                           ,ReceiptSigningNotMatchPrice
                           ,Remark
                           ,CreateUser
                           ,CreateDate
                           ,Modify
                           ,ModifyDate)
                     VALUES(
                            @FreightCalculationTableId
                           ,@OrderNo
                           ,@LateArrivalOfGoodsAssessment
                           ,@LateArrivalOfGoodsPrice
                           ,@ReturnOrderAssessment
                           ,@ReturnOrderPrice
                           ,@ReceiptSigningNotMatch
                           ,@ReceiptSigningNotMatchPrice
                           ,@Remark
                           ,@CreateUser
                           ,@CreateDate
                           ,@Modify
                           ,@ModifyDate
                           )";
            return DB<int>.Execute(sql, this) > 0;
        }


        /// <summary>
        /// 修改物流考核数据
        /// </summary>
        /// <returns></returns>
        public bool UpdateLogisticsKPI()
        {
            string sql = @"UPDATE [dbo].[HY_Logistics_KPI]
                           SET 
                               [LateArrivalOfGoodsAssessment] =@LateArrivalOfGoodsAssessment
                              ,[LateArrivalOfGoodsPrice] = @LateArrivalOfGoodsPrice
                              ,[ReturnOrderAssessment] =@ReturnOrderAssessment
                              ,[ReturnOrderPrice] = @ReturnOrderPrice
                              ,[ReceiptSigningNotMatch] = @ReceiptSigningNotMatch
                              ,[ReceiptSigningNotMatchPrice] = @ReceiptSigningNotMatchPrice
                              ,[Remark] = @Remark
                              ,[Modify] = @Modify
                              ,[ModifyDate] =@ModifyDate
                         WHERE FreightCalculationTableId=@I";
            return DB<int>.Execute(sql, this) > 0;
        }
        /// <summary>
        /// 删除物流考核信息
        /// </summary>
        /// <returns></returns>
        public bool DeleteLogisticsKPI(List<int> IdLST) 
        {
            string sql = @"DELETE [dbo].[HY_Logistics_KPI]
                         WHERE FreightCalculationTableId in @IDLST";
            return DB<int>.Execute(sql, new { IdLST= IdLST }) > 0;
        }

       
    }
}
