using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HY.TMS.Common
{
    public static class sqlBulkCopyHelper
    {
        /// <summary>
        /// SqlBulkCopy 批量更新数据
        /// </summary>
        /// <param name="dataTable">数据集</param>
        /// <param name="crateTemplateSql">临时表创建字段</param>
        /// <param name="updateSql">更新语句</param>
        public static void BulkUpdateData(DataTable dataTable, string crateTemplateSql, string updateSql)
        {
            using (var conn = new SqlConnection(ConfigSetting.GetConnectionString(default(string), "ConnectionString")))
            {
                using (var command = new SqlCommand("", conn))
                {
                    conn.Open();
                    //数据库并创建一个临时表来保存数据表的数据
                    command.CommandText = String.Format("  CREATE TABLE #TmpTable ({0})", crateTemplateSql);
                    command.ExecuteNonQuery();

                    //使用SqlBulkCopy 加载数据到临时表中
                    using (var bulkCopy = new SqlBulkCopy(conn))
                    {
                        foreach (DataColumn dcPrepped in dataTable.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(dcPrepped.ColumnName, dcPrepped.ColumnName);
                        }

                        bulkCopy.BulkCopyTimeout = 660;
                        bulkCopy.BatchSize = 2000;
                        bulkCopy.DestinationTableName = "#TmpTable";
                        bulkCopy.WriteToServer(dataTable);
                        bulkCopy.Close();
                    }
                    // 执行Command命令 使用临时表的数据去更新目标表中的数据  然后删除临时表
                    command.CommandTimeout = 300;
                    command.CommandText = updateSql;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}