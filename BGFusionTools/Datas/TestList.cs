using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.Collections;

namespace BGFusionTools.Datas
{
    class TestList:BaseData
    {
        private Dictionary<string, string> dictionaryData;
        private List<TestListStruct> listData=new List<TestListStruct>();
        public TestList(BaseParameter ConverParameter)
        {
            this.baseParameter = ConverParameter;
        }
        public Dictionary<string, string> DictionaryData { get { return dictionaryData; } set { dictionaryData = value; } }
        public List<TestListStruct> ListData { get { return listData; } set { listData = value; } }
    }
    public struct TestListStruct
    {
        private EnumerableRowCollection<DataRow> _signalMappingTable;
        private DataColumnCollection _signalColumn;
        private List<string> _elementList;
        public TestListStruct(EnumerableRowCollection<DataRow> signalMappingTable,List<string> elementList, DataColumnCollection signalColumn)
        {
            _signalMappingTable = signalMappingTable;
            _signalColumn = signalColumn;
            _elementList = elementList;
        }

        public EnumerableRowCollection<DataRow> SignalMappingTable { get { return _signalMappingTable; } set { _signalMappingTable =value; } }
        public List<string> ElementList { get { return _elementList; } set { _elementList = value; } }
        public DataColumnCollection SignalColumn { get { return _signalColumn; } set { _signalColumn = value; } }
    }
}
