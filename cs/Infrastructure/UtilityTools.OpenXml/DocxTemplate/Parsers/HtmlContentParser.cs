using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.OpenXml.DocxTemplate.Processes;

namespace UtilityTools.OpenXml.DocxTemplate.Parsers
{
    internal class HtmlContentParser : BaseTagParser, ITagParser
    {
        public HtmlContentParser(XElement startElement) : base(startElement)
        {
        }

        public XElement FindEndTag()
        {
            throw new NotImplementedException();
        }

        public XElement ParseSdt(ITagProcessor parentProcessor)
        {
            throw new NotImplementedException();
        }
    }
}
