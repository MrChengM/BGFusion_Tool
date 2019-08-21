using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BGFusionTools.Datas
{
    public class OPCData:BaseData
    {

        private bool bSignal;
        private bool bCommand;
        private bool bHour;
       

        //OPCDataList列名定义，数据类型/读写等级/扫描周期等定义
        DataTable dOPCdataTable = new DataTable("OPCData");
        OPCColName oPCColName = new OPCColName() { };
        //private string[] sOPCListColName ={"Tag Name","Address", "Data Type" ,
        //    "Respect Data Type" , "Client Access" , "Scan Rate", "Scaling",
        //    "Raw Low", "Raw High", "Scaled Low", "Scaled High", "Scaled Data Type",
        //    "Clamp Low", "Clamp High", "Eng Units","Description","Negate Value" };
        private const int iReDataType = 1;
        private const string sReadOnly ="RO";
        private const int iScanRate = 100;
        //private string[] sDataType = {"String","Boolean","Char","Byte","Short","Word"
        //        ,"Long","DWord","Float","Double","BCD","LBCD","Date"};

        private const int iAdderssIndex = 9;



        //初始化函数
        public OPCData(BaseParameter ConverParameter, bool bsingle ,bool bcommand,bool bhour )
        {

            this.baseParameter = ConverParameter;
            this.bSignal = bsingle;
            this.bCommand = bcommand;
            this.bHour = bhour;
        }

        //生成DataTable数据
        public override DataTable ToDataTable()
        {

            try
            {
                TaglistColumns colName = baseParameter.TaglistColName;
                foreach (string sColumName in oPCColName)
                    dOPCdataTable.Columns.AddRange(new DataColumn[] { new DataColumn(sColumName) });
                foreach (DataRow selectConRow in baseParameter.TaglistTable.Rows)
                {
                    var conveyor=new ConeyorRow(baseParameter.TaglistColName,selectConRow);
                    int i = 0;
                    int signalIndex = 1;
                    foreach (string signalMapping in conveyor.sSignalMapping)
                    {
                        if (bSignal && (signalMapping != ""))
                        {

                            var counts = baseParameter.SingleMappingTable.AsEnumerable().Count(p => p.Field<string>(baseParameter.SignalMappingColName.sType) == signalMapping);
                            DataRow[] OPCDataRows = OPCDataRow(counts, baseParameter.Stemp5, conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName, conveyor.sSignalAddress[i], signalIndex);
                            signalIndex = signalIndex + OPCDataRows.Count();
                            foreach (DataRow dr in OPCDataRows)
                                dOPCdataTable.Rows.Add(dr);
                        }
                        i++;
                    }
                   
                    if (bCommand && (conveyor.sCommandAddress != ""))
                    {
                        var CommandCounts = baseParameter.CommandMappingTable.AsEnumerable().Count(p => p.Field<string>(baseParameter.CommandMappingColName.sType) == conveyor.sCommandMapping);
                        DataRow[] OPCDataRows = OPCDataRow(CommandCounts, baseParameter.Stemp6, conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName, conveyor.sCommandAddress,1);
                        foreach (DataRow dr in OPCDataRows)
                            dOPCdataTable.Rows.Add(dr);
                    }
                    if (bHour && (conveyor.sRunningHours != ""))
                    {
                        int iHourCounts = 32;
                        DataRow[] OPCDataRows = OPCDataRow(iHourCounts, baseParameter.Stemp7, conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName, conveyor.sRunningHours,1);
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
            string sEquipmentElement,string sAddress,int index)
        {
            string sTagName;
            int iCounts;
            string sDaType=default(string);
            DataRow[] drs;
            if (iByteCounts <= 32)
            {

                sTagName = string.Format(sTemp, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, index);
                iCounts = (int)Math.Ceiling((float)iByteCounts / 8);
                switch (iCounts)
                {
                    case 0:
                        break;
                    case 1:
                        sDaType = OPCDataType.sByte;
                        break;
                    case 2:
                        sDaType = OPCDataType.sWord;
                        break;
                    case 3:
                        break;
                    case 4:
                        sDaType = OPCDataType.sDWord;
                        break;
                }
                DataRow dr = dOPCdataTable.NewRow();
                dr[OPCColName.sTagName] = sTagName;
                dr[OPCColName.sAddress] = sAddress;
                dr[OPCColName.sDataType] = sDaType;
                dr[OPCColName.sRespectDataType] = iReDataType;
                dr[OPCColName.sClientAccess] = sReadOnly;
                dr[OPCColName.sScanRate] = iScanRate;
                drs = new DataRow[1];
                drs[0] = dr;
            }
            else
            {
                iCounts = (int)Math.Ceiling((float)iByteCounts / 32);
                drs = new DataRow[iCounts];
                for (int i = 0; i < iCounts; i++)
                {
                    sTagName = string.Format(sTemp, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, i+ index);
                    sDaType = OPCDataType.sDWord;
                    drs[i] = dOPCdataTable.NewRow();
                    drs[i][OPCColName.sTagName] = sTagName;
                    string sAddr = soffsetAddress(sAddress, i * 4);
                    drs[i][OPCColName.sAddress] = sAddr;
                    drs[i][OPCColName.sDataType] = sDaType;
                    drs[i][OPCColName.sRespectDataType] = iReDataType;
                    drs[i][OPCColName.sClientAccess] = sReadOnly;
                    drs[i][OPCColName.sScanRate] = iScanRate;

                }
            }
            return drs;
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public override int ToInt()
        {
            throw new NotImplementedException();
        }

        public override List<List<string>> ToList()
        {
            throw new NotImplementedException();
        }
        public override void OutData()
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, string> ToDictionary()
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 实现了迭代的OPC列名
    /// </summary>
    public  class OPCColName:IEnumerable<string>
    {
        public const string sTagName ="Tag Name";
        public const string sAddress = "Address";
        public const string sDataType = "Data Type";
        public const string sRespectDataType = "Respect Data Type";
        public const string sClientAccess = "Client Access";
        public const string sScanRate = "Scan Rate";
        public const string sScaling = "Scaling";
        public const string sRawLow = "Raw Low";
        public const string sRawHigh = "Raw High";
        public const string sScaledLow = "Scaled Low";
        public const string sScaledHigh = "Scaled High";
        public const string sScaledDataType = "Scaled Data Type";
        public const string sClampLow = "Clamp Low";
        public const string sClampHigh = "Clamp High";
        public const string sEngUnits = "Eng Units";
        public const string sDescription = "Description";
        public const string sNegateValue = "Negate Value";

        public IEnumerator<string> GetEnumerator()
        {
            yield return sTagName;
            yield return sAddress;
            yield return sDataType;
            yield return sRespectDataType;
            yield return sClientAccess;
            yield return sScanRate;
            yield return sScaling;
            yield return sRawLow;
            yield return sRawHigh;
            yield return sScaledLow;
            yield return sScaledHigh;
            yield return sScaledDataType;
            yield return sClampLow;
            yield return sClampHigh;
            yield return sEngUnits;
            yield return sDescription;
            yield return sNegateValue;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    public class OPCDataType
    {
        private string[] sDataType = {"String","Boolean","Char","Byte","Short","Word"
                ,"Long","DWord","Float","Double","BCD","LBCD","Date"};
        public const string sString = "String";
        public const string sBool = "Boolean";
        public const string sChar = "Char";
        public const string sByte = "Byte";
        public const string sShort = "Short";
        public const string sWord = "Word";
        public const string sLong = "Long";
        public const string sDWord = "DWord";
        public const string sFloat = "Float";
        public const string sDouble = "Double";
        public const string sBCD = "BCD";
        public const string sLBCD = "LBCD";
        public const string sDate = "Date";
    }
}
