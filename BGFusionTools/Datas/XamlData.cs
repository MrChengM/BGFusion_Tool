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

        public XamlData(BaseParameter ConverParameter)
        {
            this.baseParameter = ConverParameter;
        }
        public List<BgElementCommonXaml> CreatL1CommonXaml(ConveyorRow conveyor)
        {
            List<BgElementCommonXaml> commonXamls = new List<BgElementCommonXaml>();


            //判断Element Style 及个数
            if (conveyor.sDrawOnViews.ToLower() == "all" || conveyor.sDrawOnViews.ToLower() == "level1only")
            {
                string sStyle = conveyor.sStyleIdentifier.Trim();
                int elementCount = 1;
                if (sStyle.StartsWith("["))
                {
                    sStyle = sStyle.TrimStart(Convert.ToChar("["));
                    sStyle = sStyle.TrimEnd(Convert.ToChar("["));
                    string[] sStyleGroups = sStyle.Split(Convert.ToChar(";"));
                    foreach (string sStyleGroup in sStyleGroups)
                    {
                        var StyleGroup = sStyleGroup.Split(Convert.ToChar(":"));
                        elementCount = elementCount + Convert.ToInt16(StyleGroup[1]);

                    }
                }
                for (int i = 1; i <= elementCount; i++)
                {
                    BgElementCommonXaml commonXaml = new BgElementCommonXaml(conveyor, baseParameter);

                    if (elementCount != 1)
                        commonXaml._name = string.Format("{0}_{1}_{2}_MULTI_{3}_{4}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, i, conveyor.sElementName);
                    commonXaml._margin = initMargin;
                    initMargin = initMargin.MarginChange(initMargin, commonXaml._iColWidth, commonXaml._iColHight);
                    commonXaml._stlye = string.Format("{StaticResource {0}}", "Conveyor_Straight_L1");
                    commonXaml._scadaLevel = "Level1";
                    commonXamls.Add(commonXaml);
                }
            }
            return commonXamls;
        }
        public List<BgElementCommonXaml> CreatL1CommonXaml1(ConveyorRow conveyor)
        {
            List<BgElementCommonXaml> commonXamls = new List<BgElementCommonXaml>();


            //判断Element Style 及个数
            if (conveyor.sDrawOnViews.ToLower() == "all" || conveyor.sDrawOnViews.ToLower() == "level1only")
            {
                string sStyle = conveyor.sStyleIdentifier.Trim();
                int elementCount = 1;
                if (sStyle.StartsWith("["))
                {
                    sStyle = sStyle.TrimStart(Convert.ToChar("["));
                    sStyle = sStyle.TrimEnd(Convert.ToChar("["));
                    string[] sStyleGroups = sStyle.Split(Convert.ToChar(";"));
                    foreach (string sStyleGroup in sStyleGroups)
                    {
                        var StyleGroup = sStyleGroup.Split(Convert.ToChar(":"));
                        elementCount = elementCount + Convert.ToInt16(StyleGroup[1]);

                    }
                }
                for (int i = 1; i <= elementCount; i++)
                {
                    BgElementCommonXaml commonXaml = new BgElementCommonXaml(conveyor, baseParameter);

                    if (elementCount != 1)
                        commonXaml._name = string.Format("{0}_{1}_{2}_MULTI_{3}_{4}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, i, conveyor.sElementName);

                    commonXaml._margin = initMargin;
                    initMargin = initMargin.MarginChange(initMargin, commonXaml._iColWidth, commonXaml._iColHight);
                    commonXaml._stlye = string.Format("{StaticResource {0}}", "Conveyor_Straight_L1");
                    commonXaml._scadaLevel = "Level1";
                    commonXaml._navigateToView = string.Format("BG_SCADA.Views.{0}", conveyor.sLevel2View);

                    int signalCounts = 0;
                    foreach (string signalMapping in conveyor.sSignalMapping)
                    {
                        var bitCounts = baseParameter.SingleMappingTable.AsEnumerable().Count(p => p.Field<string>(baseParameter.SignalMappingColName.sType) == signalMapping);
                        if (bitCounts != 0)
                            signalCounts = bitCounts / 32 + 1;
                    }
                    for (int j = 1; j <= signalCounts; j++)
                    {
                        ElementXamlSignal signalGroup = new ElementXamlSignal();
                        signalGroup._id = string.Format("Signal{0}", j);
                        signalGroup._usePostfix = "True";
                        signalGroup._postfix = string.Format("_{0}_{1}_Line_AC", conveyor.sPLC, conveyor.sLevel2View);
                        signalGroup._usePrefix = "False";
                        signalGroup._prefix = "";
                        signalGroup._keepAliveType = "Constant";
                        signalGroup._keepAliveSignal = string.Format("{0}_{1}_KAL_MAIN_KAL_MAIN_ACTIVE", conveyor.sSystem, conveyor.sPLC);
                        commonXaml._elementXamlSignal.Add(signalGroup);
                    }
                    commonXamls.Add(commonXaml);
                }
            }
            return commonXamls;
        }
        public List<BgElementCommonXaml> CreatL2CommonXaml(ConveyorRow conveyor)
        {
            List<BgElementCommonXaml> commonXamls = new List<BgElementCommonXaml>();


            //判断Element Style 及个数
            if (conveyor.sDrawOnViews.ToLower() == "all" || conveyor.sDrawOnViews.ToLower() == "level2only")
            {
                string sStyle = conveyor.sStyleIdentifier.Trim();
                int elementCount = 1;
                if (sStyle.StartsWith("["))
                {
                    sStyle = sStyle.TrimStart(Convert.ToChar("["));
                    sStyle = sStyle.TrimEnd(Convert.ToChar("["));
                    string[] sStyleGroups = sStyle.Split(Convert.ToChar(";"));
                    foreach (string sStyleGroup in sStyleGroups)
                    {
                        var StyleGroup = sStyleGroup.Split(Convert.ToChar(":"));
                        elementCount = elementCount + Convert.ToInt16(StyleGroup[1]);

                    }
                }
                for(int i = 1;i<= elementCount; i++)
                {
                    BgElementCommonXaml commonXaml = new BgElementCommonXaml(conveyor, baseParameter);
                    if (elementCount != 1)
                        commonXaml._name = string.Format("{0}_{1}_{2}_MULTI_{3}_{4}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, i, conveyor.sElementName);
                    commonXaml._margin = initMargin;
                    initMargin = initMargin.MarginChange(initMargin, commonXaml._iColWidth, commonXaml._iColHight);
                    commonXamls.Add(commonXaml);
                }
            }
            return commonXamls;

        }
        public List<BgTextBlock> CreatTextBlock(ConveyorRow conveyor)
        {
            List<BgTextBlock> textBlocks = new List<BgTextBlock>();
            BgTextBlock textBlock = new BgTextBlock();
            textBlock._name = string.Format("teblock_{0}_{1}_{2}_{3}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName);
            textBlock._text = string.Format("{0}.{1}", conveyor.sEquipmentLine, conveyor.sElementName);
            textBlock._width = "70";
            textBlock._margin = initMargin;
            initMargin = initMargin.MarginChange(initMargin, textBlock._iColWidth, textBlock._iColHight);
            textBlocks.Add(textBlock);
            return textBlocks;
        }
    }
    public enum View
    {
        L0View = 0,
        L1View = 1,
        L2View = 2,
        L3View = 3,

    }
}
