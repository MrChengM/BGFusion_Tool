using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;

namespace BGFusion_TextBlockCopy
{
    public struct DaTableConverParameter
    {
        private DataTable taglistTable;
        private DataTable singleMappingTable;
        private DataTable commandMappingTable;
        private string[,] taglistColName;
        private string[,] basefileColName;
        private string viewName;
        private int viewNum;
        private string stemp0;
        private string stemp1;
        private string stemp2;
        private string stemp3;
        private string stemp4;
        private string stemp5;
        private string stemp6;
        private string stemp7;


        public DataTable TaglistTable { get { return taglistTable; } set { taglistTable = value; } }
        public DataTable SingleMappingTable { get { return singleMappingTable; } set { singleMappingTable = value; } }
        public DataTable CommandMappingTable { get { return commandMappingTable; } set { commandMappingTable = value; } }
        public string[,] TaglistColName { get { return taglistColName; } set { taglistColName = value; } }
        public string[,] BasefileColName { get { return basefileColName; } set { basefileColName = value; } }
        public string ViewName { get { return viewName; } set { viewName = value; } }
        public int ViewNum { get { return viewNum; } set { viewNum = value; } }
        public string Stemp0 { get { return stemp0; } set { stemp0 = value; } }
        public string Stemp1 { get { return stemp1; } set { stemp1 = value; } }
        public string Stemp2 { get { return stemp2; } set { stemp2 = value; } }
        public string Stemp3 { get { return stemp3; } set { stemp3 = value; } }
        public string Stemp4 { get { return stemp4; } set { stemp4 = value; } }
        public string Stemp5 { get { return stemp5; } set { stemp5 = value; } }
        public string Stemp6 { get { return stemp6; } set { stemp6 = value; } }
        public string Stemp7 { get { return stemp7; } set { stemp7 = value; } }
    }

    public abstract class BaseTableConvert
    {
        internal DaTableConverParameter baseTableConverParameter = new DaTableConverParameter();
        public abstract string sOutData();
        public abstract int iOutData();
        public abstract List<List<string>> lOutData();
        public abstract DataTable dOutData();
        public abstract void OutData();
        public abstract Dictionary<string, string> diOutData();

        //根据Mapping判断需要的数据地址条数，生成对应的sSignalName数目集合.
        internal List<string> sSignalName(int iByteCounts, string sTemp, string sSystem, string sPlcLink, string sEquipmentLine,
            string sEquipmentElement)
        {
            List<string> ssignalName = new List<string>();
            int iCounts;
            if (iByteCounts <= 32)
            {
                ssignalName.Add(string.Format(sTemp, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, 1));
            }
            else
            {
                iCounts = (int)Math.Ceiling((float)iByteCounts / 32);
                for (int i = 0; i < iCounts; i++)
                {
                    ssignalName.Add(string.Format(sTemp, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, i + 1));
                }
            }
            return ssignalName;
        }


        //计算坐标偏移量
        internal void sMarginChange(ref int x, ref int y, int iColWidth, int iRowHight)
        {
            int ViewWidth = 1920;
            //int iColWidth = 70;
            //int iRowHight = 70;
            if (x + iColWidth > ViewWidth)
            {
                x = 0;
                y = y + iRowHight;
            }
            else
            {
                x = x + iColWidth;
            }
        }

        //计算DB Adderss偏移量
        internal string soffsetAddress(string sAddress, int bOffset)
        {
            int iEndAddress;
            int iposition = sAddress.IndexOf("DBD") != -1 ?
                sAddress.IndexOf("DBD") : sAddress.IndexOf("DBB") != -1 ?
                sAddress.IndexOf("DBB") : sAddress.IndexOf("DBW") != -1 ?
                sAddress.IndexOf("DBW") : -1;
            string sUpAddress = sAddress.Substring(0, iposition + 3);
            bool res = Int32.TryParse(sAddress.Substring(iposition + 3), out iEndAddress);
            iEndAddress = iEndAddress + bOffset;
            string sNewAddr = sUpAddress + iEndAddress;
            return sNewAddr;
        }

        internal EnumerableRowCollection<DataRow> LinqToTable()
        {

            //ConveyorRow筛选

            try
            {

                string sSelectConveyorColName = "";
                string sSelectColVal;
                switch (baseTableConverParameter.ViewNum)
                {
                    case 0:
                        break;
                    case 1:
                        sSelectConveyorColName = baseTableConverParameter.TaglistColName[1, 15];
                        break;
                    case 2:
                        sSelectConveyorColName = baseTableConverParameter.TaglistColName[1, 16];
                        break;
                }
                sSelectColVal = baseTableConverParameter.ViewName;
                var ConveyorRows = from p in baseTableConverParameter.TaglistTable.AsEnumerable()
                                   where p.Field<string>(sSelectConveyorColName) == sSelectColVal
                                   select p;

                return ConveyorRows;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Query the Conveyor Excel error: " + ex.Message);
                return null;
            }
        }
    }
}
