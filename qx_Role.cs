using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_Role
    {
        /// <summary>
        /// 获取权限Id
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public string GetPermissionIds()
        {
            string sql = @"select permissionIds from qx_Role  where RoleId=@RoleId";
            return DB<string>.QueryFirst(sql, new { RoleId=this.ROLEID });
        }
    }
}
