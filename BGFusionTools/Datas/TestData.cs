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
        public override string ToString()
        {
            string sOutPutSingleDatas = null;
            List<string> sOutPutSingleData = new List<string>();
            string sAtiveSingle = null; //使能信号
            string sLinesSingle = null;//线信号
            string sElementSingle = null;//设备信号
            EnumerableRowCollection<DataRow> MainRows = LinqToTable();
            try
            {
                var PlcGroups = from p in MainRows
                                group p by new { system = p.Field<string>(baseParameter.TaglistColName[1, 0]), plc = p.Field<string>(baseParameter.TaglistColName[1, 1]) } into pp
                                select pp;
                switch (baseParameter.ViewNum)
                {
                    case 0:
                        break;
                    case 1: //Level1数据
                        foreach (var plcGroup in PlcGroups)
                        {
                            string sSystem = plcGroup.Key.system;
                            string sPlcLink = plcGroup.Key.plc;
                            sAtiveSingle = string.Format(baseParameter.Stemp0, sSystem, sPlcLink);
                            sOutPutSingleData.Add(sAtiveSingle);
                        }
                        var ELementLineGroups = from p in MainRows
                                                group p by new { system = p.Field<string>(baseParameter.TaglistColName[1, 0]), plc = p.Field<string>(baseParameter.TaglistColName[1, 1]), line = p.Field<string>(baseParameter.TaglistColName[1, 3]), view = p.Field<string>(baseParameter.TaglistColName[1, 16]) , draw = p.Field<string>(baseParameter.TaglistColName[1, 17])} into pp
                                                select pp;
                        foreach (var ELementLineGroup in ELementLineGroups)
                        {
                            if (ELementLineGroup.Key.draw == "All")
                            {
                                string sSystem = ELementLineGroup.Key.system;
                                string sPlcLink = ELementLineGroup.Key.plc;
                                string sEquipmentLine = ELementLineGroup.Key.line;
                                string sAreaLevel2view = ELementLineGroup.Key.view;
                                //sAtiveSingle = string.Format(baseParameter.Stemp0, sSystem, sPlcLink);
                                //sOutPutSingleData.Add(sAtiveSingle);
                                sLinesSingle = string.Format(baseParameter.Stemp1, sSystem, sPlcLink, sEquipmentLine, sPlcLink, sAreaLevel2view);
                                sOutPutSingleData.Add(sLinesSingle);
                                foreach (DataRow selectConRow in ELementLineGroup)
                                {
                                    int iCounts;
                                    string sEquipmentElement = selectConRow[4].ToString();
                                    string sSingleMapping1 = selectConRow[6].ToString();
                                    var SingleCounts = baseParameter.SingleMappingTable.AsEnumerable().Count(p => p.Field<string>(baseParameter.BasefileColName[1, 0]) == sSingleMapping1);
                                    //float  dCounts = (float)SingleCounts /32;
                                    iCounts = (int)Math.Ceiling((float)SingleCounts / 32);
                                    for (int i = 1; i <= iCounts; i++)
                                    {
                                        sElementSingle = string.Format(baseParameter.Stemp2, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, i);
                                        sOutPutSingleData.Add(sElementSingle);
                                    }
                                }
                            }
                        }
                        break;
                    case 2://Level2数据
                        foreach (var plcGroup in PlcGroups)
                        {
                            string sSystem = plcGroup.Key.system;
                            string sPlcLink = plcGroup.Key.plc;
                            sAtiveSingle = string.Format(baseParameter.Stemp0, sSystem, sPlcLink);
                            sOutPutSingleData.Add(sAtiveSingle);
                            foreach (DataRow selectConRow in plcGroup)
                            {
                                int iCounts;
                                //string sPoweBox = selectConRow[2].ToString();
                                string sEquipmentLine = selectConRow[3].ToString();
                                string sEquipmentElement = selectConRow[4].ToString();
                                string sSingleMapping1 = selectConRow[6].ToString();
                                string sAreaLevel2view = selectConRow[16].ToString();
                                var SingleCounts = baseParameter.SingleMappingTable.AsEnumerable().Count(p => p.Field<string>(baseParameter.BasefileColName[1, 0]) == sSingleMapping1);
                                //float dCounts = SingleCounts / 32;
                                iCounts = (int)Math.Ceiling((float)SingleCounts / 32);
                                for (int i = 1; i <= iCounts; i++)
                                {
                                    sElementSingle = string.Format(baseParameter.Stemp2, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, i);
                                    sOutPutSingleData.Add(sElementSingle);
                                }
                            }
                        }
                        break;
                }

                foreach (string s in sOutPutSingleData)
                {
                    if (sOutPutSingleDatas == null)
                    {
                        sOutPutSingleDatas = s;
                    }
                    else
                    {
                        sOutPutSingleDatas = sOutPutSingleDatas + "," + s;
                    }
                }
                if (sOutPutSingleDatas != null)
                {
                    sOutPutSingleDatas = string.Format("[{0}]", sOutPutSingleDatas);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据处理错误：" + ex.Message);
            }
            return sOutPutSingleDatas;
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
            throw new NotImplementedException();
        }

        public override Dictionary<string, string> ToDictionary()
        {
            throw new NotImplementedException();
        }
    }

}
