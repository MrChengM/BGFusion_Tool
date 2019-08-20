using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;

namespace BGFusionTools.Datas
{
    public struct BaseParameter
    {
        private DataTable taglistTable;
        private DataTable singleMappingTable;
        private DataTable commandMappingTable;
        private TaglistColumns taglistColumns;
        private BaseListSignalMappingColumns signalMappingColumns;
        private BaseListCommandMappingColumns commandMappingColumns;
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
        public TaglistColumns TaglistColName { get { return taglistColumns; } set { taglistColumns = value; } }
        public BaseListSignalMappingColumns SignalMappingColName { get { return signalMappingColumns; } set { signalMappingColumns = value; } }
        public BaseListCommandMappingColumns CommandMappingColName { get { return commandMappingColumns; } set { commandMappingColumns = value; } }
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

    public abstract class BaseData
    {
        internal BaseParameter baseParameter = new BaseParameter();
        //public abstract string ToString();
        public abstract int ToInt();
        public abstract List<List<string>> ToList();
        public abstract DataTable ToDataTable();
        public abstract void OutData();
        public abstract Dictionary<string, string> ToDictionary();

        //根据Mapping判断需要的数据地址条数，生成对应的sSignalName数目集合.
        internal List<string> sSignalName(int iByteCounts, string sTemp, string sSystem, string sPlcLink, string sEquipmentLine,
            string sEquipmentElement,int iSignalNumber)
        {
            List<string> ssignalName = new List<string>();
            int iCounts;
            if (iByteCounts <= 32)
            {
                ssignalName.Add(string.Format(sTemp, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, iSignalNumber));
            }
            else
            {
                iCounts = (int)Math.Ceiling((float)iByteCounts / 32);
                for (int i = 0; i < iCounts; i++)
                {
                    ssignalName.Add(string.Format(sTemp, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, iSignalNumber + 1));
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
                switch (baseParameter.ViewNum)
                {
                    case 0:
                        break;
                    case 1:
                        sSelectConveyorColName = baseParameter.TaglistColName.sLevel1View;
                        break;
                    case 2:
                        sSelectConveyorColName = baseParameter.TaglistColName.sLevel2View;
                        break;
                }
                sSelectColVal = baseParameter.ViewName;
                var ConveyorRows = from p in baseParameter.TaglistTable.AsEnumerable()
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
