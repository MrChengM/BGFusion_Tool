using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
/// <summary>
/// Xml to Excel by Serializable
/// </summary>
namespace BGFusionTools.ExcelSerialization
{
    public class XmlExcelSerialiaztion //:IXmlSerializable
    {
        /// <summary>
        ///XMl Serialiaztion 
        /// </summary>
        /// <param name="The File Path on disk"></param>
        /// <param name="Double list datas"></param>
        /// <returns></returns>
        public static bool XmlSerialiaztion(string sFilePath, List<List<string>> llString)
        {
            try
            {
                XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
                xmlns.Add("", "urn:schemas-microsoft-com:office:spreadsheet");
                xmlns.Add("o", "urn:schemas-microsoft-com:office:office");
                xmlns.Add("x", "urn:schemas-microsoft-com:office:excel");
                xmlns.Add("ss", "urn:schemas-microsoft-com:office:spreadsheet");
                xmlns.Add("html", "http://www.w3.org/TR/REC-html40");
                List<Row> lXRow = new List<Row>();
                foreach (List<string> ls in llString)
                {
                    List<Cell> lXCell = new List<Cell>();
                    foreach (string s in ls)
                    {
                        Data xData = new Data(s);
                        Cell xCell = new Cell(xData);
                        lXCell.Add(xCell);
                    }
                    Row xRow = new Row(lXCell);
                    lXRow.Add(xRow);
                }
                XmlTable Table = new XmlTable(lXRow);
                XmlWorkSheet xWorkSheet = new XmlWorkSheet(Table);
                Workbook xWorkBook = new Workbook(xmlns, xWorkSheet);
                Stream sFileSteam = new FileStream(sFilePath, FileMode.Create, FileAccess.ReadWrite);
                XmlWriterSettings xmlSet = new XmlWriterSettings();

                XmlSerializer xmlserial = new XmlSerializer(typeof(Workbook));
                xmlserial.Serialize(sFileSteam, xWorkBook, xmlns);
                string sXml = "\r\n"+"<?mso-application progid=\"Excel.Sheet\"?>" ;
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
        public static bool XmlSerialiaztion(string sFilePath, Workbook xWorkbook)
        {
            try
            {
                xWorkbook.xmlns.Add("", "urn:schemas-microsoft-com:office:spreadsheet");
                xWorkbook.xmlns.Add("o", "urn:schemas-microsoft-com:office:office");
                xWorkbook.xmlns.Add("x", "urn:schemas-microsoft-com:office:excel");
                xWorkbook.xmlns.Add("ss", "urn:schemas-microsoft-com:office:spreadsheet");
                xWorkbook.xmlns.Add("html", "http://www.w3.org/TR/REC-html40");
                Stream sFileSteam = new FileStream(sFilePath, FileMode.Create, FileAccess.ReadWrite);
                XmlSerializer xmlserial = new XmlSerializer(typeof(Workbook));
                xmlserial.Serialize(sFileSteam, xWorkbook, xWorkbook.xmlns);
                string sXml = "\r\n" + "<?mso-application progid=\"Excel.Sheet\"?>" ;
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
        public static Workbook XmlDeserialize(string sFilePath)
        {
            try
            {
            Workbook xWorkBook = new Workbook();
            Stream sFileSteam = new FileStream(sFilePath, FileMode.Open, FileAccess.ReadWrite);
            XmlSerializer xmlserial = new XmlSerializer(typeof(Workbook), "urn:schemas-microsoft-com:office:spreadsheet");//Add defult namespace fixed Deserialize bug.
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
        /// <summary>
        /// File stream insert fuction
        /// </summary>
        /// <param name="sFileStream"></param>
        /// <param name="Insert string"></param>
        /// <param name="Insert Postion"></param>
        /// <param name="Data Block Size to transfer "></param>
        public static void FileStreamInsert(Stream sFileStream, string sInsert, long lPostion,int iBlockLength=10000)
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
    
    [XmlRoot("Workbook")]//xmlroot name must same to class name,or else the Deserialize error.
    public class Workbook
    {
        public Workbook() { }
        public Workbook(XmlSerializerNamespaces xmlns, XmlWorkSheet xWorkSheet) { this._xmlns = xmlns; this._xWorkSheet = xWorkSheet; }
        private XmlSerializerNamespaces _xmlns = new XmlSerializerNamespaces();
        private XmlDocumentProperties _xDocumentProperties = new XmlDocumentProperties();
        private XmlOfficeDocumentSettings _xOfficeDocumentSetting = new XmlOfficeDocumentSettings();
        private XmlExcelWorkbook _xExcelWorkbook = new XmlExcelWorkbook();
        private XmlStyles _xStyles = new XmlStyles();
        private XmlWorkSheet _xWorkSheet = new XmlWorkSheet();
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns { get { return _xmlns; } set { _xmlns = value; } }
        [XmlElement("DocumentProperties")]
        public XmlDocumentProperties DocumentProperties { get { return _xDocumentProperties; } set { _xDocumentProperties = value; } }
        [XmlElement("OfficeDocumentSettings")]
        public XmlOfficeDocumentSettings OfficeDocumentSettings { get { return _xOfficeDocumentSetting; } set { _xOfficeDocumentSetting = value; } }
        [XmlElement("ExcelWorkbook")]
        public XmlExcelWorkbook ExcelWorkbook { get { return _xExcelWorkbook; } set { _xExcelWorkbook = value; } }
        [XmlElement("Styles")]
        public XmlStyles Styles { get { return _xStyles; } set { _xStyles = value; } }
        [XmlElement("Worksheet")]
        public XmlWorkSheet Worksheet { get { return _xWorkSheet; } set { _xWorkSheet = value; } }
        public static Workbook operator +(Workbook a, Workbook b)
        {
            Workbook c = new Workbook();
            c.Worksheet = a.Worksheet + b.Worksheet;
            return c;
        }
    }
    public class XmlDocumentProperties
    {
        public XmlDocumentProperties() { }
    }

    public class XmlOfficeDocumentSettings
    {
        public XmlOfficeDocumentSettings() { }
    }
    public class XmlExcelWorkbook
    {
        public XmlExcelWorkbook() { }
    }
    public class XmlStyles
    {
        public XmlStyles() { }
    }
    public class XmlWorkSheet
    {
        public XmlWorkSheet() { }
        public XmlWorkSheet(XmlTable Table) { this._Table = Table; }
        private string _name = "Sheet1";
        private XmlTable _Table;
        private XmlWorksheetOptions _xWorksheetOptions = new XmlWorksheetOptions();
        [XmlAttribute("Name", Namespace = "urn:schemas-microsoft-com:office:spreadsheet")]
        public string Name { get { return _name; } set { _name = value; } }
        [XmlElement("Table")]
        public XmlTable Table { get { return _Table; } set { _Table = value; } }
        [XmlElement("WorksheetOptions")]
        public XmlWorksheetOptions XWorksheetOptions { get { return _xWorksheetOptions; } set { _xWorksheetOptions = value; } }
        public static XmlWorkSheet operator +(XmlWorkSheet a, XmlWorkSheet b)
        {
            XmlWorkSheet c = new XmlWorkSheet();
            c.Table = a.Table + b.Table;
            return c;
        }
    }
    public class XmlTable
    {
        public XmlTable() { }
        public XmlTable(List<Row> lXRow) { this._lXRow = lXRow; }
        private List<Row> _lXRow = new List<Row>();
        [XmlElement("Row", IsNullable = false)]
        public List<Row> LXRow { get { return _lXRow; } set { _lXRow = value; } }
        public static XmlTable operator +(XmlTable a, XmlTable b)
        {
            XmlTable c = new XmlTable();
            b.LXRow.RemoveAt(0); //删除第一列
            a.LXRow.AddRange(b.LXRow);
            c.LXRow.AddRange(a.LXRow);
            return c;
        }
    }
    public class Row
    {
        public Row() { }
        public Row(List<Cell> lXCell) { this._lXCell = lXCell; }
        private List<Cell> _lXCell;
        [XmlElement("Cell", IsNullable = false)]
        public List<Cell> LXCell { get { return _lXCell; } set { _lXCell = value; } }
    }
    public class Cell
    {
        public Cell() { }
        public Cell(Data xData) { this._xData = xData; }
        private Data _xData;
        [XmlElement("Data")]
        public Data XData { get { return _xData; } set { _xData = value; } }
    }
    public class Data
    {
        public Data() { }
        public Data(string s)
        {
            Xdata = s;
        }
        [XmlText]
        public string Xdata;
        [XmlAttribute("Type", Namespace = "urn:schemas-microsoft-com:office:spreadsheet")]
        public string sType = "String";
    }
    public class XmlWorksheetOptions
    {
        public XmlWorksheetOptions() { }
    }
}


