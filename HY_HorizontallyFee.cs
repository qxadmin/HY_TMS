using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 横放品费用维护
    /// </summary>
    public partial class HY_HorizontallyFee
    {
        /// <summary>
        /// 获取横放品费用维护分页数据
        /// </summary>
        /// <returns></returns>
        public List<HY_HorizontallyFee> GetHYHorizontallyFeeInfoList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = "*";
            string tableName = "HY_HorizontallyFee";
            return DB<HY_HorizontallyFee>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }


        /// <summary>
        /// 获取横放品费用维护信息
        /// </summary>
        /// <returns></returns>
        public HY_HorizontallyFee GetById()
        {
            string sql = @"SELECT * from HY_HorizontallyFee with(nolock) WHERE ID=@ID";
            return DB<HY_HorizontallyFee>.QuerySingleOrDefault(sql, new { ID = this.ID });
        }

        /// <summary>
        /// 新增横放品费用维护信息
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"insert into [dbo].[HY_HorizontallyFee]
                            (
                                FactoryNo,FactoryName,LogisticsNo,LogisticsName,HorizontallyProportion,CreateDate,CreateUser,Modify,ModifyDate
                            )
                            values
                            (
                                @FactoryNo,@FactoryName,@LogisticsNo,@LogisticsName,@HorizontallyProportion,@CreateDate,@CreateUser,@Modify,@ModifyDate
                            )";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 修改横放品费用维护信息
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string sql = @"UPDATE HY_HorizontallyFee SET FactoryNo=@FactoryNo,
                                FactoryName=@FactoryName,
	                            LogisticsNo=@LogisticsNo ,
	                            LogisticsName=@LogisticsName,
	                            HorizontallyProportion=@HorizontallyProportion ,
	                            Modify=@Modify,
	                            ModifyDate=@ModifyDate WHERE ID=@ID;";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 删除横放品费用维护信息
        /// </summary>
        /// <returns></returns>
        public bool DeleteById()
        {
            string sql = @"DELETE HY_HorizontallyFee WHERE ID=@ID;";
            return DB<int>.Execute(sql, new { ID = this.ID }) > 0;
        }
        /// <summary>
        /// 获取横放费用
        /// </summary>
        /// <returns></returns>
        public float GetHorizontallyFee()
        {
            string sql = string.Format("select HorizontallyProportion from HY_HorizontallyFee where FactoryNo =@FactoryNo and LogisticsNo =@LogisticsNo");
            return DB<float>.QueryFirstOrDefault(sql, new { FactoryNo = this.FactoryNo, LogisticsNo = this.LogisticsNo });
        }
    }
}
