using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;

namespace BGFusion_TextBlockCopy
{
    class DaTableToTeXml:BaseTableConvert
    {
        private int XmlTypes;

        public DaTableToTeXml(DaTableConverParameter ConverParameter, int XmlTypes)
        {
            this.baseTableConverParameter = ConverParameter;
            this.XmlTypes = XmlTypes;
        }
        public override string sOutData()
        {
            string sOutPutData = null;
            string sOutPutDatas = null;
            List<string> lOutPutDatas = new List<string>();
            EnumerableRowCollection<DataRow> MainRows = LinqToTable();
            try
            {
                int x = 0; int y = 0;
                foreach (DataRow selectConRow in MainRows)
                {
                    int iCounts;
                    string sSystem = selectConRow[0].ToString();
                    string sPlcLink = selectConRow[1].ToString();
                    string sPowrBoxc = selectConRow[2].ToString();
                    string sEquipmentLine = selectConRow[3].ToString();
                    string sEquipmentElement = selectConRow[4].ToString();
                    string sEquipmentElementtype = selectConRow[5].ToString();
                    string sSingleMapping1 = selectConRow[6].ToString();
                    string sSignalAddress1 = selectConRow[7].ToString();
                    string sCommandMapping = selectConRow[8].ToString();
                    string sCommandAddress = selectConRow[9].ToString();
                    string sDisplayNamec = selectConRow[12].ToString();
                    string sAreaLevel2view = selectConRow[16].ToString();
                    string sDrawonViews = selectConRow[17].ToString();
                    string sLeftClick = selectConRow[18].ToString();
                    var SingleCounts = baseTableConverParameter.SingleMappingTable.AsEnumerable().Count(p => p.Field<string>(baseTableConverParameter.BasefileColName[1, 0]) == sSingleMapping1);
                    iCounts = SingleCounts / 32;
                    switch (XmlTypes)
                    {
                        case 0:
                            break;
                        case 1:
                            switch (baseTableConverParameter.ViewNum)
                            {
                                case 0:
                                    break;
                                case 1:
                                    break;
                                case 2:
                                    if (sDrawonViews == "Level2Only" || sDrawonViews == "All")
                                    {
                                        XmlTestBlockCode xmlTestBlockCode = new XmlTestBlockCode();
                                        List<string> lElements = new List<string>();
                                        lElements.Add(string.Format("teblock_{0}_{1}_{2}_{3}", sSystem, sPlcLink, sEquipmentLine, sEquipmentElement));//Name
                                        lElements.Add(string.Format("{0},{1},0,0", x, y));//Margin
                                        //sMarginChange(ref x, ref y, 129, 70);
                                        sMarginChange(ref x, ref y, 0, 0);
                                    if (sEquipmentLine == "MCC01")
                                    {
                                        lElements.Add(string.Format("{0}.{1}.{2}", sPlcLink, sEquipmentLine, sEquipmentElement));//Text
                                    }
                                    else
                                    {
                                        lElements.Add(string.Format("{0}.{1}", sEquipmentLine, sEquipmentElement));//Text
                                    }
                                    xmlTestBlockCode.sTemple = baseTableConverParameter.Stemp3;
                                        xmlTestBlockCode.sElements = lElements;
                                        sOutPutData = xmlTestBlockCode.sOutPutXmlTestBlockCode();
                                        lOutPutDatas.Add(sOutPutData);

                                    }
                                    else
                                {
                                    continue;
                                }
                                    break;
                            }
                            break;
                        case 2:
                            XmlElementCode xmlElementCode = new XmlElementCode();
                            XmlElementFirst xmlElementFirst = new XmlElementFirst();
                            XmlElementThird xmlElementThird ;
                            List<XmlElementThird> xmlElementThirds = new List<XmlElementThird>();
                            XmlElementSix xmlElementSix = new XmlElementSix();
                            XmlElementnine xmlElementnine = new XmlElementnine();
                            switch (baseTableConverParameter.ViewNum)
                            {
                                case 0:
                                    break;
                                case 1:
                                    if (sDrawonViews == "All")
                                    {
                                        //line1
                                        xmlElementFirst.sLineElements.Add(string.Format("{0}_{1}_{2}_{3}", sSystem, sPlcLink, sEquipmentLine, sEquipmentElement));//Name
                                        xmlElementFirst.sLineElements.Add(string.Format("{0},{1},0,0", x, y));//Margin
                                        sMarginChange(ref x, ref y, 70, 70);
                                        xmlElementFirst.sLineElements.Add("{StaticResource Conveyor_Straight_L2}");//sStyle
                                        xmlElementFirst.sLineElements.Add("Legend_Conveyor_Straight");//sLegendStyleName
                                        xmlElementFirst.sLineElements.Add("30");//sWidth
                                        xmlElementFirst.sLineElements.Add("20");//iHeight
                                        xmlElementFirst.sLineElements.Add(string.Format("{0}_{1}_{2}", sSystem, sPlcLink, sEquipmentLine));//sElementName
                                        xmlElementFirst.sLineElements.Add(string.Format("{0}", sEquipmentLine));//sDisplayName
                                        xmlElementFirst.sLineElements.Add("LEVEL1");//sScadaLevel
                                        xmlElementFirst.sLineElements.Add(sSingleMapping1);//sControlObject
                                        xmlElementFirst.sLineElements.Add(sPowrBoxc);//sPowrBox
                                        xmlElementFirst.sLineElements.Add(string.Format("Level3View{0}", sSingleMapping1));//sLevel3View
                                        xmlElementFirst.sLineElements.Add("Navigation");//sChooseLeftClickModesNavigateToView
                                        xmlElementFirst.sLineElements.Add(string.Format("BG_SCADA.Views.Level2.{0}", sAreaLevel2view));//sNavigateToView
                                        xmlElementFirst.sLineElements.Add(sPlcLink);//sPlcName
                                        xmlElementFirst.sLineElements.Add(string.Format("{0}_{1}_{2}_{3}_CM", sSystem, sPlcLink, sEquipmentLine, sEquipmentElement));//sCommandSignalName
                                        xmlElementFirst.sLineElements.Add(sCommandMapping);//CommandMappingType
                                        xmlElementFirst.sLineElements.Add(sEquipmentElementtype);//sTypeDescription
                                                                                                 //line2
                                                                                                 //line3
                                        xmlElementThird = new XmlElementThird();
                                        xmlElementThird.sLineElements.Add(string.Format("Signal{0}", 1));//sId
                                        xmlElementThird.sLineElements.Add("True");//sUsePostfix
                                        xmlElementThird.sLineElements.Add(string.Format("_{0}_{1}_Line_AC", sPlcLink, sAreaLevel2view));//sPostfix
                                        xmlElementThird.sLineElements.Add("False");//sUsePrefix
                                        xmlElementThird.sLineElements.Add("");//sPrefix
                                        xmlElementThird.sLineElements.Add("Constant");//sKeepAliveType
                                        xmlElementThird.sLineElements.Add(string.Format("{0}_{1}_SYSTEM_KAL_MAIN_ACTIVE", sSystem, sPlcLink));//KeepAliveSignal
                                        xmlElementThirds.Add(xmlElementThird);
                                    //line4
                                    //line5
                                    //line6
                                    xmlElementSix.sLineElements.Add(string.Format("{0} StaticResource BG_COLOR_EDGE_{1}{2}", "{", sEquipmentLine, "}"));//Color
                                    //xmlElementSix.sLineElements.Add(string.Format("{ StaticResource BG_COLOR_EDGE_{0}}", sEquipmentLine));//Color
                                                                                                                                              //line7
                                                                                                                                              //line8
                                                                                                                                              //line9
                                        xmlElementnine.sLineElements.Add(string.Format("ScadaBase_Behaviors:BGLineBehavior"));//sBehaviors
                                                                                                                               //line10
                                                                                                                               //line11
                                    }
                                    else
                                {
                                    continue;
                                }
                                    break;
                                case 2:
                                    if (sDrawonViews == "Level2Only" || sDrawonViews == "All")
                                    {
                                        //line1
                                        xmlElementFirst.sLineElements.Add(string.Format("{0}_{1}_{2}_{3}", sSystem, sPlcLink, sEquipmentLine, sEquipmentElement));//Name
                                        xmlElementFirst.sLineElements.Add(string.Format("{0},{1},0,0", x, y));//Margin
                                        sMarginChange(ref x, ref y, 70, 70);
                                        xmlElementFirst.sLineElements.Add("{StaticResource Conveyor_Straight_L2}");//sStyle
                                        xmlElementFirst.sLineElements.Add("Legend_Conveyor_Straight");//sLegendStyleName
                                        xmlElementFirst.sLineElements.Add("30");//sWidth
                                        xmlElementFirst.sLineElements.Add("20");//iHeight
                                        xmlElementFirst.sLineElements.Add(string.Format("{0}_{1}_{2}_{3}", sSystem, sPlcLink, sEquipmentLine, sEquipmentElement));//sElementName
                                        xmlElementFirst.sLineElements.Add(string.Format("{0}", sDisplayNamec));//sDisplayName
                                        xmlElementFirst.sLineElements.Add("LEVEL2");//sScadaLevel
                                        xmlElementFirst.sLineElements.Add(sSingleMapping1);//sControlObject
                                        xmlElementFirst.sLineElements.Add(sPowrBoxc);//sPowrBox
                                        xmlElementFirst.sLineElements.Add(string.Format("Level3View{0}", sSingleMapping1));//sLevel3View
                                        xmlElementFirst.sLineElements.Add(string.Format("{0}", sLeftClick));//sChooseLeftClickModesNavigateToView
                                        xmlElementFirst.sLineElements.Add("");//sNavigateToView
                                        xmlElementFirst.sLineElements.Add(sPlcLink);//sPlcName
                                        xmlElementFirst.sLineElements.Add(string.Format("{0}_{1}_{2}_{3}_CM", sSystem, sPlcLink, sEquipmentLine, sEquipmentElement));//sCommandSignalName
                                        xmlElementFirst.sLineElements.Add(sCommandMapping);//CommandMappingType
                                        xmlElementFirst.sLineElements.Add(sEquipmentElementtype);//sTypeDescription
                                                                                                 //line2
                                        if (iCounts == 0)
                                        {
                                            xmlElementThird = new XmlElementThird();
                                            xmlElementThird.sLineElements.Add(string.Format("Signal{0}", 1));//sId
                                            xmlElementThird.sLineElements.Add("True");//sUsePostfix
                                            xmlElementThird.sLineElements.Add(string.Format("_SIGNAL_{0}", 1));//sPostfix
                                            xmlElementThird.sLineElements.Add("False");//sUsePrefix
                                            xmlElementThird.sLineElements.Add("");//sPrefix
                                            xmlElementThird.sLineElements.Add("Constant");//sKeepAliveType
                                            xmlElementThird.sLineElements.Add(string.Format("{0}_{1}_SYSTEM_KAL_MAIN_ACTIVE", sSystem, sPlcLink));//KeepAliveSignal
                                            xmlElementThirds.Add(xmlElementThird);
                                        }
                                        else
                                        { 
                                            for (int i = 1; i <= iCounts; i++)
                                            {
                                                xmlElementThird = new XmlElementThird();
                                                xmlElementThird.sLineElements.Add(string.Format("Signal{0}", i));//sId
                                                xmlElementThird.sLineElements.Add("True");//sUsePostfix
                                                xmlElementThird.sLineElements.Add(string.Format("_SIGNAL_{0}", i));//sPostfix
                                                xmlElementThird.sLineElements.Add("False");//sUsePrefix
                                                xmlElementThird.sLineElements.Add("");//sPrefix
                                                xmlElementThird.sLineElements.Add("Constant");//sKeepAliveType
                                                xmlElementThird.sLineElements.Add(string.Format("{0}_{1}_SYSTEM_KAL_MAIN_ACTIVE", sSystem, sPlcLink));//KeepAliveSignal
                                                xmlElementThirds.Add(xmlElementThird);
                                            }
                                    }
                                           //line4
                                        //line5
                                        //line6
                                        xmlElementSix.sLineElements.Add(string.Format("{0} StaticResource BG_COLOR_EDGE_{1}{2}", "{", sEquipmentLine, "}"));//Color
                                                                                                                                                            //line7
                                                                                                                                                            //line8
                                                                                                                                                            //line9
                                        xmlElementnine.sLineElements.Add(string.Format("BG_SCADA_Behaviors_Conveyor:{0}Behavior", sSingleMapping1));//sBehaviors
                                                                                                                                                    //line10
                                                                                                                                                    //line11
                                    }
                                    else
                                {
                                    continue;
                                }
                                    break;
                            }
                            xmlElementCode.sTemplate = baseTableConverParameter.Stemp4;
                            xmlElementCode.xmlElementFirst = xmlElementFirst;
                            xmlElementCode.xmlElementThirds = xmlElementThirds;
                            xmlElementCode.xmlElementSix = xmlElementSix;
                            xmlElementCode.xmlElementnine = xmlElementnine;
                            sOutPutData = xmlElementCode.sOutPutXmlElementCode();
                            lOutPutDatas.Add(sOutPutData);
                            break;
                    }

                }
                sOutPutDatas = DataConvert.ToString(lOutPutDatas);
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据处理错误：" + ex.Message);
            }

            return sOutPutDatas;
        }
        private void sMarginChange(ref int x,ref int y, int iColWidth, int iRowHight)
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

        public override int iOutData()
        {
            throw new NotImplementedException();
        }

        public override List<List<string>> lOutData()
        {
            throw new NotImplementedException();
        }

        public override DataTable dOutData()
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
