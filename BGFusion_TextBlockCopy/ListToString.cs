using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFusion_TextBlockCopy
{
    public class ListToString
    {
        public static string OutPutString(List<string> lStrings)
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
                    outputstring = outputstring + "\r\n" +lString;

                }
            }
            return outputstring;
        }

    }
}
