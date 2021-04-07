using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 开关表
    /// </summary>
    public partial class qx_Switch
    {
        /// <summary>
        /// 获取开关值
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetSwitchValue(string code)
        {
            string sql = "SELECT Value From qx_Switch with(nolock) where Code=@Code";
            return DB<string>.QueryFirst(sql,new {Code= code });
        }
    }
}
