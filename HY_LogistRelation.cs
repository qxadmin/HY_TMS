using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class HY_LogistRelation
    {

        /// <summary>
        /// 工厂编号和名称
        /// </summary>
        public string FactoryNoAndName 
        {
            get 
            {
                return this.FactoryNo +"/"+ this.FactoryName;
            }
        }

        public string EffectiveStartDateFormart 
        {
            get 
            {
                if (this.EffectiveStartDate != DateTime.MinValue) 
                {
                    return this.EffectiveStartDate.ToString("yyyy-MM-dd");
                }
                return "";
            }
        }
        public string EffectiveEndDateFormart
        {
            get
            {
                if (this.EffectiveEndDate != DateTime.MinValue)
                {
                    return this.EffectiveEndDate.ToString("yyyy-MM-dd");
                }
                return "";
            }
        }

        /// <summary>
        /// 序号
        /// </summary>
        public int RowNum { get;  }

        /// <summary>
        /// 获取运输商所属公司分页数据
        /// </summary>
        /// <returns></returns>
        public List<HY_LogistRelation> GetLogistRelationInfoList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = "*";
            string tableName = "HY_LogistRelation";
            return DB<HY_LogistRelation>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }


        /// <summary>
        /// 获取运输商所属公司信息
        /// </summary>
        /// <returns></returns>
        public HY_LogistRelation GetById()
        {
            string sql = @"SELECT * from HY_LogistRelation with(nolock) WHERE ID=@ID ";
            return DB<HY_LogistRelation>.QuerySingleOrDefault(sql, new { ID = this.ID });
        }

        /// <summary>
        /// 新增运输商所属公司信息
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"insert into [dbo].[HY_LogistRelation]
                            (
                             FactoryNo,
                             FactoryName, 
                             WarehouseNo,
                             WarehouseName, 
                             Province,
                             LogisticsNo,
                             LogisticsName, 
                             TransportTypeCode,
                             TransportTypeName,
                             EffectiveStartDate,
                             EffectiveEndDate,
                             CreateDate, 
                             CreateUser,
                             Modify, 
                             ModifyDate
                            )
                            values
                            (
                             @FactoryNo,
                             @FactoryName, 
                             @WarehouseNo,
                             @WarehouseName, 
                             @Province,
                             @LogisticsNo,
                             @LogisticsName, 
                             @TransportTypeCode,
                             @TransportTypeName,
                             @EffectiveStartDate,
                             @EffectiveEndDate,
                             @CreateDate, 
                             @CreateUser,
                             @Modify, 
                             @ModifyDate 
                            )";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 修改运输商所属公司信息
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string sql = @"UPDATE HY_LogistRelation SET FactoryNo=@FactoryNo,
                             FactoryName=@FactoryName, 
                             WarehouseNo=@WarehouseNo,
                             WarehouseName=@WarehouseName, 
                             Province=@Province,
                             LogisticsNo=@LogisticsNo,
                             LogisticsName=@LogisticsName, 
                             TransportTypeCode=@TransportTypeCode,
                             TransportTypeName=@TransportTypeName,
                             EffectiveStartDate=@EffectiveStartDate,
                             EffectiveEndDate=@EffectiveEndDate,
                             Modify=@Modify, 
                             ModifyDate=@ModifyDate WHERE ID=@ID;";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 删除运输商所属公司信息
        /// </summary>
        /// <returns></returns>
        public bool DeleteById()
        {
            string sql = @"DELETE HY_LogistRelation WHERE ID=@ID;";
            return DB<int>.Execute(sql, new { ID = this.ID }) > 0;
        }

        /// <summary>
        /// 查询运单发货工厂，物流商是否维护在运输商所属公司维护
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public HY_LogistRelation GetLogistRelationByOrderNo(string sapOrderNo)
        {
            string sql = @"select * from [dbo].[qx_InAndOutBoundOrder] a 
                          inner join[dbo].[HY_LogistRelation] b on b.FactoryNo = a.FROMPLANT and b.Province = a.TOPROVINCES
                          where[SAPORDERNO] =@SAPORDERNO";
            return DB<HY_LogistRelation>.QueryFirstOrDefault(sql,new { SAPORDERNO = sapOrderNo });
        }
    }
}
