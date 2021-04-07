using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 签收信息
    /// </summary>
    public partial class HY_Sign
    {
        /// <summary>
        /// 新增签收数据
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"INSERT INTO [dbo].[HY_HY_Sign]
                           (
                               FreightCalculationTableId, 
                               ActualReturnTime, 
                               ActualNum, 
                               ReceivingGoodsDeal, 
                               SignedPersonNotMatchDeal, 
                               ActualSigner,
                               ActualSignerIsMatch, 
                               LogisticsResponsibilityDefinition,
                               ActualSignerNotMatchReult,
                               Remark,
                               CreateUser, 
                               CreateDate, 
                               Modify,
                               ModifyDate
                           )
                     VALUES(
                               @FreightCalculationTableId, 
                               @ActualReturnTime, 
                               @ActualNum, 
                               @ReceivingGoodsDeal, 
                               @SignedPersonNotMatchDeal, 
                               @ActualSigner,
                               @ActualSignerIsMatch, 
                               @LogisticsResponsibilityDefinition,
                               @ActualSignerNotMatchReult,
                               @Remark,
                               @CreateUser, 
                               @CreateDate, 
                               @Modify,
                               @ModifyDate
                           )";
            return DB<int>.Execute(sql, this) > 0;
        }


        /// <summary>
        /// 修改物签收数据
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string sql = @"UPDATE [dbo].[HY_Sign]
                           SET 
                              FreightCalculationTableId=@FreightCalculationTableId,
                              ActualReturnTime=@ActualReturnTime,
                              ActualNum=@ActualNum, 
                              ReceivingGoodsDeal=@ReceivingGoodsDeal, 
                              SignedPersonNotMatchDeal=@SignedPersonNotMatchDeal,
                              ActualSigner=@ActualSigner, 
                              ActualSignerIsMatch=@ActualSignerIsMatch,
                              LogisticsResponsibilityDefinition=@LogisticsResponsibilityDefinition, 
                              ActualSignerNotMatchReult=@ActualSignerNotMatchReult, 
                              Remark=@Remark, 
                              CreateUser=@CreateUser,
                              CreateDate=@CreateDate,
                              Modify=@Modify,
                              ModifyDate=@ModifyDate
                         WHERE ID=@ID";
            return DB<int>.Execute(sql, this) > 0;
        }
        /// <summary>
        /// 删除签收数据
        /// </summary>
        /// <returns></returns>
        public bool Delete(List<int> IdLST)
        {
            string sql = @"DELETE [dbo].[HY_Sign]
                         WHERE FreightCalculationTableId in @IDLST";
            return DB<int>.Execute(sql, new { IdLST = IdLST }) > 0;
        }
    }
}
