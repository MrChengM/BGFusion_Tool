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
        public override Dictionary<string,string> ToDictionary()
        {
            //ListDatas = new List<ListData>();
            dictionaryData = new Dictionary<string, string>();
            EnumerableRowCollection<DataRow> MainRows = LinqToTable();
            try
            {
                switch (baseParameter.ViewNum)
                {
                    case 0:
                        break;
                    case 1:
                        var ELementLineGroups = from p in MainRows
                                                group p by new { system = p.Field<string>(baseParameter.TaglistColName[1, 0]), plc = p.Field<string>(baseParameter.TaglistColName[1, 1]), line = p.Field<string>(baseParameter.TaglistColName[1, 3]), view = p.Field<string>(baseParameter.TaglistColName[1, 16]) } into pp
                                                select pp;
                        foreach (var ELementLineGroup in ELementLineGroups)
                        {
                            string sSystem = ELementLineGroup.Key.system;
                            string sPlcLink = ELementLineGroup.Key.plc;
                            string sEquipmentLine = ELementLineGroup.Key.line;
                            string sAreaLevel2view = ELementLineGroup.Key.view;

                            string sLinesGroups = sPlcLink + "." + sEquipmentLine + "." + sAreaLevel2view;
                            foreach (DataRow selectConRow in ELementLineGroup)
                            {
                                string sEquipmentElement = selectConRow[4].ToString();
                                string sElementName = sEquipmentLine + "." + sEquipmentElement;
                                dictionaryData.Add(sElementName, sLinesGroups);
                            }
                        }
                        break;
                    case 2:

                        foreach (DataRow selectConRow in MainRows)
                        {
                            //string sPoweBox = selectConRow[2].ToString();
                            string sPlcLink = selectConRow[1].ToString();
                            string sEquipmentLine = selectConRow[3].ToString();
                            string sEquipmentElement = selectConRow[4].ToString();
                            string sSingleMapping1 = selectConRow[6].ToString();
                            string sAreaLevel2view = selectConRow[16].ToString();
                            string sElementName = sEquipmentLine + "." + sEquipmentElement;
                            dictionaryData.Add(sElementName, sPlcLink);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据处理错误：" + ex.Message);
            }
            return dictionaryData;
        }
        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public override int ToInt()
        {
            throw new NotImplementedException();
        }

        public override List<List<string>> ToList()
        {
            throw new NotImplementedException();
        }

        public override DataTable ToDataTable()
        {
            throw new NotImplementedException();
        }

        public override void OutData()
        {
            try
            {
                
                switch (baseParameter.ViewNum)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        var ELementGroups = from p in baseParameter.TaglistTable.AsEnumerable()
                                                    //where p.Field<string>(baseParameter.TaglistColName[1, 5])!=null
                                                group p by new { signalMapping = p.Field<string>(baseParameter.TaglistColName[1, 6]) } into pp
                                                select pp;
                        foreach (var ElementGroup in ELementGroups)
                        {
                            //string sPoweBox = selectConRow[2].ToString();
                            string signalMapping = ElementGroup.Key.signalMapping;
                            var signalMappingTable = from p in baseParameter.SingleMappingTable.AsEnumerable()
                                                     where p.Field<string>(baseParameter.BasefileColName[1, 0]) == signalMapping
                                                     select p;
                            List<string> displayNames = new List<string>();
                            foreach(DataRow dr in ElementGroup)
                            {
                                string displayName = dr[12].ToString();
                                displayNames.Add(displayName);
                            }
                            ListData.Add(new TestListStruct(signalMappingTable, displayNames,baseParameter.SingleMappingTable.Columns));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据处理错误：" + ex.Message);
            }
        }

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
