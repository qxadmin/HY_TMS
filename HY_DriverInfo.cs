using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 司机信息
    /// </summary>
    public partial class HY_DriverInfo
    {
        public string IsEnableName
        {
            get
            {
                if (this.IsEnable.HasValue)
                {
                    return this.IsEnable.Value == 1 ? "启用" : "停用";
                }
                else
                {
                    return "停用";
                }
            }
        }

        public string DriverSexName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.DriverSex))
                {
                    return this.DriverSex == "1" ? "男" : "女";
                }
                else
                {
                    return "";
                }
            }
        }


        /// <summary>
        /// 获取司机信息分页数据
        /// </summary>
        /// <returns></returns>
        public List<HY_DriverInfo> GetDriverInfoList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = "*";
            string tableName = "HY_DriverInfo";
            return DB<HY_DriverInfo>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }

        /// <summary>
        /// 获取司机信息
        /// </summary>
        /// <returns></returns>
        public HY_DriverInfo GetById()
        {
            string sql = @"SELECT * from HY_DriverInfo with(nolock) WHERE ID=@ID";
            return DB<HY_DriverInfo>.QuerySingleOrDefault(sql, new { ID = this.ID});
        }

        /// <summary>
        /// 根据承运商编号获取司机信息
        /// </summary>
        /// <param name="driVERNo">承运商编号</param>
        /// <returns></returns>
        public HY_DriverInfo GetDriver(string driVERNo)
        {
            string sql = "select DriverMobile,DriverIDCard,DriverNo,DriverName from HY_DriverInfo  where DriverNo=@DriverNo";
            return DB<HY_DriverInfo>.QueryFirstOrDefault(sql, new { DriverNo = driVERNo });
        }
        /// <summary>
        /// 获取司机信息
        /// </summary>
        /// <param name="prefixText">司机姓名</param>
        /// <param name="count">查询前多少条</param>
        /// <param name="contextKey">物流商</param>
        /// <returns></returns>
        public List<DictionaryModel> GetDriver(string prefixText, int count, string contextKey)
        {
            string strSql = string.Format("select top {0} DriverNo Code,DriverName value  from HY_DriverInfo where IsEnable=1 and DriverName like @DriverName and DriverCompanyCode=@DriverCompanyCode", count);
            return DB<DictionaryModel>.Query(strSql, new { DriverName = prefixText + "%", DriverCompanyCode = contextKey });
        }

        /// <summary>
        ///  是否黑名单司机
        /// </summary>
        /// <returns></returns>
        public bool IsBlackDriver()
        {
            string sql = "select isnull(IsEnable,0) IsEnable from HY_DriverInfo WHERE DriverIDCard=@DriverIDCard and DriverCompanyCode = @DriverCompanyCode "; /*and FORWARDERNO = @FORWARDERNO*/
            return DB<string>.QueryFirstOrDefault(sql, new { DriverIDCard = this.DriverIDCard, DriverCompanyCode = this.DriverCompanyCode }) == "0";
        }

        /// <summary>
        /// 新增司机信息
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"insert into [dbo].[HY_DriverInfo]
                            (
                             DriverNo, DriverName, DriverSex, DriverMobile, DriverCompanyCode, DriverCompanyName, DriverIDCard, EmploymentCertificateNo, DriverLicenseNo, IsEnable, CreateDate, Modify, ModifyDate
                            )
                            values
                            (
                             @DriverNo, @DriverName, @DriverSex, @DriverMobile, @DriverCompanyCode,@DriverCompanyName, @DriverIDCard
                            , @EmploymentCertificateNo, @DriverLicenseNo, @IsEnable, @CreateDate,@Modify, @ModifyDate
                            )";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 修改司机信息
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string sql = @"UPDATE HY_DriverInfo SET DriverNo=@DriverNo, DriverName=@DriverName, DriverSex=@DriverSex
                           , DriverMobile=@DriverMobile, DriverCompanyCode=@DriverCompanyCode,DriverCompanyName=@DriverCompanyName ,DriverIDCard=@DriverIDCard, EmploymentCertificateNo=@EmploymentCertificateNo
                           , DriverLicenseNo=@DriverLicenseNo, IsEnable=@IsEnable, Modify=@Modify, ModifyDate=@ModifyDate WHERE ID=@ID;";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 删除司机信息
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            string sql = @"DELETE HY_DriverInfo WHERE ID=@ID;";
            return DB<int>.Execute(sql, new { ID = this.ID }) > 0;
        }

        /// <summary>
        /// 禁用/启用车辆信息
        /// </summary>
        /// <returns></returns>
        public bool EnableDriver()
        {
            string sql = @"UPDATE HY_DriverInfo SET IsEnable=@IsEnable WHERE ID=@ID;";
            return DB<int>.Execute(sql, new { IsEnable = this.IsEnable, ID = this.ID }) > 0;
        }


        /// <summary>
        /// 物流商是否存在司机
        /// </summary>
        /// <returns></returns>
        public bool IsExistsByDriverCompanyCode()
        {
            string sql = "select count(1) from HY_DriverInfo WHERE DriverNo=@DriverNo and DriverCompanyCode=@DriverCompanyCode";
            return DB<int>.QuerySingleOrDefault(sql, this) > 0;
        }
    }
}
