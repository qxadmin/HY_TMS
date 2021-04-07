using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 结算表
    /// </summary>
    public partial class HY_SettlementPrice
    {
        public bool Insert()
        {
            string sql = @"INSERT INTO [dbo].[HY_SettlementPrice]
           ([OrderNo]
           ,[ItemId]
           ,[TansportPriceTableId]
           ,[SettlementPrice]
           ,[SettlementFormulaDesc]
           ,[DischargeCargoPrice]
           ,[OtherPrice]
           ,[UrgentPrice]
           ,[DistributionPrice]
           ,[TransportPrice]
           ,[FinancialExaminePrice]
           ,[HYConfirmStatus]
           ,[AuditStatus]
           ,[DangerousPrice]
           ,[DangerousWeight]
           ,[DangerousUpRate]
           ,[DangerousSumPrice]
           ,[KeepWarmPrice]
           ,[KeepWarmWeight]
           ,[KeepWarmUpRate]
           ,[KeepWarmSumPrice]
           ,[HorizontalPrice]
           ,[HorizontalWeight]
           ,[HorizontalUpRate]
           ,[HorizontalSumPrice]
           ,[SumPrice]
           ,[CreateDate]
           ,[Modify]
           ,[ModifyDate])
     VALUES
           (@OrderNo,
            @ItemId,
            @TansportPriceTableId,
            @SettlementPrice,
            @SettlementFormulaDesc,
            @DischargeCargoPrice,
            @OtherPrice,
            @UrgentPrice,
            @DistributionPrice,
            @TransportPrice,
            @FinancialExaminePrice,
            @HYConfirmStatus,
            @AuditStatus,
            @DangerousPrice,
            @DangerousWeight,
            @DangerousUpRate,
            @DangerousSumPrice,
            @KeepWarmPrice,
            @KeepWarmWeight,
            @KeepWarmUpRate,
            @KeepWarmSumPrice,
            @HorizontalPrice,
            @HorizontalWeight,
            @HorizontalUpRate,
            @HorizontalSumPrice,
            @SumPrice,
            GETDATE(),
            @Modify,
            GETDATE())";
            return DB<int>.Execute(sql, this) > 0;
        }

        public bool Update()
        {
            string sql = @"update HY_SettlementPrice set SettlementPrice=@SettlementPrice,SettlementFormulaDesc=@SettlementFormulaDesc,DischargeCargoPrice=@DischargeCargoPrice,
                            OtherPrice=@OtherPrice,UrgentPrice=@UrgentPrice,DistributionPrice=@DistributionPrice,TransportPrice=@TransportPrice,FinancialExaminePrice=@FinancialExaminePrice
                            ,DangerousPrice=@DangerousPrice,DangerousWeight=@DangerousWeight,DangerousUpRate=@DangerousUpRate,DangerousSumPrice=@DangerousSumPrice,KeepWarmPrice=@KeepWarmPrice,
                            KeepWarmWeight=@KeepWarmWeight,KeepWarmUpRate=@KeepWarmUpRate,KeepWarmSumPrice=@KeepWarmSumPrice,HorizontalPrice=@HorizontalPrice,HorizontalWeight=@HorizontalWeight,
                            HorizontalUpRate=@HorizontalUpRate,HorizontalSumPrice=@HorizontalSumPrice,SumPrice=@SumPrice,Modify=@Modify,ModifyDate=GETDATE()
                            where ItemId=@ItemId";
            return DB<int>.Execute(sql, this) > 0;
        }

        public bool Delete(List<int> itemIdLst)
        {
            string sql = @" delete [dbo].[HY_SettlementPrice] where ItemId in @ItemIdLst"; /*exists(select 1 from[dbo].[HY_FreightCalculationTable] where[SettlementNo] in @SettlementNoLst and id = ItemId)*/
            return DB<int>.Execute(sql, new { ItemIdLst = itemIdLst }) > 0;
        }
    }
}
