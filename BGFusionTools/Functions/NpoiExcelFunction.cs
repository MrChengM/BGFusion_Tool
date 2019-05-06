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
    public class NpoiExcelFunction
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
                    string fileType = System.IO.Path.GetExtension(filePath);
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
        public static void ExcelWrite(string outfilename, List<TestListStruct> listDatas)
        {
            IWorkbook workbook = null;
            IRow row = null;
            ICell cell = null;
            ICellStyle tittlestyle = null;
            ICellStyle columnstyle = null;
            ICellStyle signalMappingstyle = null;
            ICellStyle displaystyle = null;
            IFont tittlefont = null;
            IFont columnfont = null;
            IFont signalMappingfont = null;
            IFont displayfont = null;
            try
            {
                FileStream file = new FileStream(outfilename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                string fileType = System.IO.Path.GetExtension(outfilename);
                if (fileType == ".xls")
                    workbook = new HSSFWorkbook();
                else if (fileType == ".xlsx")
                    workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                int i = 0;

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
                columnfont.Color= IndexedColors.White.Index;
                columnfont.FontHeightInPoints = 12;

                columnstyle = workbook.CreateCellStyle();
                columnstyle.BorderBottom = BorderStyle.Thin;
                columnstyle.BottomBorderColor = IndexedColors.Black.Index;
                columnstyle.BorderLeft = BorderStyle.Thin;
                columnstyle.LeftBorderColor = IndexedColors.Black.Index;
                columnstyle.BorderRight = BorderStyle.Thin;
                columnstyle.RightBorderColor = IndexedColors.Black.Index;
                columnstyle.BorderTop = BorderStyle.Thin;
                columnstyle.TopBorderColor = IndexedColors.Black.Index;
                columnstyle.Alignment = HorizontalAlignment.Center;
                columnstyle.FillPattern = FillPattern.SolidForeground;
                columnstyle.FillForegroundColor = IndexedColors.Blue.Index;
                columnstyle.SetFont(columnfont);

                //signalmapping内容格式
                signalMappingstyle = workbook.CreateCellStyle();
                signalMappingstyle.BorderBottom = BorderStyle.Thin;
                signalMappingstyle.BottomBorderColor = IndexedColors.Black.Index;
                signalMappingstyle.BorderLeft = BorderStyle.Thin;
                signalMappingstyle.LeftBorderColor = IndexedColors.Black.Index;
                signalMappingstyle.BorderRight = BorderStyle.Thin;
                signalMappingstyle.RightBorderColor = IndexedColors.Black.Index;
                signalMappingstyle.BorderTop = BorderStyle.Thin;
                signalMappingstyle.TopBorderColor = IndexedColors.Black.Index;
                signalMappingstyle.Alignment = HorizontalAlignment.Center;
                signalMappingstyle.FillPattern = FillPattern.SolidForeground;
                signalMappingstyle.FillForegroundColor = IndexedColors.Green.Index;

                //测试单元内容格式
                displaystyle = workbook.CreateCellStyle();
                displaystyle.BorderBottom = BorderStyle.Thin;
                displaystyle.BottomBorderColor = IndexedColors.Black.Index;
                displaystyle.BorderLeft = BorderStyle.Thin;
                displaystyle.LeftBorderColor = IndexedColors.Black.Index;
                displaystyle.BorderRight = BorderStyle.Thin;
                displaystyle.RightBorderColor = IndexedColors.Black.Index;
                displaystyle.BorderTop = BorderStyle.Thin;
                displaystyle.TopBorderColor = IndexedColors.Black.Index;
                displaystyle.Alignment = HorizontalAlignment.Center;
                displaystyle.FillPattern = FillPattern.SolidForeground;
                displaystyle.FillForegroundColor = IndexedColors.Yellow.Index;

                foreach (var data in listDatas)
                {
                    var signalMapping = data.SignalMappingTable;
                    var displayNames = data.ElementList;
                    var signalCol = data.SignalColumn;
                    row = sheet.CreateRow(i);
                    cell = row.CreateCell(0);
                    cell.SetCellValue("SignalMapping");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(i, i, 0, signalMapping.First().ItemArray.Length - 1));
                    cell.CellStyle = tittlestyle;
                    i++;
                    row = sheet.CreateRow(i);
                    i++;
                    for (int j = 0; j < signalCol.Count; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.SetCellValue(signalCol[j].ColumnName);
                        cell.CellStyle = columnstyle;
                    }
                    foreach (var s in displayNames)
                    {
                        cell = row.CreateCell(row.LastCellNum);
                        cell.SetCellValue(s);
                        cell.CellStyle = displaystyle;

                    }
                    //bool firstRow = true;
                    foreach (var elements in signalMapping)
                    {
                        row = sheet.CreateRow(i);
                        i++;
                        for (int j = 0; j < elements.ItemArray.Length; j++)
                        {
                            cell = row.CreateCell(j);
                            if (elements.IsNull(j) != true)
                                cell.SetCellValue(elements[j].ToString());
                            cell.CellStyle = signalMappingstyle;
                        }
                    }
                }
                workbook.Write(file);
                file.Close();
            }
            catch (Exception ex)
            {
                workbook = null;
                //file.Close();
            }
            finally
            {
                workbook = null;
            }
        }

    }
}
 