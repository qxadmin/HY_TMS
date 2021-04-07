using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_Specialcity_Maintain
    {
        /// <summary>
        /// 判定城市是否为特殊城市
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public bool IsSpecialCityByCity()
        {
            string sql = @"select 1 from qx_Specialcity_Maintain where city=@city";
            return DB<int>.QueryFirstOrDefault(sql, new { city=this.city }) > 0;
        }
    }
}
