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
        private SignalMappingColumns signalMappingColumns;
        private CommandMappingColumns commandMappingColumns;
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
        public SignalMappingColumns SignalMappingColName { get { return signalMappingColumns; } set { signalMappingColumns = value; } }
        public CommandMappingColumns CommandMappingColName { get { return commandMappingColumns; } set { commandMappingColumns = value; } }
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
    public delegate List<List<string>> CreateDataRow(ConveyorRow ConveyordataRow) ;
    public abstract class BaseData
    {
        internal BaseParameter baseParameter = new BaseParameter();
        //public abstract string ToString();
        //public abstract int ToInt();
        public virtual List<List<string>> CreateList(CreateDataRow dataMath) 
        {
            List <List<string>>  lOutPut= new List<List<string>>();
            foreach (DataRow selectConRow in baseParameter.TaglistTable.Rows)
            {
                var conveyor = new ConveyorRow(baseParameter.TaglistColName, selectConRow);
                lOutPut.AddRange( dataMath(conveyor));
            }
            return lOutPut;
        }




        /// <summary>
        ///  根据Mapping判断需要的数据地址条数，生成对应的sSignalName数目集合.
        /// </summary>
        /// <param name="iBitCounts">bit数量</param>
        /// <param name="sTemp"></param>
        /// <param name="sSystem"></param>
        /// <param name="sPlcLink"></param>
        /// <param name="sEquipmentLine"></param>
        /// <param name="sEquipmentElement"></param>
        /// <param name="iSignalNumber"></param>
        /// <returns></returns>
        internal List<string> sSignalName(int iBitCounts,string sTemp,ConveyorRow conveyorRow,int iSignalNumber)
        {
            List<string> ssignalName = new List<string>();
            int iCounts;
            if (iBitCounts <= 32)
            {
                ssignalName.Add(string.Format(sTemp, conveyorRow.sSystem, conveyorRow.sPLC, conveyorRow.sEquipmentLine, conveyorRow.sElementName, iSignalNumber));
            }
            else
            {
                iCounts = (int)Math.Ceiling((float)iBitCounts / 32);
                for (int i = 0; i < iCounts; i++)
                {
                    ssignalName.Add(string.Format(sTemp, conveyorRow.sSystem, conveyorRow.sPLC, conveyorRow.sEquipmentLine, conveyorRow.sElementName, iSignalNumber + 1));
                }
            }
            return ssignalName;
        }


        /// <summary>
        /// 计算定位坐标偏移量
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="iColWidth"></param>
        /// <param name="iRowHight"></param>
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
    /// <summary>
    /// Taglist表中Conveyor数据行
    /// </summary>
    public class ConveyorRow
    {
        public string sSystem;
        public string sPLC;
        public string sPowerBox;
        public string sEquipmentLine;
        public string sElementType;
        public string sElementName;
        public string sBehaviorName;
        public List<string> sSignalMapping;
        public List<string> sSignalAddress;
        public string sCommandMapping;
        public string sCommandAddress;
        public string sRunningHours;
        public string sCopyRunningHours;
        public string sDisplayName;
        public string sEdgeColor;
        public string sAlarmTree;
        public string sLevel1View;
        public string sLevel2View;
        public string sDrawOnViews;
        public string sLeftClick;
        public string sRightClick;
        public string sLevel1AsLevel2;
        public string sExtendedPropertyAsCamera;
        //创建一个taglist数据行
        public  ConveyorRow (TaglistColumns columns,DataRow dataRow)
        {
            if (dataRow[0].ToString() != "")
            {
                sSystem = dataRow[columns.sSystem].ToString();
                sPLC = dataRow[columns.sPLC].ToString();
                sPowerBox = dataRow[columns.sPowerBox].ToString();
                sEquipmentLine = dataRow[columns.sEquipmentLine].ToString();
                sElementType = dataRow[columns.sElementType].ToString();
                sElementName = dataRow[columns.sElementName].ToString();
                sBehaviorName = dataRow[columns.sBehaviorName].ToString();
                foreach (string signalMapping in columns.sSignalMapping)
                {
                    string name = dataRow[signalMapping].ToString();
                    if (name != "")
                        sSignalMapping.Add(name);
                }
                foreach (string signalAddress in columns.sSignalAddress)
                {
                    string address = dataRow[signalAddress].ToString();
                    if (address != "")
                        sSignalMapping.Add(address);
                }
                sCommandMapping = dataRow[columns.sCommandMapping].ToString();
                sCommandAddress = dataRow[columns.sCommandAddress].ToString();
                sRunningHours = dataRow[columns.sRunningHours].ToString();
                sCopyRunningHours = dataRow[columns.sCopyRunningHours].ToString();
                sDisplayName = dataRow[columns.sDisplayName].ToString();
                sEdgeColor = dataRow[columns.sEdgeColor].ToString();
                sAlarmTree = dataRow[columns.sAlarmTree].ToString();
                sLevel1View = dataRow[columns.sLevel1View].ToString();
                sLevel2View = dataRow[columns.sLevel2View].ToString();
                sDrawOnViews = dataRow[columns.sDrawOnViews].ToString();
                sLeftClick = dataRow[columns.sLeftClick].ToString();
                sRightClick = dataRow[columns.sRightClick].ToString();
                sLevel1AsLevel2 = dataRow[columns.sLevel1AsLevel2].ToString();
                sExtendedPropertyAsCamera = dataRow[columns.sExtendedPropertyAsCamera].ToString();

            }
          
        }
    }
}
