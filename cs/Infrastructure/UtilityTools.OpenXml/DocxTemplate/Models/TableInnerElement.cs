using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UtilityTools.OpenXml.DocxTemplate.Models
{
    internal class TableInnerElement
    {
        public bool HasChildElements => ChildElements != null && ChildElements.Count > 0;

        public IList<TableInnerElement> ChildElements { get; set; }
        public XElement Element { get; set; }

        public TableInnerElement(XElement element)
        {
            Element = element;
        }

      

    }
}
