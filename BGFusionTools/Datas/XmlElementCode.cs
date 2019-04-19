using BGFusionTools.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BGFusionTools.Datas
{
    public class baseXamlElement
    {
        public List<string> sLineElements = new List<string>();

    }
    public class XamlElementFirst: baseXamlElement
    {

        //line1

        //public List<string> sLineElements =new List<string>();
    }
    public class XamlElementThird: baseXamlElement
    {
        //public List<string> sLineElements = new List<string>();
    }
    public class XamlElementSix:baseXamlElement
    {
        //public List<string> sLineElements = new List<string>();
    }
    public class XamlElementnine: baseXamlElement
    {
        //public List<string> sLineElements = new List<string>();
    }
    public class XamlElementCode
    {
        public string sTemplate;
        public int iSingleCounts;
        private List<string> lLines =new List<string>();
        public string[] sLines=new string[8];
        public XamlElementFirst xmlElementFirst = new XamlElementFirst();
        public List<XamlElementThird> xmlElementThirds = new List<XamlElementThird>();
        public XamlElementSix xmlElementSix = new XamlElementSix();
        public XamlElementnine xmlElementnine = new XamlElementnine();

        //public string[] sLine3Elements = new string[7];
        //public string sLine6Elements;
        //public string sLine9Elements;

        public string sOutPutXmlElementCode()
        {
            string sOutPutXmlElement=null;
            try
            {
                sLines = sTemplate.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                string[] sLines1Element = xmlElementFirst.sLineElements.ToArray<string>();
                lLines.Add(string.Format(sLines[0], sLines1Element));
                lLines.Add(sLines[1]);
                foreach (XamlElementThird xmlElementThird in xmlElementThirds)
                {
                    string[] sLines3Element = xmlElementThird.sLineElements.ToArray<string>();
                    lLines.Add(string.Format(sLines[2], sLines3Element));
                }
                lLines.Add(sLines[3]);
                lLines.Add(sLines[4]);
                string[] sLines6Element = xmlElementSix.sLineElements.ToArray<string>();

                lLines.Add(string.Format(sLines[5], sLines6Element));
                lLines.Add(sLines[6]);
                lLines.Add(sLines[7]);
                string[] sLines9Element = xmlElementnine.sLineElements.ToArray<string>();

                lLines.Add(string.Format(sLines[8], sLines9Element));

                lLines.Add(sLines[9]);
                lLines.Add(sLines[10]);
                sOutPutXmlElement = DataConvert.ToString(lLines);
            }
            catch(Exception ex)
            {
                MessageBox.Show("XmlElement convert error： " + ex.Message); 
            }
            return sOutPutXmlElement;
        }


    }
}
