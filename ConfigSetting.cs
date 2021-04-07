using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Common
{
    public static class ConfigSetting
    {


        /// <summary>
        /// 获取AppSetting配置的连接字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static T GetConnectionString<T>(T defaultValue, string key)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[key];
            if (!string.IsNullOrEmpty(connectionString.ConnectionString))
            {
                try
                {
                    defaultValue = (T)Convert.ChangeType(connectionString.ConnectionString, typeof(T));
                }
                catch
                {
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取AppSetting里面的内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static T GetAppSettingValue<T>(T defaultValue, string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    defaultValue = (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取指定的Config文件的AppSetting里面的指定键值对应的value值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="key">The key.</param>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static T GetAppSettingValue<T>(T defaultValue, string key, string file)
        {
            var map = new ExeConfigurationFileMap
            {
                ExeConfigFilename = file
            };
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            string value = config.AppSettings.Settings[key].Value;

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    defaultValue = (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                }
            }
            return defaultValue;
        }
    }
}
