using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UtilityTools.OpenXml.DocxTemplate.Tags
{
    internal class TagBase
    {
        public XElement StartElement { get; set; }

        public XElement EndElement { get; set; }

        public TagBase(XElement StartElement, XElement EndElement)
        {
            this.StartElement = StartElement;
            this.EndElement = EndElement;
        }

    }
}
