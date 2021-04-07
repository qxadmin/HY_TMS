using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 订单月度物流结算表
    /// </summary>
    public partial class HY_OrderFeeMonth
    {
        /// <summary>
        /// 根据结费单号修改结算价格
        /// </summary>
        /// <returns></returns>
        public bool UpdateSettmentPriceByMonthFeeOrderNo()
        {
            string sql = "update [dbo].[HY_OrderFeeMonth] set SettlementPrice=@SettlementPrice where MonthFeeOrderNo=@MonthFeeOrderNo";
            return DB<int>.Execute(sql, new { SettlementPrice = this.SettlementPrice, MonthFeeOrderNo = this.MonthFeeOrderNo }) > 0;
        }

        public bool Insert() 
        {
            string sql = @"insert into  [dbo].[HY_OrderFeeMonth]
                         (
                          FinancialYear, FinancialMonty, ForwarderNo, SettlementPrice, Billing, Paid, State, MonthFeeOrderNo, CreateDate, CreateUser, Modify, ModifyDate
                         )
                         values
                         (
                         @FinancialYear, @FinancialMonty, @ForwarderNo, @SettlementPrice, @Billing, @Paid, @State, @MonthFeeOrderNo, GETDATE(), @CreateUser, @Modify, GETDATE()
                         )";
            return DB<int>.Execute(sql, this) > 0;
        }
    }
}
