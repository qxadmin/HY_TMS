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
    /// 特殊城市
    /// </summary>
    public partial class HY_Specialcity_Maintain
    {
        /// <summary>
        /// 获取车辆信息分页数据
        /// </summary>
        /// <returns></returns>
        public List<HY_Specialcity_Maintain> GetSpecialcityMaintainList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = "*";
            string tableName = "HY_Specialcity_Maintain";
            List<Conditions> conditions = where.GetModel<List<Conditions>>();
            where = conditions.toSqlString();
            return DB<HY_Specialcity_Maintain>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }


        /// <summary>
        /// 获取特殊信息
        /// </summary>
        /// <returns></returns>
        public HY_Specialcity_Maintain GetById()
        {
            string sql = @"SELECT * from HY_Specialcity_Maintain with(nolock) WHERE ID=@ID ";
            return DB<HY_Specialcity_Maintain>.QuerySingleOrDefault(sql, new { ID = this.ID });
        }

        /// <summary>
        /// 新增车辆信息
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"insert into [dbo].[HY_Specialcity_Maintain]
                            (
                            [City],
	                        [CreateUser],
	                        [CreateDate],
	                        [Modify],
	                        [ModifyDate]
                            )
                            values
                            (
                             @City,
                             @CreateUser,
                             GETDATE(),
                             @Modify,
                             GETDATE()
                            )";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 修改特殊城市信息
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string sql = @"UPDATE HY_Specialcity_Maintain SET City=@City, Modify=@Modify, ModifyDate=GETDATE() WHERE Id=@Id;";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 删除特殊城市信息
        /// </summary>
        /// <returns></returns>
        public bool DeleteById()
        {
            string sql = @"DELETE HY_Specialcity_Maintain WHERE ID=@ID;";
            return DB<int>.Execute(sql, new { ID = this.ID }) > 0;
        }

        /// <summary>
        /// 判定城市是否为特殊城市
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public bool IsSpecialCityByCity()
        {
            string sql = @"select 1 from HY_Specialcity_Maintain where City=@City";
            return DB<int>.QueryFirstOrDefault(sql, new { City = this.City }) > 0;
        }
    }
}
