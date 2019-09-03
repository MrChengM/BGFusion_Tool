using BGFusionTools.Datas;
using BGFusionTools.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFusionTools.Functions
{
    public class DataConvert
    {
        public static string ToString(List<string> lStrings)
        {
            string outputstring = null;
            foreach (string lString in lStrings)
            {
                if (outputstring == null)
                {
                    outputstring = lString;
                }
                else
                {
                    //outputstring = outputstring + "\r"+ lString;
                    outputstring = outputstring + "\r\n" + lString;

                }
            }
            return outputstring;
        }
        public static string ToString(List<List<string>> lStrings)
        {
            string outputstring = null;
            foreach(List<string> ls in lStrings)
            {
                string str = null;
                foreach (string ss in ls)
                {
                    if (str == null)
                    {
                        str = ss;
                    }
                    else
                    {
                        //outputstring = outputstring + "\r"+ lString;
                        str = str + " ," + ss;

                    }
                }
                if (outputstring == null)
                    outputstring = str;
                else
                    outputstring = outputstring + "\r\n"+ str;
            }
  
            return outputstring;
        }
        public static string ToString1(List<List<string>> lStrings)
        {
            string outputstring = null;
            foreach (List<string> ls in lStrings)
            {
                string str = null;
                foreach (string ss in ls)
                {
                    if (str == null)
                    {
                        str = ss;
                    }
                    else
                    {
                        //outputstring = outputstring + "\r"+ lString;
                        str = str + " \r\n" + ss;

                    }
                }
                if (outputstring == null)
                    outputstring = str;
                else
                    outputstring = outputstring + "\r\n" + str;
            }

            return outputstring;
        }
        public static string ToString(DataTable dt)
        {
            string outputstring = null;
            foreach (DataRow sr in dt.Rows)
            {
                string sDr ="";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sDr += sr[i].ToString();
                    if(i < dt.Columns.Count - 1)
                    {
                        sDr += ",";
                    }
                }
                if (outputstring == null)
                {
                    outputstring += sDr;
                }
                else
                {
                    outputstring += "\n";
                    outputstring += sDr;
                }
            }
            return outputstring;
        }
        public static string ToString(List<ElementSeacrhStruct> elements)
        {
            string outputstring = null;
            foreach (ElementSeacrhStruct element in elements)
            {
                string sDr = string.Format("< element displayname = \"{0}\" level1 = \"{1}\" level2 = \"{2}\" name = \"{3}\" />",
                    element.DisplayName,element.Level1,element.Level2,element.Name);
          
                if (outputstring == null)
                {
                    outputstring += sDr;
                }
                else
                {
                    outputstring += "\n";
                    outputstring += sDr;
                }
            }
            return outputstring;
        }
        public static string ToString(ElementXaml elementXamls)
        {
            string outputstring = null;
            foreach (BgElementCommonXaml element in elementXamls.BgElementCommons)
            {
                if (outputstring == null)
                    outputstring += element.ToString();
                else
                    outputstring += "\n" + element.ToString();
            }
            foreach(BgTextBlock element in elementXamls.BgTextBlocks)
            {
                if (outputstring == null)
                    outputstring += element.ToString();
                else
                    outputstring += "\n" + element.ToString();
            }
            return outputstring;
        }
    }
}
