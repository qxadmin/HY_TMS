using System.Data;
using System.IO;
using System.Text;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using System.Web.UI;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Linq;
/// <summary>
/// 使用NPOI操作Excel，无需Office COM组件
/// Created By 囧月 http://lwme.cnblogs.com
/// 部分代码取自http://msdn.microsoft.com/zh-tw/ee818993.aspx
/// NPOI是POI的.NET移植版本，目前稳定版本中仅支持对xls文件（Excel 97-2003）文件格式的读写
/// NPOI官方网站http://npoi.codeplex.com/
/// </summary>
public class ExcelRender
{
    /// <summary>
    /// 根据Excel列类型获取列的值
    /// </summary>
    /// <param name="cell">Excel列</param>
    /// <returns></returns>
    private static string GetCellValue(ICell cell)
    {
        if (cell == null)
            return string.Empty;
        switch (cell.CellType)
        {
            case CellType.BLANK:
                return string.Empty;
            case CellType.BOOLEAN:
                return cell.BooleanCellValue.ToString();
            case CellType.ERROR:
                return cell.ErrorCellValue.ToString();
            case CellType.NUMERIC:
            case CellType.Unknown:
            default:
                return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
            case CellType.STRING:
                return cell.StringCellValue;
            case CellType.FORMULA:
                try
                {
                    HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                    e.EvaluateInCell(cell);
                    return cell.ToString();
                }
                catch
                {
                    return cell.NumericCellValue.ToString();
                }
        }
    }

    /// <summary>
    /// 自动设置Excel列宽
    /// </summary>
    /// <param name="sheet">Excel表</param>
    private static void AutoSizeColumns(ISheet sheet)
    {
        if (sheet.PhysicalNumberOfRows > 0)
        {
            IRow headerRow = sheet.GetRow(0);

            for (int i = 0, l = headerRow.LastCellNum; i < l; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        }
    }

    /// <summary>
    /// 保存Excel文档流到文件
    /// </summary>
    /// <param name="ms">Excel文档流</param>
    /// <param name="fileName">文件名</param>
    private static void SaveToFile(MemoryStream ms, string fileName)
    {
        using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
        {
            byte[] data = ms.ToArray();

            fs.Write(data, 0, data.Length);
            fs.Flush();

            data = null;
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

        //string csv = _service.GetData(model);
        HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
        result.Content = new StreamContent(ms);
        //a text file is actually an octet-stream (pdf, etc)
        //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
        //we used attachment to force download
        result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        result.Content.Headers.ContentDisposition.FileName = fileName;
        context.Response.AddHeader("Content-Disposition", "attachment;fileName=" + fileName);
        context.Response.BinaryWrite(ms.ToArray());
    }

    /// <summary>
    /// DataReader转换成Excel文档流
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public static MemoryStream RenderToExcel(IDataReader reader)
    {
        MemoryStream ms = new MemoryStream();

        using (reader)
        {
            using (IWorkbook workbook = new HSSFWorkbook())
            {
                using (ISheet sheet = workbook.CreateSheet())
                {
                    IRow headerRow = sheet.CreateRow(0);
                    int cellCount = reader.FieldCount;

                    // handling header.
                    for (int i = 0; i < cellCount; i++)
                    {
                        headerRow.CreateCell(i).SetCellValue(reader.GetName(i));
                    }

                    // handling value.
                    int rowIndex = 1;
                    while (reader.Read())
                    {
                        IRow dataRow = sheet.CreateRow(rowIndex);

                        for (int i = 0; i < cellCount; i++)
                        {
                            dataRow.CreateCell(i).SetCellValue(reader[i].ToString());
                        }

                        rowIndex++;
                    }

                    AutoSizeColumns(sheet);

                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                }
            }
        }
        return ms;
    }

    /// <summary>
    /// DataReader转换成Excel文档流，并保存到文件
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="fileName">保存的路径</param>
    public static void RenderToExcel(IDataReader reader, string fileName)
    {
        using (MemoryStream ms = RenderToExcel(reader))
        {
            SaveToFile(ms, fileName);
        }
    }

    /// <summary>
    /// DataReader转换成Excel文档流，并输出到客户端
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="context">HTTP上下文</param>
    /// <param name="fileName">输出的文件名</param>
    public static void RenderToExcel(IDataReader reader, HttpContext context, string fileName)
    {
        using (MemoryStream ms = RenderToExcel(reader))
        {
            RenderToBrowser(ms, context, fileName);
        }
    }

    /// <summary>
    /// DataTable转换成Excel文档流
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    public static MemoryStream RenderToExcel(DataTable table)
    {
        MemoryStream ms = new MemoryStream();

        using (table)
        {
            using (IWorkbook workbook = new HSSFWorkbook())
            {
                using (ISheet sheet = workbook.CreateSheet())
                {
                    IRow headerRow = sheet.CreateRow(0);

                    // handling header.
                    foreach (DataColumn column in table.Columns)
                        headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value

                    // handling value.
                    int rowIndex = 1;

                    foreach (DataRow row in table.Rows)
                    {
                        IRow dataRow = sheet.CreateRow(rowIndex);

                        foreach (DataColumn column in table.Columns)
                        {
                            dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                        }

                        rowIndex++;
                    }
                    AutoSizeColumns(sheet);

                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                }
            }
        }
        return ms;
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
            using (IWorkbook workbook = new HSSFWorkbook())
            {
                using (ISheet sheet = workbook.CreateSheet())
                {
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
                    AutoSizeColumns(sheet);
                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                }
            }
        }
        return ms;
    }

    /// <summary>
    /// DataTable转换成Excel文档流，并保存到文件
    /// </summary>
    /// <param name="table"></param>
    /// <param name="fileName">保存的路径</param>
    public static void RenderToExcel(DataTable table, string fileName)
    {
        using (MemoryStream ms = RenderToExcel(table))
        {
            SaveToFile(ms, fileName);
        }
    }


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
    /// DataTable转换成Excel文档流，并保存到文件,查询语句请跟列头顺序保持一致
    /// </summary>
    /// <param name="table"></param>
    /// <param name="headerDic">列头字典，Key是列的索引，value是列名称</param>
    /// <param name="fileName">保存的路径</param>
    public static MemoryStream RenderToExcelMS(DataTable table, Dictionary<string, string> headerDic, string fileName)
    {
        using (MemoryStream ms = RenderToExcel(table, headerDic))
        {
            return ms;
        }
        return null;
    }

    /// <summary>
    /// DataTable转换成Excel文档流，并输出到客户端
    /// </summary>
    /// <param name="table"></param>
    /// <param name="response"></param>
    /// <param name="fileName">输出的文件名</param>
    public static void RenderToExcel(DataTable table, HttpContext context, string fileName)
    {
        using (MemoryStream ms = RenderToExcel(table))
        {
            RenderToBrowser(ms, context, fileName);
        }
    }

    /// <summary>
    /// Excel文档流是否有数据
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <returns></returns>
    public static bool HasData(Stream excelFileStream)
    {
        return HasData(excelFileStream, 0);
    }

    /// <summary>
    /// Excel文档流是否有数据
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <param name="sheetIndex">表索引号，如第一个表为0</param>
    /// <returns></returns>
    public static bool HasData(Stream excelFileStream, int sheetIndex)
    {
        using (excelFileStream)
        {
            using (IWorkbook workbook = new HSSFWorkbook(excelFileStream))
            {
                if (workbook.NumberOfSheets > 0)
                {
                    if (sheetIndex < workbook.NumberOfSheets)
                    {
                        using (ISheet sheet = workbook.GetSheetAt(sheetIndex))
                        {
                            return sheet.PhysicalNumberOfRows > 0;
                        }
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Excel文档流转换成DataTable
    /// 第一行必须为标题行
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <param name="sheetName">表名称</param>
    /// <returns></returns>
    public static DataTable RenderFromExcel(Stream excelFileStream, string sheetName)
    {
        return RenderFromExcel(excelFileStream, sheetName, 0);
    }

    /// <summary>
    /// Excel文档流转换成DataTable
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <param name="sheetName">表名称</param>
    /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
    /// <returns></returns>
    public static DataTable RenderFromExcel(Stream excelFileStream, string sheetName, int headerRowIndex)
    {
        DataTable table = null;

        using (excelFileStream)
        {
            using (IWorkbook workbook = new HSSFWorkbook(excelFileStream))
            {
                using (ISheet sheet = workbook.GetSheet(sheetName))
                {
                    table = RenderFromExcel(sheet, headerRowIndex);
                }
            }
        }
        return table;
    }

    /// <summary>
    /// Excel文档流转换成DataTable
    /// 默认转换Excel的第一个表
    /// 第一行必须为标题行
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <returns></returns>
    public static DataTable RenderFromExcel(Stream excelFileStream)
    {
        return RenderFromExcel(excelFileStream, 0, 0);
    }

    /// <summary>
    /// Excel文档流转换成DataTable
    /// 第一行必须为标题行
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <param name="sheetIndex">表索引号，如第一个表为0</param>
    /// <returns></returns>
    public static DataTable RenderFromExcel(Stream excelFileStream, int sheetIndex)
    {
        return RenderFromExcel(excelFileStream, sheetIndex, 0);
    }

    /// <summary>
    /// Excel文档流转换成DataTable
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <param name="sheetIndex">表索引号，如第一个表为0</param>
    /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
    /// <returns></returns>
    public static DataTable RenderFromExcel(Stream excelFileStream, int sheetIndex, int headerRowIndex)
    {
        DataTable table = null;

        using (excelFileStream)
        {
            using (IWorkbook workbook = new HSSFWorkbook(excelFileStream))
            {
                using (ISheet sheet = workbook.GetSheetAt(sheetIndex))
                {
                    table = RenderFromExcel(sheet, headerRowIndex);
                }
            }
        }
        return table;
    }

    /// <summary>
    /// Excel表格转换成DataTable
    /// </summary>
    /// <param name="sheet">表格</param>
    /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
    /// <returns></returns>
    private static DataTable RenderFromExcel(ISheet sheet, int headerRowIndex)
    {
        DataTable table = new DataTable();

        IRow headerRow = sheet.GetRow(headerRowIndex);
        int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
        int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

        //handling header.
        for (int i = headerRow.FirstCellNum; i < cellCount; i++)
        {
            DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
            table.Columns.Add(column);
        }

        for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
        {
            IRow row = sheet.GetRow(i);
            DataRow dataRow = table.NewRow();

            if (row != null)
            {
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = GetCellValue(row.GetCell(j));
                }
            }

            table.Rows.Add(dataRow);
        }

        return table;
    }

    /// <summary>
    /// Excel文档导入到数据库
    /// 默认取Excel的第一个表
    /// 第一行必须为标题行
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <param name="insertSql">插入语句</param>
    /// <param name="dbAction">更新到数据库的方法</param>
    /// <returns></returns>
    public static int RenderToDb(Stream excelFileStream, string insertSql, DBAction dbAction)
    {
        return RenderToDb(excelFileStream, insertSql, dbAction, 0, 0);
    }

    public delegate int DBAction(string sql, params IDataParameter[] parameters);

    /// <summary>
    /// Excel文档导入到数据库
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <param name="insertSql">插入语句</param>
    /// <param name="dbAction">更新到数据库的方法</param>
    /// <param name="sheetIndex">表索引号，如第一个表为0</param>
    /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
    /// <returns></returns>
    public static int RenderToDb(Stream excelFileStream, string insertSql, DBAction dbAction, int sheetIndex, int headerRowIndex)
    {
        int rowAffected = 0;
        using (excelFileStream)
        {
            using (IWorkbook workbook = new HSSFWorkbook(excelFileStream))
            {
                using (ISheet sheet = workbook.GetSheetAt(sheetIndex))
                {
                    StringBuilder builder = new StringBuilder();

                    IRow headerRow = sheet.GetRow(headerRowIndex);
                    int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
                    int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

                    for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row != null)
                        {
                            builder.Append(insertSql);
                            builder.Append(" values (");
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                builder.AppendFormat("'{0}',", GetCellValue(row.GetCell(j)).Replace("'", "''"));
                            }
                            builder.Length = builder.Length - 1;
                            builder.Append(");");
                        }

                        if ((i % 50 == 0 || i == rowCount) && builder.Length > 0)
                        {
                            //每50条记录一次批量插入到数据库
                            rowAffected += dbAction(builder.ToString());
                            builder.Length = 0;
                        }
                    }
                }
            }
        }
        return rowAffected;
    }


    /// <summary>
    /// GridView转换成Excel文档流，并保存到文件
    /// </summary>
    /// <param name="IWorkbook">输出的EXCEL文件</param>
    /// <param name="fileName">保存的路径</param>
    public static void SaveFileToExcel(IWorkbook wb, Page page, string fileName)
    {

        // 產生 Excel 資料流。
        MemoryStream ms = new MemoryStream();
        wb.Write(ms);

        // 設定強制下載標頭。
        page.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + fileName));
        // 輸出檔案。
        page.Response.BinaryWrite(ms.ToArray());

        ms.Close();
        ms.Dispose();

        wb.Dispose();

    }

    /// <summary>
    /// 各类自定义格式 add by xuj
    /// </summary>
    public enum stylexls
    {
        头,
        url,
        时间,
        数字,
        钱,
        百分比,
        中文大写,
        科学计数法,
        默认,
        黄色标题    //used by OU_Dumper_list_report
    }

    /// <summary>
    /// 创建多样的格式 add by xuj
    /// </summary>
    public ICellStyle Getcellstyle(IWorkbook wb, stylexls str)
    {
        ICellStyle cellStyle = wb.CreateCellStyle();

        //定义几种字体  
        //也可以一种字体，写一些公共属性，然后在下面需要时加特殊的  
        IFont font12 = wb.CreateFont();
        font12.FontHeightInPoints = 10;
        font12.FontName = "微软雅黑";


        IFont font = wb.CreateFont();
        font.FontName = "微软雅黑";
        //font.Underline = 1;下划线  


        IFont fontcolorblue = wb.CreateFont();
        fontcolorblue.Color = HSSFColor.OLIVE_GREEN.BLUE.index;
        fontcolorblue.IsItalic = true;//下划线  
        fontcolorblue.FontName = "微软雅黑";

        IFont fontcoloryellow = wb.CreateFont();
        fontcoloryellow.Color = HSSFColor.YELLOW.index;
        fontcoloryellow.FontName = "微软雅黑";

        //边框  
        //cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.DOTTED;
        //cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.HAIR;
        //cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.HAIR;
        //cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.DOTTED;
        //边框颜色  
        cellStyle.BottomBorderColor = HSSFColor.OLIVE_GREEN.BLUE.index;
        cellStyle.TopBorderColor = HSSFColor.OLIVE_GREEN.BLUE.index;

        //背景图形，我没有用到过。感觉很丑  
        cellStyle.FillBackgroundColor = HSSFColor.OLIVE_GREEN.BLUE.index;
        cellStyle.FillForegroundColor = HSSFColor.OLIVE_GREEN.BLUE.index;
        //cellStyle.FillForegroundColor = HSSFColor.WHITE.index;
        // cellStyle.FillPattern = FillPatternType.NO_FILL;  
        //cellStyle.FillBackgroundColor = HSSFColor.BLUE.index;

        //水平对齐  
        cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;

        //垂直对齐  
        cellStyle.VerticalAlignment = VerticalAlignment.CENTER;

        //自动换行  
        cellStyle.WrapText = true;

        //缩进;当设置为1时，前面留的空白太大了。希旺官网改进。或者是我设置的不对  
        cellStyle.Indention = 0;

        //上面基本都是设共公的设置  
        //下面列出了常用的字段类型  
        cellStyle.FillBackgroundColor = 13;
        cellStyle.BorderBottom = CellBorderType.DOTTED;
        cellStyle.BorderLeft = CellBorderType.DOTTED;
        cellStyle.BorderRight = CellBorderType.DOTTED;
        cellStyle.BorderTop = CellBorderType.DOTTED;
        switch (str)
        {
            case stylexls.头:
                // cellStyle.FillPattern = FillPatternType.LEAST_DOTS;  
                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                cellStyle.SetFont(font12);
                break;
            case stylexls.时间:
                IDataFormat datastyle = wb.CreateDataFormat();

                cellStyle.DataFormat = datastyle.GetFormat("yyyy/mm/dd");
                cellStyle.SetFont(font);
                break;
            case stylexls.数字:
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                cellStyle.SetFont(font);
                break;
            case stylexls.钱:
                IDataFormat format = wb.CreateDataFormat();
                cellStyle.DataFormat = format.GetFormat("￥#,##0");
                cellStyle.SetFont(font);
                break;
            case stylexls.url:
                fontcolorblue.Underline = 1;
                cellStyle.SetFont(fontcolorblue);
                break;
            case stylexls.百分比:
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00%");
                cellStyle.SetFont(font);
                break;
            case stylexls.中文大写:
                IDataFormat format1 = wb.CreateDataFormat();
                cellStyle.DataFormat = format1.GetFormat("[DbNum2][$-804]0");
                cellStyle.SetFont(font);
                break;
            case stylexls.科学计数法:
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00E+00");
                cellStyle.SetFont(font);
                break;
            case stylexls.黄色标题:
                //fontcoloryellow.Underline = 1;
                cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.YELLOW.index2;
                cellStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;    //这句话一定要加，否则无法取到背景色
                //cellStyle.BorderBottom = CellBorderType.DOTTED;
                //cellStyle.BorderLeft = CellBorderType.DOTTED;
                //cellStyle.BorderRight = CellBorderType.DOTTED;
                //cellStyle.BorderTop = CellBorderType.DOTTED;
                cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
                cellStyle.SetFont(font);
                break;
            case stylexls.默认:
                cellStyle.VerticalAlignment = VerticalAlignment.CENTER; ;
                cellStyle.SetFont(font);
                break;
        }
        return cellStyle;


    }

    #region 重命名下载文件
    /// <summary>
    /// MVC重命名下载文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    /// <param name="extension">文件名后缀如pdf、doc</param>
    /// <returns></returns>
    public static ActionResult ReNameDownFileByMvc(string filePath, string fileName, string extension = "")
    {
        try
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.UserAgent != null &&
                HttpContext.Current.Request.UserAgent.ToLower().IndexOf("msie") > -1)
            { fileName = HttpUtility.UrlPathEncode(fileName); }

            string extensionNew = extension;
            if (string.IsNullOrEmpty(extension))
            {
                extensionNew = filePath.Split('.')[filePath.Split('.').Length - 1];
            }
            string contentType = GetContentType(extensionNew);
            return new FileStreamResult(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), contentType)
            {
                FileDownloadName = fileName
            };
        }
        catch
        {
            return new ContentResult() { Content = "下载失败！" };
        }
        // return File(new System.IO.FileStream(filePath, System.IO.FileMode.Open), "application/octet-stream", Server.UrlEncode(fileName));
    }
    /// <summary>
    /// MVC重命名下载文件
    /// </summary> 
    public static System.Web.Mvc.ActionResult ReNameDownFileByMvc(FileStream fs, string fileName)
    {
        try
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.UserAgent != null &&
                HttpContext.Current.Request.UserAgent.ToLower().IndexOf("msie") > -1)
            { fileName = HttpUtility.UrlPathEncode(fileName); }
            return new System.Web.Mvc.FileStreamResult(fs, "application/octet-stream")
            {
                FileDownloadName = fileName
            };
        }
        catch
        {
            return new System.Web.Mvc.ContentResult() { Content = "下载失败！" };
        }
        // return File(new System.IO.FileStream(filePath, System.IO.FileMode.Open), "application/octet-stream", Server.UrlEncode(fileName));
    }
    /// <summary>
    /// 重命名下载文件
    /// </summary>
    /// <param name="oldFileName">原文件名称</param>
    /// <param name="newFileName">命名后的名称</param>
    public static void ReNameDownFile(string oldFileName, string newFileName)
    {
        if (!File.Exists(oldFileName)) { return; }//检查文件是否存在
        System.IO.FileInfo file = new System.IO.FileInfo(oldFileName);
        //ToolBox.AddLog("【下载文件】", "加载文件对象");
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        /* 
         * 添加头信息, 为"文件下载/另存为"对话框指定默认文件名
         * 解决了firefox下文件名乱码问题
        */
        if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("msie") > -1)
            newFileName = HttpUtility.UrlPathEncode(newFileName);

        if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("firefox") > -1)
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + newFileName + "\"");
        else if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("360se") > -1)
        {
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + newFileName + "\"");
        }
        else if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf("safari") > -1)
        {
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + newFileName + "\"");
        }
        else
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + newFileName);

        //  添加头信息，指定文件大小，让浏览器能够显示下载进度 
        HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
        //  指定返回的是一个不能被客户端读取的流，必须被下载 
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        //  把文件流发送到客户端 
        HttpContext.Current.Response.WriteFile(file.FullName);
        //ToolBox.AddLog("【下载文件】", "写文件");
        //  停止页面的执行 
        if (HttpContext.Current.Response.IsClientConnected)
        {
            //ToolBox.AddLog("【下载文件】", "客户端仍然与服务器连接，结束响应");
            HttpContext.Current.Response.End();
        }
        else
        {
            //ToolBox.AddLog("【下载文件】", "客户端与服务器连接已断开，结束响应");
            HttpContext.Current.Response.End();
        }
    }

    public static string GetContentType(string extension)
    {
        string contentType = "application/octet-stream";
        switch (extension)
        {
            case "doc":
                contentType = "application/msword";
                break;

            case "docx":
                contentType = "application/msword";
                break;

            case "xls":
                contentType = "application/vnd.ms-excel;charset=utf-8";
                break;

            case "xlsx":
                contentType = "application/vnd.ms-excel;charset=utf-8";
                break;

            case "ppt":
                contentType = "application/vnd.ms-powerpoint";
                break;

            case "pptx":
                contentType = "application/vnd.ms-powerpoint";
                break;

            case "htm":
                contentType = "text/html";
                break;

            case "html":
                contentType = "text/html";
                break;

            case "txt":
                contentType = "text/plain";
                break;

            case "zip":
                contentType = "application/x-zip-compressed";
                break;

            case "rar":
                contentType = "application/octet-stream  ";
                break;

            case "gz":
                contentType = "application/x-gzip";
                break;

            case "7z":
                contentType = "application/x-zip-compressed";
                break;

            case "bz2":
                contentType = "application/x-bzip2";
                break;

            case "pdf":
                contentType = "application/pdf";
                break;

            case "gif":
                contentType = "image/gif";
                break;

            case "jpg":
                contentType = "image/jpeg";
                break;

            case "jpeg":
                contentType = "image/jpeg";
                break;

            case "png":
                contentType = "image/png";
                break;

            case "bmp":
                contentType = "application/x-bmp";
                break;

            case "ico":
                contentType = "image/x-icon";
                break;

            default:
                break;
        }
        return contentType;
    }
    #endregion
}
