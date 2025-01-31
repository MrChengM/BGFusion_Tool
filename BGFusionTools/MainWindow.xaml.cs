﻿using System;
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
using BGFusionTools.Helper;
using BGFusionTools.Functions;
using BGFusionTools.Serialization;
using BGFusionTools.Datas;
using System.Xml;

namespace BGFusionTools
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
        public string[] sPaths;
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
        //public string[] sConveyorSheetName;
        //public string[,] sConveyorColName;
        public TaglistColumns taglistColumns = TaglistColumns.getInstance();
        //BaseList数据表定义
        public string[] sBaseListSheetName;
        public string[,] sBaseListColName;
        public SignalMappingColumns signalMappingColumns = SignalMappingColumns.getInstance();
        public CommandMappingColumns commandMappingColumns = CommandMappingColumns.getInstance();
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
            bool? bBool = null;
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

            //TestData测试数据生成画面
            UIKey = "TestdataOutPutFilePathteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            TestdataOutPutFilePathteBox.DataContext = UIdictionary[UIKey];

            //TestList测试表格生成画面
            //UIKey = "TestListTemplateteBox";
            //UIdictionary.Add(UIKey, new DataString(sString, bBool));
            //TestListTemplateteBox.DataContext = UIdictionary[UIKey];
            UIKey = "TestListOutPutFilePathteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            TestListOutPutFilePathteBox.DataContext = UIdictionary[UIKey];

            //Level1Data生成L1画面点数据
            //Level1DataTemplateteBox.DataContext = dBinding[40];
            UIKey = "Level1DataOutPutFilePathteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            Level1DataOutPutFilePathteBox.DataContext = UIdictionary[UIKey];

            //OPC配置文件生成
            UIKey = "OPCDataOutPutFilePathteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            OPCDataOutPutFilePathteBox.DataContext = UIdictionary[UIKey];
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

            //输出数据结果显示
            UIKey = "OutPutDatasteBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            OutPutDatasteBox.DataContext = UIdictionary[UIKey];

            //合并XML文件数据
            UIKey = "MergeInputFilePathBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            MergeInputFilePathBox.DataContext = UIdictionary[UIKey];
            UIKey = "MergeOutputFilePathBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            MergeOutputFilePathBox.DataContext = UIdictionary[UIKey];
            UIKey = "WorkraButton";
            bBool = true;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            WorkraButton.DataContext = UIdictionary[UIKey];
            UIKey = "ElementSearchraButton";
            bBool = false;
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            ElementSearchraButton.DataContext = UIdictionary[UIKey];

            //SortIO OPC数据生成
            UIKey = "SorteIOInputFilePathBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            SorteIOInputFilePathBox.DataContext = UIdictionary[UIKey];
            UIKey = "SorteIOOutputFilePathBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            SorteIOOutputFilePathBox.DataContext = UIdictionary[UIKey];

            //ElementsSearch XML文件生成
            UIKey = "ElementSearchOutputFilePathBox";
            UIdictionary.Add(UIKey, new DataString(sString, bBool));
            ElementSearchOutputFilePathBox.DataContext = UIdictionary[UIKey];
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
                dConfigTable = NpoiExcelFunction.ExcelRead(sConFilePath);
                /*//Conveyor Sheet Name & Conveyor Column Name初始化
                sTableName = ConfigurationSettings.AppSettings["ConveyorTaName"];
                sTableToStringArrary(dConfigTable.Tables[sTableName], out sConveyorSheetName, out sConveyorColName);
                //BaseList Sheet Name & Conveyor Column Name初始化
                sTableName = ConfigurationSettings.AppSettings["BaseTaName"];
                sTableToStringArrary(dConfigTable.Tables[sTableName], out sBaseListSheetName, out sBaseListColName);
                */
                //TestDataTemplate
                sTableName = ConfigurationSettings.AppSettings["TestDaTeTaName"];
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
                        sArrary[i, j] = (string)dInputDataTable.Rows[j][i];
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
        private DataSet dDataLoad(ref string sFileName)
        {
            DataSet dExcelDataSet = new DataSet();
            //sEquipmentElement.Clear();
            //sEquipmentLine.Clear();
            OpenFileDialog dialog = new OpenFileDialog();//open the path
            dialog.FileName = sFileName;
            dialog.Filter = "Excel Worksheets | *.xls;*.xlsx";
            if (dialog.ShowDialog() == true)
            {
                sPath = dialog.FileName;
                sFileName = dialog.FileName;
            }
            else
            {
                return null;
            }
            if (sPath != null)
            {
                dExcelDataSet = NpoiExcelFunction.ExcelRead(sPath);
                //DataTableToOutPutList();
            }
            return dExcelDataSet;
        }
        private bool Inputfile(ref string sfileName, string sFilter)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = sfileName;
            dialog.Filter = sFilter;

            if (dialog.ShowDialog() == true)
            {
                // sPath = dialog.FileName;
                sfileName = dialog.FileName;
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 数据输出保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool Outputfile(ref string sfileName, string sFilter)
        {

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = sfileName;
            dialog.Filter = sFilter;

            // Show save file dialog box
            Nullable<bool> result = dialog.ShowDialog();
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
            GC.Collect();
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
            GC.Collect();
        }
        /// <summary>
        /// WPF XAML数据输出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XMLOutputbutton_Click(object sender, RoutedEventArgs e)
        {
            string sFileStyle = "WPF (.Xaml)|*.Xaml";
            string UIKey = "XMLOutPutFilePathteBox";
            string UIKey1 = "OutPutDatasteBox";
            string UIKey2 = "L1raButton";
            string UIKey3 = "L2raButton";
            string UIKey4 = "TeBlackraButton";
            string UIKey5 = "ElementradioButton";
            string UIKey6 = "ViewNameteBox";
            string sFilePath = UIdictionary[UIKey].MyString;
            UIdictionary[UIKey1].MyString = "";
            try
            {
                bool bOpenEnable = Outputfile(ref sFilePath, sFileStyle);
                if (bOpenEnable == true)
                {

                    BaseFactory factory = new BaseFactory();
                    factory.BaseParameter = CreateConvertParameter();
                    XamlData Data = (XamlData)factory.CreatDataClass("XamlData");
                    CreateDataMath<BgElementCommonXaml, ConveyorRow> bgXmalMath;
                    CreateDataMath<BgTextBlock, ConveyorRow> blockTextMath;

                    List<BgElementCommonXaml> bgXamlGroup = new List<BgElementCommonXaml>();
                    List<BgTextBlock> textBlockGroup = new List<BgTextBlock>();

                    if (UIdictionary[UIKey2].Mybool == true)
                    {
                        bgXmalMath = Data.CreatL1CommonXaml12307;
                        bgXamlGroup = Data.CreateList(bgXmalMath);
                    }
                    else if (UIdictionary[UIKey3].Mybool == true)
                    {
                        if (UIdictionary[UIKey5].Mybool == true)
                        {
                            bgXmalMath = Data.CreatL2CommonXaml;
                            bgXamlGroup = Data.CreateList(bgXmalMath);
                        }
                        else if (UIdictionary[UIKey4].Mybool == true)
                        {
                            blockTextMath = Data.CreatTextBlock;
                            textBlockGroup = Data.CreateList(blockTextMath);
                        }
                    }
                    ElementXaml elementXaml = new ElementXaml(bgXamlGroup, textBlockGroup, UIdictionary[UIKey6].MyString);
                    XmlSerialiaztion.XmlSerial(@sFilePath, elementXaml);
                    UIdictionary[UIKey].MyString = sFilePath;
                    UIdictionary[UIKey1].MyString = "Build Xaml Successful!";
                    //System.IO.File.WriteAllText(@sFilePath, UIdictionary[UIKey1].MyString, Encoding.UTF8);
                }
            }
            catch (Exception ex)
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
            string UIKey1 = "OutPutDatasteBox";
            string UIKey3 = "L1raButton";
            string UIKey4 = "L2raButton";
            string sFilePath = UIdictionary[UIKey].MyString;
            try
            {

                bool bOpenEnable = Outputfile(ref sFilePath, sFileStyle);
                if (bOpenEnable == true)
                {
                    BaseFactory factory = new BaseFactory();
                    factory.BaseParameter = CreateConvertParameter();
                    TestData TestData = (TestData)factory.CreatDataClass("TestData");
                    List<List<string>> TestDataList = new List<List<string>>();
                    if (UIdictionary[UIKey3].Mybool == true)
                    {
                        CreateDataMath<string, List<ConveyorRow>> dataMath = TestData.CreateL1Signal;
                        TestDataList = TestData.CreateList(dataMath);
                    }
                    if (UIdictionary[UIKey4].Mybool == true)
                    {
                        CreateDataMath<string, List<ConveyorRow>> dataMath = TestData.CreateL2Signal;
                        TestDataList = TestData.CreateList(dataMath);
                    }
                    UIdictionary[UIKey].MyString = sFilePath;
                    UIdictionary[UIKey1].MyString = DataConvert.ToString(TestDataList);
                    System.IO.File.WriteAllText(@sFilePath, UIdictionary[UIKey1].MyString, Encoding.UTF8);
                }
            }
            catch (Exception ex)
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
            string UIKey1 = "OutPutDatasteBox";
            string UIKey2 = "TestListTemplateteBox";
            string sFilePath = UIdictionary[UIKey].MyString;
            UIdictionary[UIKey1].MyString = "";
            //try
            //{

                bool bOpenEnable = Outputfile(ref sFilePath, sFileStyle);
                if (bOpenEnable == true)
                {
                    BaseFactory factory = new BaseFactory();
                    factory.BaseParameter = CreateConvertParameter();


                    TestList listData = (TestList)factory.CreatDataClass("TestList");
                    CreateDataMath<List<TestSheetRow>, ConveyorRow> testdataMath = listData.CreateTestRows;
                    CreateDataMath<List<string>, ConveyorRow> cmcdataMath = listData.CreateCMCRows;
                    List<List<TestSheetRow>> testDataRows = listData.CreateList(testdataMath);
                    List<List<string>> cmcDataRows = listData.CreateList(cmcdataMath);
                    NpoiExcelFunction.ExcelWrite(sFilePath, "CMC", cmcDataRows);
                    NpoiExcelFunction.ExcelWrite(sFilePath, "Test", testDataRows);
                    UIdictionary[UIKey].MyString = sFilePath;

                    //NpoiExcelFunction.ExcelWrite(sFilePath, (ListData as TestList).ListData);
                    UIdictionary[UIKey1].MyString = "Output Test List successful!";
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Build Test List Error： " + ex.Message);
            //}
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
            Inputfile(ref sFilePath, "Excel Worksheets | *.xls;*.xlsx");
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
            string sFileStyle = "Text documents (.vsignal)|*.vsignal";
            string UIKey = "Level1DataOutPutFilePathteBox";
            string UIKey1 = "OutPutDatasteBox";
            string sFilePath = UIdictionary[UIKey].MyString;
            UIdictionary[UIKey1].MyString = "";
            try
            {
                bool bOpenEnable = Outputfile(ref sFilePath, sFileStyle);
                if (bOpenEnable == true)
                {
                    BaseFactory factory = new BaseFactory();
                    factory.BaseParameter = CreateConvertParameter();
                    factory.TempTable = Level1DataExcelData;
                    Level1Data level1Data = (Level1Data) factory.CreatDataClass("Level1Data");
                    CreateDataMath<string, List<ConveyorRow>> dataMath = level1Data.CreateLineSignal;
                    var level1DataList = level1Data.CreateList(dataMath);
                    UIdictionary[UIKey].MyString = sFilePath;
                    UIdictionary[UIKey1].MyString = DataConvert.ToString1(level1DataList);
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
            string UIKey1 = "OutPutDatasteBox";
            string UIKey2 = "SinglecheckBox";
            string UIKey3 = "CommandcheckBox";
            string UIKey4 = "HourcheckBox";
            string sFilePath = UIdictionary[UIKey].MyString;
            bool bOpenEnable = Outputfile(ref sFilePath, sFileStyle);
            UIdictionary[UIKey1].MyString = "";
            try
            {
                if (bOpenEnable)
                {
                    BaseFactory factory = new BaseFactory();
                    factory.BaseParameter = CreateConvertParameter();
                    factory.bSingle = (bool)UIdictionary[UIKey2].Mybool;
                    factory.bCommand = (bool)UIdictionary[UIKey3].Mybool;
                    factory.bHours = (bool)UIdictionary[UIKey4].Mybool;
                    OPCData opcData =(OPCData) factory.CreatDataClass("OPCData");
                    CreateDataMath<List<string>, ConveyorRow> createOPCMath = opcData.CreateOPCRows;
                    var opcdataList= opcData.CreateList(createOPCMath);
                    UIdictionary[UIKey].MyString = sFilePath;
                    //ExcelFunction.ExcelWrite(sFilePath, dt);
                    CsvFunction.CsvWirte(sFilePath, opcdataList);
                    UIdictionary[UIKey1].MyString = DataConvert.ToString(opcdataList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Build OPC Data Error: " + ex.Message);
            }
            GC.Collect();

        }

        private void ConfOutputbutton_Click(object sender, RoutedEventArgs e)
        {
            string sFileStyle = "Excel(.xml) | *.xml";
            string UIKey = "ConfOutPutFilePathteBox";
            string UIKey1 = "OutPutDatasteBox";
            string UIKey2 = "OPCInforaButton";
            string UIKey3 = "AlarmListradioButton";
            string sFilePath = UIdictionary[UIKey].MyString;
            bool bOpenEnable = Outputfile(ref sFilePath, sFileStyle);
            UIdictionary[UIKey1].MyString = "";
            try
            {
                if (bOpenEnable)
                {
                    BaseFactory factory = new BaseFactory();
                    factory.BaseParameter = CreateConvertParameter();
                    factory.bOPCIfo = (bool)UIdictionary[UIKey2].Mybool;
                    factory.bConvAlarm = (bool)UIdictionary[UIKey3].Mybool;
                    CreateDataMath<List<string>, ConveyorRow> createMath;
                    ConfigData dConfigData ;
                    List<List<string>> lls = new List<List<string>>();
                    //ConfigData dConfigData = (ConfigData)factory.CreatDataClass("ConfigData");
                    if (UIdictionary[UIKey2].Mybool == true)
                    {
                        string[] sListColName = { "Tag Name", "Type", "Channel", "Device", "DataType" };
                        factory.sListColName = sListColName.ToList();
                        dConfigData = (ConfigData)factory.CreatDataClass("ConfigData");
                        createMath = dConfigData.CreateOPCInfoRows;
                        lls = dConfigData.CreateList(createMath);
                    }
                    if (UIdictionary[UIKey3].Mybool == true)
                    {
                        string[] sListColName = { "SignalName", "AckType", "AlarmTag", "PartName",
                    "Alarm Description", "AlarmGroup", "AlarmType", "AlarmCategory",
                    "Delayed","ConditionName","Priority","GeneralComment","Level1View",
                    "Level2View","Resetable","ResetBit","ALNumber","CCTVRecording",
                    "ElementID","ResetSignal","ExtraTagList","CISData","Technical"};
                        factory.sListColName = sListColName.ToList();
                        dConfigData = (ConfigData)factory.CreatDataClass("ConfigData");
                        createMath = dConfigData.CreateAlarmListRows;
                        lls = dConfigData.CreateList(createMath);
                    }
                    UIdictionary[UIKey].MyString = sFilePath;
                    //ExcelFunction.ExcelWrite(sFilePath, dt);
                    //XmlFuction xmlFuction = new XmlFuction();
                    //xmlFuction.XmlWrite(sFilePath, dt);
                    XmlSerialiaztion.XmlSerial(sFilePath, new Workbook(lls));
                    UIdictionary[UIKey1].MyString = "Build Datas Successful";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Build OPC Data Error: " + ex.Message);
            }
            GC.Collect();

        }
        /// <summary>
        /// 创建公共参数
        /// </summary>
        /// <returns></returns>
        private BaseParameter CreateConvertParameter()
        {
            BaseParameter convertParameter = new BaseParameter();
            convertParameter.TaglistTable = ConveyorExcelData.Tables[taglistColumns.sTagName];
            convertParameter.TaglistColName = taglistColumns;
            convertParameter.SingleMappingTable = BaseListExceLData.Tables[signalMappingColumns.sSignalMapping];
            convertParameter.SignalMappingColName = signalMappingColumns;
            convertParameter.CommandMappingTable = BaseListExceLData.Tables[commandMappingColumns.sCommandMapping];
            convertParameter.CommandMappingColName = commandMappingColumns;
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
            convertParameter.Stemp3 = sXmlTextBlock;
            convertParameter.Stemp4 = sXmlElement;
            convertParameter.Stemp5 = sOPCDaSingleTemplate;
            convertParameter.Stemp6 = sOPCDaCommandTemplate;
            convertParameter.Stemp7 = sOPCDaHourTemplate;

            return convertParameter;
        }

        private void MergeInputFilePathbutton_Click(object sender, RoutedEventArgs e)
        {
            string UIKey = "MergeInputFilePathBox";
            string sFilePath = UIdictionary[UIKey].MyString;
            //string[] sFilePaths;

            /*if (Directory.Exists(sFilePath))
            {
                sFilePaths = Directory.GetFiles(sFilePath, "*.xml");
            }*/
            OpenFileDialog dialog = new OpenFileDialog();//open the path
            dialog.FileName = sFilePath;
            dialog.Multiselect = true;
            dialog.Filter = "Excel(.xml) | *.xml";
            if (dialog.ShowDialog() == true)
            {
                sPaths = dialog.FileNames;
                UIdictionary[UIKey].MyString = System.IO.Path.GetDirectoryName(sPaths[0]);
            }
        }
        /// <summary>
        /// Merge the config files,such as :ALarm_TextList,OPCinfo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MergeOutputFilePathbutton_Click(object sender, RoutedEventArgs e)
        {
            string sFileStyle = "XML | *.xml";
            string UIKey = "MergeOutputFilePathBox";
            string UIKey1 = "OutPutDatasteBox";
            string UIKey2 = "WorkraButton";
            string UIKey3 = "ElementSearchraButton";
            string sFilePath = UIdictionary[UIKey].MyString;
            bool? btypeWorkbook = UIdictionary[UIKey2].Mybool;
            bool? btypeElementSearch = UIdictionary[UIKey3].Mybool;
            bool bOpenEnable = Outputfile(ref sFilePath, sFileStyle);
            UIdictionary[UIKey1].MyString = "";
            try
            {
                if (bOpenEnable)
                {

                    UIdictionary[UIKey1].MyString = "Merge Datas Successful";
                    UIdictionary[UIKey].MyString = sFilePath;
                    if (btypeWorkbook == true)
                    {
                        XmlSerialiaztion.XmlSerial(sFilePath, Merge<Workbook>(sPaths));
                    }
                    else if (btypeElementSearch == true)
                    {
                        XmlSerialiaztion.XmlSerial(sFilePath, Merge<ElementSearchXml>(sPaths));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Merge files Error: " + ex.Message);
            }
        }
        private T Merge<T>(string[] spaths) where T : IOperation<T>, new()
        {
            int i = 0;
            T files = new T();
            foreach (string s in sPaths)
            {
                T flie = XmlSerialiaztion.XmlDeserial<T>(s);
                files.Add(flie);
                i++;
            }
            return files;
        }
        private void SorteIOOutputFilePathbutton_Click(object sender, RoutedEventArgs e)
        {
            string sFileStyle = "Excel(.csv) | *.csv";
            string UIKey = "SorteIOOutputFilePathBox";
            string UIKey1 = "OutPutDatasteBox";
            string UIKey2 = "SorteIOInputFilePathBox";
            string sOutFilePath = UIdictionary[UIKey].MyString;
            string sInFilePath = UIdictionary[UIKey2].MyString;

            bool bOpenEnable = Outputfile(ref sOutFilePath, sFileStyle);
            UIdictionary[UIKey1].MyString = "";
            try
            {
                if (bOpenEnable & sInFilePath != "")
                {
                    DataTable dt = XmlSerialiaztion.XmlDeserial<SignalMonitor>(sInFilePath).ToDataTable();
                    UIdictionary[UIKey].MyString = sOutFilePath;
                    //ExcelFunction.ExcelWrite(sFilePath, dt);
                    CsvFunction.CsvWirte(sOutFilePath, dt);
                    UIdictionary[UIKey1].MyString = DataConvert.ToString(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Build OPC Data Error: " + ex.Message);
            }
            GC.Collect();


        }

        private void SorteIOInputFilePathbutton_Click(object sender, RoutedEventArgs e)
        {
            string UIKey = "SorteIOInputFilePathBox";
            string sFileStyle = "Xml|*.xml";
            string sFilePath = UIdictionary[UIKey].MyString;
            if (Inputfile(ref sFilePath, sFileStyle))
                UIdictionary[UIKey].MyString = sFilePath;
        }

        private void ElementSearchOutputFilePathbutton_Click(object sender, RoutedEventArgs e)
        {
            string sFileStyle = "XML | *.xml";
            string UIKey = "ElementSearchOutputFilePathBox";
            string UIKey1 = "OutPutDatasteBox";
            string sOutFilePath = UIdictionary[UIKey].MyString;
            bool bOpenEnable = Outputfile(ref sOutFilePath, sFileStyle);
            UIdictionary[UIKey1].MyString = "";
            try
            {
                if (bOpenEnable)
                {
                    BaseFactory factory = new BaseFactory(CreateConvertParameter());
                    ElementSearchData elmentSearchData = (ElementSearchData)factory.CreatDataClass("ElementSearchData");
                    CreateDataMath<ElementSeacrhStruct, ConveyorRow> elmentSearchMath = elmentSearchData.CreateElements;
                    List<ElementSeacrhStruct> lelmentSearchData = elmentSearchData.CreateList(elmentSearchMath);
                    ElementSearchXml elementSearchXml = new ElementSearchXml();
                    elementSearchXml.Elements = lelmentSearchData;
                    UIdictionary[UIKey1].MyString = DataConvert.ToString(lelmentSearchData);
                    UIdictionary[UIKey].MyString = sOutFilePath;
                    XmlSerialiaztion.XmlSerial(sOutFilePath, elementSearchXml);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Build Element Search Data Error: " + ex.Message);
            }
            GC.Collect();
        }
    }
   
}
