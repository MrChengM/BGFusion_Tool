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
}
