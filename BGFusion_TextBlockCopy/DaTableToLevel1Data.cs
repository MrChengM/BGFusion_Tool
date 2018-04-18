using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;

namespace BGFusion_TextBlockCopy
{
   public  class DaTableToLevel1Data
    {
        //public EnumerableRowCollection<DataRow> MainRows { get; set; }
        private EnumerableRowCollection<DataRow> MainRows;
        //public string[,] MainColName{ get; set; }
        private string[,] MainColName;
        //public int ViewNum { get; set; }
        //private int ViewNum;
        //public string sTemp0 { get; set; }
        //private string sTemp0;
        //public string sTemp1 { get; set; }
        private string sTemp1;
        //public string sTemp2 { get; set; }
        private string sTemp2;
        //public DataTable SecondTable { get; set; }
        private DataTable SecondTable;
        //public string[,] SecondColName { get; set; }
        private string[,] SecondColName;
        //public DataTable TemplatTable { get; set; }   
        private DataTable TemplatTable;
        public DaTableToLevel1Data(EnumerableRowCollection<DataRow> mainRows, string[,] mainColName,  DataTable secondTable, string[,] secondColName, DataTable templateTable)
        {
            this.MainRows = mainRows;
            this.MainColName = mainColName;
            //this.ViewNum = ViewNum;
            //this.sTemp0 = temp0;
            //this.sTemp1 = temp1;
            //this.sTemp2 = temp2;
            this.SecondTable = secondTable;
            this.SecondColName = secondColName;
            this.TemplatTable = templateTable;
        }
        public string sOutData()
        {
            string sOutPutSingleDatas = null;
            List<string> sOutPutSingleData = new List<string>();
            //string sAtiveSingle = null; //使能信号


            try
            {
                var ELementLineGroups = from p in MainRows
                                        group p by new { system = p.Field<string>(MainColName[1, 0]), plc = p.Field<string>(MainColName[1, 1]), line = p.Field<string>(MainColName[1, 3]), view = p.Field<string>(MainColName[1, 16]) } into pp
                                        select pp;
                foreach (var ELementLineGroup in ELementLineGroups)//遍历相同PLC.Line.view的数据集合
                {
                string sLinesSingle = null;//线信号name
                string sLinesSingleValue = null;//Line single 值
                string sLinesSingleGroup = null;
                string sElementSingle = null;//设备信号name
                string sElementSingleBit = null;//设备信号bit

                string sDescrible = null;//Line single 描述
                string[] sLinesSingleBitGroup = new string[16];//Line single Bit 赋值；
                string[] sLinesSingleBit = new string[16]; //line single bit name
                string[] sLinesSingleBitValue = new string[16];//line single bit value;
                string sSystem = ELementLineGroup.Key.system;
                    string sPlcLink = ELementLineGroup.Key.plc;
                    string sEquipmentLine = ELementLineGroup.Key.line;
                    string sAreaLevel2view = ELementLineGroup.Key.view;
                    //sAtiveSingle = string.Format(sTemp0, sSystem, sPlcLink);
                    //sOutPutSingleData.Add(sAtiveSingle);
                    sLinesSingle = string.Format("{0}_{1}_{2}_{3}_{4}_Line_AC", sSystem , sPlcLink ,sEquipmentLine , sPlcLink ,sAreaLevel2view );//line single name
                    for (int i = 0; i <= 15; i++)
                    {
                        sLinesSingleBit[i] = string.Format("{0}_{1}_{2}_{3}_{4}_Sigl_Bit_{5}", sSystem, sPlcLink, sEquipmentLine, sPlcLink, sAreaLevel2view, i.ToString().PadLeft(2, '0'));
                    }
                    sDescrible = string.Format("//LineCode for {0}", sLinesSingle)+"\r\n";

                    sOutPutSingleData.Add(sDescrible);
                    foreach (DataRow selectConRow in ELementLineGroup) //遍历相同PLC.Line.view的数据集合每一行
                    {
                        int iCounts;
                        string sEquipmentElement = selectConRow[4].ToString();
                        string sSingleMapping1 = selectConRow[6].ToString();
                        var SingleCounts = SecondTable.AsEnumerable().Count(p => p.Field<string>(SecondColName[1, 0]) == sSingleMapping1);
                        //float  dCounts = (float)SingleCounts /32;
                        iCounts = (int)Math.Ceiling((float)SingleCounts / 32); //判断single个数
                        for (int i = 1; i <= iCounts; i++) //single个数
                        {

                            sElementSingle = string.Format("{0}_{1}_{2}_{3}_SIGNAL_{4}", sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, i);
                            string sColumName = string.Format("{0}_Single{1}", sSingleMapping1, i);
                            //DataColumn sColumDatas = TemplatTable.Columns[sColumName];


                            for (int j = 0; j < TemplatTable.Rows.Count; j++) //遍历模板行数
                            {
                                string sBit = TemplatTable.Rows[j][sColumName].ToString();
                                if (sBit != "")
                                {
                                    sElementSingleBit = sElementSingle + sBit;
                                    if (sLinesSingleBitValue[j] ==null)
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
                    for (int i = 0; i <= 15; i++)
                    {
                        if(sLinesSingleBitValue[i] == null)
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
                    sLinesSingleGroup= sLinesSingle + " = "+ sLinesSingleValue +"\r\n";
                    sOutPutSingleData.Add(sLinesSingleGroup);
                }
                sOutPutSingleDatas = ListToString.OutPutString(sOutPutSingleData);
            }

            catch (Exception ex)
            {
                MessageBox.Show("数据处理错误：" + ex.Message);
            }
            return sOutPutSingleDatas;
        }
    }
}
