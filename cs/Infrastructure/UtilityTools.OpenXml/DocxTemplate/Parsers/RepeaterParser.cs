using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.OpenXml.DocxTemplate.Processes;
using UtilityTools.OpenXml.Extensions;

namespace UtilityTools.OpenXml.DocxTemplate.Parsers
{
    internal class RepeaterParser : BaseTagParser, ITagParser
    {

        public RepeaterParser(XElement startElement) : base(startElement)
        {

        }

        public XElement ParseSdt(ITagProcessor parentProcessor)
        {

            var endTag = FindEndTag();

            var process = new RepeaterProcessor(StartElement, endTag);

            DrillDown(process, StartElement, endTag);

            parentProcessor.AddProcessor(process);
            return endTag;
        }

        public XElement FindEndTag()
        {
            var nextElements = StartElement.GetNextTagElements(SdtTag.EndRepeater);

            if (!nextElements.Any())
            {
                throw new Exception("repeater end tag can not be matched");
            }
            return nextElements.First();

        }

    }
}
