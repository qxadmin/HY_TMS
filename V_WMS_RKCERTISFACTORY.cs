using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 凭证工厂
    /// </summary>
    public partial class V_WMS_RKCERTISFACTORY
    {
        /// <summary>
        /// 获取原凭证号
        /// </summary>
        /// <param name="rkcertificateList">凭证号列表</param>
        public List<string> GetYRKCERTIFICATE(List<string> rkcertificateList) 
        {
            string strSql = string.Format("select yrkCertificate from [V_WMS_RKCERTISFACTORY]  WITH (NOLOCK) where rkCertificate in @rkcertificateList");
            return DB<string>.Query(strSql, new { rkcertificateList = rkcertificateList });
        }
    }
}
