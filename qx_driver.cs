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
    public partial class qx_driver
    {
        /// <summary>
        /// 根据承运商编号获取司机信息
        /// </summary>
        /// <param name="driVERNo">承运商编号</param>
        /// <returns></returns>
        public qx_driver GetDriver(string driVERNo)
        {
            string sql = "select DRIVERTEL,DRIVERIDCARD,DRIVERNO,DRIVERNAME from qx_driver where DRIVERNo=@DRIVERNo";
            return DB<qx_driver>.QueryFirst(sql, new { DRIVERNo = driVERNo });
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
            string strSql = string.Format("select top {0} DRIVERNO Code,DRIVERNAME value  from qx_driver where STATUS=1 and DRIVERNAME like @DRIVERNAME and FORWARDERNO=@FORWARDERNO", count);
            return DB<DictionaryModel>.Query(strSql, new { DRIVERNAME = prefixText + "%", FORWARDERNO = contextKey });
        }

        /// <summary>
        ///  是否黑名单司机
        /// </summary>
        /// <returns></returns>
        public bool IsBlackDriver()
        {
            string sql = "select isnull(STATUS,0) STATUS from qx_driver WHERE DRIVERIDCARD=@DRIVERIDCARD and FORWARDERNO = @FORWARDERNO "; /*and FORWARDERNO = @FORWARDERNO*/
            return DB<string>.QueryFirstOrDefault(sql, new { DRIVERIDCARD=this.DRIVERIDCARD, FORWARDERNO = this.FORWARDERNO}) == "0";
        }


        /// <summary>
        /// 是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsEists()
        {
            string sql = "select Count(1) from qx_driver where DRIVERNO=@DRIVERNO and FORWARDERNO=@FORWARDERNO";
            return DB<int>.QuerySingleOrDefault(sql, this) > 0;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"insert into qx_driver
                          (
                            DRIVERNO, DRIVERNAME, DRIVERTEL, DRIVERIDCARD, DRIVERSTATE,FORWARDERNO
                          )
                          values(@DRIVERNO, @DRIVERNAME, @DRIVERTEL, @DRIVERIDCARD, @DRIVERSTATE,@FORWARDERNO)";
            return DB<int>.Execute(sql, this) > 0;
        }

    }
}
