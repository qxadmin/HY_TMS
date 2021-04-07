using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFYH.Common
{
    public static class AutoMapperHelper
    {
        /// <summary>
        /// 单个对象映射
        /// </summary>
        /// <typeparam name="TSource">源对象</typeparam>
        /// <typeparam name="TDestination">目标对象</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            if (source == null) return default(TDestination);
            return MapTo<TSource,TDestination>(source);
        }

        /// <summary>
        ///  集合列表类型映射  
        /// </summary>
        /// <typeparam name="TSource">源对象</typeparam>
        /// <typeparam name="TDestination">目标对象</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            if (source == null) return default(List<TDestination>);
            return MapToList<TSource, TDestination>(source);
        }


        /// <summary>
        ///  集合列表类型映射  
        /// </summary>
        /// <typeparam name="TSource">源对象</typeparam>
        /// <typeparam name="TDestination">目标对象</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination MapToDataTable<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            if (source == null) return default(TDestination);
            return MapToDataTable<TSource, TDestination>(source);
        }
    }
}
