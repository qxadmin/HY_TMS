using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public class UserData
    {
        /// <summary>
        /// ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 角色ID列表
        /// </summary>
        public List<int> Roles { get; set; }
    }
}
