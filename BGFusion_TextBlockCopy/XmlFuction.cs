using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BGFusion_TextBlockCopy
{
    public class XmlFuction
    {
 
        //private static string _sFileName;
        //private List<List<string>> _lDataList;

        //节点名定义
        private  string _sMainNode = "/a:Workbook/a:Worksheet/a:Table";
        private  string _sLimbNode= "Row";
        private  string _sItemNode= "Cell";
        private  string _sDataNode= "Data";

        public XmlFuction()
        {
            //xmlDoc = new XmlDocument();
        }
       /* public string sFileName
        {
            get { return _sFileName; }
            set  {  _sFileName=value; }
        }
        */
        public string sMainNode
        {
            get { return _sMainNode; }
            set { _sMainNode = value; }
        }
        public string sLimbNode
        {
            get { return _sLimbNode; }
            set { _sLimbNode = value; }
        }
        public string sItemNode
        {
            get { return _sItemNode; }
            set { _sItemNode = value; }
        }
        public string sDataNode
        {
            get { return _sDataNode; }
            set { _sDataNode = value; }
        }
        public void XmlWrite(string _sFileName,List<List<string>> _lDataList)
        {
            int iCounts = (int)(_lDataList.Count / 1000);
            for(int i=0; i <= iCounts; i++)
            {
                 XmlDocument xmlDoc = new XmlDocument();
                int iStartIndex = i * 1000;
                int iLength;
                if (i == iCounts)
                    iLength = _lDataList.Count - iStartIndex;
                else
                    iLength = 1000;
                List<List<string>> _lDataListSmall = _lDataList.GetRange(iStartIndex, iLength);
                xmlDoc.Load(_sFileName);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                nsmgr.AddNamespace("a", "urn:schemas-microsoft-com:office:spreadsheet");
                XmlNode root = xmlDoc.SelectSingleNode(_sMainNode, nsmgr);
                if( i==0)
                root.RemoveAll();
                foreach (List<string> ls in _lDataListSmall)
                {
                    XmlElement xeLimb = xmlDoc.CreateElement(_sLimbNode, nsmgr.LookupNamespace("a"));
                    foreach (string ss in ls)
                    {
                        XmlElement xeItem = xmlDoc.CreateElement(_sItemNode, nsmgr.LookupNamespace("a"));
                        XmlElement xeData = xmlDoc.CreateElement(_sDataNode, nsmgr.LookupNamespace("a"));
                        XmlAttribute xa = xmlDoc.CreateAttribute("ss", "Type", nsmgr.LookupNamespace("a"));
                        xa.Value = "String";
                        xeData.Attributes.Append(xa);
                        xeData.InnerText = ss;
                        xeItem.AppendChild(xeData);
                        xeLimb.AppendChild(xeItem);
                    }
                    root.AppendChild(xeLimb);
                }
                xmlDoc.Save(_sFileName);
            }
        }
    }
}
