using BGFusionTools.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BGFusionTools.Serialization
{
    [XmlRoot("elements")]
    public class ElementSearchXml : IXmlSerializable, IOperation<ElementSearchXml>
    {
        private List<ElementSeacrhStruct> elements = new List<ElementSeacrhStruct>();
        public List<ElementSeacrhStruct> Elements { get { return elements; } set { elements = value; } }
        public ElementSearchXml() { }
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            elements.Clear();
            while (reader.Read())
            {
                if (reader.IsStartElement("element"))
                {
                    ElementSeacrhStruct element = new ElementSeacrhStruct();
                    element.DisplayName = reader["displayname"];
                    element.Level1 = reader["level1"];
                    element.Level2 = reader["level2"];
                    element.Name = reader["name"];
                    elements.Add(element);
                }
            }
        }
        public void WriteXml(XmlWriter writer)
        {
            foreach (ElementSeacrhStruct et in elements)
            {
                writer.WriteStartElement("element");
                writer.WriteAttributeString("displayname", et.DisplayName);
                writer.WriteAttributeString("level1", et.Level1);
                writer.WriteAttributeString("level2", et.Level2);
                writer.WriteAttributeString("name", et.Name);
                writer.WriteEndElement();
            }
        }
        public ElementSearchXml Add(ElementSearchXml T1, ElementSearchXml T2)
        {
            T1.elements.AddRange(T2.elements);
            return T1;
        }

        public ElementSearchXml Subtract(ElementSearchXml T1, ElementSearchXml T2)
        {
            throw new NotImplementedException();
        }

        public ElementSearchXml Multiply(ElementSearchXml T1, ElementSearchXml T2)
        {
            throw new NotImplementedException();
        }

        public ElementSearchXml Div(ElementSearchXml T1, ElementSearchXml T2)
        {
            throw new NotImplementedException();
        }

        public ElementSearchXml Add(ElementSearchXml T1)
        {
            elements.AddRange(T1.elements);
            return this;
        }

        public ElementSearchXml Subtract(ElementSearchXml T1)
        {
            throw new NotImplementedException();
        }

        public ElementSearchXml Multiply(ElementSearchXml T1)
        {
            throw new NotImplementedException();
        }

        public ElementSearchXml Div(ElementSearchXml T1)
        {
            throw new NotImplementedException();
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            throw new NotImplementedException();
        }
    }

    public class ElementSeacrhStruct
    {
        private string _displayName;
        private string _level1;
        private string _level2;
        private string _name;
        public string DisplayName { get { return _displayName; } set { _displayName = value; } }
        public string Level1 { get { return _level1; } set { _level1 = value; } }
        public string Level2 { get { return _level2; } set { _level2 = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public ElementSeacrhStruct() { }
        public ElementSeacrhStruct(string displayname, string level1, string level2, string name)
        {
            _displayName = displayname;
            _level1 = level1;
            _level2 = level2;
            _name = name;
        }
    }
}
