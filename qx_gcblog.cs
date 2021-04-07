using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_GCBLog
    {
        /// <summary>
        /// 获取管车宝信息
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public qx_GCBLog GCBLogSingleInfo(string orderNo)
        {
            string sql = @"select top 1 area_name,img_url from qx_gcblog where phone = (
                            select drivertel from qx_driver where driverno = (
                            select top 1 DRIVER from qx_InAndOutBoundOrder where INORDERNO =@INORDERNO
                            )) order by CREATETIME desc";
            return DB<qx_GCBLog>.QueryFirstOrDefault(sql, new { INORDERNO = orderNo });
        }
    }
}
