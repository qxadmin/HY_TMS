using HY.TMS.Common;
using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 卸货派送费用价格维护
    /// </summary>
    public partial class HY_UnloadingTransshipment
    {
        /// <summary>
        /// 获取卸货派送费用信息分页数据
        /// </summary>
        /// <returns></returns>
        public List<HY_UnloadingTransshipment> GetUnloadingTransshipmentInfoList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = "*";
            string tableName = "HY_UnloadingTransshipment";
            return DB<HY_UnloadingTransshipment>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }


        /// <summary>
        /// 获取卸货派送费用信息
        /// </summary>
        /// <returns></returns>
        public HY_UnloadingTransshipment GetById()
        {
            string sql = @"SELECT * from HY_UnloadingTransshipment with(nolock) WHERE ID=@ID";
            return DB<HY_UnloadingTransshipment>.QuerySingleOrDefault(sql, new { ID = this.ID });
        }

        /// <summary>
        /// 新增卸货派送费用信息
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"insert into [dbo].[HY_UnloadingTransshipment]
                            (
                                FactoryNo, FactoryName, LogisticsNo, LogisticsName, SpecialCityFee, OrdinaryCityFee, MixDischargeFee, MaxDischargeFee, CreateDate, CreateUser, Modify, ModifyDate
                            )
                            values
                            (
                               @FactoryNo, @FactoryName, @LogisticsNo,@LogisticsName, @SpecialCityFee, @OrdinaryCityFee, @MixDischargeFee, @MaxDischargeFee, @CreateDate
                               , @CreateUser, @Modify, @ModifyDate
                            )";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 修改卸货派送费用信息
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string sql = @"UPDATE HY_UnloadingTransshipment SET FactoryNo=@FactoryNo,
                                FactoryName=@FactoryName,
	                            LogisticsNo=@LogisticsNo ,
	                            LogisticsName=@LogisticsName,
	                            SpecialCityFee=@SpecialCityFee ,
	                            OrdinaryCityFee=@OrdinaryCityFee ,
	                            MixDischargeFee=@MixDischargeFee,
	                            MaxDischargeFee=@MaxDischargeFee,
	                            Modify=@Modify,
	                            ModifyDate=@ModifyDate WHERE ID=@ID;";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 删除卸货派送费用信息
        /// </summary>
        /// <returns></returns>
        public bool DeleteById()
        {
            string sql = @"DELETE HY_UnloadingTransshipment WHERE ID=@ID;";
            return DB<int>.Execute(sql, new { ID = this.ID }) > 0;
        }

        public qx_Unloading_transshipment getDischargeFee()
        {
            string sql = @"select MixDischargeFee,MaxDischargeFee from [dbo].[HY_UnloadingTransshipment] where FactoryNo = @FactoryNo and LogisticsNo = @LogisticsNo";
            return DB<qx_Unloading_transshipment>.QueryFirstOrDefault(sql, new { FactoryNo = this.FactoryNo, LogisticsNo = this.LogisticsNo });
        }
    }
}
