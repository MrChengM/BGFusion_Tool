using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using BGFusionTools.Helper;

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
    public delegate List<T1> CreateDataMath<T1, T2>(T2 ConveyordataRow);
    
    //public delegate List<string> CreateDataRows(List<ConveyorRow> ConveyordataRow);
    public abstract class BaseData
    {
        internal BaseParameter baseParameter = new BaseParameter();
        //public abstract string ToString();
        //public abstract int ToInt();
        public virtual List<List<string>> CreateList(CreateDataMath<List<string>,ConveyorRow> dataMath) 
        {
            List <List<string>>  lOutPut= new List<List<string>>();
            foreach (DataRow selectConRow in baseParameter.TaglistTable.Rows)
            {
                var conveyor = new ConveyorRow(baseParameter.TaglistColName, selectConRow);
                lOutPut.AddRange( dataMath(conveyor));
            }
            return lOutPut;
        }
        public virtual List<List<string>> CreateList(CreateDataMath<string, List<ConveyorRow>>  dataMath)
        {
            List<List<string>> lOutPut = new List<List<string>>();
            List<ConveyorRow> conveyors = new List<ConveyorRow>();
            foreach (DataRow selectConRow in baseParameter.TaglistTable.Rows)
            {
                var conveyor = new ConveyorRow(baseParameter.TaglistColName, selectConRow);
                conveyors.Add(conveyor);
            }
            lOutPut.Add(dataMath(conveyors));
            return lOutPut;
        }
        public virtual List<List<T1>> CreateList<T1>(CreateDataMath<T1, List<ConveyorRow>> dataMath)
        {

            List<List<T1>> lOutPut = new List<List<T1>>();
            List<ConveyorRow> conveyors = new List<ConveyorRow>();
            foreach (DataRow selectConRow in baseParameter.TaglistTable.Rows)
            {
                var conveyor = new ConveyorRow(baseParameter.TaglistColName, selectConRow);
                conveyors.Add(conveyor);
            }
            lOutPut.Add(dataMath(conveyors));
            return lOutPut;
        }
        public virtual List<T1> CreateList<T1>(CreateDataMath<T1, ConveyorRow> dataMath)
        {
            
            List<T1> lOutPut = new List<T1>();
            foreach (DataRow selectConRow in baseParameter.TaglistTable.Rows)
            {
                var conveyor = new ConveyorRow(baseParameter.TaglistColName, selectConRow) ;
                lOutPut.AddRange(dataMath(conveyor));
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
                    ssignalName.Add(string.Format(sTemp, conveyorRow.sSystem, conveyorRow.sPLC, conveyorRow.sEquipmentLine, conveyorRow.sElementName, iSignalNumber + i));
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


        /// <summary>
        /// 创建DB地址
        /// </summary>
        /// <param name="sAddress">原始地址</param>
        /// <param name="bOffset">偏移量</param>
        /// <param name="iDBXBit">DBX位</param>
        /// <param name="bDBXSwitch">是否创建成DBX地址</param>
        /// <returns></returns>
        internal string creatDBAddress(string sAddress, int bOffset, int iDBXBit = 0,bool bDBXSwitch=false)
        {
            string sHeadAddress = "";
            int iEndAddress = -1;
            string sNewAddr;
            string[] addressGroup = sAddress.Split(new char[] { ',','.' });

            for (int i = 0; i < addressGroup[1].Length; i++)
            {
                bool res = int.TryParse(addressGroup[1].Substring(i), out iEndAddress);
                if (res)
                {
                    sHeadAddress = addressGroup[1].Substring(0, i);
                    break;
                }
            }
            iEndAddress = iEndAddress + bOffset;
            if (bDBXSwitch)
            {
                sHeadAddress = "DBX";
                if (addressGroup.Length == 3)
                {
                    sNewAddr = addressGroup[0] + "." + sHeadAddress + iEndAddress + "." + addressGroup[2];
                }
                else
                {
                    sNewAddr = addressGroup[0] + "." + sHeadAddress + iEndAddress + "." + iDBXBit;
                }
            }
            else
            {
                if (addressGroup.Length == 3)
                    sNewAddr = addressGroup[0] + "." + sHeadAddress + iEndAddress + "." + addressGroup[2];
                else
                    sNewAddr = addressGroup[0] + "." + sHeadAddress + iEndAddress;

            }
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
                                   where (p.Field<string>(sSelectConveyorColName) == sSelectColVal)
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
        public string sTypeDescription;
        public string sElementName;
        public string sStyleIdentifier;
        public string sBehaviorName;
        public List<string> sSignalMapping=new List<string>();
        public List<string> sSignalAddress=new List<string>();
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
        public Dictionary<string, string> sSignalMapping_Adderss = new Dictionary<string, string>();
        //创建一个taglist数据行
        public ConveyorRow()
        {

        }
        public  ConveyorRow (TaglistColumns columns,DataRow dataRow)
        {
            if (dataRow[0].ToString() != "")
            {
                if (columns.sSystem=="")
                    sSystem = "";
                else
                    sSystem = dataRow[columns.sSystem].ToString();
                if (columns.sPLC=="")
                    sPLC = "";
                else
                    sPLC = dataRow[columns.sPLC].ToString();
                if (columns.sPowerBox=="")
                    sPowerBox = "";
                else
                    sPowerBox = dataRow[columns.sPowerBox].ToString();
                if (columns.sEquipmentLine=="")
                    sEquipmentLine = "";
                else
                    sEquipmentLine = dataRow[columns.sEquipmentLine].ToString();
                if (columns.sElementType=="")
                    sElementType = "";
                else
                    sElementType = dataRow[columns.sElementType].ToString();
                if (columns.sTypeDescription=="")
                    sTypeDescription = "";
                else
                    sTypeDescription = dataRow[columns.sTypeDescription].ToString();
                if (columns.sElementName=="")
                    sElementName = "";
                else
                    sElementName = dataRow[columns.sElementName].ToString();
                if (columns.sStyleIdentifier=="")
                    sStyleIdentifier = "";
                else
                    sStyleIdentifier = dataRow[columns.sStyleIdentifier].ToString();
                if (columns.sBehaviorName=="")
                    sBehaviorName = "";
                else
                    sBehaviorName = dataRow[columns.sBehaviorName].ToString();
                foreach (string signalMapping in columns.sSignalMapping)
                {
                    if (signalMapping!="")
                    {
                        string name = dataRow[signalMapping].ToString();
                        sSignalMapping.Add(name);
                    }
                      
                }
                foreach (string signalAddress in columns.sSignalAddress)
                {
                    if (signalAddress!="")
                    {
                        string address = dataRow[signalAddress].ToString();
                        sSignalAddress.Add(address);
                    }
                }
                if (columns.sCommandMapping=="")
                    sCommandMapping = "";
                else
                    sCommandMapping = dataRow[columns.sCommandMapping].ToString();
                if (columns.sCommandAddress=="")
                    sCommandAddress = "";
                else
                    sCommandAddress = dataRow[columns.sCommandAddress].ToString();
                if (columns.sRunningHours=="")
                    sRunningHours = "";
                else
                    sRunningHours = dataRow[columns.sRunningHours].ToString();
                if (columns.sCopyRunningHours=="")
                    sCopyRunningHours = "";
                else
                    sCopyRunningHours = dataRow[columns.sCopyRunningHours].ToString();
                if(columns.sDisplayName=="")
                    sDisplayName ="";
                else
                    sDisplayName = dataRow[columns.sDisplayName].ToString();
                if (columns.sEdgeColor=="")
                    sEdgeColor = "";
                else
                    sEdgeColor = dataRow[columns.sEdgeColor].ToString();
                if (columns.sAlarmTree=="")
                    sAlarmTree = "";
                else
                    sAlarmTree = dataRow[columns.sAlarmTree].ToString();
                if (columns.sLevel1View=="")
                    sLevel1View = "";
                else
                    sLevel1View = dataRow[columns.sLevel1View].ToString();
                if (columns.sLevel2View=="")
                    sLevel2View = "";
                else
                    sLevel2View = dataRow[columns.sLevel2View].ToString();
                if (columns.sDrawOnViews=="")
                    sDrawOnViews = "";
                else
                    sDrawOnViews = dataRow[columns.sDrawOnViews].ToString();
                if (columns.sLeftClick=="")
                    sLeftClick = "";
                else
                    sLeftClick = dataRow[columns.sLeftClick].ToString();
                if (columns.sRightClick=="")
                    sRightClick = "";
                else
                    sRightClick = dataRow[columns.sRightClick].ToString();
                if (columns.sLevel1AsLevel2=="")
                    sLevel1AsLevel2 = "";
                else
                    sLevel1AsLevel2 = dataRow[columns.sLevel1AsLevel2].ToString();
                if (columns.sExtendedPropertyAsCamera=="")
                    sExtendedPropertyAsCamera = "";
                else
                    sExtendedPropertyAsCamera = dataRow[columns.sExtendedPropertyAsCamera].ToString();

            }

            //生成SignalMapping与Adderss的键值对
            for (int i = 0; i < sSignalMapping.Count; i++)
            {
                if(sSignalMapping[i] !="" && !sSignalMapping_Adderss.ContainsKey(sSignalMapping[i]))
                sSignalMapping_Adderss.Add(sSignalMapping[i], sSignalAddress[i]);
            }

        }
    }
}
