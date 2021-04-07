using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Common
{
    /// <summary>
    /// 异常信息显示
    /// </summary>
    public class MvcException
    {
        /// <summary>
        /// 是否已经获取的允许显示异常
        /// </summary>
        private static bool HasGetExceptionEnabled = false;

        private static bool isExceptionEnabled;

        /// <summary>
        /// 是否显示异常信息
        /// </summary>
        /// <returns>是否显示异常信息</returns>
        public static bool IsExceptionEnabled()
        {
            if (!HasGetExceptionEnabled)
            {
                isExceptionEnabled = GetExceptionEnabled();
                HasGetExceptionEnabled = true;
            }
            return isExceptionEnabled;
        }

        /// <summary>
        /// 根据Web.config AppSettings节点下的ExceptionEnabled值来决定是否显示异常信息
        /// </summary>
        /// <returns></returns>
        private static bool GetExceptionEnabled()
        {
            bool result;
            if (!Boolean.TryParse(ConfigurationManager.AppSettings["ExceptionEnabled"], out result))
            {
                return false;
            }
            return result;
        }
    }
}
