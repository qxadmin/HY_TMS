using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class V_FACTORY
    {
        /// <summary>
        /// 获取工厂信息
        /// </summary>
        /// <returns></returns>
        public List<DictionaryModel> GetFactory()
        {
            string where = " 1=1";
            if (qx_User.CurrentUser != null && (qx_User.CurrentUser.UTYPE.HasValue && qx_User.CurrentUser.UTYPE.Value != -1) 
                && qx_User.CurrentUser.FACTORYNO != "" && qx_User.CurrentUser.FACTORYNO != "-1")
            {
                where += " and factoryNo='" + qx_User.CurrentUser.FACTORYNO + "'";
            }
            string sql = "select factoryNo Code,factoryName value from [dbo].[V_FACTORY] WHERE " + where;
            return DB<DictionaryModel>.Query(sql, this);
        }
    }
}
