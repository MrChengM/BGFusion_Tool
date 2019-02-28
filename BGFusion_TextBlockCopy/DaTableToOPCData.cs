using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BGFusion_TextBlockCopy
{
    public class DaTableToOPCData:BaseTableConvert
    {

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
        public DaTableToOPCData(DaTableConverParameter ConverParameter, bool bsingle ,bool bcommand,bool bhour )
        {

            this.baseTableConverParameter = ConverParameter;
            this.bSingle = bsingle;
            this.bCommand = bcommand;
            this.bHour = bhour;
        }

        //生成DataTable数据
        public override DataTable dOutData()
        {

            try
            {
                foreach (string sColumName in sOPCListColName)
                    dOPCdataTable.Columns.AddRange(new DataColumn[] { new DataColumn(sColumName) });
                foreach (DataRow selectConRow in baseTableConverParameter.TaglistTable.Rows)
                {
                    if (selectConRow[baseTableConverParameter.TaglistColName[1, 0]].ToString() == "")
                        break;
                    string sSystem = selectConRow[baseTableConverParameter.TaglistColName[1, 0]].ToString();
                    string sPlcLink = selectConRow[baseTableConverParameter.TaglistColName[1, 1]].ToString();
                    string sEquipmentLine = selectConRow[baseTableConverParameter.TaglistColName[1, 3]].ToString();
                    string sEquipmentElement = selectConRow[baseTableConverParameter.TaglistColName[1, 4]].ToString();
                    string sEquipmentElementtype = selectConRow[baseTableConverParameter.TaglistColName[1, 5]].ToString();
                    string sSingleMapping1 = selectConRow[baseTableConverParameter.TaglistColName[1, 6]].ToString();
                    string sSignalAddress1 = selectConRow[baseTableConverParameter.TaglistColName[1, 7]].ToString();
                    string sCommandMapping = selectConRow[baseTableConverParameter.TaglistColName[1, 8]].ToString();
                    string sCommandAddress = selectConRow[baseTableConverParameter.TaglistColName[1, 9]].ToString();
                    string sRunningHours = selectConRow[baseTableConverParameter.TaglistColName[1, 10]].ToString();
                    if (bSingle && (sSignalAddress1 != ""))
                    {
                        var SingleCounts = baseTableConverParameter.SingleMappingTable.AsEnumerable().Count(p => p.Field<string>(baseTableConverParameter.BasefileColName[1, 0]) == sSingleMapping1);
                        DataRow[] OPCDataRows = OPCDataRow(SingleCounts,baseTableConverParameter.Stemp5, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, sSignalAddress1);
                        foreach (DataRow dr in OPCDataRows)
                            dOPCdataTable.Rows.Add(dr);
                    }
                    if (bCommand && (sCommandAddress != ""))
                    {
                        var CommandCounts = baseTableConverParameter.CommandMappingTable.AsEnumerable().Count(p => p.Field<string>(baseTableConverParameter.BasefileColName[2, 0]) == sCommandMapping);
                        DataRow[] OPCDataRows = OPCDataRow(CommandCounts, baseTableConverParameter.Stemp6, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, sCommandAddress);
                        foreach (DataRow dr in OPCDataRows)
                            dOPCdataTable.Rows.Add(dr);
                    }
                    if (bHour && (sRunningHours != ""))
                    {
                        int iHourCounts = 32;
                        DataRow[] OPCDataRows = OPCDataRow(iHourCounts, baseTableConverParameter.Stemp7, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, sRunningHours);
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

        //生成对应的OPCData行数组
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

        public override string sOutData()
        {
            throw new NotImplementedException();
        }

        public override int iOutData()
        {
            throw new NotImplementedException();
        }

        public override List<List<string>> lOutData()
        {
            throw new NotImplementedException();
        }
        public override void OutData()
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, string> diOutData()
        {
            throw new NotImplementedException();
        }
    }
}
