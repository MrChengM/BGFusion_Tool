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
        //public static string[] sInPutList;
        //public static string sOutPutText;
        //public static string[] sOutPutlist;
        //Binding实例定义
        Dictionary<string, DataString> UIdictionary = new Dictionary<string, DataString>();
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
        //public string  sEquipmentElement="";
        //public string  sPLCLink = "";
        //public string sEquipmentLine = "";

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
            InitializeComponent();
            InitializeViewBinding();
            InitializeParametr();

        }
        void InitializeViewBinding()
        {
            //Gui对应的画面名
            string UIKey = "ViewNameteBox";
            string sString = "PVG_BHS_S1_LA_0002_0003";
            bool? bBool=null ;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            ViewNameteBox.DataContext = UIdictionary[UIKey];

            //输入的taglist
            UIKey = "InPutFilePathteBox";
            sString = null;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            InPutFilePathteBox.DataContext = UIdictionary[UIKey];

            //输入的BaseList
            UIKey = "BaseListFilePathTeBox";
            sString = null;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            BaseListFilePathTeBox.DataContext = UIdictionary[UIKey];

            //L1 L2画面选择
            //L0raButton.DataContext = dBinding[3];
            UIKey = "L1raButton";
            sString = null;
            bBool = false;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            L1raButton.DataContext = UIdictionary[UIKey];
            UIKey = "L2raButton";
            bBool = true;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            L2raButton.DataContext = UIdictionary[UIKey];

            //XAML代码生成
            UIKey = "TeBlackraButton";
            bBool = true;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            TeBlackraButton.DataContext = UIdictionary[UIKey];
            UIKey = "ElementradioButton";
            bBool = false;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            ElementradioButton.DataContext = UIdictionary[UIKey];
            UIKey = "XMLOutPutFilePathteBox";
            bBool = null;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            XMLOutPutFilePathteBox.DataContext = UIdictionary[UIKey];
            UIKey = "XMLOutPutDatasteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            XMLOutPutDatasteBox.DataContext = UIdictionary[UIKey];

            //TestData测试数据生成画面
            UIKey = "TestdataOutPutFilePathteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            TestdataOutPutFilePathteBox.DataContext = UIdictionary[UIKey];
            UIKey = "TestDataOutPutDatasteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            TestDataOutPutDatasteBox.DataContext = UIdictionary[UIKey];

            //TestList测试表格生成画面
            UIKey = "TestListTemplateteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            TestListTemplateteBox.DataContext = UIdictionary[UIKey];
            UIKey = "TestListOutPutFilePathteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            TestListOutPutFilePathteBox.DataContext = UIdictionary[UIKey];
            UIKey = "TestListOutPutDatasteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            TestListOutPutDatasteBox.DataContext = UIdictionary[UIKey];

            //Level1Data生成L1画面点数据
            //Level1DataTemplateteBox.DataContext = dBinding[40];
            UIKey = "Level1DataOutPutFilePathteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            Level1DataOutPutFilePathteBox.DataContext = UIdictionary[UIKey];
            UIKey = "Level1DataOutPutDatasteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            Level1DataOutPutDatasteBox.DataContext = UIdictionary[UIKey];

            //OPC配置文件生成
            UIKey = "OPCDataOutPutFilePathteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            OPCDataOutPutFilePathteBox.DataContext = UIdictionary[UIKey];
            UIKey = "OPCDataOutPutDatasteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            OPCDataOutPutDatasteBox.DataContext = UIdictionary[UIKey];
            UIKey = "SinglecheckBox";
            bBool = true;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            SinglecheckBox.DataContext = UIdictionary[UIKey];
            UIKey = "CommandcheckBox";
            bBool = true;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            CommandcheckBox.DataContext = UIdictionary[UIKey];
            UIKey = "HourcheckBox";
            bBool = false;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            HourcheckBox.DataContext = UIdictionary[UIKey];

            //AlarmLinkConf AlarmList配置表生成
            UIKey = "OPCInforaButton";
            bBool = true;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            OPCInforaButton.DataContext = UIdictionary[UIKey];
            UIKey = "AlarmListradioButton";
            bBool = false;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            AlarmListradioButton.DataContext = UIdictionary[UIKey];
            UIKey = "ConfOutPutFilePathteBox";
            bBool = null;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            ConfOutPutFilePathteBox.DataContext = UIdictionary[UIKey];
            UIKey = "ConfOutPutDatasteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            ConfOutPutDatasteBox.DataContext = UIdictionary[UIKey];

        }
        void InitializeParametr()
        {        

            try
            {
                string sTableName;
                //配置参数初始化
                DataSet dConfigTable = new DataSet();
                string sConFilePath = ConfigurationSettings.AppSettings["Config.FilePath"];
                //ConveyorSheetName初始化
                dConfigTable = ExcelFunction.ExcelRead(sConFilePath);
                //Conveyor Sheet Name & Conveyor Column Name初始化
                sTableName = ConfigurationSettings.AppSettings["ConveyorTaName"];
                sTableToStringArrary(dConfigTable.Tables[sTableName], out sConveyorSheetName, out sConveyorColName);
                //BaseList Sheet Name & Conveyor Column Name初始化
                sTableName = ConfigurationSettings.AppSettings["BaseTaName"]; 
                sTableToStringArrary(dConfigTable.Tables[sTableName], out sBaseListSheetName, out sBaseListColName);

                //TestDataTemplate
                sTableName = ConfigurationSettings.AppSettings["TestDaTeTaName"] ;
                sTestDaActive = (string)dConfigTable.Tables[sTableName].Rows[0][0];
                sTestDaTemplateL1 = (string)dConfigTable.Tables[sTableName].Rows[1][0];
                sTestDaTemplateL2 = (string)dConfigTable.Tables[sTableName].Rows[2][0];

                //XmlTemplate
                sTableName = ConfigurationSettings.AppSettings["XmlTeTaName"]; 
                sXmlTextBlock = (string)dConfigTable.Tables[sTableName].Rows[0][0];
                sXmlElement = (string)dConfigTable.Tables[sTableName].Rows[1][0];

                //L1AlarmDataTemplate
                sTableName = ConfigurationSettings.AppSettings["L1AlmDaTeTaName"]; 
                Level1DataExcelData = dConfigTable.Tables[sTableName];

                //OPCDataTemplate
                sTableName = ConfigurationSettings.AppSettings["OpcDaTeTaName"]; 
                sOPCDaSingleTemplate = (string)dConfigTable.Tables[sTableName].Rows[0][0];
                sOPCDaCommandTemplate = (string)dConfigTable.Tables[sTableName].Rows[1][0];
                sOPCDaHourTemplate = (string)dConfigTable.Tables[sTableName].Rows[2][0];

                //dConfigTable.Clear();

            }
            catch
            {

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
            string UIKey = "InPutFilePathteBox";
            string sFilePath = UIdictionary[UIKey].MyString;
            ConveyorExcelData = dDataLoad(ref sFilePath);
            UIdictionary[UIKey].MyString = sFilePath;
            //if (ConveyorExcelData != null)  SelectConRows = LinqToTable(); 
      
         }
        /// <summary>
        /// BaseList表格输入（PVG_CrisBeltBase_V016_9）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseListbutton_Click(object sender, RoutedEventArgs e)
        {
            string UIKey = "BaseListFilePathTeBox";
            string sFilePath = UIdictionary[UIKey].MyString;
            BaseListExceLData = dDataLoad(ref sFilePath);
            UIdictionary[UIKey].MyString = sFilePath;
        }
        /// <summary>
        /// WPF XML数据输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XMLOutputbutton_Click(object sender, RoutedEventArgs e)
        {
            string sFileStyle = "Text documents (.txt)|*.txt";

            string UIKey = "XMLOutPutFilePathteBox";
            string UIKey1 = "XMLOutPutDatasteBox";
            string UIKey4= "TeBlackraButton";
            string UIKey5= "ElementradioButton";
            string sFilePath = UIdictionary[UIKey].MyString;

            BaseFactory factory = new BaseFactory();
            factory.BaseFactoryParameter = CreateConvertParameter();
            if (UIdictionary[UIKey4].Mybool == true)
            {
                factory.iXmlType = 1;
            }
            else if (UIdictionary[UIKey5].Mybool == true)
            {
                factory.iXmlType = 2;
            }
            try
            {
                bool bOpenEnable = bDataOutput(ref sFilePath, sFileStyle);
                if (bOpenEnable == true)
                {
                    BaseTableConvert XmlData = factory.CreatTableConvert("ToXml");
                    UIdictionary[UIKey].MyString = sFilePath;
                    UIdictionary[UIKey1].MyString = XmlData.sOutData();
                    System.IO.File.WriteAllText(@sFilePath, UIdictionary[UIKey1].MyString, Encoding.UTF8);
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
            string sFileStyle = "SIG File(.signaltester) | *.signaltester";
            string UIKey = "TestdataOutPutFilePathteBox";
            string UIKey1 = "TestDataOutPutDatasteBox";
            string sFilePath = UIdictionary[UIKey].MyString;
            BaseFactory factory = new BaseFactory();
            factory.BaseFactoryParameter = CreateConvertParameter();
            try
            {

                bool bOpenEnable = bDataOutput(ref sFilePath, sFileStyle);
                if (bOpenEnable == true)
                {
                    BaseTableConvert TestData =  factory.CreatTableConvert("ToTeData");
                    UIdictionary[UIKey].MyString = sFilePath;
                    UIdictionary[UIKey1].MyString = TestData.sOutData();
                    System.IO.File.WriteAllText(@sFilePath, UIdictionary[UIKey1].MyString, Encoding.UTF8);
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
            string sFileStyle = "Excel(.xlsx) | *.xlsx|Excel(.xls) | *.xls";
            string UIKey = "TestListOutPutFilePathteBox";
            string UIKey1 = "TestListOutPutDatasteBox";
            string sFilePath = UIdictionary[UIKey].MyString;
            BaseFactory factory = new BaseFactory();
            factory.BaseFactoryParameter = CreateConvertParameter();

            try
            {

             bool bOpenEnable = bDataOutput(ref sFilePath, sFileStyle);
                if (bOpenEnable == true)
                {
                    BaseTableConvert ListData = factory.CreatTableConvert("ToTeList");
                    UIdictionary[UIKey].MyString = sFilePath;
                    string sOutPutLiData = null;
                    Dictionary<string, string> telistDictionary = ListData.diOutData();
                    foreach ( KeyValuePair<string,string> listdata in telistDictionary)
                    {
                        if (sOutPutLiData == null)
                        {
                            sOutPutLiData = listdata.Key + " " + listdata.Value;
                        }
                        else
                        {
                            sOutPutLiData = sOutPutLiData + "\r" + listdata.Key + " " + listdata.Value;
                        }
                    }
                    UIdictionary[UIKey1].MyString= sOutPutLiData;
                    ExcelFunction.ExcelWrite(UIdictionary[UIKey1].MyString, sFilePath, telistDictionary);
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
            string UIKey = "TestListTemplateteBox";
            string sFilePath = UIdictionary[UIKey].MyString;
            TemlateExcelData = dDataLoad(ref sFilePath);
            UIdictionary[UIKey].MyString = sFilePath;
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
            string UIKey = "Level1DataOutPutFilePathteBox";
            string UIKey1 = "Level1DataOutPutDatasteBox";
            string sFilePath = UIdictionary[UIKey].MyString;
            BaseFactory factory = new BaseFactory();
            factory.BaseFactoryParameter = CreateConvertParameter();
            factory.TempTable = Level1DataExcelData;
            try
            {
                bool bOpenEnable = bDataOutput(ref sFilePath, sFileStyle);
                if (bOpenEnable == true)
                {
                    BaseTableConvert Level1Data =  factory.CreatTableConvert("ToLevel1Data");
                    UIdictionary[UIKey].MyString = sFilePath;
                    UIdictionary[UIKey1].MyString = Level1Data.sOutData();
                    System.IO.File.WriteAllText(@sFilePath, UIdictionary[UIKey1].MyString, Encoding.UTF8);
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
            string UIKey = "OPCDataOutPutFilePathteBox";
            string UIKey1 = "OPCDataOutPutDatasteBox";
            string UIKey2 = "SinglecheckBox";
            string UIKey3 = "CommandcheckBox";
            string UIKey4 = "HourcheckBox";
            string sFilePath = UIdictionary[UIKey].MyString;
            bool bOpenEnable = bDataOutput(ref sFilePath, sFileStyle);

            BaseFactory factory = new BaseFactory();
            factory.BaseFactoryParameter = CreateConvertParameter();
            factory.bSingle = (bool)UIdictionary[UIKey2].Mybool;
            factory.bCommand = (bool)UIdictionary[UIKey3].Mybool;
            factory.bHours = (bool)UIdictionary[UIKey4].Mybool;

            try
            {
                if (bOpenEnable)
                {
                    BaseTableConvert OPCData = factory.CreatTableConvert("ToOPCData");
                    DataTable dt = OPCData.dOutData();
                    UIdictionary[UIKey].MyString = sFilePath;
                    //ExcelFunction.ExcelWrite(sFilePath, dt);
                    CsvFunction.CsvWirte(sFilePath, dt);
                    UIdictionary[UIKey1].MyString = DataConvert.ToString(dt);
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
            string UIKey = "ConfOutPutFilePathteBox";
            string UIKey1 = "ConfOutPutDatasteBox";
            string UIKey2 = "OPCInforaButton";
            string UIKey3 = "AlarmListradioButton";
            string sFilePath = UIdictionary[UIKey].MyString;
            bool bOpenEnable = bDataOutput(ref sFilePath, sFileStyle);

            BaseFactory factory = new BaseFactory();
            factory.BaseFactoryParameter = CreateConvertParameter();
            factory.bOPCIfo=(bool)UIdictionary[UIKey2].Mybool;
            factory.bConvAlarm = (bool)UIdictionary[UIKey3].Mybool;
            if (UIdictionary[UIKey2].Mybool==true )
            {
                string[] sListColName = { "Tag Name", "Type", "Channel", "Device", "DataType" };
                factory.sListColName = sListColName.ToList<string>();
            }
            if (UIdictionary[UIKey3].Mybool==true)
            {
                string[] sListColName = { "SignalName", "AckType", "AlarmTag", "PartName",
                    "Alarm Description", "AlarmGroup", "AlarmType", "AlarmCategory",
                    "Delayed","ConditionName","Priority","GeneralComment","Level1View",
                    "Level2View","Resetable","ResetBit","ALNumber","CCTVRecording",
                    "ElementID","ResetSignal","ExtraTagList","CISData","Technical"};
                factory.sListColName = sListColName.ToList<string>();
            }
            try
            {
                if (bOpenEnable)
                {
                    BaseTableConvert dConfigData = factory.CreatTableConvert("ToConfig");
                    List<List<string>> dt = dConfigData.lOutData();
                    UIdictionary[UIKey].MyString = sFilePath;
                    //ExcelFunction.ExcelWrite(sFilePath, dt);
                    XmlFuction xmlFuction = new XmlFuction();
                    xmlFuction.XmlWrite(sFilePath, dt);
                    //UIdictionary[UIKey1].MyString = DataConvert.ToString(dt);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Build OPC Data Error: " + ex.Message);
            }
            GC.Collect();

        }
        private DaTableConverParameter CreateConvertParameter()
        {
            DaTableConverParameter convertParameter = new DaTableConverParameter();
            convertParameter.TaglistTable = ConveyorExcelData.Tables[sConveyorSheetName[1]];
            convertParameter.TaglistColName = sConveyorColName;
            convertParameter.SingleMappingTable = BaseListExceLData.Tables[sBaseListSheetName[1]];
            convertParameter.CommandMappingTable = BaseListExceLData.Tables[sBaseListSheetName[2]];
            convertParameter.BasefileColName = sBaseListColName;
            string UIKey1 = "L1raButton";
            string UIKey2 = "L2raButton";
            string UIKey3 = "ViewNameteBox";
            convertParameter.ViewName = UIdictionary[UIKey3].MyString;
            if (UIdictionary[UIKey1].Mybool == true)
            {
                convertParameter.ViewNum = 1;
            }
            else if (UIdictionary[UIKey2].Mybool == true)
            {
                convertParameter.ViewNum = 2;
            }
            convertParameter.Stemp0 = sTestDaActive;
            convertParameter.Stemp1 = sTestDaTemplateL1;
            convertParameter.Stemp2 = sTestDaTemplateL2;

            convertParameter.Stemp3 = sXmlTextBlock ;
            convertParameter.Stemp4 = sXmlElement;

            convertParameter.Stemp5 = sOPCDaSingleTemplate;
            convertParameter.Stemp6 = sOPCDaCommandTemplate;
            convertParameter.Stemp7 = sOPCDaHourTemplate;

            return convertParameter;
        }
    }
}
