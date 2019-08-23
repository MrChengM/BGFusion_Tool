using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;


namespace BGFusionTools.Datas
{
    public class TestData:BaseData //数据表转换为测试singlename；
    {
        public TestData(BaseParameter ConverParameter)
        {
            this.baseParameter = ConverParameter;

        }
        public override List<List<string>> CreateList(CreateDataRow<string, List<ConveyorRow>> dataMath)
        {
            List<List<string>> sOutPutSingleData = new List<List<string>>();

            string sAtiveSingle = null; //使能信号
            EnumerableRowCollection<DataRow> MainRows = LinqToTable();
            var PlcGroups = from p in MainRows
                            group p by 
                            new { system = p.Field<string>(baseParameter.TaglistColName.sSystem),
                                plc = p.Field<string>(baseParameter.TaglistColName.sSystem) }
                            into pp
                            select pp;
            foreach(var plcGroup in PlcGroups)
            {
                List<ConveyorRow> conveyorRows = new List<ConveyorRow>();
                List<string> SignalGroup = new List<string>();
                string sSystem = plcGroup.Key.system;
                string sPlcLink = plcGroup.Key.plc;
                sAtiveSingle = string.Format(baseParameter.Stemp0, sSystem, sPlcLink);
                SignalGroup.Add(sAtiveSingle);
                foreach (DataRow selectRow in plcGroup)
                    conveyorRows.Add(new ConveyorRow(baseParameter.TaglistColName, selectRow));
                SignalGroup.AddRange(dataMath(conveyorRows));
            }
            return base.CreateList(dataMath);
        }
        public List<string> CreateL1Signal(List<ConveyorRow> conveyorRows)
        {
            List<string> outSignals = new List<string>();
            var ELementLineGroups = from p in conveyorRows
                                    group p by 
                                    new { system = p.sSystem, plc = p.sPLC,line = p.sEquipmentLine,
                                        view = p.sLevel2View, draw = p.sDrawOnViews }
                                    into pp
                                    select pp;
            foreach(var ELementLineGroup in ELementLineGroups)
            {
                string sSystem = ELementLineGroup.Key.system;
                string sPlcLink = ELementLineGroup.Key.plc;
                string sEquipmentLine = ELementLineGroup.Key.line;
                string sAreaLevel2view = ELementLineGroup.Key.view;
                string sLinesSingle = string.Format(baseParameter.Stemp1, sSystem, sPlcLink, sEquipmentLine, sPlcLink, sAreaLevel2view);
                outSignals.Add(sLinesSingle);
                foreach (ConveyorRow converyor in ELementLineGroup)
                {
                    List<string> signals = new List<string>();
                    foreach (string signalMapping in converyor.sSignalMapping)
                    {
                        var Counts = baseParameter.SingleMappingTable.AsEnumerable().Count
                            (p => p.Field<string>(baseParameter.SignalMappingColName.sType) == signalMapping);
                        var signal = sSignalName(Counts, baseParameter.Stemp2, converyor, signals.Count + 1);
                        signals.AddRange(signal);
                    }
                    outSignals.AddRange(signals);
                }
            }
           
            return outSignals;
        }
        public List<string> CreateL2Signal(List<ConveyorRow> conveyorRows)
        {
            List<string> outSignals = new List<string>();

            foreach(ConveyorRow converyor in conveyorRows)
            {
                List<string> signals = new List<string>();
                foreach (string signalMapping in converyor.sSignalMapping)
                {
                    var Counts = baseParameter.SingleMappingTable.AsEnumerable().Count
                        (p => p.Field<string>(baseParameter.SignalMappingColName.sType) == signalMapping);
                    var signal = sSignalName(Counts, baseParameter.Stemp2, converyor, signals.Count+1);
                    signals.AddRange(signal);
                }
                outSignals.AddRange(signals);
            }
            return outSignals;
        } 
    }

}
