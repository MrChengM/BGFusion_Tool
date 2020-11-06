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
        public TestList(BaseParameter ConverParameter)
        {
            this.baseParameter = ConverParameter;
        }
        public List<List<TestSheetRow>> CreateTestRows(ConveyorRow conveyor)
        {
            List<TestSheetRow> testSheetRows = new List<TestSheetRow>();

            //SignalName行
            TestSheetRow signalNameRow = new TestSheetRow();
            signalNameRow.SignalName_Mapping = string.Format("//{0}_{1}_{2}_{3}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName);
            testSheetRows.Add(signalNameRow);

            foreach (var signalMapping_Adderss in conveyor.sSignalMapping_Adderss)
            {
                //SignalMapping Name行
                TestSheetRow signalMapping = new TestSheetRow();
                signalMapping.SignalName_Mapping ="//"+ signalMapping_Adderss.Key;
                testSheetRows.Add(signalMapping);

                //SignalAddress Name行
                TestSheetRow signalAddress = new TestSheetRow();
                signalAddress.SignalName_Mapping = "//"+signalMapping_Adderss.Value;
                testSheetRows.Add(signalAddress);

                //signalMapping state alarm 行
                var dataRows = from p in baseParameter.SingleMappingTable.AsEnumerable()
                               where p.Field<string>(baseParameter.SignalMappingColName.sType) == signalMapping_Adderss.Key
                               &&
                               p.Field<string>(baseParameter.SignalMappingColName.sAlarmStatusNumber).ToLower() != "spare"
                               select p;
                foreach(var dataRow in dataRows)
                {
                    TestSheetRow datas = new TestSheetRow();
                    var col = baseParameter.SignalMappingColName;
                    var word =Convert.ToInt32( dataRow[col.sWord]);
                    var bit = Convert.ToInt32(dataRow[col.sBit]);
                    int offset = (word - 1) * 4 + bit / 8;
                    int dBxbit =bit % 8;
                    if (signalMapping_Adderss.Value != "" && signalMapping_Adderss.Value != null)
                        datas.PLCAddress = creatDBAddress(signalMapping_Adderss.Value, offset, dBxbit, true);
                    else
                        datas.PLCAddress = "";
                    datas.Function = dataRow[col.sAlarmStatusNumber].ToString();
                    datas.State = dataRow[col.sStateRef].ToString();
                    datas.FunctionText = dataRow[col.sFunctionalText].ToString();
                    datas.StateText = dataRow[col.sStateText].ToString();
                    if(col.sStateColor!=""&& col.sStateColor != null)
                    datas.Color = dataRow[col.sStateColor].ToString();
                    datas.Level1view = conveyor.sLevel1View;
                    datas.Level2view = conveyor.sLevel2View;
                    datas.LeftClick = conveyor.sLeftClick;
                    datas.PartName = dataRow[col.sPartName].ToString();
                    testSheetRows.Add(datas);
                }
            }
            List<List<TestSheetRow>> llTestSheetRows = new List<List<TestSheetRow>>();
            llTestSheetRows.Add(testSheetRows);
            return llTestSheetRows;
        }
        public List<List<string>> CreateCMCRows(ConveyorRow conveyor)
        {
            List<List<string>> llCMCRows = new List<List<string>>();
            List<string> _CMCRows = new List<string>();

            //SignalName行

            var SignalName = string.Format("//{0}_{1}_{2}_{3}", conveyor.sSystem, conveyor.sPLC, conveyor.sEquipmentLine, conveyor.sElementName);
            _CMCRows.Add(SignalName);
            if (SignalName.Contains("ENERGYSAVE"))
            {

                _CMCRows.Add("//" + conveyor.sCommandMapping);
                _CMCRows.Add("//" + conveyor.sCommandAddress);
                string PLCAddress = creatDBAddress(conveyor.sCommandAddress,0);
                _CMCRows.Add(PLCAddress);
            }
            else
            {
                foreach (var signalMapping_Adderss in conveyor.sSignalMapping_Adderss)
                {
                    _CMCRows.Add("//" + signalMapping_Adderss.Key);
                    _CMCRows.Add("//" + signalMapping_Adderss.Value);
                    var dataRows = from p in baseParameter.SingleMappingTable.AsEnumerable()
                                   where p.Field<string>(baseParameter.SignalMappingColName.sType) == signalMapping_Adderss.Key
                                   &&
                                   p.Field<string>(baseParameter.SignalMappingColName.sAlarmStatusNumber).ToLower() != "spare"
                                   select p;
                    foreach (var dataRow in dataRows)
                    {
                        var col = baseParameter.SignalMappingColName;
                        var word = Convert.ToInt32(dataRow[col.sWord]);
                        var bit = Convert.ToInt32(dataRow[col.sBit]);
                        int offset = (word - 1) * 4 + bit / 8;
                        int dBxbit = bit % 8;
                        string PLCAddress = creatDBAddress(signalMapping_Adderss.Value, offset, dBxbit, true);
                        _CMCRows.Add(PLCAddress);
                    }
                }
                if (conveyor.sCommandMapping != "" && conveyor.sCommandAddress != "" && conveyor.sCommandAddress.ToLower() != "no_signal")
                {
                    _CMCRows.Add("//" + conveyor.sCommandMapping);
                    _CMCRows.Add("//" + conveyor.sCommandAddress);
                    var dataRows = from p in baseParameter.CommandMappingTable.AsEnumerable()
                                   where p.Field<string>(baseParameter.CommandMappingColName.sType) == conveyor.sCommandMapping
                                   &&
                                   p.Field<string>(baseParameter.CommandMappingColName.sCommandText).ToLower() != "spare"
                                   select p;
                    foreach (var dataRow in dataRows)
                    {
                        var col = baseParameter.CommandMappingColName;
                        var word = Convert.ToInt32(dataRow[col.sElementLink]);
                        var bit = Convert.ToInt32(dataRow[col.sBit]);
                        int offset = (word - 1) * 4 + bit / 8;
                        int dBxbit = bit % 8;
                        string PLCAddress = creatDBAddress(conveyor.sCommandAddress, offset, dBxbit, true);
                        _CMCRows.Add(PLCAddress);
                    }
                }
            }
           
            llCMCRows.Add(_CMCRows);
            return llCMCRows;
        }
    }

    public class TestSheetRow:IEnumerable<string>
    {
        public string SignalName_Mapping { get; set; }
        public string PLCAddress { get; set; }
        public string Function { get; set; }
        public string State { get; set; }
        public string FunctionText { get; set; }
        public string StateText { get; set; }
        public string Color { get; set; }
        public string Level1view { get; set; }
        public string Level2view { get; set; }
        public string LeftClick { get; set; }
        public string PartName { get; set; }

        public IEnumerator<string> GetEnumerator()
        {
            yield return SignalName_Mapping;
            yield return PLCAddress;
            yield return Function;
            yield return State;
            yield return FunctionText;
            yield return StateText;
            yield return Color;
            yield return Level1view;
            yield return Level2view;
            yield return LeftClick;
            yield return PartName;

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    public static class TestColumns
    {
        public const string SignalName_Mapping = "Signal Name / Mapping Name";
        public const string PLCAddress = "PLC Address";
        public const string Function = "Function";
        public const string State = "State";
        public const string FunctionText = "Functional Text";
        public const string StateText = "State Text";
        public const string Color = "Color";
        public const string Level1view = "Level 1 view";
        public const string Level2view = "Level 2 view";
        public const string LeftClick = "Left Click functionality";
        public const string PartName = "Part Name";
        public const string TestData = "Test Date";
        public const string TestInitials = "Test Initials";
        public const string TestResult = "Test Result";
        public const string TestResultAlarm = "Test Result, Alarm";
        public const string TestComment = "Test Comment";
        
        public static int Count()
        {
            int i = 0;
            foreach(var s in GetEnumerable())
            {
                i++;
            }
            return i;
        }

        public static IEnumerable<string> GetEnumerable()
        {
            yield return SignalName_Mapping;
            yield return PLCAddress;
            yield return Function;
            yield return State;
            yield return FunctionText;
            yield return StateText;
            yield return Color;
            yield return Level1view;
            yield return Level2view;
            yield return LeftClick;
            yield return PartName;
            yield return TestData;
            yield return TestInitials;
            yield return TestResult;
            yield return TestResultAlarm;
            yield return TestComment;
        }
    }
}
