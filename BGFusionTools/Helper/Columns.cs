using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BGFusionTools.Helper
{
    /// <summary>
    /// 输送机taglist表列名获取
    /// </summary>
    public class TaglistColumns
    {
        public string sTagName = "";
        public string sSystem = "";
        public string sPLC = "";
        public string sPowerBox = "";
        public string sEquipmentLine = "";
        public string sElementType = "";
        public string sTypeDescription = "";
        public string sElementName = "";
        public string sBehaviorName = "";
        public string sStyleIdentifier = "";
        public List<string> sSignalMapping = new List<string>();
        public List<string> sSignalAddress = new List<string>();
        public string sCommandMapping = "";
        public string sCommandAddress = "";
        public string sRunningHours = "";
        public string sCopyRunningHours = "";
        public string sDisplayName = "";
        public string sEdgeColor = "";
        public string sAlarmTree = "";
        public string sLevel1View = "";
        public string sLevel2View = "";
        public string sDrawOnViews = "";
        public string sLeftClick = "";
        public string sRightClick = "";
        public string sLevel1AsLevel2 = "";
        public string sExtendedPropertyAsCamera = "";
        private TaglistColumns()
        {
            const string filePath = "../../Configuration.xml";
            Stream sFileSteam = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            XmlReader xmlReader = XmlReader.Create(sFileSteam);
            while (xmlReader.Read())
            {
                xmlReader.MoveToContent();
                if (xmlReader.IsStartElement("DataSet") && xmlReader["name"] == "sTagName")
                {
                    sTagName = xmlReader["value"];
                    xmlReader.Read();
                    //遍历configuration文件，加载配置
                    while (xmlReader.IsStartElement("Column"))
                    {
                        switch (xmlReader["name"])
                        {
                            case "sTagName":
                                sTagName = xmlReader["value"];
                                break;
                            case "sSystem":
                                sSystem = xmlReader["value"];
                                break;
                            case "sPLC":
                                sPLC = xmlReader["value"];
                                break;
                            case "sPowerBox":
                                sPowerBox = xmlReader["value"];
                                break;
                            case "sEquipmentLine":
                                sEquipmentLine = xmlReader["value"];
                                break;
                            case "sElementType":
                                sElementType = xmlReader["value"];
                                break;
                            case "sTypeDescription":
                                sTypeDescription = xmlReader["value"];
                                break;
                            case "sElementName":
                                sElementName = xmlReader["value"];
                                break;
                            case "sBehaviorName":
                                sBehaviorName = xmlReader["value"];
                                break;
                            case "sStyleIdentifier":
                                sStyleIdentifier = xmlReader["value"];
                                break;
                            case "sSignalMapping1":
                                if (xmlReader["enable"] != "false")
                                    sSignalMapping.Add(xmlReader["value"]);
                                break;
                            case "sSignalMapping2":
                                if (xmlReader["enable"] != "false")
                                    sSignalMapping.Add(xmlReader["value"]);
                                break;
                            case "sSignalMapping3":
                                if (xmlReader["enable"] != "false")
                                    sSignalMapping.Add(xmlReader["value"]);
                                break;
                            case "sSignalMapping4":
                                if (xmlReader["enable"] != "false")
                                    sSignalMapping.Add(xmlReader["value"]);
                                break;
                            case "sSignalMapping5":
                                if (xmlReader["enable"] != "false")
                                    sSignalMapping.Add(xmlReader["value"]);
                                break;
                            case "sSignalMapping6":
                                if (xmlReader["enable"] != "false")
                                    sSignalMapping.Add(xmlReader["value"]);
                                break;
                            case "sSignalMapping7":
                                if (xmlReader["enable"] != "false")
                                    sSignalMapping.Add(xmlReader["value"]);
                                break;
                            case "sSignalMapping8":
                                if (xmlReader["enable"] != "false")
                                    sSignalMapping.Add(xmlReader["value"]);
                                break;
                            case "sSignalMapping9":
                                if (xmlReader["enable"] != "false")
                                    sSignalMapping.Add(xmlReader["value"]);
                                break;
                            case "sSignalMapping10":
                                if (xmlReader["enable"] != "false")
                                    sSignalMapping.Add(xmlReader["value"]);
                                break;
                            case "sSignalMapping11":
                                if (xmlReader["enable"] != "false")
                                    sSignalMapping.Add(xmlReader["value"]);
                                break;
                            case "sSignalAddress1":
                                if (xmlReader["enable"] != "false")
                                    sSignalAddress.Add(xmlReader["value"]);
                                break;
                            case "sSignalAddress2":
                                if (xmlReader["enable"] != "false")
                                    sSignalAddress.Add(xmlReader["value"]);
                                break;
                            case "sSignalAddress3":
                                if (xmlReader["enable"] != "false")
                                    sSignalAddress.Add(xmlReader["value"]);
                                break;
                            case "sSignalAddress4":
                                if (xmlReader["enable"] != "false")
                                    sSignalAddress.Add(xmlReader["value"]);
                                break;
                            case "sSignalAddress5":
                                if (xmlReader["enable"] != "false")
                                    sSignalAddress.Add(xmlReader["value"]);
                                break;
                            case "sSignalAddress6":
                                if (xmlReader["enable"] != "false")
                                    sSignalAddress.Add(xmlReader["value"]);
                                break;
                            case "sSignalAddress7":
                                if (xmlReader["enable"] != "false")
                                    sSignalAddress.Add(xmlReader["value"]);
                                break;
                            case "sSignalAddress8":
                                if (xmlReader["enable"] != "false")
                                    sSignalAddress.Add(xmlReader["value"]);
                                break;
                            case "sSignalAddress9":
                                if (xmlReader["enable"] != "false")
                                    sSignalAddress.Add(xmlReader["value"]);
                                break;
                            case "sSignalAddress10":
                                if (xmlReader["enable"] != "false")
                                    sSignalAddress.Add(xmlReader["value"]);
                                break;
                            case "sSignalAddress11":
                                if (xmlReader["enable"] != "false")
                                    sSignalAddress.Add(xmlReader["value"]);
                                break;
                            case "sCommandMapping":
                                sCommandMapping = xmlReader["value"];
                                break;
                            case "sCommandAddress":
                                sCommandAddress = xmlReader["value"];
                                break;
                            case "sRunningHours":
                                sRunningHours = xmlReader["value"];
                                break;
                            case "sCopyRunningHours":
                                sCopyRunningHours = xmlReader["value"];
                                break;
                            case "sDisplayName":
                                sDisplayName = xmlReader["value"];
                                break;
                            case "sEdgeColor":
                                sEdgeColor = xmlReader["value"];
                                break;
                            case "sAlarmTree":
                                sAlarmTree = xmlReader["value"];
                                break;
                            case "sLevel1View":
                                sLevel1View = xmlReader["value"];
                                break;
                            case "sLevel2View":
                                sLevel2View = xmlReader["value"];
                                break;
                            case "sDrawOnViews":
                                sDrawOnViews = xmlReader["value"];
                                break;
                            case "sLeftClick":
                                sLeftClick = xmlReader["value"];
                                break;
                            case "sRightClick":
                                sRightClick = xmlReader["value"];
                                break;
                            case "sLevel1AsLevel2":
                                sLevel1AsLevel2 = xmlReader["value"];
                                break;
                            case "sExtendedPropertyAsCamera":
                                sExtendedPropertyAsCamera = xmlReader["value"];
                                break;
                        }
                        xmlReader.Read();
                    }
                }
            }
            xmlReader.Dispose();
            sFileSteam.Dispose();
        }

        private static TaglistColumns instance = new TaglistColumns();
        public static TaglistColumns getInstance()
        {
            return instance;
        }

    }
    /// <summary>
    /// SignalMapping表列名获取
    /// </summary>
    public class SignalMappingColumns
    {
        public string sSignalMapping = "";
        public string sType = "";
        public string sWord = "";
        public string sBit = "";
        public string sAnalogValue = "";
        public string sAlarmStatusNumber = "";
        public string sPartName = "";
        public string sShowOnSCADA = "";
        public string sStateRef = "";
        public string sFunctionalText = "";
        public string sStateText = "";
        public string sStatespriority = "";
        public string sStateColor = "";
        public string sStateSpecialFunction = "";
        public string sAlarmStatusPriority = "";
        public string sPriorityFormula = "";
        private SignalMappingColumns()
        {
            const string filePath = "../../Configuration.xml";
            Stream sFileSteam = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            XmlReader xmlReader = XmlReader.Create(sFileSteam);
            while (xmlReader.Read())
            {
                xmlReader.MoveToContent();
                if (xmlReader.IsStartElement("DataSet") && xmlReader["name"] == "sSignalMapping")
                {
                    sSignalMapping = xmlReader["value"];
                    xmlReader.Read();
                    while (xmlReader.IsStartElement("Column"))
                    {
                        switch (xmlReader["name"])
                        {
                            case "sType":
                                sType = xmlReader["value"];
                                break;
                            case "sWord":
                                sWord = xmlReader["value"];
                                break;
                            case "sBit":
                                sBit = xmlReader["value"];
                                break;
                            case "sAnalogValue":
                                sAnalogValue = xmlReader["value"];
                                break;
                            case "sAlarmStatusNumber":
                                sAlarmStatusNumber = xmlReader["value"];
                                break;
                            case "sPartName":
                                sPartName = xmlReader["value"];
                                break;
                            case "sShowOnSCADA":
                                sShowOnSCADA = xmlReader["value"];
                                break;
                            case "sStateRef":
                                sStateRef = xmlReader["value"];
                                break;
                            case "sFunctionalText":
                                sFunctionalText = xmlReader["value"];
                                break;
                            case "sStateText":
                                sStateText = xmlReader["value"];
                                break;
                            case "sStatespriority":
                                sStatespriority = xmlReader["value"];
                                break;
                            case "sStateColor":
                                sStateColor = xmlReader["value"];
                                break;
                            case "sStateSpecialFunction":
                                sStateSpecialFunction = xmlReader["value"];
                                break;
                            case "sAlarmStatusPriority":
                                sAlarmStatusPriority = xmlReader["value"];
                                break;
                            case "sPriorityFormula":
                                sPriorityFormula = xmlReader["value"];
                                break;
                        }
                        xmlReader.Read();
                    }
                }
            }
            xmlReader.Dispose();
            sFileSteam.Dispose();
        }
        private static SignalMappingColumns instance = new SignalMappingColumns();
        public static SignalMappingColumns getInstance()
        {
            return instance;
        }
    }
    /// <summary>
    /// CommandMapping列表名获取
    /// </summary>
    public class CommandMappingColumns
    {
        public string sCommandMapping = "";
        public string sType = "";
        public string sElementLink = "";
        public string sBit = "";
        public string sAnalogValue = "";
        public string sCommandNumber = "";
        public string sPartName = "";
        public string sCommandPriority = "";
        public string sCommandText = "";
        private CommandMappingColumns()
        {
            const string filePath = "../../Configuration.xml";
            Stream sFileSteam = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            XmlReader xmlReader = XmlReader.Create(sFileSteam);
            while (xmlReader.Read())
            {
                xmlReader.MoveToContent();
                if (xmlReader.IsStartElement("DataSet") && xmlReader["name"] == "sCommandMapping")
                {
                    sCommandMapping = xmlReader["value"];
                    xmlReader.Read();
                    while (xmlReader.IsStartElement("Column"))
                    {
                        switch (xmlReader["name"])
                        {
                            case "sType":
                                sType = xmlReader["value"];
                                break;
                            case "sElementLink":
                                sElementLink = xmlReader["value"];
                                break;
                            case "sBit":
                                sBit = xmlReader["value"];
                                break;
                            case "sAnalogValue":
                                sAnalogValue = xmlReader["value"];
                                break;
                            case "sCommandNumber":
                                sCommandNumber = xmlReader["value"];
                                break;
                            case "sPartName":
                                sPartName = xmlReader["value"];
                                break;
                            case "sCommandPriority":
                                sCommandPriority = xmlReader["value"];
                                break;
                            case "sCommandText":
                                sCommandText = xmlReader["value"];
                                break;
                        }
                        xmlReader.Read();
                    }
                }
            }
            xmlReader.Dispose();
            sFileSteam.Dispose();
        }
        private static CommandMappingColumns instance = new CommandMappingColumns();
        public static CommandMappingColumns getInstance()
        {
            return instance;
        }
    }
}
