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
    /// 特殊费用
    /// </summary>
    public partial class HY_SpecialFee
    {
        /// <summary>
        /// 获取特殊费用信息分页数据
        /// </summary>
        /// <returns></returns>
        public List<HY_SpecialFee> GetSpecialFeeInfoList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = "*";
            string tableName = "HY_SpecialFee";
            List<Conditions> conditions = where.GetModel<List<Conditions>>();
            where = conditions.toSqlString();
            if (qx_User.CurrentUser != null && !string.IsNullOrWhiteSpace(qx_User.CurrentUser.FACTORYNO) && qx_User.CurrentUser.FACTORYNO != "-1")
            {
                where += " and FactoryNo='" + qx_User.CurrentUser.FACTORYNO+"'";
            }
            return DB<HY_SpecialFee>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }


        /// <summary>
        /// 获取特殊费用信息
        /// </summary>
        /// <returns></returns>
        public HY_SpecialFee GetById()
        {
            string sql = @"SELECT * from HY_SpecialFee with(nolock) WHERE ID=@ID";
            return DB<HY_SpecialFee>.QuerySingleOrDefault(sql, new { ID = this.ID });
        }

        /// <summary>
        /// 新增特殊费用信息
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"insert into [dbo].[HY_SpecialFee]
                            (
                                [FactoryNo],
                                [FactoryName],
	                            [LogisticsNo] ,
	                            [LogisticsName],
	                            [Danger_One] ,
	                            [Danger_Two] ,
	                            [Danger_Three] ,
	                            [Danger_Four] ,
	                            [Danger_Five] ,
	                            [Preservation_One] ,
	                            [Preservation_Two] ,
	                            [Preservation_Three],
	                            [Preservation_Four] ,
	                            [Preservation_Five] ,
	                            [CreateDate],
	                            [CreateUser] ,
	                            Modify,
	                            ModifyDate	
                            )
                            values
                            (
                                @FactoryNo,
                                @FactoryName,
	                            @LogisticsNo ,
	                            @LogisticsName,
	                            @Danger_One ,
	                            @Danger_Two ,
	                            @Danger_Three,
	                            @Danger_Four,
	                            @Danger_Five,
	                            @Preservation_One,
	                            @Preservation_Two,
	                            @Preservation_Three,
	                            @Preservation_Four,
	                            @Preservation_Five,
	                            GETDATE(),
	                            @CreateUser,
	                            @Modify,
	                            GETDATE()	
                            )";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 修改特殊费用信息
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string sql = @"UPDATE HY_SpecialFee SET FactoryNo=@FactoryNo,
                                FactoryName=@FactoryName,
	                            LogisticsNo=@LogisticsNo ,
	                            LogisticsName=@LogisticsName,
	                            Danger_One=@Danger_One ,
	                            Danger_Two=@Danger_Two ,
	                            Danger_Three=@Danger_Three,
	                            Danger_Four=@Danger_Four,
	                            Danger_Five=@Danger_Five,
	                            Preservation_One=@Preservation_One,
	                            Preservation_Two=@Preservation_Two,
	                            Preservation_Three=@Preservation_Three,
	                            Preservation_Four=@Preservation_Four,
	                            Preservation_Five=@Preservation_Five,
	                            Modify=@Modify,
	                            ModifyDate=GETDATE() WHERE ID=@ID;";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 删除特殊费用信息
        /// </summary>
        /// <returns></returns>
        public bool DeleteById()
        {
            string sql = @"DELETE HY_SpecialFee WHERE ID=@ID;";
            return DB<int>.Execute(sql, new { ID = this.ID }) > 0;
        }


        /// <summary>
        /// 获取危险品和保温品上浮比例
        /// </summary>
        /// <param name="weinum">重量标识</param>
        /// <param name="type">1.危险品；2.保温品</param>
        /// <returns></returns>
        public float GetSpecialFee(string weinum, string type)
        {
            float num = 0;
            string sql = "select * from HY_SpecialFee where FactoryNo =@FactoryNo and LogisticsNo =@LogisticsNo";
            var specialFee = DB<HY_SpecialFee>.QueryFirstOrDefault(sql, new { FactoryNo = this.FactoryNo, LogisticsNo = this.LogisticsNo });
            if (type == "1")
            {
                if (weinum == "1")
                {
                    num = specialFee.Danger_One.HasValue ? float.Parse(specialFee.Danger_One.Value.ToString()) : 0;
                }
                else if (weinum == "2")
                {
                    num = specialFee.Danger_Two.HasValue ? float.Parse(specialFee.Danger_Two.Value.ToString()) : 0;
                }
                else if (weinum == "3")
                {
                    num = specialFee.Danger_Three.HasValue ? float.Parse(specialFee.Danger_Three.Value.ToString()) : 0;
                }
                else if (weinum == "4")
                {
                    num = specialFee.Danger_Four.HasValue ? float.Parse(specialFee.Danger_Four.Value.ToString()) : 0;
                }
                else if (weinum == "5")
                {
                    num = specialFee.Danger_Five.HasValue ? float.Parse(specialFee.Danger_Five.Value.ToString()) : 0;
                }
            }
            else
            {
                if (weinum == "1")
                {
                    num = specialFee.Preservation_One.HasValue ? float.Parse(specialFee.Preservation_One.Value.ToString()) : 0;
                }
                else if (weinum == "2")
                {
                    num = specialFee.Preservation_Two.HasValue ? float.Parse(specialFee.Preservation_Two.Value.ToString()) : 0;
                }
                else if (weinum == "3")
                {
                    num = specialFee.Preservation_Three.HasValue ? float.Parse(specialFee.Preservation_Three.Value.ToString()) : 0;
                }
                else if (weinum == "4")
                {
                    num = specialFee.Preservation_Four.HasValue ? float.Parse(specialFee.Preservation_Four.Value.ToString()) : 0;
                }
                else if (weinum == "5")
                {
                    num = specialFee.Preservation_Five.HasValue ? float.Parse(specialFee.Preservation_Five.Value.ToString()) : 0;
                }
            }
            return num;
        }
    }
}
