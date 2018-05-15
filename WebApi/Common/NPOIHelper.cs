using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;


namespace WebApi.Common
{
    /// <summary>
    /// NPOI EXCEL操作工具类
    /// </summary>
    public class NPOIHelper
    {
        /// <summary>
        /// 导出到本地文件
        /// </summary>
        public static void ExportToFile(DataSet dataSet, string fileFullPath)
        {
            List<DataTable> dts = new List<DataTable>();
            foreach (DataTable dt in dataSet.Tables) dts.Add(dt);
            ExportToFile(dts, fileFullPath);
        }

        /// <summary>
        /// 导出到本地文件
        /// </summary>
        public static void ExportToFile(DataTable dataTable, string fileFullPath)
        {
            List<DataTable> dts = new List<DataTable>();
            dts.Add(dataTable);
            ExportToFile(dts, fileFullPath);
        }

        public static void ExportToFile(IEnumerable<DataTable> dataTables, string fileFullPath)
        {
            IWorkbook workbook = new XSSFWorkbook();
            int i = 0;
            foreach (DataTable dt in dataTables)
            {
                string sheetName = string.IsNullOrEmpty(dt.TableName)
                    ? "Sheet " + (++i).ToString()
                    : dt.TableName;
                ISheet sheet = workbook.CreateSheet(sheetName);

                IRow headerRow = sheet.CreateRow(0);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string columnName = string.IsNullOrEmpty(dt.Columns[j].ColumnName)
                        ? "Column " + j.ToString()
                        : dt.Columns[j].ColumnName;
                    headerRow.CreateCell(j).SetCellValue(columnName);
                }

                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    DataRow dr = dt.Rows[a];
                    IRow row = sheet.CreateRow(a + 1);
                    for (int b = 0; b < dt.Columns.Count; b++)
                    {
                        row.CreateCell(b).SetCellValue(dr[b] != DBNull.Value ? dr[b].ToString() : string.Empty);
                    }
                }
            }

            using (FileStream fs = File.Create(fileFullPath))
            {
                workbook.Write(fs);
            }
        }

        /// <summary>
        /// 转化成流
        /// </summary>
        public static byte[] ExportToByteArray(IEnumerable<DataTable> dataTables)
        {
            IWorkbook workbook = new XSSFWorkbook();
            int i = 0;
            foreach (DataTable dt in dataTables)
            {
                string sheetName = string.IsNullOrEmpty(dt.TableName)
                    ? "Sheet " + (++i).ToString()
                    : dt.TableName;
                ISheet sheet = workbook.CreateSheet(sheetName);

                IRow headerRow = sheet.CreateRow(0);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string columnName = string.IsNullOrEmpty(dt.Columns[j].ColumnName)
                        ? "Column " + j.ToString()
                        : dt.Columns[j].ColumnName;
                    headerRow.CreateCell(j).SetCellValue(columnName);
                }

                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    DataRow dr = dt.Rows[a];
                    IRow row = sheet.CreateRow(a + 1);
                    for (int b = 0; b < dt.Columns.Count; b++)
                    {
                        row.CreateCell(b).SetCellValue(dr[b] != DBNull.Value ? dr[b].ToString() : string.Empty);
                    }
                }
            }

            using (MemoryStream stream = new MemoryStream())
            {
                workbook.Write(stream);

                byte[] bytes = stream.ToArray();

                return bytes;
            }

        }

        public static List<DataTable> GetDataTablesFrom(string xlsxFile)
        {
            if (!File.Exists(xlsxFile))
                throw new FileNotFoundException("文件不存在");

            List<DataTable> result = new List<DataTable>();
            Stream stream = new MemoryStream(File.ReadAllBytes(xlsxFile));
            IWorkbook workbook = new XSSFWorkbook(stream);
            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                DataTable dt = new DataTable();
                ISheet sheet = workbook.GetSheetAt(i);
                IRow headerRow = sheet.GetRow(0);

                int cellCount = headerRow.LastCellNum;
                for (int j = headerRow.FirstCellNum; j < cellCount; j++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(j).StringCellValue);
                    dt.Columns.Add(column);
                }

                int rowCount = sheet.LastRowNum;
                for (int a = (sheet.FirstRowNum + 1); a < rowCount; a++)
                {
                    IRow row = sheet.GetRow(a);
                    if (row == null) continue;

                    DataRow dr = dt.NewRow();
                    for (int b = row.FirstCellNum; b < cellCount; b++)
                    {
                        if (row.GetCell(b) == null) continue;
                        dr[b] = row.GetCell(b).ToString();
                    }

                    dt.Rows.Add(dr);
                }
                result.Add(dt);
            }
            stream.Close();

            return result;
        }
    }
}