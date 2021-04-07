using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 车牌号信息
    /// </summary>
    public partial class qx_vehicleinfo
    {
        /// <summary>
        /// 获取车牌号信息
        /// </summary>
        /// <param name="prefixText">车牌号</param>
        /// <param name="count">查询前多少条</param>
        /// <param name="forwarderno">物流商编号</param>
        public List<DictionaryModel> GetCARNUMBER(string prefixText, int count, string contextKey)
        {
            string strSql = string.Format("select top 20 VEHICLENUM value,VEHICLEID Code  from qx_vehicleinfo where STATUS=1 and VEHICLENUM like @VEHICLENUM"); //and forwarderno = @forwarderno    ,  forwarderno= contextKey
            return DB<DictionaryModel>.Query(strSql, new { VEHICLENUM = prefixText + "%" });
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsEists()
        {
            string sql = "select count(1) from qx_vehicleinfo where VEHICLENUM = @VEHICLENUM and FORWARDERNO = @FORWARDERNO";
            return DB<int>.QuerySingleOrDefault(sql, this) > 0;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"insert into qx_vehicleinfo
                          (
                            VEHICLENUM,FORWARDERNO,CREATETIME,CREATEUSER
                          )
                          values(@VEHICLENUM,@FORWARDERNO,@CREATETIME,@CREATEUSER)";
            return DB<int>.Execute(sql, this) > 0;
        }
    }
}
