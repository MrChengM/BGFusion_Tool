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
    public class OPCData : BaseData
    {

        private bool bSignal;
        private bool bCommand;
        private bool bHour;


        OPCColName oPCColName = new OPCColName() { };

        private const int iReDataType = 1;
        private const string sReadOnly = "RO";
        private const int iScanRate = 100;
        private const int iAdderssIndex = 9;



        //初始化函数
        public OPCData(BaseParameter ConverParameter, bool bsingle, bool bcommand, bool bhour)
        {

            this.baseParameter = ConverParameter;
            this.bSignal = bsingle;
            this.bCommand = bcommand;
            this.bHour = bhour;
        }

        //生成DataTable数据
        public override List<List<string>> CreateList(CreateDataMath<List<string>,ConveyorRow> dataMath)
        {
            List<List<string>> lOutPut = new List<List<string>>();
            List<string> opcColunms = new List<string>();
            foreach (string colunm in oPCColName)
                opcColunms.Add(colunm);
            lOutPut.Add(opcColunms);
            foreach (DataRow selectConRow in baseParameter.TaglistTable.Rows)
            {
                var conveyor = new ConveyorRow(baseParameter.TaglistColName, selectConRow);
                lOutPut.AddRange(dataMath(conveyor));
            }
            return lOutPut;
        }
        public List<List<string>> CreateOPCRows(ConveyorRow conveyor)
        {
            List<List<string>> OPCRows = new List<List<string>>();
            foreach (string signalMapping in conveyor.sSignalMapping)
            {
                int i = 0;
                int signalIndex = 1;
                if (bSignal && (signalMapping != ""))
                {

                    var counts = baseParameter.SingleMappingTable.AsEnumerable().Count(p => p.Field<string>(baseParameter.SignalMappingColName.sType) == signalMapping);
                    List<List<string>> OPCDataRows = CreateOPCRows(counts, baseParameter.Stemp5, conveyor, conveyor.sSignalAddress[i], signalIndex);
                    signalIndex = signalIndex + OPCDataRows.Count();
                    OPCRows.AddRange(OPCDataRows);
                }
                i++;
            }

            if (bCommand && (conveyor.sCommandAddress != ""))
            {
                var CommandCounts = baseParameter.CommandMappingTable.AsEnumerable().Count(p => p.Field<string>(baseParameter.CommandMappingColName.sType) == conveyor.sCommandMapping);
                List<List<string>> OPCDataRows = CreateOPCRows(CommandCounts, baseParameter.Stemp6, conveyor, conveyor.sCommandAddress, 1);
                OPCRows.AddRange(OPCDataRows);
            }
            if (bHour && (conveyor.sRunningHours != ""))
            {
                int iHourCounts = 32;
                List<List<string>> OPCDataRows = CreateOPCRows(iHourCounts, baseParameter.Stemp7, conveyor, conveyor.sRunningHours, 1);
                OPCRows.AddRange(OPCDataRows);
            }
            return OPCRows;
        }

        //生成对应的OPCData行数组
        private DataRow[] CreateOPCRows1(int iBits, string sTemp, string sSystem, string sPlcLink, string sEquipmentLine,
            string sEquipmentElement, string sAddress, int index)
        {
            DataTable dOPCdataTable = new DataTable("OPCTable");
            string sTagName;
            int iCounts;
            string sDaType = default(string);
            DataRow[] drs;
            if (iBits <= 32)
            {

                sTagName = string.Format(sTemp, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, index);
                iCounts = (int)Math.Ceiling((float)iBits / 8);
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
                iCounts = (int)Math.Ceiling((float)iBits / 32);
                drs = new DataRow[iCounts];
                for (int i = 0; i < iCounts; i++)
                {
                    sTagName = string.Format(sTemp, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, i + index);
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

        private List<List<string>> CreateOPCRows(int iBits, string sTemp, ConveyorRow conveyor, string sAddress, int index)
        {
            OPCDataRow opcRow = new OPCDataRow();
            List<List<string>> opcDataRows = new List<List<string>>();
            string sTagName;
            int iCounts;
            string sDaType = default(string);
            if (iBits <= 32)
            {
                List<string> opcDataRow = new List<string>();
                sTagName = string.Format(sTemp, conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName, index);
                iCounts = (int)Math.Ceiling((float)iBits / 8);
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
                opcRow.sTagName = sTagName;
                opcRow.sAddress = sAddress;
                opcRow.sDataType = sDaType;
                foreach (string s in opcRow)
                {
                    opcDataRow.Add(s);
                }
                opcDataRows.Add(opcDataRow);
            }
            else
            {
                iCounts = (int)Math.Ceiling((float)iBits / 32);
                for (int i = 0; i < iCounts; i++)
                {
                    List<string> opcDataRow = new List<string>();
                    sTagName = string.Format(sTemp, conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName, i + index);
                    sDaType = OPCDataType.sDWord;
                    opcRow.sTagName = sTagName;
                    string sAddr = soffsetAddress(sAddress, i * 4);
                    opcRow.sAddress = sAddress;
                    opcRow.sDataType = sDaType;
                    foreach (string s in opcRow)
                    {
                        opcDataRow.Add(s);
                    }
                    opcDataRows.Add(opcDataRow);

                }
            }
            return opcDataRows;
        }
        /// <summary>
        /// 实现了迭代的OPC列名
        /// </summary>
        public class OPCColName : IEnumerable<string>
        {
            public const string sTagName = "Tag Name";
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
        public class OPCDataRow : IEnumerable<string>
        {
            public string sTagName = "";
            public string sAddress = "";
            public string sDataType = "";
            public string sRespectDataType = "1";
            public string sClientAccess = "R/W";
            public string sScanRate = "100";
            public string sScaling = "";
            public string sRawLow = "";
            public string sRawHigh = "";
            public string sScaledLow = "";
            public string sScaledHigh = "";
            public string sScaledDataType = "";
            public string sClampLow = "";
            public string sClampHigh = "";
            public string sEngUnits = "";
            public string sDescription = "";
            public string sNegateValue = "";

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
    }
}
