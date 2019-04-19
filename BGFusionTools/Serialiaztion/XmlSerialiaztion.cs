using BGFusionTools.Datas;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public static bool XmlElementSearchDataSerialiaztion(string sFilePath,ElementSearchData elementSearchData)
        {
            try
            {
                Stream sFileSteam = new FileStream(sFilePath, FileMode.Create, FileAccess.ReadWrite);
                XmlSerializer xmlserial = new XmlSerializer(typeof(ElementSearchData));
                xmlserial.Serialize(sFileSteam, elementSearchData);
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
        public static ElementSearchData XmlElementSearchDatDeserialize(string sFilePath)
        {
            try
            {
                ElementSearchData elementSearchData = new ElementSearchData();
                Stream sFileSteam = new FileStream(sFilePath, FileMode.Open, FileAccess.ReadWrite);
                XmlSerializer xmlserial = new XmlSerializer(typeof(Workbook), "urn:schemas-microsoft-com:office:spreadsheet");
                sFileSteam.Position = 0;
                elementSearchData = (ElementSearchData)xmlserial.Deserialize(sFileSteam);
                sFileSteam.Flush();
                sFileSteam.Close();
                return elementSearchData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("XmlDeserialiaztion error： " + ex.Message);
                return default(ElementSearchData);
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
