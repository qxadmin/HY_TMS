using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 投诉
    /// </summary>
    public partial class qx_OrderComplaint
    {
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <returns></returns>
        public bool IsExists() 
        {
            string sql = "select * from qx_OrderComplaint where Tmsorder =@Tmsorder and productNo = @productNo";
            var result=DB<qx_OrderComplaint>.QuerySingleOrDefault(sql, new { Tmsorder = this.Tmsorder, productNo=this.productNo });
            if (result == null) 
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"INSERT INTO qx_OrderComplaint
                    (Tmsorder,
                     Fromplant,
                     saporderno,
                     salesaccount,
                     saleofficedesc,
                     logistics,
                     endtime,
                     receiveconpanydesc,
                     tocontact,
                     toaddress,
                     ComplaintTime,
                     ComplaintRank,
                     ComplaintType,
                     productNo,
                     ComplaintNum,
                     ComplaintText,
                     ComplaintEnd,
                     createtime,
                     createuser,
                     punishment,
                     punishment_money,
                     Area)
        VALUES      ( @Tmsorder,
                      @Fromplant,
                      @saporderno,
                      @salesaccount,
                      @saleofficedesc,
                      @logistics,
                      @endtime,
                      @receiveconpanydesc,
                      @tocontact,
                      @toaddress,
                      @ComplaintTime,
                      @ComplaintRank,
                      @ComplaintType,
                      @productNo,
                      @ComplaintNum,
                      @ComplaintText,
                      @ComplaintEnd,
                      @createtime,
                      @createuser,
                      @punishment,
                      @punishment_money,
                      @Area)";
            return DB<int>.Execute(sql, this) > 0;
        }
    }
}
