using BGFusionTools.Datas;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xaml;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
/// <summary>
/// XmlSerializable  by IXmlSerializable
/// </summary>
namespace BGFusionTools.Serialization
{
    public class XmlSerialiaztion
    {
        /// <summary>
        ///XMl Serialiaztion 
        /// </summary>
        /// <param name="The File Path on disk"></param>
        /// <param name="Double list datas"></param>
        /// <returns></returns>
        public static bool XmlExcelSerialiaztion(string sFilePath, List<List<string>> llString)
        {
            try
            {
                Stream sFileSteam = new FileStream(sFilePath, FileMode.Create, FileAccess.ReadWrite);
                Workbook xWorkBook = new Workbook(llString);
                XmlSerializer xmlserial = new XmlSerializer(typeof(Workbook));
                xmlserial.Serialize(sFileSteam, xWorkBook);
                string sXml = "\r\n" + "<?mso-application progid=\"Excel.Sheet\"?>";
                FileStreamInsert(sFileSteam, sXml, 21);
                sFileSteam.Flush();
                sFileSteam.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("XmlSerialiaztion error： " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// XML Serialiaztion
        /// </summary>
        /// <param name="The File Path on disk"></param>
        /// <param name="Class used for Excel xml style"></param>
        /// <returns></returns>
        public static bool XmlExcelSerialiaztion(string sFilePath, Workbook xWorkbook)
        {
            try
            {
                Stream sFileSteam = new FileStream(sFilePath, FileMode.Create, FileAccess.ReadWrite);
                XmlSerializer xmlserial = new XmlSerializer(typeof(Workbook));
                xmlserial.Serialize(sFileSteam, xWorkbook);
                string sXml = "\r\n" + "<?mso-application progid=\"Excel.Sheet\"?>";
                FileStreamInsert(sFileSteam, sXml, 21);
                sFileSteam.Flush();
                sFileSteam.Close();
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("XmlSerialiaztion error： " + ex.Message);
                return false;
            }
        }
        public static Workbook XmlExcelDeserialize(string sFilePath)
        {
            try
            {
                Workbook xWorkBook = new Workbook();
                Stream sFileSteam = new FileStream(sFilePath, FileMode.Open, FileAccess.ReadWrite);
                XmlSerializer xmlserial = new XmlSerializer(typeof(Workbook), "urn:schemas-microsoft-com:office:spreadsheet");
                sFileSteam.Position = 0;
                xWorkBook = (Workbook)xmlserial.Deserialize(sFileSteam);
                sFileSteam.Flush();
                sFileSteam.Close();
                return xWorkBook;
            }
            catch (Exception ex)
            {
                MessageBox.Show("XmlDeserialiaztion error： " + ex.Message);
                return default(Workbook);
            }
        }
        public static bool XmlSorteMointerSerialiaztion(string sFilePath, SignalMonitor signalMonitor)
        {
            return default(bool);
        }
        public static SignalMonitor XmlSorteMointerDeserialize(string sFilePath)
        {
            try
            {
                SignalMonitor _signalMonitor = new SignalMonitor();
                Stream sFileSteam = new FileStream(sFilePath, FileMode.Open, FileAccess.ReadWrite);
                XmlSerializer xmlserial = new XmlSerializer(typeof(SignalMonitor));
                sFileSteam.Position = 0;
                _signalMonitor = (SignalMonitor)xmlserial.Deserialize(sFileSteam);
                sFileSteam.Flush();
                sFileSteam.Close();
                return _signalMonitor;
            }
            catch (Exception ex)
            {
                MessageBox.Show("XmlDeserialiaztion error： " + ex.Message);
                return default(SignalMonitor);
            }

        }
        public static bool XmlElementSearchDataSerialiaztion(string sFilePath,ElementSearchXml elementSearchXml)
        {
            try
            {
                Stream sFileSteam = new FileStream(sFilePath, FileMode.Create, FileAccess.ReadWrite);
                XmlSerializer xmlserial = new XmlSerializer(typeof(ElementSearchXml));
                xmlserial.Serialize(sFileSteam, elementSearchXml);
                //string sXml = "\r\n" + "<?mso-application progid=\"Excel.Sheet\"?>";
                //FileStreamInsert(sFileSteam, sXml, 21);
                sFileSteam.Flush();
                sFileSteam.Close();
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("XmlSerialiaztion error： " + ex.Message);
                return false;
            }
        }
        public static ElementSearchXml XmlElementSearchDatDeserialize(string sFilePath)
        {
            try
            {
                ElementSearchXml elementSearchData = new ElementSearchXml();
                Stream sFileSteam = new FileStream(sFilePath, FileMode.Open, FileAccess.ReadWrite);
                XmlSerializer xmlserial = new XmlSerializer(typeof(Workbook), "urn:schemas-microsoft-com:office:spreadsheet");
                sFileSteam.Position = 0;
                elementSearchData = (ElementSearchXml)xmlserial.Deserialize(sFileSteam);
                sFileSteam.Flush();
                sFileSteam.Close();
                return elementSearchData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("XmlDeserialiaztion error： " + ex.Message);
                return default(ElementSearchXml);
            }
        }

        public static bool XmlSerial<T>(string sFilePath,T XmlSerialClass)
        {
            try
            {
                Stream sFileSteam = new FileStream(sFilePath, FileMode.Create, FileAccess.ReadWrite);
                XmlSerializer xmlserial = new XmlSerializer(typeof(T));
                xmlserial.Serialize(sFileSteam, XmlSerialClass);
                if(typeof(T)== typeof(Workbook))
                {
                    string sXml = "\r\n" + "<?mso-application progid=\"Excel.Sheet\"?>";
                    FileStreamInsert(sFileSteam, sXml, 21);
                }
                sFileSteam.Flush();
                sFileSteam.Close();
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(typeof(T).ToString() + ":" + ex.Message);
                return false;
            }

        }
        public static T XmlDeserial<T>(string sFilePath)
        {
            try
            {
                //T xmlDeserialClass = new T();
                Stream sFileSteam = new FileStream(sFilePath, FileMode.Open, FileAccess.ReadWrite);
                XmlSerializer xmlserial;
                if (typeof(T) == typeof(Workbook))
                {
                    xmlserial = new XmlSerializer(typeof(T), "urn:schemas-microsoft-com:office:spreadsheet");
                }
                else
                {
                    xmlserial = new XmlSerializer(typeof(T));
                }
                sFileSteam.Position = 0;
                T xmlDeserialClass = (T)xmlserial.Deserialize(sFileSteam);
                sFileSteam.Flush();
                sFileSteam.Close();
                return xmlDeserialClass;

            }
            catch (Exception ex)
            {
                MessageBox.Show(typeof(T).ToString()+ ":" + ex.Message);
                return default(T);
            }

        }
        /// <summary>
        /// File stream insert fuction
        /// </summary>
        /// <param name="sFileStream"></param>
        /// <param name="Insert string"></param>
        /// <param name="Insert Postion"></param>
        /// <param name="Data Block Size to transfer "></param>
        public static void FileStreamInsert(Stream sFileStream, string sInsert, long lPostion, int iBlockLength = 10000)
        {
            //string sInsertXml = "\r\n" + "<?mso-application progid=\"Excel.Sheet\"?>" + "\r\n";
            var insertChars = sInsert.ToCharArray();
            var insertBytes = new byte[insertChars.Length];
            Encoder e = Encoding.UTF8.GetEncoder();
            e.GetBytes(insertChars, 0, insertChars.Length, insertBytes, 0, true);
            //int iBlockLength = 10000;
            long lSurplusLength = sFileStream.Length - lPostion;
            for (long i = 0; i < lSurplusLength; i += iBlockLength)
            {
                int length = iBlockLength;
                if ((lSurplusLength - i) < iBlockLength)
                    length = (int)(lSurplusLength - i);
                var fileBytes = new byte[length];
                if (i == 0)//第一次移位增加长度，写Seek值变小（负数增大）
                {
                    sFileStream.Seek(-(i + length), SeekOrigin.End);
                    sFileStream.Read(fileBytes, 0, length);
                    sFileStream.Seek(-(i + length - insertBytes.Length), SeekOrigin.End);
                    sFileStream.Write(fileBytes, 0, length);
                }
                else //后面移位由于长度增加，读Seek值增大（负数变小）
                {
                    sFileStream.Seek(-(i + length + insertBytes.Length), SeekOrigin.End);
                    sFileStream.Read(fileBytes, 0, length);
                    sFileStream.Seek(-(i + length), SeekOrigin.End);
                    sFileStream.Write(fileBytes, 0, length);
                }
            }
            sFileStream.Seek(lPostion, SeekOrigin.Begin);
            sFileStream.Write(insertBytes, 0, insertBytes.Length);
        }
    }


    [XmlRoot("Workbook")]
    public class Workbook : IXmlSerializable
    {
        public Workbook() { }
        public Workbook(List<List<string>> llstrings) { _llStrings = llstrings; }
        private List<List<string>> _llStrings = new List<List<string>>();

        public List<List<string>> llStrings { get { return _llStrings; } set { _llStrings = value; } }
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            while (reader.Read())
            {
                reader.MoveToContent();
                if (reader.IsStartElement("Row"))
                {
                    List<string> ls = new List<string>();
                    reader.Read();
                    while (reader.IsStartElement("Cell"))
                    {
                        reader.Read();
                        ls.Add(reader.ReadElementContentAsString());
                        reader.Read();
                    }
                    //reader.MoveToContent();
                    _llStrings.Add(ls);
                }
                reader.MoveToContent();
            }

        }

        public void WriteXml(XmlWriter writer)
        {
            //Namespace.
            writer.WriteAttributeString("xmlns", null, null, "urn:schemas-microsoft-com:office:spreadsheet");
            writer.WriteAttributeString("xmlns", "o", null, "urn:schemas-microsoft-com:office:spreadsheet");
            writer.WriteAttributeString("xmlns", "x", null, "urn:schemas-microsoft-com:office:excel");
            writer.WriteAttributeString("xmlns", "ss", null, "urn:schemas-microsoft-com:office:spreadsheet");
            writer.WriteAttributeString("xmlns", "html", null, "http://www.w3.org/TR/REC-html40");

            //Workbook Property Setup
            //"DocumentProperties"
            writer.WriteStartElement(null, "DocumentProperties", "urn:schemas-microsoft-com:office:office");
            writer.WriteElementString("Author", "BGBuild");
            writer.WriteElementString("LastAuthor", "BGBuild");
            writer.WriteElementString("Created", DateTime.Now.ToString());
            writer.WriteElementString("LastSaved", DateTime.Now.ToString());
            writer.WriteElementString("Version", "14.0");
            writer.WriteEndElement();
            //OfficeDocumentSettings 
            writer.WriteStartElement(null, "OfficeDocumentSettings", "urn:schemas-microsoft-com:office:office");
            writer.WriteElementString("AllowPNG", null);
            writer.WriteEndElement();
            //ExcelWorkbook 
            writer.WriteStartElement(null, "ExcelWorkbook", "urn:schemas-microsoft-com:office:excel");
            writer.WriteElementString("WindowHeight", "12660");
            writer.WriteElementString("WindowWidth", "15180");
            writer.WriteElementString("WindowTopX", "480");
            writer.WriteElementString("WindowTopY", "120");
            writer.WriteElementString("ProtectStructure", "False");
            writer.WriteElementString("ProtectWindows", "False");
            writer.WriteEndElement();
            //Styles
            writer.WriteStartElement("Styles");
            writer.WriteStartElement("Style");
            writer.WriteAttributeString("ss", "ID", null, "Default");
            writer.WriteAttributeString("ss", "Name", null, "Normal");
            writer.WriteStartElement("Alignment");
            writer.WriteAttributeString("ss", "Vertical", null, "Bottom");
            writer.WriteEndElement();
            writer.WriteElementString("Borders", null);
            writer.WriteStartElement("Font");
            writer.WriteAttributeString("ss", "FontName", null, "Arial");
            writer.WriteEndElement();
            writer.WriteElementString("Interior", null);
            writer.WriteElementString("NumberFormat", null);
            writer.WriteElementString("Protection", null);
            writer.WriteEndElement();
            writer.WriteStartElement("Style");
            writer.WriteAttributeString("ss", "ID", null, "s62");
            writer.WriteStartElement("Font");
            writer.WriteAttributeString("ss", "FontName", null, "Arial");
            writer.WriteAttributeString("x", "Family", null, "Swiss");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("Style");
            writer.WriteAttributeString("ss", "ID", null, "s63");
            writer.WriteStartElement("Font");
            writer.WriteAttributeString("ss", "FontName", null, "Arial");
            writer.WriteAttributeString("x", "Family", null, "Swiss");
            writer.WriteAttributeString("x", "Color", null, "#33333");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            //Worksheet
            writer.WriteStartElement("Worksheet");
            writer.WriteAttributeString("ss", "Name", null, "Sheet1");
            //Table
            writer.WriteStartElement("Table");
            foreach (List<string> ls in _llStrings)
            {
                //Row
                writer.WriteStartElement("Row");
                foreach (string s in ls)
                {
                    //Cell
                    writer.WriteStartElement("Cell");
                    writer.WriteStartElement("Data");
                    writer.WriteAttributeString("ss", "Type", null, "String");
                    writer.WriteString(s);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            //WorksheetOptions
            writer.WriteStartElement("WorksheetOptions", "urn:schemas-microsoft-com:office:excel");
            writer.WriteElementString("Unsynced", null);
            writer.WriteStartElement("Print");
            writer.WriteElementString("ValidPrinterInfo", null);
            writer.WriteElementString("PaperSizeIndex", "9");
            writer.WriteElementString("HorizontalResolution", "600");
            writer.WriteElementString("VerticalResolution", "300");
            writer.WriteEndElement();
            writer.WriteElementString("Selected", null);
            writer.WriteElementString("TopRowVisible", "0");
            writer.WriteElementString("ProtectObjects", "False");
            writer.WriteElementString("ProtectScenarios", "False");
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        public static Workbook operator +(Workbook a, Workbook b)
        {
            Workbook c = new Workbook();
            b.llStrings.RemoveAt(0);
            a.llStrings.AddRange(b.llStrings);
            c.llStrings.AddRange(a.llStrings);
            return c;

        }
    }
    [XmlRoot("monitor")]
    public class SignalMonitor : IXmlSerializable
    {
        private List<KepWareData> kepWareDatas = new List<KepWareData>();
        private string name = "12523_JD_FS01_1_IO_v1_1";
        private string shortname = "Sorter No.: FS01";

        public SignalMonitor() { }
        public List<KepWareData> KepWareDatas { get { return kepWareDatas; } set { kepWareDatas = value; } }

        public DataTable ToDataTable()
        {

            DataTable dOPCdataTable = new DataTable("OPCData");
            string[] sOPCListColName ={"Tag Name","Address", "Data Type" ,
            "Respect Data Type" , "Client Access" , "Scan Rate", "Scaling",
            "Raw Low", "Raw High", "Scaled Low", "Scaled High", "Scaled Data Type",
            "Clamp Low", "Clamp High", "Eng Units","Description","Negate Value" };
            foreach (string sColumName in sOPCListColName)
                dOPCdataTable.Columns.AddRange(new DataColumn[] { new DataColumn(sColumName) });
            foreach (KepWareData kepWareData in kepWareDatas)
            {
                DataRow row = dOPCdataTable.NewRow();
                row["Tag Name"] = kepWareData.TagName;
                row["Address"] = kepWareData.Address;
                row["Data Type"] = kepWareData.DataType;
                row["Respect Data Type"] = kepWareData.RespectData;
                row["Client Access"] = kepWareData.ClientAccess;
                row["Scan Rate"] = kepWareData.ScanRate;
                row["Description"] = kepWareData.Description;
                dOPCdataTable.Rows.Add(row);
            }

            return dOPCdataTable;
        }
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }
        public void ReadXml(XmlReader reader)
        {
            while (reader.Read())
            {
                reader.MoveToContent();
                if (reader.IsStartElement("group"))
                {
                    if (reader["name"] != "Index Mapping - Display Names")
                    {
                        reader.Read();
                        while (reader.IsStartElement("signal"))
                        {
                            KepWareData kpdata = new KepWareData();
                            kpdata.TagName = reader["signalname"];
                            kpdata.Address = reader["ionumber"].Split(".".ToCharArray())[0].Insert(1, "B");
                            kpdata.DataType = "Byte";
                            //kpdata.Description = reader["description"];
                            kpdata.RespectData = "1";
                            kpdata.ClientAccess = "RO";
                            kpdata.ScanRate = "100";
                            var value = from p in kepWareDatas where p.TagName == kpdata.TagName select p;
                            if (value.Count<KepWareData>() == 0)
                                kepWareDatas.Add(kpdata);
                            reader.Read();
                        }
                    }
                }
                reader.MoveToContent();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
        [XmlRoot("elements")]
        public class ElementSearchXml : IXmlSerializable, IOperation<ElementSearchXml>
        {
            private List<ElementSeacrhStruct> elements = new List<ElementSeacrhStruct>();
            public List<ElementSeacrhStruct> Elements { get { return elements; }set { elements = value; } }
            public ElementSearchXml() { }
            public XmlSchema GetSchema()
            {
                throw new NotImplementedException();
            }

            public void ReadXml(XmlReader reader)
            {
                elements.Clear();
                while (reader.Read())
                {
                    if (reader.IsStartElement("element"))
                    {
                        ElementSeacrhStruct element = new ElementSeacrhStruct();
                        element.DisplayName = reader["displayname"];
                        element.Level1 = reader["level1"];
                        element.Level2 = reader["level2"];
                        element.Name = reader["name"];
                        elements.Add(element);
                    }
                }
            }
            public void WriteXml(XmlWriter writer)
            {
                foreach (ElementSeacrhStruct et in elements)
                {
                    writer.WriteStartElement("element");
                    writer.WriteAttributeString("displayname", et.DisplayName);
                    writer.WriteAttributeString("level1", et.Level1);
                    writer.WriteAttributeString("level2", et.Level2);
                    writer.WriteAttributeString("name", et.Name);
                    writer.WriteEndElement();
                }
            }

            public ElementSearchXml Add(ElementSearchXml T1, ElementSearchXml T2)
            {
                T1.elements.AddRange(T2.elements);
                return T1;
            }

            public ElementSearchXml Subtract(ElementSearchXml T1, ElementSearchXml T2)
            {
                throw new NotImplementedException();
            }

            public ElementSearchXml Multiply(ElementSearchXml T1, ElementSearchXml T2)
            {
                throw new NotImplementedException();
            }

            public ElementSearchXml Div(ElementSearchXml T1, ElementSearchXml T2)
            {
                throw new NotImplementedException();
            }

            public ElementSearchXml Add(ElementSearchXml T1)
            {
                elements.AddRange(T1.elements);
                return this;
            }

            public ElementSearchXml Subtract(ElementSearchXml T1)
            {
                throw new NotImplementedException();
            }

            public ElementSearchXml Multiply(ElementSearchXml T1)
            {
                throw new NotImplementedException();
            }

            public ElementSearchXml Div(ElementSearchXml T1)
            {
                throw new NotImplementedException();
            }

        }
    [XmlRoot("UserControl")]
    public class ElementXaml : IXmlSerializable, IOperation<ElementXaml>
    {
        //Viewbox _viewbox;
        //GridXaml _grid;
        public List<BgElementCommonXaml> _bgElementCommons =new List<BgElementCommonXaml>();
        public List<BgTextBlock> _bgTextBlocks=new List<BgTextBlock>();

        public ElementXaml Add(ElementXaml T1)
        {
            Margin _margin = new Margin();
            BgTextBlock bgtextblock = _bgTextBlocks.Last();
            _margin = _margin.MarginChange(bgtextblock._margin, bgtextblock._iColWidth, bgtextblock._iColHight);
            foreach (BgElementCommonXaml bgCommonXaml in T1._bgElementCommons)
                bgCommonXaml._margin.Add(_margin);
            foreach (BgTextBlock bgText in T1._bgTextBlocks)
                bgText._margin.Add(_margin);
            this._bgElementCommons.AddRange(T1._bgElementCommons);
            this._bgTextBlocks.AddRange(T1._bgTextBlocks);
            return this;

        }

        public ElementXaml Add(ElementXaml T1, ElementXaml T2)
        {
            Margin _margin = new Margin();
            BgTextBlock bgtextblock = T1._bgTextBlocks.Last();
            _margin = _margin.MarginChange(bgtextblock._margin, bgtextblock._iColWidth, bgtextblock._iColHight);
            foreach (BgElementCommonXaml bgCommonXaml in T2._bgElementCommons)
                bgCommonXaml._margin.Add(_margin);
            foreach (BgTextBlock bgText in T2._bgTextBlocks)
                bgText._margin.Add(_margin);
            this._bgElementCommons.AddRange(T2._bgElementCommons);
            this._bgTextBlocks.AddRange(T2._bgTextBlocks);
            return T1;
        }


        public ElementXaml Div(ElementXaml T1)
        {
            throw new NotImplementedException();
        }
        public ElementXaml Div(ElementXaml T1, ElementXaml T2)
        {
            throw new NotImplementedException();
        }
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public ElementXaml Multiply(ElementXaml T1)
        {
            throw new NotImplementedException();
        }
        public ElementXaml Multiply(ElementXaml T1, ElementXaml T2)
        {
            throw new NotImplementedException();
        }
        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public ElementXaml Subtract(ElementXaml T1)
        {
            throw new NotImplementedException();
        }

        public ElementXaml Subtract(ElementSearchXml T1)
        {
            throw new NotImplementedException();
        }

        public ElementXaml Subtract(ElementXaml T1, ElementXaml T2)
        {
            throw new NotImplementedException();
        }
        public void WriteXml(XmlWriter writer)
        {
            //Namespace.
            writer.WriteAttributeString("xmlns", "WIMAP_Controls", null, "clr-namespace:WIMAP.Common.Controls;assembly=WIMAP.Common.SL");
            writer.WriteAttributeString("xmlns", "", null, "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            writer.WriteAttributeString("xmlns", "x", null, "http://schemas.microsoft.com/winfx/2006/xaml");
            writer.WriteAttributeString("xmlns", "d", null, "http://schemas.microsoft.com/winfx/2006/xaml");
            writer.WriteAttributeString("xmlns", "mc", null, "http://schemas.openxmlformats.org/markup-compatibility/2006");
            writer.WriteAttributeString("xmlns", "i", null, "http://schemas.microsoft.com/expression/2010/interactivity");
            writer.WriteAttributeString("xmlns", "BG_SCADA_Behaviors", null, "clr-namespace:BG_SCADA.Behaviors");
            writer.WriteAttributeString("xmlns", "BG_SCADA_Controls", null, "clr -namespace:BG_SCADA.Controls");
            writer.WriteAttributeString("xmlns", "ScadaBase_Controls", "","clr-namespace:ScadaBase.Controls;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "bgElementCommon", "", "clr-namespace:ScadaBase.Controls.BGElement;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "counter", "", "clr-namespace:ScadaBase.Controls.Alarm;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "command", "", "clr-namespace:ScadaBase.Controls.Command;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "display", "", "clr-namespace:ScadaBase.Controls.Display;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "ScadaBase_Controls_Alarm", "", "clr-namespace:ScadaBase.Controls.Alarm;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "BG_SCADA", "", "clr-namespace:ScadaBase.Controls.Alarm;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "ScadaBase_Behaviors", "", "clr-namespace:ScadaBase.Controls.Alarm;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "BG_SCADA_Behaviors_Conveyor", "", "clr-namespace:ScadaBase.Controls.Alarm;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "helper", "", "clr-namespace:ScadaBase.Controls.Alarm;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "Class", "", "BG_SCADA.Views.L2_Arrival_International_Claim_Area_1");

            //Workbook Property Setup
            //"ViewBox"
            writer.WriteStartElement("Viewbox");
            writer.WriteAttributeString("Margin", "0");
            //写Grid
            writer.WriteStartElement("Grid");
            writer.WriteAttributeString("x", "Name",null, "LayoutRoot");
            writer.WriteAttributeString("Background", "#00000000");
            writer.WriteAttributeString("Height", "1080");
            writer.WriteAttributeString("Width", "1920");
            //写bgElement
            foreach(BgElementCommonXaml bgElementCommonXaml in _bgElementCommons)
            {
                writer.WriteStartElement("bgElementCommon", "BGElementCommon", null);
                writer.WriteAttributeString("x", "Name", null, bgElementCommonXaml._name);
                writer.WriteAttributeString("Margin", bgElementCommonXaml._margin.ToString());
                writer.WriteAttributeString("LegendStyleName", bgElementCommonXaml._legendStyleName);
                writer.WriteAttributeString("Width", bgElementCommonXaml._width);
                writer.WriteAttributeString("Height", bgElementCommonXaml._height);
                writer.WriteAttributeString("ElementName", bgElementCommonXaml._elementName);
                writer.WriteAttributeString("DisplayName", bgElementCommonXaml._displayName);
                writer.WriteAttributeString("Commands", bgElementCommonXaml._commands);
                writer.WriteAttributeString("ScadaLevel", bgElementCommonXaml._scadaLevel);
                writer.WriteAttributeString("ControlObject", bgElementCommonXaml._controlObject);
                writer.WriteAttributeString("PowerBox", bgElementCommonXaml._powerBox);
                writer.WriteAttributeString("Level3View", bgElementCommonXaml._level3View);
                writer.WriteAttributeString("ChooseLeftClickMode", bgElementCommonXaml._chooseLeftClickMode);
                writer.WriteAttributeString("NavigateToView", bgElementCommonXaml._navigateToView);
                writer.WriteAttributeString("ElementType", bgElementCommonXaml._elementType);
                writer.WriteAttributeString("PLCName", bgElementCommonXaml._plcName);
                writer.WriteAttributeString("CommandSignalName", bgElementCommonXaml._commandSignalName);
                writer.WriteAttributeString("CommandMappingType", bgElementCommonXaml._commandMappingType);
                writer.WriteAttributeString("TypeDescription", bgElementCommonXaml._typeDescription);
                writer.WriteAttributeString("HasRightclickMenu", bgElementCommonXaml._hasRightclickMenu);

                //写Signal属性
                writer.WriteStartElement("bgElementCommon", "BGElementCommon.Signals", null);
                foreach (ElementXamlSignal signal in bgElementCommonXaml._elementXamlSignal)
                {
                    writer.WriteStartElement("helper", "BGSignal", null);
                    writer.WriteAttributeString("Id", signal._id);
                    writer.WriteAttributeString("UsePostfix", signal._usePostfix);
                    writer.WriteAttributeString("Postfix", signal._postfix);
                    writer.WriteAttributeString("UsePrefix", signal._usePrefix);
                    writer.WriteAttributeString("Prefix", signal._prefix);
                    writer.WriteAttributeString("KeepAliveType", signal._keepAliveType);
                    writer.WriteAttributeString("KeepAliveSignal", signal._keepAliveSignal);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                //写Background属性
                writer.WriteStartElement("bgElementCommon", "BGElementCommon.Background", null);
                writer.WriteStartElement("SolidColorBrush");
                writer.WriteAttributeString("Color", bgElementCommonXaml._elementXmalBackground._color);
                writer.WriteEndElement();
                writer.WriteEndElement();

                //写Behavior属性
                writer.WriteStartElement("i", "Interaction.Behaviors", null);
                writer.WriteStartElement("BG_SCADA_Behaviors_Conveyor", bgElementCommonXaml._elementXmalBehavior._behaviorName);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            foreach(BgTextBlock bgTextBlock in _bgTextBlocks)
            {
                writer.WriteStartElement("TextBlock");
                writer.WriteAttributeString("x","Name", bgTextBlock._name);
                writer.WriteAttributeString("Text", bgTextBlock._text);
                writer.WriteAttributeString("Height", bgTextBlock._height);
                writer.WriteAttributeString("FontSize", bgTextBlock._fontSize);
                writer.WriteAttributeString("Width", bgTextBlock._width);
                writer.WriteAttributeString("Margin", bgTextBlock._margin.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }

    public class ElementSeacrhStruct
        {
            private string _displayName;
            private string _level1;
            private string _level2;
            private string _name;
            public string DisplayName { get { return _displayName; } set { _displayName = value; } }
            public string Level1 { get { return _level1; } set { _level1 = value; } }
            public string Level2 { get { return _level2; } set { _level2 = value; } }
            public string Name { get { return _name; } set { _name = value; } }
            public ElementSeacrhStruct() { }
            public ElementSeacrhStruct(string displayname, string level1, string level2, string name)
            {
                _displayName = displayname;
                _level1 = level1;
                _level2 = level2;
                _name = name;
            }
        }
   public class GridXaml
    {
        private string _name;
        private string _background;
        private string _height;
        private string _width;
    }
    public class BgXamlBase
    {
        public string _name;
        public string _width = "20";
        public string _height="20";
        public Margin _margin;
        public  int _iColWidth = 70;
        public  int _iColHight = 70;
    }
    public struct Margin
    {
        public int a;
        public int b;
        public int c;
        public int d;
        private string _margin;

        public override string ToString()
        {
            _margin = string.Format("{0},{1},{2},{3}", a, b, c, d);
            return _margin;
        }
        public Margin Add(Margin T1)
        {
            this.a = this.a + T1.a;
            this.b = this.a + T1.b;
            this.c = this.a + T1.c;
            this.d = this.a + T1.d;
            return this;
        }
        public Margin MarginChange(Margin margin, int iColWidth, int iRowHight)
        {
            const int viewWidth = 1920;
            //int iColWidth = 70;
            //int iRowHight = 70;
            if (margin.a + iColWidth > viewWidth)
            {
                margin.a = 0;
                margin.b = margin.b + iRowHight;
            }
            else
            {
                margin.b = margin.b + iColWidth;
            }
            return margin;
        }
    }
    public class BgTextBlock:BgXamlBase
    {
        public string _text;

        public string _fontSize;
    }
    public class BgElementCommonXaml:BgXamlBase
    {
        private ConveyorRow _conveyor;
        public string _stlye = "BG_DefaultSymbol";
        public string _legendStyleName= "BG_DefaultSymbol";
        public string _elementName;
        public string _displayName;
        public string _commands;
        public string _scadaLevel="Level2";
        public string _controlObject;
        public string _powerBox;
        public string _level3View= "Level3ViewConv_47";
        public string _chooseLeftClickMode;
        public string _navigateToView;
        public string _elementType;
        public string _plcName;
        public string _commandSignalName;
        public string _commandMappingType;
        public string _typeDescription;
        public string _hasRightclickMenu;
        public List<ElementXamlSignal> _elementXamlSignal = new List<ElementXamlSignal>();
        public ElementXamlBackground _elementXmalBackground ;
        public ElementXamlBehavior _elementXmalBehavior ;
        public BgElementCommonXaml()
        {

        }
        public BgElementCommonXaml(ConveyorRow conveyor,BaseParameter baseParameter)
        {
            _conveyor = conveyor;
            _name = string.Format("{0}_{1}_{2}_{3}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName);
            _height = "20";
            _margin = new Margin();
            _stlye = string.Format("{StaticResource {0}}", "Conveyor_Straight_L2");
            _scadaLevel = "Level2";
            _navigateToView = "";
            _elementName = string.Format("{0}_{1}_{2}_{3}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName);
            _chooseLeftClickMode = conveyor.sLeftClick;
            _legendStyleName = "BG_DefaultSymbol";
            _displayName = conveyor.sDisplayName;
            _commands = _elementName + "_CM";
            _controlObject = conveyor.sBehaviorName;
            _powerBox = conveyor.sPowerBox;
            _level3View = "Level3ViewConv_47";
            _elementType = conveyor.sElementType;
            _plcName = conveyor.sPLC;
            _commandSignalName = _elementName + "_CM";
            _commandMappingType = conveyor.sCommandMapping;
            _typeDescription = conveyor.sTypeDescription;
            _hasRightclickMenu = "True";

            int signalCounts = 0;
            foreach (string signalMapping in conveyor.sSignalMapping)
            {
                var bitCounts = baseParameter.SingleMappingTable.AsEnumerable().Count(p => p.Field<string>(baseParameter.SignalMappingColName.sType) == signalMapping);
                if (bitCounts != 0)
                    signalCounts = bitCounts / 32 + 1;
            }
            for (int i = 1; i <= signalCounts; i++)
            {
                ElementXamlSignal signalGroup = new ElementXamlSignal();
                signalGroup._id = string.Format("Signal{0}", i);
                signalGroup._usePostfix = "True";
                signalGroup._postfix = string.Format("_SIGNAL_{0}",i);
                signalGroup._usePrefix = "False";
                signalGroup._prefix = "";
                signalGroup._keepAliveType = "Constant";
                signalGroup._keepAliveSignal = string.Format("{0}_{1}_KAL_MAIN_KAL_MAIN_ACTIVE", conveyor.sSystem, conveyor.sPLC);
                _elementXamlSignal.Add(signalGroup);
            }
            ElementXamlBackground _elementXmalBackground = new ElementXamlBackground();
            _elementXmalBackground._color = string.Format("{StaticResource BG_COLOR_EDGE_{0}}", conveyor.sEdgeColor);

            ElementXamlBehavior _elementXmalBehavior = new ElementXamlBehavior();
            _elementXmalBehavior._behaviorName = string.Format("{0}Behavior", conveyor.sBehaviorName);
        }
     
    }

    public class ElementXamlSignal
    {
        public string _id;
        public string _usePostfix;
        public string _postfix;
        public string _usePrefix;
        public string _prefix;
        public string _keepAliveType;
        public string _keepAliveSignal;

    }
    public class ElementXamlBackground
    {
        public string _color;
    }
    public class ElementXamlBehavior
    {
        public string _behaviorName;
    }
    public struct KepWareData
    {
        public string TagName;
        public string Address;
        public string DataType;
        public string RespectData ;
        public string ClientAccess;
        public string ScanRate;
        public string Description;
    }
}
