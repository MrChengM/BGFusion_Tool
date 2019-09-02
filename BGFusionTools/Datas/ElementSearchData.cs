using BGFusionTools.Helper;
using BGFusionTools.Serialization;
using System;
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
    public class ElementSearchData : BaseData
    {
        public ElementSearchData() { }
        public ElementSearchData(BaseParameter ConvParameter)
        {
            base.baseParameter = ConvParameter;
        }
        public override List<T1> CreateList<T1>(CreateDataMath<T1, ConveyorRow> dataMath)
        {
            return base.CreateList<T1>(dataMath);
        }
        public List<ElementSeacrhStruct> CreateElements(ConveyorRow conveyor)
        {
            List<ElementSeacrhStruct> element = new List<ElementSeacrhStruct>();
            if (conveyor.sDrawOnViews.ToLower() == "all" || conveyor.sDrawOnViews.ToLower() == "level2only")
                element.Add(new ElementSeacrhStruct(conveyor.sDisplayName, conveyor.sLevel1View, conveyor.sLevel2View, conveyor.sElementName));
            return element;
        }
  
    }


}
