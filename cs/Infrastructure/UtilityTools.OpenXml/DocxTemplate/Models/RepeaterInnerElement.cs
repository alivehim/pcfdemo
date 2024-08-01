using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UtilityTools.OpenXml.DocxTemplate.Models
{
    internal class RepeaterInnerElement
    {
        public XElement Element { get; set; }

        public bool HasChildElements => ChildElements!=null && ChildElements.Count>0;

        public IList<RepeaterInnerElement> ChildElements { get; set; }

        public RepeaterInnerElement(XElement element)
        {
            Element = element;
        }

       
    }
}
