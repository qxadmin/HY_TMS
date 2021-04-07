using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 外加剂距离维护
    /// </summary>
    public partial class HY_MilesWai
    {
        /// <summary>
        /// 获取外加剂距离维护信息分页数据
        /// </summary>
        /// <returns></returns>
        public List<HY_MilesWai> GetMilesWaiInfoList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = "*";
            string tableName = "HY_MilesWai";
            return DB<HY_MilesWai>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }


        /// <summary>
        /// 获取外加剂距离维护信息
        /// </summary>
        /// <returns></returns>
        public HY_MilesWai GetById()
        {
            string sql = @"SELECT * from HY_MilesWai with(nolock) WHERE ID=@ID ";
            return DB<HY_MilesWai>.QuerySingleOrDefault(sql, new { ID = this.ID });
        }

        /// <summary>
        /// 获取外加剂距离
        /// </summary>
        /// <returns></returns>
        public qx_MILES_wai GetMILES()
        {
            string sql = @"select Distance from HY_MilesWai where FactoryNo =@FactoryNo and Province = @Province 
                         and City =@City and CustomerName = @CustomerName and PlaceOfShipment = @PlaceOfShipment";
            return DB<qx_MILES_wai>.QueryFirstOrDefault(sql, new
            {
                ractoryno = this.FactoryNo,
                province = this.Province,
                city = this.City,
                Customerdesc = this.CustomerName,
                origintransport = this.PlaceOfShipment
            });
        }

        /// <summary>
        /// 新增外加剂距离维护信息
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"insert into [dbo].[HY_MilesWai]
                            (
                             FactoryNo, 
                             FactoryName, 
                             Province,
                             City, 
                             Distance,
                             AirLine,
                             DistanceToCustomers, 
                             CustomerName, 
                             PlaceOfShipment,
                             CreateDate,
                             CreateUser,
                             Modify,
                             ModifyDate
                            )
                            values
                            (
                              @FactoryNo, 
                              @FactoryName, 
                              @Province,
                              @City, 
                              @Distance,
                              @AirLine,
                              @DistanceToCustomers, 
                              @CustomerName, 
                              @PlaceOfShipment,
                              @CreateDate,
                              @CreateUser,
                              @Modify,
                              @ModifyDate
                            )";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 修改外加剂距离维护信息
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string sql = @"UPDATE HY_MilesWai SET  FactoryNo=@FactoryNo, 
                           FactoryName=@FactoryName, 
                           Province=@Province,
                           City=@City, 
                           Distance=@Distance,
                           AirLine=@AirLine,
                           DistanceToCustomers=@DistanceToCustomers, 
                           CustomerName=@CustomerName, 
                           PlaceOfShipment=@PlaceOfShipment,
                           Modify=@Modify,
                           ModifyDate=@ModifyDate WHERE Id=@Id;";
            return DB<int>.Execute(sql, this) > 0;
        }
    }
}
