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
    /// 城市距离维护
    /// </summary>
    public partial class HY_CityDistance
    {
        /// <summary>
        /// 获取城市距离分页数据
        /// </summary>
        /// <returns></returns>
        public List<HY_CityDistance> GetCityDistanceInfoList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = "*";
            string tableName = "HY_CityDistance";
            return DB<HY_CityDistance>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }


        /// <summary>
        /// 获取城市距离信息
        /// </summary>
        /// <returns></returns>
        public HY_CityDistance GetById()
        {
            string sql = @"SELECT * from HY_CityDistance with(nolock) WHERE ID=@ID";
            return DB<HY_CityDistance>.QuerySingleOrDefault(sql, new { ID = this.ID });
        }

        /// <summary>
        /// 新增城市距离信息
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"insert into [dbo].[HY_CityDistance]
                            (
                                [FactoryNo],
                                [FactoryName],
	                            [Province] ,
	                            [City],
	                            [Distance] ,
	                            [CreateDate] ,
	                            [CreateUser] ,
	                            [Modify],
	                            [ModifyDate]	
                            )
                            values
                            (
                                @FactoryNo,
                                @FactoryName,
	                            @Province ,
	                            @City,
	                            @Distance ,
	                            @CreateDate ,
	                            @CreateUser,
	                            @Modify,
	                            @ModifyDate
                            )";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 修改城市距离信息
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string sql = @"UPDATE HY_CityDistance SET FactoryNo=@FactoryNo,
                                FactoryName=@FactoryName,
	                            Province=@Province ,
	                            City=@City,
	                            Distance=@Distance ,
	                            CreateDate=@CreateDate,
	                            CreateUser=@CreateUser,
	                            Modify=@Modify,
	                            ModifyDate=@ModifyDate WHERE ID=@ID;";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 删除城市距离信息
        /// </summary>
        /// <returns></returns>
        public bool DeleteById()
        {
            string sql = @"DELETE HY_CityDistance WHERE ID=@ID;";
            return DB<int>.Execute(sql, new { ID = this.ID }) > 0;
        }

        /// <summary>
        /// 获取距离
        /// </summary>
        /// <returns></returns>
        public int GetMILES()
        {
            string sql = @"select Distance from HY_MilesWai where FactoryNo =@FactoryNo and Province = @Province and City =@City ";
            return DB<int>.QueryFirstOrDefault(sql, new
            {
                FactoryNo = this.FactoryNo,
                Province = this.Province,
                City = this.City
            });
        }
    }
}
