using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_MILES
    {
        /// <summary>
        /// 获取距离
        /// </summary>
        /// <returns></returns>
        public int GetMILES() 
        {
            string sql = "select top 1 MILES from qx_MILES WIHT(NOLOCK) where city = @city and ractoryno=@ractoryno";
            return DB<int>.QueryFirstOrDefault(sql,new { city=this.city, ractoryno=this.ractoryno });
        }
    }
}
