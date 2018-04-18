using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace BGFusion_TextBlockCopy
{
    public class WriteExcel
    {
        /// <summary>
        /// 写Excel数据表（COM方式）
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="outputstring"></param>
        public static void WriteToExcel(string infilename, string outfilename, List<ListData> listdatas)
        {
            //新建输出Excel
            Excel.Application xapps = new Excel.Application();
            Excel.Workbook xbook = xapps.Workbooks.Add(Missing.Value);
            Excel.Worksheet xsheet = xbook.Sheets[1];
            Excel.Range xrng;

            //新建输入Excel
            Excel.Application iapps = new Excel.Application();
            Excel.Workbook ibook = iapps.Workbooks.Open(@infilename);
            Excel.Worksheet isheet = ibook.Sheets[1];
            Excel.Range irng;

            try
            {
                string sfilename = @outfilename;
                if (File.Exists(sfilename))
                {
                    File.Delete(sfilename);
                }
                int ColCount = isheet.UsedRange.Columns.Count;
                int RowCounts = listdatas.Count;
                //列名导入
                int RowHeights = 20;
                for (int i = 1; i <= ColCount; i++)
                {
                    irng = (Excel.Range)isheet.Cells[1, i];
                    //xrng = (Excel.Range)xsheet.Cells[1, i];
                    xrng = xsheet.Range[xsheet.Cells[1, i], xsheet.Cells[1, i]];
                    xrng.Value = irng.Value;
                    xrng.Borders.LineStyle = irng.Borders.LineStyle;
                    xrng.ColumnWidth = irng.ColumnWidth;
                    //RowHeights = irng.RowHeight;
                    //xsheet.Cells[1, i] = xrng;
                }

                //EXCEL 数据导入
                int icounts = 0;

                foreach (ListData listdata in listdatas)
                {
                    //int icounts= 0;
                    icounts = icounts + 1;
                    string sACell = "A" + (icounts + 1);
                    String sBCell = "B" + (icounts + 1);
                    xrng = xsheet.get_Range(sACell, Missing.Value);
                    xrng.Value = listdata.sColName;
                    //xrng.Borders.Color = "Black";
                    //xrng.Borders.LineStyle = 1;
                    xrng = xsheet.get_Range(sBCell, Missing.Value);
                    xrng.Value = listdata.sColGroup;
                }
                //单元格式
                string sCell1 = "A1";
                string sCell2 = "F" + (RowCounts + 1);

                xrng = xsheet.get_Range(sCell1, sCell2);
                xrng.Borders.LineStyle = 1;
                xrng.RowHeight = RowHeights;
                //保存文件
                xbook.SaveAs(sfilename, Missing.Value, Missing.Value, Missing.Value, false, Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Write to Excel Error： " + ex.Message);
                ibook.Close();
                isheet = null;
                iapps.Quit();
                iapps = null;
                xbook.Close();
                xsheet = null;
                xapps.Quit();
                xapps = null;
            }
            finally
            {
                ibook.Close();
                isheet = null;
                iapps.Quit();
                iapps = null;
                xbook.Close();
                xsheet = null;
                xapps.Quit();
                xapps = null;
            }
            //释放内存

        }
    }
}
