using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;


namespace BGFusion_TextBlockCopy
{
    public class DaTableToTeData //数据表转换为测试singlename；
    {
        //public EnumerableRowCollection<DataRow> MainRows { get; set; }
        private EnumerableRowCollection<DataRow> MainRows;
        //public string[,] MainColName{ get; set; }
        private string[,] MainColName;
        //public int ViewNum { get; set; }
        private int ViewNum;
        //public string sTemp0 { get; set; }
        private string sTemp0;
        //public string sTemp1 { get; set; }
        private string sTemp1;
        //public string sTemp2 { get; set; }
        private string sTemp2;
        //public DataTable SecondTable { get; set; }
        private DataTable SecondTable;
        //public string[,] SecondColName { get; set; }
        private string[,] SecondColName;
        public DaTableToTeData(EnumerableRowCollection<DataRow> mainRows, string[,] mainColName, int ViewNum, string temp0, string temp1, string temp2, DataTable secondTable, string[,] secondColName)
        {
            this.MainRows = mainRows;
            this.MainColName = mainColName;
            this.ViewNum = ViewNum;
            this.sTemp0 = temp0;
            this.sTemp1 = temp1;
            this.sTemp2 = temp2;
            this.SecondTable = secondTable;
            this.SecondColName = secondColName;
        }
        public string sOutData()
        {
            string sOutPutSingleDatas = null;
            List<string> sOutPutSingleData = new List<string>();
            string sAtiveSingle = null; //使能信号
            string sLinesSingle = null;//线信号
            string sElementSingle = null;//设备信号
            try
            {
                var PlcGroups = from p in MainRows
                                group p by new { system = p.Field<string>(MainColName[1, 0]), plc = p.Field<string>(MainColName[1, 1]) } into pp
                                select pp;
                switch (ViewNum)
                {
                    case 0:
                        break;
                    case 1: //Level1数据
                        foreach (var plcGroup in PlcGroups)
                        {
                            string sSystem = plcGroup.Key.system;
                            string sPlcLink = plcGroup.Key.plc;
                            sAtiveSingle = string.Format(sTemp0, sSystem, sPlcLink);
                            sOutPutSingleData.Add(sAtiveSingle);
                        }
                        var ELementLineGroups = from p in MainRows
                                                group p by new { system = p.Field<string>(MainColName[1, 0]), plc = p.Field<string>(MainColName[1, 1]), line = p.Field<string>(MainColName[1, 3]), view = p.Field<string>(MainColName[1, 16]) } into pp
                                                select pp;
                        foreach (var ELementLineGroup in ELementLineGroups)
                        {
                            string sSystem = ELementLineGroup.Key.system;
                            string sPlcLink = ELementLineGroup.Key.plc;
                            string sEquipmentLine = ELementLineGroup.Key.line;
                            string sAreaLevel2view = ELementLineGroup.Key.view;
                            //sAtiveSingle = string.Format(sTemp0, sSystem, sPlcLink);
                            //sOutPutSingleData.Add(sAtiveSingle);
                            sLinesSingle = string.Format(sTemp1, sSystem, sPlcLink, sEquipmentLine, sPlcLink, sAreaLevel2view);
                            sOutPutSingleData.Add(sLinesSingle);
                            foreach (DataRow selectConRow in ELementLineGroup)
                            {
                                int iCounts;
                                string sEquipmentElement = selectConRow[4].ToString();
                                string sSingleMapping1 = selectConRow[6].ToString();
                                var SingleCounts = SecondTable.AsEnumerable().Count(p => p.Field<string>(SecondColName[1, 0]) == sSingleMapping1);
                                //float  dCounts = (float)SingleCounts /32;
                                iCounts = (int)Math.Ceiling((float)SingleCounts / 32);
                                for (int i = 1; i <= iCounts; i++)
                                {
                                    sElementSingle = string.Format(sTemp2, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, i);
                                    sOutPutSingleData.Add(sElementSingle);
                                }
                            }
                        }
                        break;
                    case 2://Level2数据
                        foreach (var plcGroup in PlcGroups)
                        {
                            string sSystem = plcGroup.Key.system;
                            string sPlcLink = plcGroup.Key.plc;
                            sAtiveSingle = string.Format(sTemp0, sSystem, sPlcLink);
                            sOutPutSingleData.Add(sAtiveSingle);
                            foreach (DataRow selectConRow in plcGroup)
                            {
                                int iCounts;
                                //string sPoweBox = selectConRow[2].ToString();
                                string sEquipmentLine = selectConRow[3].ToString();
                                string sEquipmentElement = selectConRow[4].ToString();
                                string sSingleMapping1 = selectConRow[6].ToString();
                                string sAreaLevel2view = selectConRow[16].ToString();
                                var SingleCounts = SecondTable.AsEnumerable().Count(p => p.Field<string>(SecondColName[1, 0]) == sSingleMapping1);
                                //float dCounts = SingleCounts / 32;
                                iCounts = (int)Math.Ceiling((float)SingleCounts / 32);
                                for (int i = 1; i <= iCounts; i++)
                                {
                                    sElementSingle = string.Format(sTemp2, sSystem, sPlcLink, sEquipmentLine, sEquipmentElement, i);
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
    }

}
