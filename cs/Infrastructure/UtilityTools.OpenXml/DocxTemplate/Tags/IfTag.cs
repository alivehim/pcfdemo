using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UtilityTools.OpenXml.DocxTemplate.Tags
{
    internal class IfTag : TagBase
    {
        public IfTag(XElement StartElement, XElement EndElement) : base(StartElement, EndElement)
        {
        }
    }
}
