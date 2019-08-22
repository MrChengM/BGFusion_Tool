using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using BGFusionTools.Functions;

namespace BGFusionTools.Datas
{
   public  class Level1Data:BaseData
    {
        private DataTable TemplatTable;
        public Level1Data(BaseParameter  ConverParameter,  DataTable templateTable )
        {
            baseParameter = ConverParameter;
            this.TemplatTable = templateTable;
        }
        public override List<List<string>> CreateList(CreateDataRow<string,List<ConveyorRow>> dataMath)
        {
            EnumerableRowCollection<DataRow> MainRows = LinqToTable();
            List<List<string>> lOutPut = new List<List<string>>();
            var ELementLineGroups = from p in MainRows
                                    group p by
                                    new
                                    {
                                        system = p.Field<string>(baseParameter.TaglistColName.sSystem),
                                        plc = p.Field<string> (baseParameter.TaglistColName.sPLC),
                                        line = p.Field<string>(baseParameter.TaglistColName.sEquipmentLine),
                                        view = p.Field<string>(baseParameter.TaglistColName.sLevel2View),
                                        draw = p.Field<string>(baseParameter.TaglistColName.sDrawOnViews)
                                    }
                                        into pp
                                    select pp;
            foreach(var ELementLineGroup in ELementLineGroups)
            {
                List<string> lineSiganlData = new List<string>();
                string sSystem = ELementLineGroup.Key.system;
                string sPlcLink = ELementLineGroup.Key.plc;
                string sEquipmentLine = ELementLineGroup.Key.line;
                string sAreaLevel2view = ELementLineGroup.Key.view;
                string sDraw = ELementLineGroup.Key.draw;
                if (sDraw.ToLower() == "all")
                {
                    string sLinesSingle = string.Format("{0}_{1}_{2}_{3}_{4}_Line_AC", sSystem, sPlcLink, sEquipmentLine, sPlcLink, sAreaLevel2view);
                    string sDescrible = string.Format("//LineCode for {0}", sLinesSingle) + "\r\n";
                    lineSiganlData.Add(sDescrible);
                    List<ConveyorRow> conveyorRows = new List<ConveyorRow>();
                    foreach (DataRow selectConRow in ELementLineGroup)
                    {
                        var conveyorRow = new ConveyorRow(baseParameter.TaglistColName, selectConRow);
                        conveyorRows.Add(conveyorRow);
                    }
                    lineSiganlData.AddRange(dataMath(conveyorRows));
                    lOutPut.Add(lineSiganlData);
                }
              
            }
            return lOutPut;
        }

        public List<string> CreateLineSignal(List<ConveyorRow> conveyorRows)
        {
            List<string> sOutPutSingleData = new List<string>();
            string sLinesSingle = null;//线信号name
            string sLinesSingleValue = null;//Line single 值
            string sLinesSingleGroup = null;
            string sElementSingle = null;//设备信号name
            string sElementSingleBit = null;//设备信号bit

            string[] sLinesSingleBitGroup = new string[32];//Line single Bit 赋值；
            string[] sLinesSingleBit = new string[32]; //line single bit name
            string[] sLinesSingleBitValue = new string[32];//line single bit value;

            foreach (ConveyorRow conveyorRow in conveyorRows) //遍历相同PLC.Line.view的数据集合每一行
            {

                string sEquipmentElement = conveyorRow.sElementName;
                List<string> sSignalMappings = conveyorRow.sSignalMapping;
                List<string> signalNames = new List<string>();
                foreach (string sSignalMapping in sSignalMappings)
                {
                    var Counts = baseParameter.SingleMappingTable.AsEnumerable().Count(p => p.Field<string>(baseParameter.SignalMappingColName.sType) == sSignalMapping);
                    var signalName = sSignalName(Counts, baseParameter.Stemp5, conveyorRow, signalNames.Count + 1);
                    for (int i = 1; i <= signalName.Count; i++)
                    {
                        string sColumName = string.Format("{0}_SIGNAL{1}", sSignalMapping, i);
                        for (int j = 0; j < TemplatTable.Rows.Count; j++) //遍历模板行数
                        {
                            string sBit = TemplatTable.Rows[j][sColumName].ToString();
                            if (sBit != "")
                            {
                                sElementSingleBit = sElementSingle + sBit;
                                if (sLinesSingleBitValue[j] == null)
                                {
                                    sLinesSingleBitValue[j] = sElementSingleBit;
                                }
                                else
                                {
                                    sLinesSingleBitValue[j] = sLinesSingleBitValue[j] + " or " + sElementSingleBit;
                                }
                            }
                        }
                    }
                    signalNames.AddRange(signalName);
                }
            }
            for (int i = 0; i <= 31; i++)
            {
                if (sLinesSingleBitValue[i] == null)
                {
                    if (sLinesSingleValue == null)
                    {
                        sLinesSingleValue = "0";
                    }
                    else
                    {
                        sLinesSingleValue = sLinesSingleValue + "," + "0";
                    }
                }
                else
                {
                    sLinesSingleBitGroup[i] = sLinesSingleBit[i] + " = " + sLinesSingleBitValue[i];
                    if (sLinesSingleValue == null)
                    {
                        sLinesSingleValue = sLinesSingleBit[i];
                    }
                    else
                    {
                        sLinesSingleValue = sLinesSingleValue + "," + sLinesSingleBit[i];
                    }
                    sOutPutSingleData.Add(sLinesSingleBitGroup[i]);
                }
            }
            sLinesSingleValue = string.Format("[{0}]", sLinesSingleValue);
            sLinesSingleGroup = sLinesSingle + " = " + sLinesSingleValue + "\r\n";
            sOutPutSingleData.Add(sLinesSingleGroup);
            return sOutPutSingleData;
        }
    }
}
