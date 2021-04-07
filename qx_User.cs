using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HY.TMS.ORM;
namespace HY.TMS.Model
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public partial class qx_User
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        public static qx_User CurrentUser { get; set; }


        public static qx_User GetUser(string uid) 
        {
            string sql =string.Format("select * from qx_User with(nolock) where uid=@uid");
            return DB<qx_User>.QuerySingleOrDefault(sql, new { uid= uid });
        }
    }
}
