using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class HY_FreightasDeterminepx
    {
        public int GetDangerousFoods()
        {
            string sql = "select ISNULL(Dangerous_goods,0) Dangerous_goods from HY_FreightasDeterminepx where FactoryNo = @FactoryNo and ForwarderNo =@ForwarderNo";
            return DB<int>.QueryFirstOrDefault(sql, new { FactoryNo = this.FactoryNo, ForwarderNo = this.ForwarderNo });
        }

        public int GetInsulationProducts()
        {
            string sql = "select ISNULL(Insulation_products,0) Insulation_products from HY_FreightasDeterminepx where FactoryNo = @FactoryNo and ForwarderNo =@ForwarderNo";
            return DB<int>.QueryFirstOrDefault(sql, new { FactoryNo = this.FactoryNo, ForwarderNo = this.ForwarderNo });
        }

        public HY_FreightasDeterminepx IsExistDangerousFoods()
        {
            string sql = "select Dangerous_goods Dangerous_goods from HY_FreightasDeterminepx where FactoryNo = @FactoryNo and ForwarderNo =@ForwarderNo";
            return DB<HY_FreightasDeterminepx>.QueryFirstOrDefault(sql, new { FactoryNo = this.FactoryNo, ForwarderNo = this.ForwarderNo });
        }

        public HY_FreightasDeterminepx IsExistInsulationProducts()
        {
            string sql = "select Insulation_products Insulation_products from HY_FreightasDeterminepx where FactoryNo = @FactoryNo and ForwarderNo =@ForwarderNo";
            return DB<HY_FreightasDeterminepx>.QueryFirstOrDefault(sql, new { FactoryNo = this.FactoryNo, ForwarderNo = this.ForwarderNo });
        }

        /// <summary>
        /// 获取特殊费用是否参与计算分页信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<HY_FreightasDeterminepx> GetFreightasDeterminepxLst(string where,string orderby,int pageIndex,int pageSize,out int total) 
        {
            string files = "a.FactoryNo,a.LogisticsNo as ForwarderNo,a.LogisticsName as ForwarderName,c.Dangerous_goods,Insulation_products,c.CreateDate,c.ModifyDate";
            string tableName = @"[dbo].[HY_LogistRelation] a
                                 left join[dbo].[HY_Forwarder] f on a.LogisticsNo = f.FORWARDERNO
                                 left join HY_FreightasDeterminepx c on a.FactoryNo=c.FactoryNo and a.LogisticsNo=c.ForwarderNo";
            return DB<HY_FreightasDeterminepx>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }


        /// <summary>
        /// 新增特殊费用是否参与计算
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"insert into HY_FreightasDeterminepx
                (
                   [FactoryNo]
                   ,[ForwarderNo]
                   ,[ForwarderName]
                   ,[CreateUser]
                   ,[CreateDate]
                   ,[Dangerous_goods]
                   ,[Insulation_products] 
                   ,[Modify]
                   ,[ModifyDate]
                )
                values
                (      
                    @FactoryNo
                   ,@ForwarderNo
                   ,@ForwarderName
                   ,@CreateUser
                   ,@CreateDate
                   ,@Dangerous_goods
                   ,@Insulation_products
                   ,@Modify
                   ,@ModifyDate
                )";
            return DB<int>.Execute(sql, this) > 0;
        }
        /// <summary>
        /// 修改危险品是否参与计算
        /// </summary>
        /// <returns></returns>
        public bool UpdateDangerousFoods()
        {
            string sql = "update HY_FreightasDeterminepx set Dangerous_goods=@Dangerous_goods,Modify=@Modify,ModifyDate=@ModifyDate where FactoryNo = @FactoryNo and ForwarderNo =@ForwarderNo";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 修改保温品是否参与计算
        /// </summary>
        /// <returns></returns>
        public bool UpdateInsulationProducts()
        {
            string sql = "update HY_FreightasDeterminepx set Insulation_products=@Insulation_products,Modify=@Modify,ModifyDate=@ModifyDate  where FactoryNo = @FactoryNo and ForwarderNo =@ForwarderNo";
            return DB<int>.Execute(sql, this) > 0;
        }

    }
}
