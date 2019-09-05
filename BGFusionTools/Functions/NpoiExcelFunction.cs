using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data;
using NPOI.SS.UserModel;
using System.IO;
using BGFusionTools.Datas;

namespace BGFusionTools.Functions
{
    public static class NpoiExcelFunction
    {
        public static DataSet ExcelRead(string filePath)
        {
            DataSet da = new DataSet();
            DataTable dt = null;
            IWorkbook workbook = null;

            try
            {
                if (File.Exists(filePath))
                {
                    FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    string fileType = Path.GetExtension(filePath);
                    if (fileType == ".xls")
                        workbook = new HSSFWorkbook(file);
                    else if (fileType == ".xlsx")
                        workbook = new XSSFWorkbook(file);
                    file.Close();
                }
                if (workbook != null)
                {
                    for (int index = 0; index < workbook.NumberOfSheets; index++)
                    {
                        dt = new DataTable(workbook.GetSheetName(index) + "$");
                        ISheet sheet = workbook.GetSheetAt(index);
                        IRow row = sheet.GetRow(0);
                        foreach (ICell cell in row.Cells)
                        {
                            DataColumn col = new DataColumn(cell.StringCellValue);
                            dt.Columns.Add(col);
                        }
                        for (int i = 1; i < sheet.LastRowNum; i++)
                        {
                            row = sheet.GetRow(i);
                            DataRow dtrow = dt.NewRow();
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                if (row == null)
                                    continue;
                                ICell cell = row.GetCell(j);
                                if (cell == null)
                                    dtrow[j] = "";
                                else
                                    switch (cell.CellType)
                                    {
                                        case CellType.Formula:
                                            switch (cell.CachedFormulaResultType)
                                            {
                                                case CellType.Numeric:
                                                    dtrow[j] = cell.NumericCellValue.ToString();
                                                    break;
                                                case CellType.String:
                                                    dtrow[j] = cell.StringCellValue;
                                                    break;
                                                case CellType.Boolean:
                                                    dtrow[j] = cell.BooleanCellValue.ToString();
                                                    break;
                                                case CellType.Error:
                                                    dtrow[j] = "";
                                                    break;
                                                default:
                                                    dtrow[j] = "";
                                                    break;
                                            }
                                            break;
                                        case CellType.Numeric:
                                            dtrow[j] = cell.NumericCellValue.ToString();
                                            break;
                                        case CellType.Boolean:
                                            dtrow[j] = cell.BooleanCellValue.ToString();
                                            break;
                                        case CellType.String:
                                            dtrow[j] = cell.StringCellValue;
                                            break;
                                        default:
                                            dtrow[j] = "";
                                            break;
                                    }
                            }
                            dt.Rows.Add(dtrow);
                        }
                        da.Tables.Add(dt);
                    }
                }
                return da;
            }
            catch (Exception ex)
            {
                return da;
            }
            finally
            {
                if (da != null) da.Dispose();
                if (dt != null) dt.Dispose();
                workbook =null;
            }

        }
        public static void ExcelWrite(string infilename, string outfilename, Dictionary<string, string> listdatas)
        {
            try
            {

            }
            catch (Exception ex)
            {
            }
            finally
            {
               
            }
            //释放内存

        }
        public static void ExcelWrite(string filename,string sheetName, List<List<TestSheetRow>> listDatas)
        {
            IWorkbook workbook = null;
            IRow row = null;
            ICell cell = null;
            ICellStyle tittlestyle = null;
            ICellStyle columnstyle = null;
            IFont tittlefont = null;
            IFont columnfont = null;
            int rowIndex = 0;
            int columnIndex = 0;
            string fileType = Path.GetExtension(filename);

            //判断是否存在当前文件，存在就读取
            if (File.Exists(filename))
            {
                FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read);
                if (fileType == ".xls")
                {
                    workbook = new HSSFWorkbook(infile);
                }
                else if (fileType == ".xlsx")
                {
                    workbook = new XSSFWorkbook(infile);
                }
                infile.Close();
            }
            else
            {
                if (fileType == ".xls")
                {
                    workbook = new HSSFWorkbook();
                }
                else if (fileType == ".xlsx")
                {
                    workbook = new XSSFWorkbook();
                }
            }
            //判断是否存在当前sheet
            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                if (sheetName == workbook.GetSheetName(i))
                {
                    workbook.RemoveSheetAt(i);
                    break;
                }
            }
            ISheet sheet = workbook.CreateSheet(sheetName);

            //标题格式
            tittlefont = workbook.CreateFont();
            tittlefont.Color = IndexedColors.Red.Index;
            tittlefont.FontHeightInPoints = 14;
            tittlestyle = workbook.CreateCellStyle();
            tittlestyle.BorderBottom = BorderStyle.Thin;
            tittlestyle.BottomBorderColor = IndexedColors.Black.Index;
            tittlestyle.BorderLeft = BorderStyle.Thin;
            tittlestyle.LeftBorderColor = IndexedColors.Black.Index;
            tittlestyle.BorderRight = BorderStyle.Thin;
            tittlestyle.RightBorderColor = IndexedColors.Black.Index;
            tittlestyle.BorderTop = BorderStyle.Thin;
            tittlestyle.TopBorderColor = IndexedColors.Black.Index;
            tittlestyle.Alignment = HorizontalAlignment.Center;
            tittlestyle.FillPattern = FillPattern.SolidForeground;
            tittlestyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            tittlestyle.SetFont(tittlefont);


            //列格式
            columnfont = workbook.CreateFont();
            //columnfont.Color = IndexedColors.White.Index;
            columnfont.FontHeightInPoints = 12;
            columnstyle = workbook.CreateCellStyle();
            /*columnstyle.BorderBottom = BorderStyle.Thin;
            columnstyle.BottomBorderColor = IndexedColors.Black.Index;
            columnstyle.BorderLeft = BorderStyle.Thin;
            columnstyle.LeftBorderColor = IndexedColors.Black.Index;
            columnstyle.BorderRight = BorderStyle.Thin;
            columnstyle.RightBorderColor = IndexedColors.Black.Index;
            columnstyle.BorderTop = BorderStyle.Thin;
            columnstyle.TopBorderColor = IndexedColors.Black.Index;
            columnstyle.Alignment = HorizontalAlignment.Center;*/
            //columnstyle.FillPattern = FillPattern.SolidForeground;
            //columnstyle.FillForegroundColor = IndexedColors.Blue.Index;
            columnstyle.SetFont(columnfont);

            //写标题
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, TestColumns.Count() - 1));

            row = sheet.CreateRow(rowIndex);
            cell = row.CreateCell(columnIndex);
            cell.SetCellValue("FAT TEST LIST");
            cell.CellStyle = tittlestyle;
            rowIndex++;
            //写columns
            row = sheet.CreateRow(rowIndex);
            foreach (string s in TestColumns.GetEnumerable())
            {
                cell = row.CreateCell(columnIndex);
                cell.SetCellValue(s);
                cell.CellStyle = columnstyle;
                columnIndex++;
            }
            rowIndex++;
            //写数据
            foreach (List<TestSheetRow> testSheetRows in listDatas)
            {
                foreach (TestSheetRow testSheetRow in testSheetRows)
                {
                    columnIndex = 0;
                    row = sheet.CreateRow(rowIndex);
                    foreach (string s in testSheetRow)
                    {
                        cell = row.CreateCell(columnIndex);
                        cell.SetCellValue(s);
                        columnIndex++;
                    }
                    rowIndex++;
                }
                rowIndex++;
            }
            FileStream outfiles = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            workbook.Write(outfiles);
            workbook.Close();
            outfiles.Close();
        }
        public static void ExcelWrite(string filename, string sheetName, List<List<string>> listDatas)
        {
            IWorkbook workbook = null;
            IRow row = null;
            ICell cell = null;
            ICellStyle tittlestyle = null;
            IFont tittlefont = null;
            int rowIndex = 0;
            int columnIndex = 0;
            string fileType = Path.GetExtension(filename);

            if (File.Exists(filename))
            {
                FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read);
                if (fileType == ".xls")
                {
                        workbook = new HSSFWorkbook(infile);
                }
                else if (fileType == ".xlsx")
                {
                        workbook = new XSSFWorkbook(infile);
                }
                infile.Close();

            }
            else
            {
                if (fileType == ".xls")
                {
                        workbook = new HSSFWorkbook();
                }
                else if (fileType == ".xlsx")
                {
                        workbook = new XSSFWorkbook();
                }
            }
            
            for(int i=0;i< workbook.NumberOfSheets; i++)
            {
                if (sheetName == workbook.GetSheetName(i))
                {
                    workbook.RemoveSheetAt(i);
                    break;
                }
            }
            ISheet sheet = workbook.CreateSheet(sheetName);
            //标题格式
            tittlefont = workbook.CreateFont();
            tittlefont.Color = IndexedColors.Red.Index;
            tittlefont.FontHeightInPoints = 14;
            tittlestyle = workbook.CreateCellStyle();
            tittlestyle.BorderBottom = BorderStyle.Thin;
            tittlestyle.BottomBorderColor = IndexedColors.Black.Index;
            tittlestyle.BorderLeft = BorderStyle.Thin;
            tittlestyle.LeftBorderColor = IndexedColors.Black.Index;
            tittlestyle.BorderRight = BorderStyle.Thin;
            tittlestyle.RightBorderColor = IndexedColors.Black.Index;
            tittlestyle.BorderTop = BorderStyle.Thin;
            tittlestyle.TopBorderColor = IndexedColors.Black.Index;
            tittlestyle.Alignment = HorizontalAlignment.Center;
            tittlestyle.FillPattern = FillPattern.SolidForeground;
            tittlestyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            tittlestyle.SetFont(tittlefont);

            //写标题
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, TestColumns.Count() - 1));
            row = sheet.CreateRow(rowIndex);
            cell = row.CreateCell(columnIndex);
            cell.SetCellValue("COPY TO PLC");
            cell.CellStyle = tittlestyle;
            rowIndex++;
            //写数据
            foreach (List<string> ls in listDatas)
            {
                foreach (string s in ls)
                {
                    columnIndex = 0;
                    row = sheet.CreateRow(rowIndex);
                    cell = row.CreateCell(columnIndex);
                    cell.SetCellValue(s);
                    rowIndex++;
                }
                rowIndex++;
            }
            FileStream outfile = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            workbook.Write(outfile);
            workbook.Close();
            outfile.Close();


        }

    }
}
 