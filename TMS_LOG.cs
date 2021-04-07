using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class TMS_LOG
    {
        /// <summary>
        /// 增加TMS日志
        /// </summary>
        /// <param name="logcontent">日志内容</param>
        /// <param name="logmodel">日志模块</param>
        /// <param name="user">操作用户</param>
        /// <returns></returns>
        public static bool AddLOG(string logcontent,string logmodel,string user) 
        {
            string sql = "insert into TMS_LOG(logcontent,logmodel,createtime,opuser)values(@logcontent,@logmodel,getdate(),@opuser)";
            return DB<int>.Execute(sql,new { logcontent= logcontent, logmodel= logmodel, opuser= user }) > 0;
        }

        /// <summary>
        /// 增加TMS日志
        /// </summary>
        /// <param name="logcontent">日志内容</param>
        /// <param name="logmodel">日志模块</param>
        /// <param name="user">操作用户</param>
        /// <returns></returns>
        public static bool AddLOG(string logcontent, string logmodel, string user,string modular)
        {
            string sql = "insert into TMS_LOG(logcontent,logmodel,createtime,opuser,modular)values(@logcontent,@logmodel,getdate(),@opuser,@modular)";
            return DB<int>.Execute(sql, new { logcontent = logcontent, logmodel = logmodel, opuser = user, modular= modular }) > 0;
        }
    }
}
