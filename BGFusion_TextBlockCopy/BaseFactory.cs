using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFusion_TextBlockCopy
{
    class BaseFactory 
    {
        private DaTableConverParameter baseFactoryParameter = new DaTableConverParameter();
        private List<string> slistColName;
        private DataTable tempTable;
        private bool bconvAlarm;
        private bool boPCIfo;
        private bool bsingle;
        private bool bcommand;
        private bool bhours;
        private int ixmlType;

        public DaTableConverParameter BaseFactoryParameter
        {
            get
            {
                return baseFactoryParameter;
            }
            set
            {
                baseFactoryParameter = value;
            }
        }
        public List<string> sListColName
        {
            get
            {
                return slistColName;
            }
            set
            {
                slistColName = value;
            }
        }
        public DataTable TempTable
        {
            get
            {
                return tempTable;
            }
            set
            {
                tempTable = value;
            }
        }
        public bool bConvAlarm
        {
            get
            {
                return bconvAlarm;
            }
            set
            {
                bconvAlarm = value;
            }
        }
        public bool bOPCIfo
        {
            get
            {
                return boPCIfo;
            }
            set
            {
                boPCIfo = value;
            }
        }
        public bool bSingle
        {
            get
            {
                return bsingle;
            }
            set
            {
                bsingle = value;
            }
        }
        public bool bCommand
        {
            get
            {
                return bcommand;
            }
            set
            {
                bcommand = value;
            }
        }
        public bool bHours
        {
            get
            {
                return bhours;
            }
            set
            {
                bhours = value;
            }
        }
        public int iXmlType
        {
            get
            {
                return ixmlType;
            }
            set
            {
                ixmlType = value;
            }
        }
        public BaseTableConvert CreatTableConvert(string ConvertType)
        {
            BaseTableConvert baTableConvert ;
            if (ConvertType == "ToConfig")
                baTableConvert = new DaTableToConfig(baseFactoryParameter, bconvAlarm, boPCIfo, slistColName);
            else if(ConvertType == "ToLevel1Data")
                baTableConvert = new DaTableToLevel1Data(baseFactoryParameter, tempTable);
            else if (ConvertType == "ToOPCData")
                baTableConvert = new DaTableToOPCData(baseFactoryParameter, bsingle, bcommand, bhours);
            else if (ConvertType == "ToTeData")
                baTableConvert = new DaTableToTeData(baseFactoryParameter);
            else if(ConvertType == "ToXml")
                baTableConvert = new DaTableToTeXml(baseFactoryParameter, ixmlType);
            else if (ConvertType == "ToTeList")
                baTableConvert = new DaTableToTeList(baseFactoryParameter);
            else
                throw new NotImplementedException();
            return baTableConvert;
        }
    }

}
