using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Common
{
    public static class EntityExtension
    {
        /// <summary>
        /// 将List转换为DataTable
        /// </summary>
        /// <param name="list">请求数据</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(this List<T> list)
        {
            //创建一个名为"tableName"的空表
            DataTable dt = new DataTable("tableName");

            //创建传入对象名称的列
            foreach (var item in list.FirstOrDefault().GetType().GetProperties())
            {
                 dt.Columns.Add(item.Name);
            }
            //循环存储
            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                int j = 0;
                //通过遍历实体类型的属性和数据字段的名字来对应填充数据
                foreach (System.Reflection.PropertyInfo p in typeof(T).GetProperties())
                {
                    //先满足完全匹配的
                    if (dt.Columns.Contains(p.Name))//判断列是否存在
                    {
                        try
                        { row[p.Name] = p.GetValue(item, null); }
                        catch
                        { row[p.Name] = p.PropertyType.FullName.ToString(); }
                        j++;
                        if (j >= dt.Columns.Count)//表格填充列数已满则跳出循环
                        {
                            break;
                        }
                    }
                }
                dt.Rows.Add(row);
            }
            return dt;
        }


        /// <summary>
        /// 将实体集合转换为DataTable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体集合</param>
        public static DataTable ToDataTable<T>(this List<T> entities)
        {
            var result = CreateTable<T>();
            FillData(result, entities);
            return result;
        }

        /// <summary>
        /// 创建表
        /// </summary>
        private static DataTable CreateTable<T>()
        {
            var result = new DataTable();
            var type = typeof(T);
            foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                var propertyType = property.PropertyType;
                if ((propertyType.IsGenericType) && (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    propertyType = propertyType.GetGenericArguments()[0];
                result.Columns.Add(property.Name, propertyType);
            }
            return result;
        }
        /// <summary>
        /// 填充数据
        /// </summary>
        private static void FillData<T>(DataTable dt, IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                dt.Rows.Add(CreateRow(dt, entity));
            }
        }

        /// <summary>
        /// 创建行
        /// </summary>
        private static DataRow CreateRow<T>(DataTable dt, T entity)
        {
            DataRow row = dt.NewRow();
            var type = typeof(T);
            foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                row[property.Name] = property.GetValue(entity) ?? DBNull.Value;
            }
            return row;
        }
    }
}
