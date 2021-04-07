using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 车辆信息
    /// </summary>
    public partial class HY_CarInfo
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

        /// <summary>
        /// 获取车辆信息分页数据
        /// </summary>
        /// <returns></returns>
        public List<HY_CarInfo> GetCarInfoList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = "*";
            string tableName = "HY_CarInfo";
            return DB<HY_CarInfo>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }


        /// <summary>
        /// 获取车辆信息
        /// </summary>
        /// <returns></returns>
        public HY_CarInfo GetById()
        {
            string sql = @"SELECT * from HY_CarInfo with(nolock) WHERE ID=@ID ";
            return DB<HY_CarInfo>.QuerySingleOrDefault(sql, new { ID = this.ID });
        }

        /// <summary>
        /// 新增车辆信息
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"insert into [dbo].[HY_CarInfo]
                            (
                             CarNumber,
                             TransportationCompanyCode,
                             TransportationCompanyName,
                             DriverLicense,
                             CarInsuranceCertificateNo,
                             CarSizeLong,
                             CarSizeWeight,
                             CarSizeHeight,
                             VerificationQuality,
                             CarIamge,
                             CreateUser,
                             CreateDate,
                             Modify,
                             ModifyDate
                            )
                            values
                            (
                             @CarNumber,
                             @TransportationCompanyCode,
                             @TransportationCompanyName,
                             @DriverLicense,
                             @CarInsuranceCertificateNo,
                             @CarSizeLong,
                             @CarSizeWeight,
                             @CarSizeHeight,
                             @VerificationQuality,
                             @CarIamge,
                             @CreateUser,
                             @CreateDate,
                             @Modify,
                             @ModifyDate
                            )";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 修改车辆信息
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string sql = @"UPDATE HY_CarInfo SET CarNumber=@CarNumber, TransportationCompanyCode=@TransportationCompanyCode, TransportationCompanyName=@TransportationCompanyName
                           , DriverLicense=@DriverLicense, CarInsuranceCertificateNo=@CarInsuranceCertificateNo, CarSizeLong=@CarSizeLong, CarSizeWeight=@CarSizeWeight
                           , CarSizeHeight=@CarSizeHeight, VerificationQuality=@VerificationQuality, CarIamge=@CarIamge, Modify=@Modify, ModifyDate=@ModifyDate WHERE ID=@ID;";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 删除车辆信息
        /// </summary>
        /// <returns></returns>
        public bool DeleteById()
        {
            string sql = @"DELETE HY_CarInfo WHERE ID=@ID;";
            return DB<int>.Execute(sql, new { ID = this.ID }) > 0;
        }

        /// <summary>
        /// 禁用/启用车辆信息
        /// </summary>
        /// <returns></returns>
        public bool EnableCarById()
        {
            string sql = @"UPDATE HY_CarInfo SET IsEnable=@IsEnable WHERE ID=@ID;";
            return DB<int>.Execute(sql, new { IsEnable = this.IsEnable, ID = this.ID }) > 0;
        }

        /// <summary>
        /// 获取车牌号信息
        /// </summary>
        /// <param name="prefixText">车牌号</param>
        /// <param name="count">查询前多少条</param>
        /// <param name="forwarderno">物流商编号</param>
        public List<DictionaryModel> GetCARNUMBER(string prefixText, int count, string contextKey)
        {
            string strSql = string.Format("select top 20 CarNumber value,ID Code  from HY_CarInfo where IsEnable=1 and CarNumber like @CarNumber"); //and forwarderno = @forwarderno    ,  forwarderno= contextKey
            return DB<DictionaryModel>.Query(strSql, new { CarNumber = prefixText + "%" });
        }


        /// <summary>
        /// 物流商是否存在车辆
        /// </summary>
        /// <returns></returns>
        public bool IsExistsByTransportationCompanyCode()
        {
            string sql = "select count(1) from HY_CarInfo WHERE CarNumber=@CarNumber and TransportationCompanyCode=@TransportationCompanyCode";
            return DB<int>.QuerySingleOrDefault(sql, this) > 0;
        }
    }
}
