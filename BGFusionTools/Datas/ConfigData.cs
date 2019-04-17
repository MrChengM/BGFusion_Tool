

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
    public class ConfigData :BaseData
    {


        private bool bConvAlarms;
        private bool bOPCInfo;
        //ALarmLinkOPCInfo.xml列名定义
        private List<string> sListColName;



        //初始化函数
        public ConfigData(BaseParameter ConverParameter,bool bconvAlarms,bool boPCInfo, List<string> slistColName)
        {
            baseParameter = ConverParameter;
            this.bConvAlarms = bconvAlarms;
            this.bOPCInfo = boPCInfo;
            this.sListColName = slistColName;
        }

        //生成DataTable数据
        public override List<List<string>> ToList()
        {
            List<List<string>> llOutData = new List<List<string>>();
            try
            {

                llOutData.Add(sListColName);
                foreach (DataRow selectConRow in baseParameter.TaglistTable.Rows)
                {

                    if (selectConRow[baseParameter.TaglistColName[1, 0]].ToString() == "")
                        break;
                    string sSystem = selectConRow[baseParameter.TaglistColName[1, 0]].ToString();
                    string sPlcLink = selectConRow[baseParameter.TaglistColName[1, 1]].ToString();
                    string sEquipmentLine = selectConRow[baseParameter.TaglistColName[1, 3]].ToString();
                    string sEquipmentElement = selectConRow[baseParameter.TaglistColName[1, 4]].ToString();
                    string sEquipmentElementtype = selectConRow[baseParameter.TaglistColName[1, 5]].ToString();
                    string sSingleMapping1 = selectConRow[baseParameter.TaglistColName[1, 6]].ToString();
                    string sSignalAddress1 = selectConRow[baseParameter.TaglistColName[1, 7]].ToString();
                    string sCommandMapping = selectConRow[baseParameter.TaglistColName[1, 8]].ToString();
                    string sCommandAddress = selectConRow[baseParameter.TaglistColName[1, 9]].ToString();
                    string sRunningHours = selectConRow[baseParameter.TaglistColName[1, 10]].ToString();
                    string sDisplayName = selectConRow[baseParameter.TaglistColName[1, 12]].ToString();
                    string sAlarmTree = selectConRow[baseParameter.TaglistColName[1, 14]].ToString();
                    string sLevel1View = selectConRow[baseParameter.TaglistColName[1, 15]].ToString();
                    string sLevel2View = selectConRow[baseParameter.TaglistColName[1, 16]].ToString();
                    var SingleCounts = baseParameter.SingleMappingTable.AsEnumerable().Count(p => 
                    p.Field<string>(baseParameter.BasefileColName[1, 0]) == sSingleMapping1);

                    //float dCounts = SingleCounts / 32;
                    if (bOPCInfo)
                    {
                        if (sSignalAddress1 != "")
                        {
                            List<string> lSingName = sSignalName(SingleCounts, baseParameter.Stemp5, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement);
                            foreach (string ss in lSingName)
                            {
                                List<string> lOutData = new List<string>();
                                lOutData.Add(ss);
                                lOutData.Add("Subscribe");
                                lOutData.Add(string.Format("S7_{0}", sPlcLink));
                                lOutData.Add("Main");
                                lOutData.Add("u32");
                                llOutData.Add(lOutData);
                            }
                        }
                        if (sCommandMapping != "")
                        {
                            var CommandCounts = baseParameter.CommandMappingTable.AsEnumerable().Count(p => p.Field<string>(baseParameter.BasefileColName[2, 0]) == sCommandMapping);

                            List<string> lSingName = sSignalName(CommandCounts, baseParameter.Stemp6, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement);
                            foreach (string ss in lSingName)
                            {
                                List<string> lOutData = new List<string>();
                                lOutData.Add(ss);
                                lOutData.Add("Command");
                                lOutData.Add(string.Format("S7_{0}", sPlcLink));
                                lOutData.Add("Main");
                                lOutData.Add("u32");
                                llOutData.Add(lOutData);
                            }
                        }
                    }
                    if (bConvAlarms)
                    {
                        string sSelectColName = baseParameter.BasefileColName[1, 0];
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
                        List<string> lSingName = sSignalName(SingleCounts, baseParameter.Stemp5, sSystem, sPlcLink,
                            sEquipmentLine, sEquipmentElement);
                        var SingMappingRows = from p in baseParameter.SingleMappingTable.AsEnumerable()
                                              where p.Field<string>(sSelectColName) == sSingleMapping1
                                              select p;
                        //foreach (string sSingName in lSingName)
                        //{
                        foreach (DataRow dr in SingMappingRows)
                        {
                            if (dr[4].ToString().Substring(0, 2) == "AL")
                            {
                                int indexSingal = Convert.ToInt32(dr[1]);
                                _sSignalName = lSingName[indexSingal - 1];
                                int iAlarmPriority = Convert.ToInt32(dr[13]);
                                if (iAlarmPriority <= 40)
                                    _sAckType = "AutoAck";
                                else
                                    _sAckType = "Normal";
                                _sAlarmTag = string.Format("{0}_{1}_{2}_{3}_{4}", sSystem, sPlcLink, sEquipmentLine,
                                                sEquipmentElement, dr[4].ToString());
                                string sPart = dr[5].ToString();
                                Regex reg = new Regex(@"\d");
                                Match match = reg.Match(sPlcLink);
                                string sCISPlcLink = reg.Replace(sPlcLink, string.Format("#{0}", match.Groups[0].Value), 1);
                                match = reg.Match(sEquipmentElement);
                                string sCISEquipmentElement = reg.Replace(sEquipmentElement, string.Format("#{0}", match.Groups[0].Value), 1);

                                if (sPart != "Spare")
                                {
                                    _sAlarmTag = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", sSystem, sPlcLink, sEquipmentLine,
                                                sEquipmentElement, sPart, dr[4].ToString());
                                    _sPartName = sDisplayName + "." + sPart;
                                    match = reg.Match(sPart);
                                    string sCISPart = reg.Replace(sPart, string.Format("#{0}", match.Groups[0].Value), 1);
                                    _sCISData = string.Format("{0}#.{1}.{2}#.{3}.{4}", sSystem, sCISPlcLink, sEquipmentLine, sCISEquipmentElement, sCISPart);

                                }

                                else
                                {
                                    _sAlarmTag = string.Format("{0}_{1}_{2}_{3}_{4}", sSystem, sPlcLink, sEquipmentLine,
                                            sEquipmentElement, dr[4].ToString());

                                    _sPartName = sDisplayName;
                                    _sCISData = string.Format("{0}#.{1}.{2}#.{3}", sSystem, sCISPlcLink, sEquipmentLine, sCISEquipmentElement);

                                }

                                _sAlarmDes = dr[9].ToString();
                                _sAlarmGrp = sAlarmTree;
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
                                _sLevel1View = sLevel1View;
                                _sLevel2View = sLevel2View;
                                _sResetable = "-";
                                _sResetBit = iBitVal.ToString();
                                _sALNumber = dr[4].ToString();
                                _sCCTVRecording = "-";
                                _sElementID = string.Format("{0}_{1}_{2}_{3}", sSystem, sPlcLink, sEquipmentLine, sEquipmentElement);
                                _sResetSignal = "";
                                _sExtraTagList = "";
                                _sTechnical = "Flase";
                                List<string> lOutData = new List<string>();
                                lOutData.Add(_sSignalName);
                                lOutData.Add(_sAckType);
                                lOutData.Add(_sAlarmTag);
                                lOutData.Add(_sPartName);
                                lOutData.Add(_sAlarmDes);
                                lOutData.Add(_sAlarmGrp);
                                lOutData.Add(_sAlarmType);
                                lOutData.Add(_sAlarmCategory);
                                lOutData.Add(_sDelayed);
                                lOutData.Add(_sConditionName);
                                lOutData.Add(_sPriority);
                                lOutData.Add(_sGeneralComment);
                                lOutData.Add(_sLevel1View);
                                lOutData.Add(_sLevel2View);
                                lOutData.Add(_sResetable);
                                lOutData.Add(_sResetBit);
                                lOutData.Add(_sALNumber);
                                lOutData.Add(_sCCTVRecording);
                                lOutData.Add(_sElementID);
                                lOutData.Add(_sResetSignal);
                                lOutData.Add(_sExtraTagList);
                                lOutData.Add(_sCISData);
                                lOutData.Add(_sTechnical);
                                llOutData.Add(lOutData);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("数据处理错误：" + ex.Message);
            }
            return llOutData;
        }



        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public override int ToInt()
        {
            throw new NotImplementedException();
        }

        public override DataTable ToDataTable()
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
}

