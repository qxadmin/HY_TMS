using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HY.TMS.Common
{
    public static class JSONExtension
    {
        public static string ToJSON(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(obj);
        }

        public static T GetModel<T>(this string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Include;
            settings.Formatting = Formatting.None;
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}