using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;

namespace BGFusion_TextBlockCopy
{
    public class ListData
    {

        public string sColName { get; set; }

        public string sColGroup { get; set; }

    }
    class DaTableToTeList
    { 
        private EnumerableRowCollection<DataRow> MainRows;
        private string[,] MainColName;
        private int ViewNum;
        private DataTable SecondTable;
        private string[,] SecondColName;
        public DaTableToTeList(EnumerableRowCollection<DataRow> mainRows, string[,] mainColName, int ViewNum, DataTable secondTable, string[,] secondColName)
        {
            this.MainRows = mainRows;
            this.MainColName = mainColName;
            this.ViewNum = ViewNum;
            this.SecondTable = secondTable;
            this.SecondColName = secondColName;
        }
        public void OutLiData(out List<ListData> ListDatas)
        {

            ListDatas = new List<ListData>();
            try
            {
                switch (ViewNum)
                {
                    case 0:
                        break;
                    case 1:
                        var ELementLineGroups = from p in MainRows
                                                group p by new { system = p.Field<string>(MainColName[1, 0]), plc = p.Field<string>(MainColName[1, 1]), line = p.Field<string>(MainColName[1, 3]), view = p.Field<string>(MainColName[1, 16]) } into pp
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
                                ListData listDataL1 = new ListData();
                                listDataL1.sColName = sElementName;
                                listDataL1.sColGroup = sLinesGroups;
                                ListDatas.Add(listDataL1);
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
                            ListData listDataL2 = new ListData();
                            listDataL2.sColName = sElementName;
                            listDataL2.sColGroup = sPlcLink;
                            ListDatas.Add(listDataL2);
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
}
