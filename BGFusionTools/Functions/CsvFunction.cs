using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace BGFusionTools.Functions
{
    public class CsvFunction
    {
        public static DataSet CsvRead(string sfilePath)
        {
           /*tring sfilename = @sfilePath;
            FileStream fs = new FileStream(sfilePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(sfilePath, Encoding.UTF8);
            sr.ReadLine();*/
            return default(DataSet);
        }
        public static void CsvWirte(string sfilePath,DataTable dt)
        {
            string sfilename = @sfilePath;
            if (File.Exists(sfilename))
            {
                File.Delete(sfilename);
            }
           // File.Create(sfilename);
            FileStream fs = new FileStream(sfilename,FileMode.Create,FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

            int iRowCounts = dt.Rows.Count;
            int iColCounts = dt.Columns.Count;
            string sWirteFirseLine = "";
            string sWirteLine = "";

            try
            {
                for (int i = 0; i < iColCounts; i++)
                {

                    sWirteFirseLine += dt.Columns[i].ToString();
                    if (i < iColCounts - 1)
                        sWirteFirseLine += ",";
                }
                sw.WriteLine(sWirteFirseLine);
                for (int i = 0; i < iRowCounts; i++)
                {
                    sWirteLine = "";
                    for (int j = 0; j < iColCounts; j++)
                    {
                        sWirteLine += dt.Rows[i][j].ToString();
                        if (j < iColCounts - 1)
                            sWirteLine += ",";
                    }
                    sw.WriteLine(sWirteLine);
                }
                sw.Close();
                sw.Dispose();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Write to CSV Error： " + ex.Message);
                sw.Close();
                sw.Dispose();
            }
        }
    }
}
