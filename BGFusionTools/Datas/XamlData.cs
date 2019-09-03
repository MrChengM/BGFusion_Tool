using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using BGFusionTools.Functions;
using BGFusionTools.Serialization;

namespace BGFusionTools.Datas
{
    class XamlData : BaseData
    {
        private Margin initMargin = new Margin();
        private string keepAlive;
        public XamlData(BaseParameter ConverParameter)
        {
            this.baseParameter = ConverParameter;
            getKeepAliveValue();
        }
        public override List<T1> CreateList<T1>(CreateDataMath<T1, ConveyorRow> dataMath)
        {
            List<T1> lOutPut = new List<T1>();
            EnumerableRowCollection<DataRow> MainRows = LinqToTable();
            foreach (DataRow selectConRow in MainRows)
            {
                var conveyor = new ConveyorRow(baseParameter.TaglistColName, selectConRow);
                lOutPut.AddRange(dataMath(conveyor));
            }
            return lOutPut;
        }
        /// <summary>
        /// 创建L1画面元素XAML代码类（11302模式）
        /// </summary>
        /// <param name="conveyor"></param>
        /// <returns></returns>
        public List<BgElementCommonXaml> CreatL1CommonXaml11302(ConveyorRow conveyor)
        {
            List<BgElementCommonXaml> commonXamls = new List<BgElementCommonXaml>();


            //判断Element Style 及个数
            if (conveyor.sDrawOnViews.ToLower() == "all" || conveyor.sDrawOnViews.ToLower() == "level1only")
            {
                int elementCount = createElementCount(conveyor.sStyleIdentifier);
                for (int i = 1; i <= elementCount; i++)
                {
                    BgElementCommonXaml commonXaml = new BgElementCommonXaml();

                    if (elementCount == 1)
                        commonXaml.Name = string.Format("{0}_{1}_{2}_{3}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName);
                    else
                        commonXaml.Name = string.Format("{0}_{1}_{2}_MULTI_{3}_{4}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, i, conveyor.sElementName);

                    commonXaml.Style = string.Format("{0}StaticResource {1}{2}", "{", "Conveyor_Straight_L1", "}");
                    commonXaml.ScadaLevel = "Level1";
                    commonXaml.NavigateToView = string.Format("BG_SCADA.Views.{0}", conveyor.sLevel2View);
                    commonXaml.ElementName = string.Format("{0}_{1}_{2}_{3}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName);
                    commonXaml.ChooseLeftClickMode = "Navigation";
                    commonXaml.DisplayName = conveyor.sDisplayName;

                    //当BehaviorName为空时，通过SignalMapping转换.
                    if (conveyor.sBehaviorName == "")
                    {
                        foreach (string signalMapping in conveyor.sSignalMapping)
                        {
                            if (conveyor.sBehaviorName == "")
                            {
                                conveyor.sBehaviorName = signalMapping;
                            }
                            else
                            {
                                conveyor.sBehaviorName += signalMapping.Split(Convert.ToChar("_")).Last();
                            }
                        }
                    }
                    commonXaml.ControlObject = conveyor.sBehaviorName;
                    commonXaml.ElementXamlBehavior.behaviorName = string.Format("{0}Behavior", conveyor.sBehaviorName);

                    //Margin位置计算
                    commonXaml.Margin = initMargin;
                    initMargin = initMargin.MarginChange(initMargin, commonXaml.iColWidth, commonXaml.iColHight);

                    commonXaml.PowerBox = conveyor.sPowerBox;
                    commonXaml.ElementType = conveyor.sElementType;
                    commonXaml.PlcName = conveyor.sPLC;
                    commonXaml.CommandSignalName = commonXaml.ElementName + "_CM";
                    commonXaml.CommandMappingType = conveyor.sCommandMapping;
                    commonXaml.TypeDescription = conveyor.sTypeDescription;

                    //判断RightClick是否存在，存在则为“True”
                    if (conveyor.sRightClick != "")
                    {
                        commonXaml.HasRightclickMenu = "True";
                        if (conveyor.sRightClick.Contains("Commands"))
                            commonXaml.Commands = string.Format(" Binding CommandSet[{0}#{1}]", commonXaml.CommandMappingType, commonXaml.CommandSignalName);
                    }

                    //signal个数计算并遍历生成ElementXamlSignal集合
                    int signalCounts = 0;
                    foreach (string signalMapping in conveyor.sSignalMapping)
                    {
                        var bitCounts = baseParameter.SingleMappingTable.AsEnumerable().Count(p => p.Field<string>(baseParameter.SignalMappingColName.sType) == signalMapping);
                        if (bitCounts != 0)
                            signalCounts += (int)Math.Ceiling((float)bitCounts / 32);
                    }
                    for (int j = 1; j <= signalCounts; j++)
                    {
                        ElementXamlSignal signalGroup = new ElementXamlSignal();
                        signalGroup.Id = string.Format("Signal{0}", j);
                        signalGroup.UsePostfix = "True";
                        signalGroup.Postfix = string.Format("_SIGNAL_{0}", j);
                        signalGroup.UsePrefix = "False";
                        signalGroup.Prefix = "";
                        signalGroup.KeepAliveType = "Constant";
                        signalGroup.KeepAliveSignal = keepAlive;
                        commonXaml.ElementXamlSignal.Add(signalGroup);
                    }
                    if (conveyor.sEdgeColor == "")
                        conveyor.sEdgeColor = "Blue";
                    commonXaml.ElementXamlBackground.Color = string.Format("{0}StaticResource BG_COLOR_EDGE_{1}{2}", "{", conveyor.sEdgeColor, "}");

                    commonXamls.Add(commonXaml);
                }
            }
            return commonXamls;
        }
        /// <summary>
        /// 创建L1画面元素XAML代码类（12307模式）
        /// </summary>
        /// <param name="conveyor"></param>
        /// <returns></returns>
        public List<BgElementCommonXaml> CreatL1CommonXaml12307(ConveyorRow conveyor)
        {
            List<BgElementCommonXaml> commonXamls = new List<BgElementCommonXaml>();


            
            if (conveyor.sDrawOnViews.ToLower() == "all" || conveyor.sDrawOnViews.ToLower() == "level1only")
            {
                int elementCount = createElementCount(conveyor.sStyleIdentifier);
                for (int i = 1; i <= elementCount; i++)
                {
                    BgElementCommonXaml commonXaml = new BgElementCommonXaml();

                    if (elementCount == 1)
                        commonXaml.Name = string.Format("{0}_{1}_{2}_{3}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName);
                    else
                        commonXaml.Name = string.Format("{0}_{1}_{2}_MULTI_{3}_{4}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, i, conveyor.sElementName);

                    commonXaml.Style = string.Format("{0}StaticResource {1}{2}", "{", "Conveyor_Straight_L1", "}");
                    commonXaml.ScadaLevel = "Level1";
                    commonXaml.NavigateToView = string.Format("BG_SCADA.Views.{0}", conveyor.sLevel2View);
                    commonXaml.ElementName = string.Format("{0}_{1}_{2}_{3}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName);
                    commonXaml.ChooseLeftClickMode = "Navigation";
                    commonXaml.DisplayName = conveyor.sDisplayName;
                    commonXaml.ElementXamlBehavior.behaviorName = "BGLineBehavior";
                    commonXaml.ControlObject = "BGLine";

                    //Margin位置计算
                    commonXaml.Margin = initMargin;
                    initMargin = initMargin.MarginChange(initMargin, commonXaml.iColWidth, commonXaml.iColHight);

                    commonXaml.PowerBox = conveyor.sPowerBox;
                    commonXaml.ElementType = conveyor.sElementType;
                    commonXaml.PlcName = conveyor.sPLC;
                    commonXaml.CommandSignalName = commonXaml.ElementName + "_CM";
                    commonXaml.CommandMappingType = conveyor.sCommandMapping;
                    commonXaml.TypeDescription = conveyor.sTypeDescription;

                    //判断RightClick是否存在，存在则为“True”
                    if (conveyor.sRightClick != "")
                    {
                        commonXaml.HasRightclickMenu = "True";
                        if (conveyor.sRightClick.Contains("Commands"))
                            commonXaml.Commands = string.Format(" Binding CommandSet[{0}#{1}]", commonXaml.CommandMappingType, commonXaml.CommandSignalName);
                    }

                    //L1 Signal个数为一
                    ElementXamlSignal signalGroup = new ElementXamlSignal();
                    signalGroup.Id = string.Format("Signal{0}", 1);
                    signalGroup.UsePostfix = "True";
                    signalGroup.Postfix = string.Format("_{0}_{1}_Line_AC", conveyor.sPLC, conveyor.sLevel2View);
                    signalGroup.UsePrefix = "False";
                    signalGroup.Prefix = "";
                    signalGroup.KeepAliveType = "Constant";
                    signalGroup.KeepAliveSignal = keepAlive;
                    commonXaml.ElementXamlSignal.Add(signalGroup);

                    if (conveyor.sEdgeColor == "")
                        conveyor.sEdgeColor = "Blue";
                    commonXaml.ElementXamlBackground.Color = string.Format("{0}StaticResource BG_COLOR_EDGE_{1}{2}", "{", conveyor.sEdgeColor, "}");

                    commonXamls.Add(commonXaml);
                }
            }
            return commonXamls;
        }
        /// <summary>
        /// 创建L2画面元素XAML代码类
        /// </summary>
        /// <param name="conveyor"></param>
        /// <returns></returns>
        public List<BgElementCommonXaml> CreatL2CommonXaml(ConveyorRow conveyor)
        {
            List<BgElementCommonXaml> commonXamls = new List<BgElementCommonXaml>();


            if (conveyor.sDrawOnViews.ToLower() == "all" || conveyor.sDrawOnViews.ToLower() == "level2only")
            {
                int elementCount = createElementCount(conveyor.sStyleIdentifier);
                //生成ElementXaml代码类集合
                for (int i = 1;i<= elementCount; i++)
                {
                    BgElementCommonXaml commonXaml = new BgElementCommonXaml();
                    if (elementCount == 1)
                        commonXaml.Name = string.Format("{0}_{1}_{2}_{3}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName);
                    else
                        commonXaml.Name = string.Format("{0}_{1}_{2}_MULTI_{3}_{4}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, i, conveyor.sElementName);
                    commonXaml.Style = string.Format("{0}StaticResource {1}{2}", "{", "Conveyor_Straight_L2", "}");
                    commonXaml.ScadaLevel = "Level2";
                    commonXaml.NavigateToView = "";
                    commonXaml.ElementName = string.Format("{0}_{1}_{2}_{3}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName);
                    commonXaml.ChooseLeftClickMode = conveyor.sLeftClick;
                    commonXaml.DisplayName = conveyor.sDisplayName;

                    //当BehaviorName为空时，通过SignalMapping转换.
                    if (conveyor.sBehaviorName == "")
                    {
                        foreach (string signalMapping in conveyor.sSignalMapping)
                        {
                            if (conveyor.sBehaviorName == "")
                            {
                                conveyor.sBehaviorName = signalMapping;
                            }
                            else
                            {
                                conveyor.sBehaviorName += signalMapping.Split(Convert.ToChar("_")).Last();
                            }
                        }
                    }
                    commonXaml.ControlObject = conveyor.sBehaviorName;
                    commonXaml.ElementXamlBehavior.behaviorName = string.Format("{0}Behavior", conveyor.sBehaviorName);

                    //Margin位置计算
                    commonXaml.Margin = initMargin;
                    initMargin = initMargin.MarginChange(initMargin, commonXaml.iColWidth, commonXaml.iColHight);

                    commonXaml.PowerBox = conveyor.sPowerBox;
                    commonXaml.ElementType = conveyor.sElementType;
                    commonXaml.PlcName = conveyor.sPLC;
                    commonXaml.CommandSignalName = commonXaml.ElementName + "_CM";
                    commonXaml.CommandMappingType = conveyor.sCommandMapping;
                    commonXaml.TypeDescription = conveyor.sTypeDescription;

                    //判断RightClick是否存在，存在则为“True”
                    if (conveyor.sRightClick != "")
                    {
                        commonXaml.HasRightclickMenu = "True";
                        if (conveyor.sRightClick.Contains("Commands"))
                            commonXaml.Commands = string.Format(" Binding CommandSet[{0}#{1}]", commonXaml.CommandMappingType, commonXaml.CommandSignalName);
                    }

                    //signal个数计算并遍历生成ElementXamlSignal集合
                    int signalCounts = 0;
                    foreach (string signalMapping in conveyor.sSignalMapping)
                    {
                        var bitCounts = baseParameter.SingleMappingTable.AsEnumerable().Count(p => p.Field<string>(baseParameter.SignalMappingColName.sType) == signalMapping);
                        if (bitCounts != 0)
                            signalCounts += (int)Math.Ceiling((float)bitCounts / 32);
                    }
                    for (int j = 1; j <= signalCounts;j++)
                    {
                        ElementXamlSignal signalGroup = new ElementXamlSignal();
                        signalGroup.Id = string.Format("Signal{0}", j);
                        signalGroup.UsePostfix = "True";
                        signalGroup.Postfix = string.Format("_SIGNAL_{0}", j);
                        signalGroup.UsePrefix = "False";
                        signalGroup.Prefix = "";
                        signalGroup.KeepAliveType = "Constant";
                        signalGroup.KeepAliveSignal = keepAlive;
                        commonXaml.ElementXamlSignal.Add(signalGroup);
                    }
                    if (conveyor.sEdgeColor == "")
                        conveyor.sEdgeColor = "Blue";
                    commonXaml.ElementXamlBackground.Color = string.Format("{0}StaticResource BG_COLOR_EDGE_{1}{2}", "{", conveyor.sEdgeColor, "}");

                    commonXamls.Add(commonXaml);
                }
            }
            return commonXamls;

        }
        /// <summary>
        /// 创建TextBlock标签XAML代码类
        /// </summary>
        /// <param name="conveyor"></param>
        /// <returns></returns>
        public List<BgTextBlock> CreatTextBlock(ConveyorRow conveyor)
        {
            List<BgTextBlock> textBlocks = new List<BgTextBlock>();
            BgTextBlock textBlock = new BgTextBlock();
            textBlock.Name = string.Format("teblock_{0}_{1}_{2}_{3}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName);
            textBlock.Text = string.Format("{0}.{1}", conveyor.sEquipmentLine, conveyor.sElementName);
            textBlock.Margin = initMargin;
            initMargin = initMargin.MarginChange(initMargin, textBlock.iColWidth, textBlock.iColHight);
            textBlocks.Add(textBlock);
            return textBlocks;
        }

        /// <summary>
        /// 判断生成元素XAML类的个数
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        private int createElementCount(string style)
        {
            string sStyle = style.Trim();
            int elementCount = 1;

            if (sStyle.StartsWith("["))
            {
                elementCount = 0;
                sStyle = sStyle.TrimStart(Convert.ToChar("["));
                sStyle = sStyle.TrimEnd(Convert.ToChar("["));
                string[] sStyleGroups = sStyle.Split(Convert.ToChar(";"));

                foreach (string sStyleGroup in sStyleGroups)
                {
                    var StyleGroup = sStyleGroup.Split(Convert.ToChar(":"));
                    elementCount = elementCount + Convert.ToInt16(StyleGroup[1]);
                }
            }
            return elementCount;
        }
        private string getKeepAliveValue()
        {
            var ConveyorRows = from p in baseParameter.TaglistTable.AsEnumerable()
                               where (p.Field<string>(baseParameter.TaglistColName.sElementName) == "KAL_MAIN")
                               select p;
            foreach(DataRow conveyorRow in ConveyorRows)
            {
                ConveyorRow conveyor = new ConveyorRow(baseParameter.TaglistColName, conveyorRow);
                keepAlive = string.Format("{0}_{1}_{2}_{3}_ACTIVE", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName);
                break; 
            }
            if (ConveyorRows == null)
                keepAlive = "Constant";
            return keepAlive;
        }
    }
}
