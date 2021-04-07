using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 外加剂运价底表
    /// </summary>
    public partial class HY_TransportFeeMWJJ
    {
        /// <summary>
        /// 获取外加剂运价底表信息分页数据
        /// </summary>
        /// <returns></returns>
        public List<HY_TransportFeeMWJJ> GetTransportFeeMWJJfoList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = "*";
            string tableName = "HY_TransportFeeMWJJ";
            return DB<HY_TransportFeeMWJJ>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }


        /// <summary>
        /// 获取外加剂运价底表信息
        /// </summary>
        /// <returns></returns>
        public HY_TransportFeeMWJJ GetById()
        {
            string sql = @"SELECT * from HY_TransportFeeMWJJ with(nolock) WHERE ID=@ID ";
            return DB<HY_TransportFeeMWJJ>.QuerySingleOrDefault(sql, new { ID= this.ID });
        }

        /// <summary>
        /// 新增外加剂运价底表信息
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"insert into [dbo].[HY_TransportFeeMWJJ]
                            (
                             NeedMaxQty, NeedMinQty, NeedMaxMiles, NeedMinMiles, FeeType, Price, LogisticsNo, LogisticsName, PlaceOfOrigin, CreateDate, CreateUser, Modify, ModifyDate
                            )
                            values
                            (
                             @NeedMaxQty, @NeedMinQty, @NeedMaxMiles, @NeedMinMiles, @FeeType, @Price, @LogisticsNo, @LogisticsName, @PlaceOfOrigin, @CreateDate, @CreateUser, @Modify, @ModifyDate
                            )";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 修改外加剂运价底表信息
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string sql = @"UPDATE HY_TransportFeeMWJJ SET NeedMaxQty=@NeedMaxQty, NeedMinQty=@NeedMinQty, NeedMaxMiles=@NeedMaxMiles
                           , NeedMinMiles=@NeedMinMiles, FeeType=@FeeType, Price=@Price, LogisticsNo=@LogisticsNo
                           , LogisticsName=@LogisticsName, PlaceOfOrigin=@PlaceOfOrigin, Modify=@Modify, ModifyDate=@ModifyDate WHERE ID=@ID;";
            return DB<int>.Execute(sql, this) > 0;
        }


        public HY_TransportFeeMWJJ GetWjjFee(string qty, string miles, string logistics, string origintransport)
        {
            string sql = @"select FeeType,Price from HY_TransportFeeMWJJ where @NeedMaxQty >NeedMaxQty  and @NeedMinQty <= NeedMinQty
                           and @NeedMaxMiles >NeedMaxMiles and @NeedMinMiles<=NeedMinMiles and LogisticsNo =@LogisticsNo and PlaceOfOrigin =@PlaceOfOrigin";
            return DB<HY_TransportFeeMWJJ>.QueryFirstOrDefault(sql, new { needmaxqty = qty, Needmaxmiles = miles, logistics = logistics, origintransport = origintransport });


        }
    }
}
