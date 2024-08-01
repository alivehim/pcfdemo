using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.OpenXml.DocxTemplate.Processes;

namespace UtilityTools.OpenXml.DocxTemplate.Parsers
{
    internal interface ITagParser 
    {
        XElement StartElement { set; get; }

        XElement ParseSdt(ITagProcessor parentProcessor);

        XElement FindEndTag();
    }
}
