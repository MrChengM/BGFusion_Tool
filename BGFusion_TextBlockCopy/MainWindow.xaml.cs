using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.IO;
using System.Configuration;

namespace BGFusion_TextBlockCopy
{
    /// <summary>
    /// Interaction logic for MainWndow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public static string sInPutText;
        public static string[] sInPutList;
        //public static string sOutPutText;
        public static string[] sOutPutlist;
        //Binding实例定义
        public static  DataString[] dBinding = new DataString[100];
       
       //Excel 数据表处理
        public string sPath;
        ///public DataSet MyExcelData;
        public DataSet ConveyorExcelData;
        public DataSet BaseListExceLData;
        public DataSet TemlateExcelData;
        public DataTable Level1DataExcelData;
        public EnumerableRowCollection<DataRow> SelectConRows;
        public DataTable SelectConveyorExcelTable;
        
         //public EnumerableRowCollection<object> ConveyorRows;

        //DataTable列常量定义

        //Conveyor数据表定义
        public string[] sConveyorSheetName;
        public string[,] sConveyorColName;
        //BaseList数据表定义
        public string[] sBaseListSheetName ;
        public string[,] sBaseListColName ;

        //输出变量定义
        public string  sEquipmentElement="";
        public string  sPLCLink = "";
        public string sEquipmentLine = "";

        //TestData输出模板
        public string sTestDaActive;
        public string sTestDaTemplateL1;
        public string sTestDaTemplateL2;

        //Xml输出模板
        public string sXmlElement;
        public string sXmlTextBlock;

        //OPC输出模板
        public string sOPCDaSingleTemplate;
        public string sOPCDaCommandTemplate;
        public string sOPCDaHourTemplate;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                //ViewsBinding
                //WPF初始化
                for (int i = 0; i < 100; i++)
                {
                    dBinding[i] = new DataString();
                }
                //默认View为Level2，值为PVG_BHS_S1_LA_0002_0003，
                dBinding[0].MyString = "PVG_BHS_S1_LA_0002_0003";
                dBinding[4].Mybool = false;
                dBinding[5].Mybool = true;

                //默认生成的XML为Textblack
                dBinding[10].Mybool = true;
                dBinding[11].Mybool = false;

                //默认生成OPC数据包含：Single/Commad/Hour
                dBinding[52].Mybool = true;
                dBinding[53].Mybool = true;
                dBinding[54].Mybool = true;
                //默认生成AlarmCofig数据包含：OPCInfo
                dBinding[60].Mybool = true;

                //Genenarl
                ViewNameteBox.DataContext = dBinding[0];
                InPutFilePathteBox.DataContext = dBinding[1];
                BaseListFilePathTeBox.DataContext = dBinding[2];
                //L0raButton.DataContext = dBinding[3];
                L1raButton.DataContext = dBinding[4];
                L2raButton.DataContext = dBinding[5];
                //XML
                TeBlackraButton.DataContext = dBinding[10];
                ElementradioButton.DataContext = dBinding[11];
                XMLOutPutFilePathteBox.DataContext = dBinding[12];
                XMLOutPutDatasteBox.DataContext = dBinding[13];
                //TestData
                TestdataOutPutFilePathteBox.DataContext = dBinding[20];
                TestDataOutPutDatasteBox.DataContext = dBinding[21];
                //TestList
                TestListTemplateteBox.DataContext = dBinding[30];
                TestListOutPutFilePathteBox.DataContext = dBinding[31];
                TestListOutPutDatasteBox.DataContext = dBinding[32];

                //Level1Data
                //Level1DataTemplateteBox.DataContext = dBinding[40];
                Level1DataOutPutFilePathteBox.DataContext = dBinding[41];
                Level1DataOutPutDatasteBox.DataContext = dBinding[42];

                //OPCData
                OPCDataOutPutFilePathteBox.DataContext = dBinding[50];
                OPCDataOutPutDatasteBox.DataContext = dBinding[51];
                SinglecheckBox.DataContext = dBinding[52];
                CommandcheckBox.DataContext = dBinding[53];
                HourcheckBox.DataContext = dBinding[54];

                //AlarmLinkConf
                OPCInforaButton.DataContext = dBinding[60];
                AlarmListradioButton.DataContext = dBinding[61];
                ConfOutPutFilePathteBox.DataContext = dBinding[62];
                ConfOutPutDatasteBox.DataContext = dBinding[63];
                //配置参数初始化
                DataSet dConfigTable = new DataSet();
                string sConFilePath =ConfigurationSettings.AppSettings["Config.FilePath"];
                //ConveyorSheetName初始化
                dConfigTable =ExcelFunction.ExcelRead(sConFilePath);
                //Conveyor Sheet Name & Conveyor Column Name初始化
                sTableToStringArrary(dConfigTable.Tables["Conveyor$"], out sConveyorSheetName, out sConveyorColName);
                //BaseList Sheet Name & Conveyor Column Name初始化
                sTableToStringArrary(dConfigTable.Tables["BaseList$"], out sBaseListSheetName, out sBaseListColName);

                //TestDataTemplate
                sTestDaActive = (string)dConfigTable.Tables["TestDataTemplate$"].Rows[0][0];
                sTestDaTemplateL1 = (string)dConfigTable.Tables["TestDataTemplate$"].Rows[1][0];
                sTestDaTemplateL2 = (string)dConfigTable.Tables["TestDataTemplate$"].Rows[2][0];

                //XmlTemplate
                sXmlTextBlock = (string)dConfigTable.Tables["XmlTemplate$"].Rows[0][0];
                sXmlElement = (string)dConfigTable.Tables["XmlTemplate$"].Rows[1][0];

                //L1AlarmDataTemplate
                Level1DataExcelData = dConfigTable.Tables["L1AlarmDataTemple$"];

                //OPCDataTemplate
                sOPCDaSingleTemplate = (string)dConfigTable.Tables["OPCDataTemplate$"].Rows[0][0];
                sOPCDaCommandTemplate = (string)dConfigTable.Tables["OPCDataTemplate$"].Rows[1][0];
                sOPCDaHourTemplate = (string)dConfigTable.Tables["OPCDataTemplate$"].Rows[2][0];

                //dConfigTable.Clear();


            }
            catch (Exception ex)
            {
                //MessageBox.Show("Config Error,Initialize fail： " + ex.Message);
                //Application.Current.Shutdown();
            }

        }
        /// <summary>
        /// DataTable提取例表名及数据二维数组
        /// </summary>
        /// <param name="dInputDataTable"></param>
        /// <returns></returns>
        private void sTableToStringArrary(DataTable dInputDataTable, out string[] sTableColName, out string[,] sTableData)
        {
            string[] sColumnsName;   
            string[,] sArrary;
            int iRowsCount;
            int iColumnsCount;
            iRowsCount = dInputDataTable.Rows.Count;
            iColumnsCount = dInputDataTable.Columns.Count;
            sColumnsName = new string[iColumnsCount];

            sArrary = new string[iColumnsCount, iRowsCount];
            for (int i = 0; i < iColumnsCount; i++)
            {
                sColumnsName[i] = dInputDataTable.Columns[i].ColumnName;
                for (int j = 0; j < iRowsCount; j++)
                {
                    if (dInputDataTable.Rows[j][i] is System.DBNull == false)
                    {
                        sArrary[i, j] = (string)dInputDataTable.Rows[j ][i];
                    }
                }
            }
            sTableColName = sColumnsName;
            sTableData = sArrary;
        }
        /// <summary>
        /// 数据导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private DataSet dDataLoad (ref string sFileName)
        {
            DataSet dExcelDataSet =new DataSet();
            //sEquipmentElement.Clear();
            //sEquipmentLine.Clear();
            OpenFileDialog dialog = new OpenFileDialog();//open the path
            dialog.FileName = sFileName;
            dialog.Filter = "Excel Worksheets | *.xls;*.xlsx";
            if (dialog.ShowDialog() == true)
            {
                sPath = dialog.FileName;
                sFileName=dialog.FileName;
            }
            else
            {
                return null;
            }
            if (sPath != null)
            {
                dExcelDataSet =ExcelFunction.ExcelRead(sPath);
                //DataTableToOutPutList();
            }
            return dExcelDataSet;
        }
        /// <summary>
        ///Table数据筛选
        /// </summary>
        private EnumerableRowCollection <DataRow> LinqToTable()
        {

            //ConveyorRow筛选
            
            try
            {
                string sSelectConveyorColName;
                string sSelectColVal;
                if (dBinding[4].Mybool == true)
                {
                    sSelectConveyorColName = sConveyorColName[1, 15];
                }
                else
                {
                    sSelectConveyorColName = sConveyorColName[1, 16];
                }
                sSelectColVal = dBinding[0].MyString;

                var ConveyorRows = from p in ConveyorExcelData.Tables[sConveyorSheetName[1]].AsEnumerable()
                                   where p.Field<string>(sSelectConveyorColName) == sSelectColVal
                                   select p;
                
                return ConveyorRows;

            }
            catch(Exception ex)
            {
                MessageBox.Show("Query the Conveyor Excel error: " + ex.Message);
                return null;
            }
            
          
        }
        /// <summary>
        /// 数据输出保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool  bDataOutput(ref string sfileName ,string sFilter)
        {

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = sfileName; 
            dialog.Filter = sFilter;

            // Show save file dialog box
            Nullable <bool> result = dialog.ShowDialog();
            // Process save file dialog box results
            if (result == true)
            {
                sfileName = dialog.FileName;
                return true;
            }
            else
            {
                return false;
            }
     
        }
        /// <summary>
        /// TagList表格输入（PVG_SCADA_CNV_...）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Inputbutton_Click(object sender, RoutedEventArgs e)
        {
            string sFilePath = dBinding[1].MyString;
            ConveyorExcelData = dDataLoad(ref sFilePath);
            dBinding[1].MyString = sFilePath;
            if (ConveyorExcelData != null)  SelectConRows = LinqToTable(); 
      
         }
        /// <summary>
        /// BaseList表格输入（PVG_CrisBeltBase_V016_9）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseListbutton_Click(object sender, RoutedEventArgs e)
        {
            string sFilePath = dBinding[2].MyString;
            BaseListExceLData = dDataLoad(ref sFilePath);
            dBinding[2].MyString = sFilePath;
        }
        /// <summary>
        /// WPF XML数据输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XMLOutputbutton_Click(object sender, RoutedEventArgs e)
        {
            string sFileStyle = "Text documents (.txt)|*.txt";
            string sFilePath = dBinding[12].MyString;
            int ViewNumber = 0;
            int XmlChange = 0;
            try
            {
                if (dBinding[4].Mybool == true)
                {
                    ViewNumber = 1;
                }
                else if (dBinding[5].Mybool == true)
                {
                    ViewNumber = 2;
                }
                if (dBinding[10].Mybool == true)
                {
                    XmlChange = 1;
                }
                else if (dBinding[11].Mybool == true)
                {
                    XmlChange = 2;
                }
                bool bOpenEnable = bDataOutput(ref sFilePath, sFileStyle);
                if (bOpenEnable == true)
                {
                    DaTableToTeXml XmlData = new DaTableToTeXml(SelectConRows, sConveyorColName, ViewNumber, XmlChange, sXmlTextBlock, sXmlElement, BaseListExceLData.Tables[sBaseListSheetName[1]], sBaseListColName);
                    dBinding[12].MyString = sFilePath;
                    dBinding[13].MyString = XmlData.sOutData();
                    System.IO.File.WriteAllText(@sFilePath, dBinding[13].MyString, Encoding.UTF8);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Build XML Data Error: " + ex.Message);
            }
            GC.Collect();

        }
        /// <summary>
        /// 测试数据输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestDataOutputbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sFileStyle = "SIG File(.signaltester) | *.signaltester";
                string sFilePath = dBinding[20].MyString;
                int ViewNumber = 0;

                if (dBinding[4].Mybool == true)
                {
                    ViewNumber = 1;
                }
                else if (dBinding[5].Mybool == true)
                {
                    ViewNumber = 2;
                }
                bool bOpenEnable = bDataOutput(ref sFilePath, sFileStyle);
                if (bOpenEnable == true)
                {
                    DaTableToTeData TestData = new DaTableToTeData(SelectConRows, sConveyorColName, ViewNumber, sTestDaActive, sTestDaTemplateL1, sTestDaTemplateL2, BaseListExceLData.Tables[sBaseListSheetName[1]], sBaseListColName);
                    dBinding[20].MyString = sFilePath;
                    dBinding[21].MyString = TestData.sOutData();
                    System.IO.File.WriteAllText(@sFilePath, dBinding[21].MyString, Encoding.UTF8);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Build Test data Error： " + ex.Message);
            }
            GC.Collect();

        }
        /// <summary>
        /// 测试记录表格输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestListOutputbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sFileStyle = "Excel(.xlsx) | *.xlsx|Excel(.xls) | *.xls";
                string sFilePath = dBinding[31].MyString;
                int ViewNumber = 0;

                if (dBinding[4].Mybool == true)
                {
                    ViewNumber = 1;
                }
                else if (dBinding[5].Mybool == true)
                {
                    ViewNumber = 2;
                }
             bool bOpenEnable = bDataOutput(ref sFilePath, sFileStyle);
                if (bOpenEnable == true)
                {
                    DaTableToTeList ListData = new DaTableToTeList(SelectConRows, sConveyorColName, ViewNumber, BaseListExceLData.Tables[sBaseListSheetName[1]], sBaseListColName);
                    dBinding[31].MyString = sFilePath;
                    string sOutPutLiData = null;
                    List<ListData> TeListDatas = new List<ListData>();
                    ListData.OutLiData(out TeListDatas);
                    foreach (ListData listdata in TeListDatas)
                    {
                        if (sOutPutLiData == null)
                        {
                            sOutPutLiData = listdata.sColName + " " + listdata.sColGroup;
                        }
                        else
                        {
                            sOutPutLiData = sOutPutLiData + "\r" + listdata.sColName + " " + listdata.sColGroup;
                        }
                    }
                    dBinding[32].MyString = sOutPutLiData;
                    ExcelFunction.ExcelWrite(dBinding[30].MyString,sFilePath, TeListDatas);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Build Test List Error： " + ex.Message);
            }
            GC.Collect();

        }
        /// <summary>
        /// 测试表格模板输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestListTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            string sFilePath = dBinding[30].MyString;
            TemlateExcelData = dDataLoad(ref sFilePath);
            dBinding[30].MyString = sFilePath;
        }

        private void Level1DataTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            //string sFilePath = dBinding[40].MyString;
            //Level1DataExcelData = dDataLoad(ref sFilePath);
            //dBinding[40].MyString = sFilePath;
        }

        private void Level1DataOutputbutton_Click(object sender, RoutedEventArgs e)
        {
            string sFileStyle = "Text documents (.txt)|*.txt";
            string sFilePath = dBinding[41].MyString;
            try
            {
                bool bOpenEnable = bDataOutput(ref sFilePath, sFileStyle);
                if (bOpenEnable == true)
                {
                    DaTableToLevel1Data Level1Data = new DaTableToLevel1Data(SelectConRows, sConveyorColName, BaseListExceLData.Tables[sBaseListSheetName[1]], sBaseListColName, Level1DataExcelData);
                    dBinding[41].MyString = sFilePath;
                    dBinding[42].MyString = Level1Data.sOutData();
                    System.IO.File.WriteAllText(@sFilePath, dBinding[42].MyString, Encoding.UTF8);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Build Level1 Data Error: " + ex.Message);
            }
            GC.Collect();

        }


        private void OPCDataOutputbutton_Click(object sender, RoutedEventArgs e)
        {
            //string sFileStyle = "Excel(.xlsx) | *.xlsx|Excel(.xls) | *.xls";
            string sFileStyle = "Excel(.csv) | *.csv";
            string sFilePath = dBinding[50].MyString;
            bool bOpenEnable = bDataOutput(ref sFilePath, sFileStyle);
            try
            {
                if (bOpenEnable)
                {
                    DaTableToOPCData OPCData = new DaTableToOPCData(ConveyorExcelData.Tables[sConveyorSheetName[1]],
                        BaseListExceLData.Tables[sBaseListSheetName[1]], BaseListExceLData.Tables[sBaseListSheetName[2]],
                        sConveyorColName, sBaseListColName, sOPCDaSingleTemplate, sOPCDaCommandTemplate, sOPCDaHourTemplate,
                        dBinding[52].Mybool, dBinding[53].Mybool, dBinding[54].Mybool);
                    DataTable dt = OPCData.dOutData();
                    dBinding[50].MyString = sFilePath;
                    //ExcelFunction.ExcelWrite(sFilePath, dt);
                    CsvFunction.CsvWirte(sFilePath, dt);
                    dBinding[51].MyString = DataConvert.ToString(dt);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Build OPC Data Error: " + ex.Message);
            }
            GC.Collect();

        }

        private void ConfOutputbutton_Click(object sender, RoutedEventArgs e)
        {
            string sFileStyle = "Excel(.xml) | *.xml";
            string sFilePath = dBinding[62].MyString;
            bool bOpenEnable = bDataOutput(ref sFilePath, sFileStyle);
            List<string> lListColName = new List<string>();
            if (dBinding[60].Mybool )
            {
                lListColName.Add("Tag Name");
                lListColName.Add("Type");
                lListColName.Add("Channel");
                lListColName.Add("Device");
                lListColName.Add("DataType");
            }
            if (dBinding[61].Mybool)
            {
                lListColName.Add("SignalName");
                lListColName.Add("AckType");
                lListColName.Add("AlarmTag");
                lListColName.Add("PartName");
                lListColName.Add("Alarm Description");
                lListColName.Add("AlarmGroup");
                lListColName.Add("AlarmType");
                lListColName.Add("AlarmCategory");
                lListColName.Add("Delayed");
                lListColName.Add("ConditionName");
                lListColName.Add("Priority");
                lListColName.Add("GeneralComment");
                lListColName.Add("Level1View");
                lListColName.Add("Level2View");
                lListColName.Add("Resetable");
                lListColName.Add("ResetBit");
                lListColName.Add("ALNumber");
                lListColName.Add("CCTVRecording");
                lListColName.Add("ElementID");
                lListColName.Add("ResetSignal");
                lListColName.Add("ExtraTagList");
                lListColName.Add("CISData");
                lListColName.Add("Technical");
            }
            try
            {
                if (bOpenEnable)
                {
                    DaTableToConfig dConfigData = new DaTableToConfig(ConveyorExcelData.Tables[sConveyorSheetName[1]],
                        BaseListExceLData.Tables[sBaseListSheetName[1]], BaseListExceLData.Tables[sBaseListSheetName[2]],
                        sConveyorColName, sBaseListColName,dBinding[60].Mybool,dBinding[61].Mybool, sOPCDaSingleTemplate, 
                        sOPCDaCommandTemplate, lListColName);
                    List<List<string>> dt = dConfigData.dOutData();
                    dBinding[62].MyString = sFilePath;
                    //ExcelFunction.ExcelWrite(sFilePath, dt);
                    XmlFuction xmlFuction = new XmlFuction();
                    xmlFuction.XmlWrite(sFilePath, dt);
                    //dBinding[63].MyString = DataConvert.ToString(dt);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Build OPC Data Error: " + ex.Message);
            }
            GC.Collect();

        }
    }
}
