using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using log4net;
using System.Configuration;

namespace TMS.Common
{
    public static class Log4netHelper
    {
        public static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void SetConfig()
        {
            object o = ConfigurationManager.GetSection("log4net");
            log4net.Config.XmlConfigurator.Configure(o as System.Xml.XmlElement);
        }

        public static void LogInfo(string Message)
        {
            if (!log.IsInfoEnabled)
                SetConfig();
            log.Info(Message);
        }

        public static void Error(string Message, Exception ex)
        {
            if (!log.IsErrorEnabled)
                SetConfig();
            log.Error(Message, ex);
        }
        public static void ErrorInfo(string Message)
        {
            if (!log.IsInfoEnabled)
                SetConfig();
            log.Error(Message);
        }

        public static void DebugInfo(string Message)
        {
            if (!log.IsInfoEnabled)
                SetConfig();
            log.Debug(Message);
        }
    }
}