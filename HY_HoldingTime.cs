using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 保温品时间维护
    /// </summary>
    public partial class HY_HoldingTime
    {
        public int RowNum { get; set; }


        /// <summary>
        /// 获取保温品时间维护分页数据
        /// </summary>
        /// <returns></returns>
        public List<HY_HoldingTime> GetHoldingTimeInfoList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = "*";
            string tableName = "HY_HoldingTime";
            return DB<HY_HoldingTime>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }


        /// <summary>
        /// 修改保温品时间维护信息
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string sql = @"UPDATE HY_HoldingTime SET FirstTime=@FirstTime,LastTime=@LastTime,Modify=@Modify,ModifyDate=@ModifyDate WHERE FactoryNo=@FactoryNo;";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 是否有保温时间
        /// </summary>
        /// <returns></returns>
        public bool IsHaveHoldingTime()
        {
            string sql = @"select * from HY_HoldingTime where FirstTime<=@FirstTime and LastTime>=@FirstTime and FactoryNo=@FactoryNo";
            return DB<HY_HoldingTime>.QueryFirstOrDefault(sql, this) !=null;
        }


    }
}
