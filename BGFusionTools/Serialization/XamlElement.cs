using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using BGFusionTools.Helper;
using BGFusionTools.Datas;

namespace BGFusionTools.Serialization
{
    [XmlRoot("UserControl")]
    public class ElementXaml : IXmlSerializable, IOperation<ElementXaml>
    {
        //Viewbox _viewbox;
        //GridXaml _grid;
        public List<BgElementCommonXaml> BgElementCommons = new List<BgElementCommonXaml>();
        public List<BgTextBlock> BgTextBlocks = new List<BgTextBlock>();
        public string _className = "";
        public ElementXaml() { }
        public ElementXaml(List<BgElementCommonXaml> elementCommons, List<BgTextBlock> textBlocks)
        {
            BgElementCommons = elementCommons;
            BgTextBlocks = textBlocks;
        }
        public ElementXaml(List<BgElementCommonXaml> elementCommons, List<BgTextBlock> textBlocks, string view)
        {
            BgElementCommons = elementCommons;
            BgTextBlocks = textBlocks;
            _className = string.Format("BG_SCADA.Views.{0}", view);
        }
        public ElementXaml Add(ElementXaml T1)
        {
            Margin _margin = new Margin();
            BgTextBlock bgtextblock = BgTextBlocks.Last();
            _margin = _margin.MarginChange(bgtextblock.Margin, bgtextblock.iColWidth, bgtextblock.iColHight);
            foreach (BgElementCommonXaml bgCommonXaml in T1.BgElementCommons)
                bgCommonXaml.Margin.Add(_margin);
            foreach (BgTextBlock bgText in T1.BgTextBlocks)
                bgText.Margin.Add(_margin);
            this.BgElementCommons.AddRange(T1.BgElementCommons);
            this.BgTextBlocks.AddRange(T1.BgTextBlocks);
            return this;

        }

        public ElementXaml Add(ElementXaml T1, ElementXaml T2)
        {
            Margin _margin = new Margin();
            BgTextBlock bgtextblock = T1.BgTextBlocks.Last();
            _margin = _margin.MarginChange(bgtextblock.Margin, bgtextblock.iColWidth, bgtextblock.iColHight);
            foreach (BgElementCommonXaml bgCommonXaml in T2.BgElementCommons)
                bgCommonXaml.Margin.Add(_margin);
            foreach (BgTextBlock bgText in T2.BgTextBlocks)
                bgText.Margin.Add(_margin);
            this.BgElementCommons.AddRange(T2.BgElementCommons);
            this.BgTextBlocks.AddRange(T2.BgTextBlocks);
            return T1;
        }


        public ElementXaml Div(ElementXaml T1)
        {
            throw new NotImplementedException();
        }
        public ElementXaml Div(ElementXaml T1, ElementXaml T2)
        {
            throw new NotImplementedException();
        }
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public ElementXaml Multiply(ElementXaml T1)
        {
            throw new NotImplementedException();
        }
        public ElementXaml Multiply(ElementXaml T1, ElementXaml T2)
        {
            throw new NotImplementedException();
        }
        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public ElementXaml Subtract(ElementXaml T1)
        {
            throw new NotImplementedException();
        }

        public ElementXaml Subtract(ElementSearchXml T1)
        {
            throw new NotImplementedException();
        }

        public ElementXaml Subtract(ElementXaml T1, ElementXaml T2)
        {
            throw new NotImplementedException();
        }
        public void WriteXml(XmlWriter writer)
        {
            //Namespace.
            writer.WriteAttributeString("xmlns", "WIMAP_Controls", null, "clr-namespace:WIMAP.Common.Controls;assembly=WIMAP.Common.SL");
            writer.WriteAttributeString("xmlns", "", null, "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            writer.WriteAttributeString("xmlns", "x", null, "http://schemas.microsoft.com/winfx/2006/xaml");
            writer.WriteAttributeString("xmlns", "d", null, "http://schemas.microsoft.com/winfx/2006/xaml");
            writer.WriteAttributeString("xmlns", "mc", null, "http://schemas.openxmlformats.org/markup-compatibility/2006");
            writer.WriteAttributeString("xmlns", "i", null, "http://schemas.microsoft.com/expression/2010/interactivity");
            writer.WriteAttributeString("xmlns", "BG_SCADA_Behaviors", null, "clr-namespace:BG_SCADA.Behaviors");
            writer.WriteAttributeString("xmlns", "BG_SCADA_Controls", null, "clr -namespace:BG_SCADA.Controls");
            writer.WriteAttributeString("xmlns", "ScadaBase_Controls", null, "clr-namespace:ScadaBase.Controls;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "bgElementCommon", null, "clr-namespace:ScadaBase.Controls.BGElement;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "counter", null, "clr-namespace:ScadaBase.Controls.Alarm;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "command", null, "clr-namespace:ScadaBase.Controls.Command;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "display", null, "clr-namespace:ScadaBase.Controls.Display;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "ScadaBase_Controls_Alarm", null, "clr-namespace:ScadaBase.Controls.Alarm;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "BG_SCADA", null, "clr-namespace:ScadaBase.Controls.Alarm;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "ScadaBase_Behaviors", null, "clr-namespace:ScadaBase.Behaviors;assembly=ScadaBase.SL");
            writer.WriteAttributeString("xmlns", "BG_SCADA_Behaviors_Conveyor", null, "clr-namespace:BG_SCADA.Behaviors.Conveyor");
            writer.WriteAttributeString("xmlns", "helper", null, "clr-namespace:WIMAP.Common.Helper;assembly=WIMAP.Common.SL");
            writer.WriteAttributeString("xmlns", "Class", null, _className);

            //Workbook Property Setup
            //"ViewBox"
            writer.WriteStartElement("Viewbox");
            writer.WriteAttributeString("Margin", "0");
            //写Grid
            writer.WriteStartElement("Grid");
            writer.WriteAttributeString("x", "Name", null, "LayoutRoot");
            writer.WriteAttributeString("Background", "#00000000");
            writer.WriteAttributeString("Height", "1080");
            writer.WriteAttributeString("Width", "1920");
            //写bgElement
            foreach (BgElementCommonXaml bgElementCommonXaml in BgElementCommons)
            {
                writer.WriteStartElement("bgElementCommon", "BGElementCommon", null);
                writer.WriteAttributeString("x", "Name", null, bgElementCommonXaml.Name);
                writer.WriteAttributeString("Margin", bgElementCommonXaml.Margin.ToString());
                writer.WriteAttributeString("VerticalAlignment", bgElementCommonXaml.VerticalAlignment);
                writer.WriteAttributeString("HorizontalAlignment", bgElementCommonXaml.HorizontalAlignment);
                writer.WriteAttributeString("Style", bgElementCommonXaml.Style.ToString());
                writer.WriteAttributeString("LegendStyleName", bgElementCommonXaml.LegendStyleName);
                writer.WriteAttributeString("Width", bgElementCommonXaml.Width);
                writer.WriteAttributeString("Height", bgElementCommonXaml.Height);
                writer.WriteAttributeString("ElementName", bgElementCommonXaml.ElementName);
                writer.WriteAttributeString("DisplayName", bgElementCommonXaml.DisplayName);
                if (bgElementCommonXaml.Commands != null)
                    writer.WriteAttributeString("Commands", bgElementCommonXaml.Commands);
                writer.WriteAttributeString("ScadaLevel", bgElementCommonXaml.ScadaLevel);
                writer.WriteAttributeString("ControlObject", bgElementCommonXaml.ControlObject);
                writer.WriteAttributeString("PowerBox", bgElementCommonXaml.PowerBox);
                writer.WriteAttributeString("Level3View", bgElementCommonXaml.Level3View);
                writer.WriteAttributeString("ChooseLeftClickMode", bgElementCommonXaml.ChooseLeftClickMode);
                writer.WriteAttributeString("NavigateToView", bgElementCommonXaml.NavigateToView);
                writer.WriteAttributeString("ElementType", bgElementCommonXaml.ElementType);
                writer.WriteAttributeString("PLCName", bgElementCommonXaml.PlcName);
                writer.WriteAttributeString("CommandSignalName", bgElementCommonXaml.CommandSignalName);
                writer.WriteAttributeString("CommandMappingType", bgElementCommonXaml.CommandMappingType);
                writer.WriteAttributeString("TypeDescription", bgElementCommonXaml.TypeDescription);
                if (bgElementCommonXaml.HasRightclickMenu != null)
                    writer.WriteAttributeString("HasRightclickMenu", bgElementCommonXaml.HasRightclickMenu);

                //写Signal属性
                writer.WriteStartElement("bgElementCommon", "BGElementCommon.Signals", null);
                foreach (ElementXamlSignal signal in bgElementCommonXaml.ElementXamlSignal)
                {
                    writer.WriteStartElement("helper", "BGSignal", null);
                    writer.WriteAttributeString("Id", signal.Id);
                    writer.WriteAttributeString("UsePostfix", signal.UsePostfix);
                    writer.WriteAttributeString("Postfix", signal.Postfix);
                    writer.WriteAttributeString("UsePrefix", signal.UsePrefix);
                    writer.WriteAttributeString("Prefix", signal.Prefix);
                    writer.WriteAttributeString("KeepAliveType", signal.KeepAliveType);
                    writer.WriteAttributeString("KeepAliveSignal", signal.KeepAliveSignal);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                //写Background属性
                writer.WriteStartElement("bgElementCommon", "BGElementCommon.Background", null);
                writer.WriteStartElement("SolidColorBrush");
                writer.WriteAttributeString("Color", bgElementCommonXaml.ElementXamlBackground.Color);
                writer.WriteEndElement();
                writer.WriteEndElement();

                //写Behavior属性
                writer.WriteStartElement("i", "Interaction.Behaviors", null);
                if (bgElementCommonXaml.ElementXamlBehavior.behaviorName == "BGLineBehavior")
                    writer.WriteStartElement("ScadaBase_Behaviors", bgElementCommonXaml.ElementXamlBehavior.behaviorName, null);
                else
                    writer.WriteStartElement("BG_SCADA_Behaviors_Conveyor", bgElementCommonXaml.ElementXamlBehavior.behaviorName, null);
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            foreach (BgTextBlock bgTextBlock in BgTextBlocks)
            {
                writer.WriteStartElement("TextBlock");
                writer.WriteAttributeString("x", "Name", null, bgTextBlock.Name);
                writer.WriteAttributeString("Text", bgTextBlock.Text);
                writer.WriteAttributeString("Height", bgTextBlock.Height);
                writer.WriteAttributeString("FontSize", bgTextBlock.FontSize);
                writer.WriteAttributeString("Width", bgTextBlock.Width);
                writer.WriteAttributeString("Margin", bgTextBlock.Margin.ToString());
                writer.WriteAttributeString("VerticalAlignment", bgTextBlock.VerticalAlignment);
                writer.WriteAttributeString("HorizontalAlignment", bgTextBlock.HorizontalAlignment);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            throw new NotImplementedException();
        }
    }
    public class GridXaml
    {
        private string _name;
        private string _background;
        private string _height;
        private string _width;
    }
    public class BgXamlBase
    {
        public string Name;
        public string Width;
        public string Height;
        public Margin Margin;
        public int iColWidth;
        public int iColHight;
        public string VerticalAlignment;
        public string HorizontalAlignment;
        public BgXamlBase()
        {
            Width = "40";
            Height = "20";
            iColWidth = 70;
            iColHight = 40;
            HorizontalAlignment = "Left";
            VerticalAlignment = "Top";
        }
    }
    public struct Margin
    {
        public int A;
        public int B;
        public int C;
        public int D;
        private string _margin;

        public override string ToString()
        {
            _margin = string.Format("{0},{1},{2},{3}", A, B, C, D);
            return _margin;
        }
        public Margin Add(Margin T1)
        {
            this.A = this.A + T1.A;
            this.B = this.A + T1.B;
            this.C = this.A + T1.C;
            this.D = this.A + T1.D;
            return this;
        }
        public Margin MarginChange(Margin margin, int iColWidth, int iRowHight)
        {
            const int viewWidth = 1920;
            //int iColWidth = 70;
            //int iRowHight = 70;
            if (margin.A + iColWidth > viewWidth)
            {
                margin.A = 0;
                margin.B = margin.B + iRowHight;
            }
            else
            {
                margin.A = margin.A + iColWidth;
            }
            return margin;
        }
    }
    public class BgTextBlock : BgXamlBase
    {
        public string Text;
        public string FontSize;
        public BgTextBlock()
        {
            Width = "65";
            Height = "16";
            FontSize = "11";
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
    public class BgElementCommonXaml : BgXamlBase
    {
        private ConveyorRow _conveyor;
        public string Style;
        public string LegendStyleName;
        public string ElementName;
        public string DisplayName;
        public string Commands;
        public string ScadaLevel;
        public string ControlObject;
        public string PowerBox;
        public string Level3View;
        public string ChooseLeftClickMode;
        public string NavigateToView;
        public string ElementType;
        public string PlcName;
        public string CommandSignalName;
        public string CommandMappingType;
        public string TypeDescription;
        public string HasRightclickMenu;
        public List<ElementXamlSignal> ElementXamlSignal = new List<ElementXamlSignal>();
        public ElementXamlBackground ElementXamlBackground = new ElementXamlBackground();
        public ElementXamlBehavior ElementXamlBehavior = new ElementXamlBehavior();
        public BgElementCommonXaml()
        {
            Style = "BG_DefaultSymbol";
            LegendStyleName = "BG_DefaultSymbol";
            ScadaLevel = "Level2";
            Level3View = "Level3ViewConv_47";
            ChooseLeftClickMode = "NoAction";
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class ElementXamlSignal
    {
        public string Id;
        public string UsePostfix;
        public string Postfix;
        public string UsePrefix;
        public string Prefix;
        public string KeepAliveType;
        public string KeepAliveSignal;

    }
    public class ElementXamlBackground
    {
        public string Color = "";
    }
    public class ElementXamlBehavior
    {
        public string behaviorName = "";
    }
}
