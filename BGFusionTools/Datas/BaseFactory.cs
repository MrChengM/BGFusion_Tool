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
        public BaseFactory()
        {
        }
        public BaseFactory(BaseParameter baseParameter)
        {
            this.baseParameter = baseParameter;
        }

        public BaseParameter BaseParameter
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
        public BaseData CreatDataClass(string DataClassName)
        {
            BaseData baTableConvert ;
            if (DataClassName == "ConfigData")
                baTableConvert = new ConfigData(baseParameter, bconvAlarm, boPCIfo, slistColName);
            else if(DataClassName == "Level1Data")
                baTableConvert = new Level1Data(baseParameter, tempTable);
            else if (DataClassName == "OPCData")
                baTableConvert = new OPCData(baseParameter, bsingle, bcommand, bhours);
            else if (DataClassName == "TestData")
                baTableConvert = new TestData(baseParameter);
            else if(DataClassName == "XamlData")
                baTableConvert = new XamlData(baseParameter, ixmlType);
            else if (DataClassName == "TestList")
                baTableConvert = new TestList(baseParameter);
            else if(DataClassName == "ElementSearchData")
                baTableConvert = new ElementSearchData(baseParameter);
            else
                throw new NotImplementedException();
            return baTableConvert;
        }
    }

}
