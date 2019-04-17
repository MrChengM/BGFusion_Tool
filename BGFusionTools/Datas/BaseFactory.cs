using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGFusionTools.Datas
{
    class BaseFactory 
    {
        private BaseParameter baseParameter = new BaseParameter();
        private List<string> slistColName;
        private DataTable tempTable;
        private bool bconvAlarm;
        private bool boPCIfo;
        private bool bsingle;
        private bool bcommand;
        private bool bhours;
        private int ixmlType;

        public BaseParameter BaseFactoryParameter
        {
            get
            {
                return baseParameter;
            }
            set
            {
                baseParameter = value;
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
        public BaseData CreatTableConvert(string ConvertType)
        {
            BaseData baTableConvert ;
            if (ConvertType == "ToConfig")
                baTableConvert = new ConfigData(baseParameter, bconvAlarm, boPCIfo, slistColName);
            else if(ConvertType == "ToLevel1Data")
                baTableConvert = new Level1Data(baseParameter, tempTable);
            else if (ConvertType == "ToOPCData")
                baTableConvert = new OPCData(baseParameter, bsingle, bcommand, bhours);
            else if (ConvertType == "ToTeData")
                baTableConvert = new TestData(baseParameter);
            else if(ConvertType == "ToXml")
                baTableConvert = new XmlData(baseParameter, ixmlType);
            else if (ConvertType == "ToTeList")
                baTableConvert = new TestList(baseParameter);
            else
                throw new NotImplementedException();
            return baTableConvert;
        }
    }

}
