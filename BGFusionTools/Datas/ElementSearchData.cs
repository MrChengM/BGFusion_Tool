<<<<<<< HEAD
﻿using BGFusionTools.Helper;
using BGFusionTools.Serialization;
using System;
=======
﻿using System;
>>>>>>> parent of 24775a4... V2.03
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BGFusionTools.Datas
{
<<<<<<< HEAD
    public class ElementSearchData : BaseData
=======
    [XmlRoot("elements")]
    public class ElementSearchData : BaseData,IXmlSerializable
>>>>>>> parent of 24775a4... V2.03
    {
        public ElementSearchData() { }
        public ElementSearchData(BaseParameter ConvParameter)
        {
            base.baseParameter = ConvParameter;
        }
        public override List<T1> CreateList<T1>(CreateDataRow<T1, ConveyorRow> dataMath)
        {
<<<<<<< HEAD
            return base.CreateList<T1>(dataMath);
        }
        public List<ElementSeacrhStruct> CreateElements(ConveyorRow conveyor)
        {
            List<ElementSeacrhStruct> element = new List<ElementSeacrhStruct>();
            if (conveyor.sDrawOnViews.ToLower() == "all" || conveyor.sDrawOnViews.ToLower() == "level2only")
                element.Add(new ElementSeacrhStruct(conveyor.sDisplayName, conveyor.sLevel1View, conveyor.sLevel2View, conveyor.sElementName));
            return element;
        }
  
=======
            try
            {
                foreach (DataRow selectConRow in baseParameter.TaglistTable.Rows)
                {

                    if (selectConRow[baseParameter.TaglistColName[1, 0]].ToString() == "")
                        break;
                    string sSystem = selectConRow[baseParameter.TaglistColName[1, 0]].ToString();
                    string sPlcLink = selectConRow[baseParameter.TaglistColName[1, 1]].ToString();
                    string sEquipmentLine = selectConRow[baseParameter.TaglistColName[1, 3]].ToString();
                    string sEquipmentElement = selectConRow[baseParameter.TaglistColName[1, 4]].ToString();
                    string sElementName = string.Format("{0}_{1}_{2}_{3}", sSystem, sPlcLink, sEquipmentLine, sEquipmentElement);
                    string sDisplayName = selectConRow[baseParameter.TaglistColName[1, 12]].ToString();
                    string sLevel1View = selectConRow[baseParameter.TaglistColName[1, 15]].ToString();
                    string sLevel2View = selectConRow[baseParameter.TaglistColName[1, 16]].ToString();
                    string sDrawViews = selectConRow[baseParameter.TaglistColName[1, 17]].ToString();
                    if (sDrawViews == "All" || sDrawViews == "Level2Only")
                        elements.Add(new Element(sDisplayName, sLevel1View, sLevel2View, sElementName));
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("数据处理错误" + e.Message);
            }

            return Functions.DataConvert.ToString(elements);
        }
        public override void OutData()
        {
            throw new NotImplementedException();
        }

        public override DataTable ToDataTable()
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, string> ToDictionary()
        {
            throw new NotImplementedException();
        }

        public override int ToInt()
        {
            throw new NotImplementedException();
        }

        public override List<List<string>> ToList()
        {
            throw new NotImplementedException();
        }

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
                    Element element = new Element();
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
            foreach(Element et in elements)
            {
                writer.WriteStartElement("element");
                writer.WriteAttributeString("displayname", et.DisplayName);
                writer.WriteAttributeString("level1", et.Level1);
                writer.WriteAttributeString("level2", et.Level2);
                writer.WriteAttributeString("name", et.Name);
                writer.WriteEndElement();
            }
        }
        public static ElementSearchData operator +(ElementSearchData a, ElementSearchData b)
        {
            ElementSearchData c = new ElementSearchData();
            //b.elements.RemoveAt(0);
            a.elements.AddRange(b.elements);
            c.elements.AddRange(a.elements);
            return c;
        }
>>>>>>> parent of 24775a4... V2.03
    }


}
