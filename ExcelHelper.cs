using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HY.TMS.Common
{
    /// <summary>
    /// excel帮助类
    /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        /// excel文件流转化成datatable
        /// </summary>
        public static DataTable ExcelToTableForXLSX(Stream fileStream, bool haveNote = false)
        {
            var dt = new DataTable();
            using (var fs = fileStream)
            {
                var xssfworkbook = new XSSFWorkbook(fs);
                var sheet = xssfworkbook.GetSheetAt(0);
                //表头  判断是否包含备注
                var firstRowNum = sheet.FirstRowNum;
                if (haveNote)
                {
                    firstRowNum += 1;
                }
                var header = sheet.GetRow(firstRowNum);
                var columns = new List<int>();
                for (var i = 0; i < header.LastCellNum; i++)
                {
                    var obj = GetValueTypeForXLSX(header.GetCell(i) as XSSFCell);
                    if (obj == null || obj.ToString() == string.Empty)
                    {
                        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                        //continue;
                    }
                    else
                        dt.Columns.Add(new DataColumn(obj.ToString()));
                    columns.Add(i);
                }
                //数据
                for (var i = firstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    var dr = dt.NewRow();
                    var hasValue = false;
                    if (sheet.GetRow(i) == null)
                    {
                        continue;
                    }
                    foreach (var j in columns)
                    {
                        var cell = sheet.GetRow(i).GetCell(j);
                        if (cell != null && cell.CellType == CellType.Numeric)
                        {
                            //NPOI中数字和日期都是NUMERIC类型的，这里对其进行判断是否是日期类型
                            if (DateUtil.IsCellDateFormatted(cell)) //日期类型
                            {
                                dr[j] = cell.DateCellValue;
                            }
                            else //其他数字类型
                            {
                                dr[j] = cell.NumericCellValue;
                            }
                        }
                        else
                        {
                            dr[j] = GetValueTypeForXLSX(sheet.GetRow(i).GetCell(j) as XSSFCell);
                        }
                        if (dr[j] != null && dr[j].ToString() != string.Empty)
                        {
                            hasValue = true;
                        }
                    }
                    if (hasValue)
                    {
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 获取单元格类型(xlsx)
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static object GetValueTypeForXLSX(XSSFCell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {

                case CellType.Blank: //BLANK:
                    return null;
                case CellType.Boolean: //BOOLEAN:
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:
                    return cell.NumericCellValue;
                case CellType.String: //STRING:
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:
                default:
                    return "=" + cell.CellFormula;
            }
        }

        #region  转化实体为dataTable

        /// <summary>
        /// Convert a List{T} to a DataTable.
        /// </summary>
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        #endregion

        #region datatable to list

        /// <summary>
        /// DataTable转成List
        /// </summary>
        public static List<T> ToDataList<T>(this DataTable dt)
        {
            var list = new List<T>();
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());
            foreach (DataRow item in dt.Rows)
            {
                var s = Activator.CreateInstance<T>();
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    var info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        try
                        {
                            if (!Convert.IsDBNull(item[i]))
                            {
                                object v = null;
                                if (info.PropertyType.ToString().Contains("System.Nullable"))
                                {
                                    v = Convert.ChangeType(item[i], Nullable.GetUnderlyingType(info.PropertyType));
                                }
                                else
                                {
                                    v = Convert.ChangeType(item[i], info.PropertyType);
                                }
                                info.SetValue(s, v, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("字段[" + info.Name + "]转换出错," + ex.Message);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }
        #endregion


        /// <summary>
        /// DataTable转换成Excel文档流，并保存到文件,查询语句请跟列头顺序保持一致
        /// </summary>
        /// <param name="table"></param>
        /// <param name="headerDic">列头字典，Key是列的索引，value是列名称</param>
        /// <param name="fileName">保存的路径</param>
        public static void RenderToExcel(DataTable table, Dictionary<string, string> headerDic, string fileName)
        {
            using (MemoryStream ms = RenderToExcel(table, headerDic))
            {
                RenderToBrowser(ms, HttpContext.Current, fileName);
            }
        }

        /// <summary>
        /// 输出文件到浏览器
        /// </summary>
        /// <param name="ms">Excel文档流</param>
        /// <param name="context">HTTP上下文</param>
        /// <param name="fileName">文件名</param>
        private static void RenderToBrowser(MemoryStream ms, HttpContext context, string fileName)
        {
            if (context.Request.Browser.Browser == "IE")
                fileName = HttpUtility.UrlEncode(fileName);
            context.Response.AddHeader("Content-Disposition", "attachment;fileName=" + fileName);
            context.Response.BinaryWrite(ms.ToArray());
        }

        /// <summary>
        ///  DataTable转换成Excel文档流
        /// </summary>
        /// <param name="table"></param>
        /// <param name="clomunDic">列头字典，Key是列的索引，value是列名称</param>
        /// <returns></returns>
        public static MemoryStream RenderToExcel(DataTable table, Dictionary<string, string> headerDic)
        {
            MemoryStream ms = new MemoryStream();

            using (table)
            {
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                IRow headerRow = sheet.CreateRow(0);
                int index = 0;
                foreach (var item in headerDic)
                {
                    headerRow.CreateCell(index).SetCellValue(item.Value);//If Caption not set, returns the ColumnName value
                    index++;
                }
                //第一个参数表示要冻结的列数；
                //第二个参数表示要冻结的行数，这里只冻结列所以为0；
                //第三个参数表示右边区域可见的首列序号，从1开始计算；
                //第四个参数表示下边区域可见的首行序号，也是从1开始计算，这里是冻结列，所以为0；
                //冻结首行
                sheet.CreateFreezePane(0, 1, 0, 1);

                // handling value.
                int rowIndex = 1;
                List<KeyValuePair<string, string>> list = headerDic.ToList();
                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in table.Columns)
                    {
                        if (headerDic.Keys.Contains(column.ColumnName))
                        {
                            int cloumnIndex = list.FindIndex(t => t.Key == column.ColumnName);
                            dataRow.CreateCell(cloumnIndex).SetCellValue(row[column.ColumnName].ToString());
                        }
                    }
                    rowIndex++;
                }
                //AutoSizeColumns(sheet);
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }
    }
}
