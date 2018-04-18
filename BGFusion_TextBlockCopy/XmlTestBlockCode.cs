using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BGFusion_TextBlockCopy
{
    public class XmlTestBlockCode
    {
        public string sTemple;
        public List<string> sElements = new List<string>();
        public string sOutPutXmlTestBlockCode()
        {
            string sOutPutXmlTestBlock;
            try
            {

            }
            catch(Exception ex)
            {
                MessageBox.Show("Xml TestBlock convert error： " + ex.Message);
            }
            sOutPutXmlTestBlock =string.Format(sTemple,sElements.ToArray<string>());
            return sOutPutXmlTestBlock; 
        }
    }
}
