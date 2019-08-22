

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;

namespace BGFusionTools.Datas
{
    public class ConfigData : BaseData
    {


       // private bool bConvAlarms;
       // private bool bOPCInfo;
        //ALarmLinkOPCInfo.xml列名定义
        private List<string> sListColName;

        //初始化函数
        public ConfigData(BaseParameter ConverParameter, List<string> slistColName)
        {
            baseParameter = ConverParameter;
           // this.bConvAlarms = bconvAlarms;
            //this.bOPCInfo = boPCInfo;
            this.sListColName = slistColName;
        }
        public override List<List<string>> CreateList(CreateDataRow<List<string>,ConveyorRow> dr)
        {
            List<List<string>> lOutPut = new List<List<string>>();
            lOutPut.Add(sListColName);
            foreach (DataRow selectConRow in baseParameter.TaglistTable.Rows)
            {
                var conveyor = new ConveyorRow(baseParameter.TaglistColName, selectConRow);
                lOutPut.AddRange(dr(conveyor));
            }
            return lOutPut;
        }
       public List<List<string>> CreateOPCInfoRows(ConveyorRow coneyorRow)
        {
            List<List<string>> opcInfoRows = new List<List<string>>();
            List<string> lSignalName = new List<string>();
            foreach (string sSignalMapping in coneyorRow.sSignalMapping)
            {
                if (sSignalMapping != "")
                {
                    var SignalCounts = baseParameter.SingleMappingTable.AsEnumerable().Count(p =>
                    p.Field<string>(baseParameter.SignalMappingColName.sType) == sSignalMapping);
                    List<string> lSignalName1 = sSignalName(SignalCounts, baseParameter.Stemp5, coneyorRow, lSignalName.Count() + 1);
                    lSignalName.AddRange(lSignalName1);
                }
            }
            foreach (string ss in lSignalName)
            {
                List<string> opcInfoRow = new List<string>();
                opcInfoRow.Add(ss);
                opcInfoRow.Add("Subscribe");
                opcInfoRow.Add(string.Format("S7_{0}", coneyorRow.sPLC));
                opcInfoRow.Add("Main");
                opcInfoRow.Add("u32");
                opcInfoRows.Add(opcInfoRow);
            }

            if (coneyorRow.sCommandMapping != "")
            {
                var CommandCounts = baseParameter.CommandMappingTable.AsEnumerable().Count(p =>
                p.Field<string>(baseParameter.CommandMappingColName.sType) == coneyorRow.sCommandMapping);
                List<string> lSingName = sSignalName(CommandCounts, baseParameter.Stemp6, coneyorRow, 1);
                foreach (string ss in lSingName)
                {
                    List<string> opcInfoRow = new List<string>();
                    opcInfoRow.Add(ss);
                    opcInfoRow.Add("Command");
                    opcInfoRow.Add(string.Format("S7_{0}", coneyorRow.sPLC));
                    opcInfoRow.Add("Main");
                    opcInfoRow.Add("u32");
                    opcInfoRows.Add(opcInfoRow);
                }
            }
            return opcInfoRows;
        }
        /// <summary>
        /// 创建AlarmList行
        /// </summary>
        /// <param name="coneyorRow"></param>
        /// <returns></returns>
        public List<List<string>> CreateAlarmListRows(ConveyorRow coneyorRow)
        {
            List<List<string>> aLarmListRows = new List<List<string>>();
            string sSelectColName = baseParameter.SignalMappingColName.sType;
            string _sSignalName;
            string _sAckType;
            string _sAlarmTag;
            string _sPartName;
            string _sAlarmDes;
            string _sAlarmGrp;
            string _sAlarmType = "";
            string _sAlarmCategory;
            string _sDelayed;
            string _sConditionName;
            string _sPriority;
            string _sGeneralComment;
            string _sLevel1View;
            string _sLevel2View;
            string _sResetable;
            string _sResetBit;
            string _sALNumber;
            string _sCCTVRecording;
            string _sElementID;
            string _sResetSignal;
            string _sExtraTagList;
            string _sCISData;
            string _sTechnical;
            List<string> lSignalName = new List<string>();
            foreach (string sSignalMapping in coneyorRow.sSignalMapping)
            {
                if (sSignalMapping != "")
                {
                    var SignalCounts = baseParameter.SingleMappingTable.AsEnumerable().Count(p =>
                    p.Field<string>(baseParameter.SignalMappingColName.sType) == sSignalMapping);
                    var SingMappingRows = from p in baseParameter.SingleMappingTable.AsEnumerable()
                                          where p.Field<string>(sSelectColName) == sSignalMapping
                                          select p;
                    List<string> lSignalName1 = sSignalName(SignalCounts, baseParameter.Stemp5, coneyorRow, lSignalName.Count() + 1);

                    foreach (DataRow dr in SingMappingRows)
                    {
                        if (dr[4].ToString().Substring(0, 2) == "AL")
                        {
                            int indexSingal = Convert.ToInt32(dr[1]);
                            _sSignalName = lSignalName1[indexSingal - 1];
                            int iAlarmPriority = Convert.ToInt32(dr[13]);
                            if (iAlarmPriority <= 40)
                                _sAckType = "AutoAck";
                            else
                                _sAckType = "Normal";
                            _sAlarmTag = string.Format("{0}_{1}_{2}_{3}_{4}", coneyorRow.sSystem, coneyorRow.sPLC, coneyorRow.sEquipmentLine,
                                            coneyorRow.sElementName, dr[4].ToString());
                            string sPart = dr[5].ToString();
                            Regex reg = new Regex(@"\d");
                            Match match = reg.Match(coneyorRow.sPLC);
                            string sCISPlcLink = reg.Replace(coneyorRow.sPLC, string.Format("#{0}", match.Groups[0].Value), 1);
                            match = reg.Match(coneyorRow.sElementName);
                            string sCISEquipmentElement = reg.Replace(coneyorRow.sElementName, string.Format("#{0}", match.Groups[0].Value), 1);

                            if (sPart != "Spare")
                            {
                                _sAlarmTag = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", coneyorRow.sSystem, coneyorRow.sPLC, coneyorRow.sEquipmentLine,
                                            coneyorRow.sElementName, sPart, dr[4].ToString());
                                _sPartName = coneyorRow.sDisplayName + "." + sPart;
                                match = reg.Match(sPart);
                                string sCISPart = reg.Replace(sPart, string.Format("#{0}", match.Groups[0].Value), 1);
                                _sCISData = string.Format("{0}#.{1}.{2}#.{3}.{4}", coneyorRow.sSystem, sCISPlcLink, coneyorRow.sEquipmentLine, sCISEquipmentElement, sCISPart);

                            }

                            else
                            {
                                _sAlarmTag = string.Format("{0}_{1}_{2}_{3}_{4}", coneyorRow.sSystem, coneyorRow.sPLC, coneyorRow.sEquipmentLine,
                                       coneyorRow.sElementName, dr[4].ToString());

                                _sPartName = coneyorRow.sDisplayName;
                                _sCISData = string.Format("{0}#.{1}.{2}#.{3}", coneyorRow.sSystem, sCISPlcLink, coneyorRow.sEquipmentLine, sCISEquipmentElement);

                            }

                            _sAlarmDes = dr[9].ToString();
                            _sAlarmGrp = coneyorRow.sAlarmTree;
                            if (iAlarmPriority <= 20)
                                _sAlarmType = "Alarm Information";
                            else if (iAlarmPriority <= 40)
                                _sAlarmType = "Alarm Trivial";
                            else if (iAlarmPriority <= 60)
                                _sAlarmType = "Alarm Minor";
                            else if (iAlarmPriority <= 80)
                                _sAlarmType = "Alarm Major";
                            else if (iAlarmPriority > 80)
                                _sAlarmType = "Alarm Critical";
                            _sAlarmCategory = "";
                            _sDelayed = "0";
                            int iBitVal = (int)Math.Log(Convert.ToInt64(dr[3]), 2);
                            if (iBitVal < 10)
                                _sConditionName = string.Format("AlarmWordBit0{0}", iBitVal);
                            else
                                _sConditionName = string.Format("AlarmWordBit{0}", iBitVal);

                            _sPriority = iAlarmPriority.ToString();
                            _sGeneralComment = "";
                            _sLevel1View = coneyorRow.sLevel1View;
                            _sLevel2View = coneyorRow.sLevel2View;
                            _sResetable = "-";
                            _sResetBit = iBitVal.ToString();
                            _sALNumber = dr[4].ToString();
                            _sCCTVRecording = "-";
                            _sElementID = string.Format("{0}_{1}_{2}_{3}", coneyorRow.sSystem, coneyorRow.sPLC, coneyorRow.sEquipmentLine, coneyorRow.sElementName);
                            _sResetSignal = "";
                            _sExtraTagList = "";
                            _sTechnical = "Flase";
                            List<string> alarmListRow = new List<string>();
                            alarmListRow.Add(_sSignalName);
                            alarmListRow.Add(_sAckType);
                            alarmListRow.Add(_sAlarmTag);
                            alarmListRow.Add(_sPartName);
                            alarmListRow.Add(_sAlarmDes);
                            alarmListRow.Add(_sAlarmGrp);
                            alarmListRow.Add(_sAlarmType);
                            alarmListRow.Add(_sAlarmCategory);
                            alarmListRow.Add(_sDelayed);
                            alarmListRow.Add(_sConditionName);
                            alarmListRow.Add(_sPriority);
                            alarmListRow.Add(_sGeneralComment);
                            alarmListRow.Add(_sLevel1View);
                            alarmListRow.Add(_sLevel2View);
                            alarmListRow.Add(_sResetable);
                            alarmListRow.Add(_sResetBit);
                            alarmListRow.Add(_sALNumber);
                            alarmListRow.Add(_sCCTVRecording);
                            alarmListRow.Add(_sElementID);
                            alarmListRow.Add(_sResetSignal);
                            alarmListRow.Add(_sExtraTagList);
                            alarmListRow.Add(_sCISData);
                            alarmListRow.Add(_sTechnical);
                            aLarmListRows.Add(alarmListRow);
                        }
                    }
                }
            }
            return aLarmListRows;
        }
        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}

