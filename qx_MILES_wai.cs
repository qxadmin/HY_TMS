using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_MILES_wai
    {
        /// <summary>
        /// 获取距离
        /// </summary>
        /// <returns></returns>
        public qx_MILES_wai GetMILES()
        {
            string sql = "select MILES from qx_MILES_wai where ractoryno =@ractoryno and province = @province and city =@city and Customerdesc =@Customerdesc and origintransport =@origintransport";
            return DB<qx_MILES_wai>.QueryFirstOrDefault(sql,new { ractoryno=this.ractoryno, province=this.province,city=this.city
                , Customerdesc=this.Customerdesc, origintransport=this.origintransport });
        }
    }
}
