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
 ;
            this.TemplatTable = templateTable;
        }
        public override string ToString()
        {
            string sOutPutSingleDatas = null;
            List<string> sOutPutSingleData = new List<string>();
            //string sAtiveSingle = null; //使能信号

            EnumerableRowCollection<DataRow>  MainRows = LinqToTable();
            try
            {
                var ELementLineGroups = from p in MainRows
                                        group p by 
                                        new
                                        { system = p.Field<string>(baseParameter.TaglistColName[1, 0]),
                                            plc = p.Field<string>(baseParameter.TaglistColName[1, 1]),
                                            line = p.Field<string>(baseParameter.TaglistColName[1, 3]),
                                            view = p.Field<string>(baseParameter.TaglistColName[1, 16]),
                                            draw = p.Field<string>(baseParameter.TaglistColName[1, 17])
                                        } 
                                        into pp
                                        select pp;
                foreach (var ELementLineGroup in ELementLineGroups)//遍历相同PLC.Line.view的数据集合
                {
                    string sLinesSingle = null;//线信号name
                    string sLinesSingleValue = null;//Line single 值
                    string sLinesSingleGroup = null;
                    string sElementSingle = null;//设备信号name
                    string sElementSingleBit = null;//设备信号bit

                    string sDescrible = null;//Line single 描述
                    string[] sLinesSingleBitGroup = new string[32];//Line single Bit 赋值；
                    string[] sLinesSingleBit = new string[32]; //line single bit name
                    string[] sLinesSingleBitValue = new string[32];//line single bit value;
                    string sSystem = ELementLineGroup.Key.system;
                    string sPlcLink = ELementLineGroup.Key.plc;
                    string sEquipmentLine = ELementLineGroup.Key.line;
                    string sAreaLevel2view = ELementLineGroup.Key.view;
                    string sDraw = ELementLineGroup.Key.draw;
                    //sAtiveSingle = string.Format(sTemp0, sSystem, sPlcLink);
                    //sOutPutSingleData.Add(sAtiveSingle);
                    if (sDraw == "All")
                    {
                        sLinesSingle = string.Format("{0}_{1}_{2}_{3}_{4}_Line_AC", sSystem, sPlcLink, sEquipmentLine, sPlcLink, sAreaLevel2view);//line single name
                        for (int i = 0; i <= 31; i++)
                        {
                            sLinesSingleBit[i] = string.Format("{0}_{1}_{2}_{3}_{4}_Sigl_Bit_{5}", sSystem, sPlcLink, sEquipmentLine, sPlcLink, sAreaLevel2view, i.ToString().PadLeft(2, '0'));
                        }
                        sDescrible = string.Format("//LineCode for {0}", sLinesSingle) + "\r\n";

                        sOutPutSingleData.Add(sDescrible);
                        foreach (DataRow selectConRow in ELementLineGroup) //遍历相同PLC.Line.view的数据集合每一行
                        {
                            int iCounts;
                            string sEquipmentElement = selectConRow[4].ToString();
                            string sSingleMapping1 = selectConRow[6].ToString();
                            var SingleCounts = baseParameter.SingleMappingTable.AsEnumerable().Count(p => p.Field<string>(baseParameter.BasefileColName[1, 0]) == sSingleMapping1);
                            //float  dCounts = (float)SingleCounts /32;
                            iCounts = (int)Math.Ceiling((float)SingleCounts / 32); //判断single个数
                            for (int i = 1; i <= iCounts; i++) //single个数
                            {
                                sElementSingle = string.Format("{0}_{1}_{2}_{3}_SIGNAL_{4}", sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, i);
                                string sColumName = string.Format("{0}_SIGNAL{1}", sSingleMapping1, i);
                                //DataColumn sColumDatas = TemplatTable.Columns[sColumName];
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
                    }

                }
                sOutPutSingleDatas = DataConvert.ToString(sOutPutSingleData);
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
