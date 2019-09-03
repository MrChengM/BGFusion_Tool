using BGFusionTools.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BGFusionTools.Serialization
{
    [XmlRoot("monitor")]
    public class SignalMonitor : IXmlSerializable, IOperation<SignalMonitor>
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
                            if (value.Count() == 0)
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

        public SignalMonitor Add(SignalMonitor T1, SignalMonitor T2)
        {
            throw new NotImplementedException();
        }

        public SignalMonitor Subtract(SignalMonitor T1, SignalMonitor T2)
        {
            throw new NotImplementedException();
        }

        public SignalMonitor Multiply(SignalMonitor T1, SignalMonitor T2)
        {
            throw new NotImplementedException();
        }

        public SignalMonitor Div(SignalMonitor T1, SignalMonitor T2)
        {
            throw new NotImplementedException();
        }

        public SignalMonitor Add(SignalMonitor T1)
        {
            throw new NotImplementedException();
        }

        public SignalMonitor Subtract(SignalMonitor T1)
        {
            throw new NotImplementedException();
        }

        public SignalMonitor Multiply(SignalMonitor T1)
        {
            throw new NotImplementedException();
        }

        public SignalMonitor Div(SignalMonitor T1)
        {
            throw new NotImplementedException();
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            throw new NotImplementedException();
        }
    }

    public struct KepWareData
    {
        public string TagName;
        public string Address;
        public string DataType;
        public string RespectData;
        public string ClientAccess;
        public string ScanRate;
        public string Description;
    }
}
