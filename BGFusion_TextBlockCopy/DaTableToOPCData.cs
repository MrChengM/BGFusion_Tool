using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BGFusion_TextBlockCopy
{
    public class DaTableToOPCData
    {
        private DataTable MainTable;
        //public string[,] MainColName{ get; set; }
        private string[,] MainColName;
        //public int ViewNum { get; set; }
        //private int ViewNum;
        //public string sTemp0 { get; set; }
        private string sTemp0;
        //public string sTemp1 { get; set; }
        private string sTemp1;
        //public string sTemp2 { get; set; }
        private string sTemp2;
        //public string sTemp3 { get; set; }
        //private string sTemp3;
        //public DataTable SecondTable { get; set; }
        private DataTable SecondTable0;
        //public DataTable SecondTable1 { get; set; }
        private DataTable SecondTable1;
        //public string[,] SecondColName { get; set; }
        private string[,] SecondColName;

        private bool bSingle;
        private bool bCommand;
        private bool bHour;
       

        //OPCDataList列名定义，数据类型/读写等级/扫描周期等定义
        DataTable dOPCdataTable = new DataTable("OPCData");
        private string[] sOPCListColName ={"Tag Name","Address", "Data Type" ,
            "Respect Data Type" , "Client Access" , "Scan Rate", "Scaling",
            "Raw Low", "Raw High", "Scaled Low", "Scaled High", "Scaled Data Type",
            "Clamp Low", "Clamp High", "Eng Units","Description","Negate Value" };
        private const int iReDataType = 1;
        private string[] sClienAccess = { "R","R/W" };
        private const int iScanRate = 100;
        private string[] sDataType = {"String","Boolean","Char","Byte","Short","Word"
                ,"Long","DWord","Float","Double","BCD","LBCD","Date"};

        private const int iAdderssIndex = 9;



        //初始化函数
        public DaTableToOPCData(DataTable mainTable,DataTable secondtable0, DataTable secondtable1,
            string[,]mainColName,string[,]secondColName,string stemp0,string stemp1,string stemp2,
            bool bsingle ,bool bcommand,bool bhour )
        {
            this.MainTable = mainTable;
            this.MainColName = mainColName;
            this.SecondTable0 = secondtable0;
            this.SecondTable1 = secondtable1;
            this.SecondColName = secondColName;
            this.sTemp0 = stemp0;
            this.sTemp1 = stemp1;
            this.sTemp2 = stemp2;
            this.bSingle = bsingle;
            this.bCommand = bcommand;
            this.bHour = bhour;
        }

        //生成DataTable数据
        public DataTable dOutData()
        {

            try
            {
                foreach (string sColumName in sOPCListColName)
                    dOPCdataTable.Columns.AddRange(new DataColumn[] { new DataColumn(sColumName) });
                foreach (DataRow selectConRow in MainTable.Rows)
                {
                    if (selectConRow[MainColName[1, 0]].ToString() == "")
                        break;
                    string sSystem = selectConRow[MainColName[1, 0]].ToString();
                    string sPlcLink = selectConRow[MainColName[1, 1]].ToString();
                    string sEquipmentLine = selectConRow[MainColName[1, 3]].ToString();
                    string sEquipmentElement = selectConRow[MainColName[1, 4]].ToString();
                    string sEquipmentElementtype = selectConRow[MainColName[1, 5]].ToString();
                    string sSingleMapping1 = selectConRow[MainColName[1, 6]].ToString();
                    string sSignalAddress1 = selectConRow[MainColName[1, 7]].ToString();
                    string sCommandMapping = selectConRow[MainColName[1, 8]].ToString();
                    string sCommandAddress = selectConRow[MainColName[1, 9]].ToString();
                    string sRunningHours = selectConRow[MainColName[1, 10]].ToString();
                    if (bSingle && (sSignalAddress1 != ""))
                    {
                        var SingleCounts = SecondTable0.AsEnumerable().Count(p => p.Field<string>(SecondColName[1, 0]) == sSingleMapping1);
                        DataRow[] OPCDataRows = OPCDataRow(SingleCounts, sTemp0, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, sSignalAddress1);
                        foreach (DataRow dr in OPCDataRows)
                            dOPCdataTable.Rows.Add(dr);
                    }
                    if (bCommand && (sCommandAddress != ""))
                    {
                        var CommandCounts = SecondTable1.AsEnumerable().Count(p => p.Field<string>(SecondColName[2, 0]) == sCommandMapping);
                        DataRow[] OPCDataRows = OPCDataRow(CommandCounts, sTemp1, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, sCommandAddress);
                        foreach (DataRow dr in OPCDataRows)
                            dOPCdataTable.Rows.Add(dr);
                    }
                    if (bHour && (sRunningHours != ""))
                    {
                        int iHourCounts = 32;
                        DataRow[] OPCDataRows = OPCDataRow(iHourCounts, sTemp2, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, sRunningHours);
                        foreach (DataRow dr in OPCDataRows)
                            dOPCdataTable.Rows.Add(dr);
                    }
                    //float dCounts = SingleCounts / 32;

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("数据处理错误：" + ex.Message);
            }

          
            return dOPCdataTable;
        }

        //根据Mapping判断需要的数据地址条数，生成对应的DataRow.
        private DataRow[] OPCDataRow(int iByteCounts,string sTemp, string sSystem,string sPlcLink,string sEquipmentLine,
            string sEquipmentElement,string sAddress)
        {
            string sTagName;
            int iCounts;
            string sDaType=default(string);
            DataRow[] drs;
            if (iByteCounts <= 32)
            {

                sTagName = string.Format(sTemp, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, 1);
                iCounts = (int)Math.Ceiling((float)iByteCounts / 8);
                switch (iCounts)
                {
                    case 0:
                        break;
                    case 1:
                        sDaType = sDataType[3];
                        break;
                    case 2:
                        sDaType = sDataType[5];
                        break;
                    case 3:
                        break;
                    case 4:
                        sDaType = sDataType[7];
                        break;
                }
                DataRow dr = dOPCdataTable.NewRow();
                dr[sOPCListColName[0]] = sTagName;
                dr[sOPCListColName[1]] = sAddress;
                dr[sOPCListColName[2]] = sDaType;
                dr[sOPCListColName[3]] = iReDataType;
                dr[sOPCListColName[4]] = sClienAccess[1];
                dr[sOPCListColName[5]] = iScanRate;
                drs = new DataRow[1];
                drs[0] = dr;
            }
            else
            {
                iCounts = (int)Math.Ceiling((float)iByteCounts / 32);
                drs = new DataRow[iCounts];
                for (int i = 0; i < iCounts; i++)
                {
                    sTagName = string.Format(sTemp, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, i+1);
                    sDaType = sDataType[7];
                    drs[i] = dOPCdataTable.NewRow();
                    drs[i][sOPCListColName[0]] = sTagName;
                    string sAddr = soffsetAddress(sAddress, i * 4);
                    drs[i][sOPCListColName[1]] = sAddr;
                    drs[i][sOPCListColName[2]] = sDaType;
                    drs[i][sOPCListColName[3]] = iReDataType;
                    drs[i][sOPCListColName[4]] = sClienAccess[1];
                    drs[i][sOPCListColName[5]] = iScanRate;

                }
            }
            return drs;
        }

        //计算地址偏移
        private string soffsetAddress(string sAddress,int bOffset)
        {
            int iEndAddress;
            int iposition = sAddress.IndexOf("DBD") != -1 ? 
                sAddress.IndexOf("DBD") : sAddress.IndexOf("DBB") != -1 ? 
                sAddress.IndexOf("DBB") : sAddress.IndexOf("DBW") != -1 ? 
                sAddress.IndexOf("DBW") :-1;
            string sUpAddress = sAddress.Substring(0, iposition +3);
            bool res  = Int32.TryParse(sAddress.Substring(iposition +3),out iEndAddress);
            iEndAddress = iEndAddress + bOffset;
            string sNewAddr = sUpAddress + iEndAddress;
            return sNewAddr;
        }
    }
}
