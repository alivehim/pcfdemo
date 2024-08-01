using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.OpenXml.DocxTemplate.Processes;
using UtilityTools.OpenXml.DocxTemplate.Tags;
using UtilityTools.OpenXml.Extensions;

namespace UtilityTools.OpenXml.DocxTemplate.Parsers
{
    internal class IfParser : BaseTagParser, ITagParser
    {
        public IfParser(XElement startElement) : base(startElement)
        {
        }

        public XElement ParseSdt(ITagProcessor parentProcessor)
        {

            var endifTag = FindEndTag();

            var expression = StartElement.GetExpression();

            var tag = new IfTag(StartElement, endifTag) { };

            var process = new IfProcessor(StartElement, endifTag) { };

            DrillDown(process, StartElement, endifTag);

            parentProcessor.AddProcessor(process);
            return endifTag;
        }

        public XElement FindEndTag()
        {
            var brothers = StartElement.GetNextTagElements();

            XElement? result = null;

            int count = 1;

            foreach (var element in brothers)
            {
                var tagName = element.GetTagName();

                if (tagName == "if")
                {
                    count++;
                }
                else if (tagName == "endif")
                {
                    count--;
                }

                if (count == 0)
                {
                    result = element;
                    break;
                }
            }

            if (result == null)
            {
                throw new Exception(" can not find matched endif tag");
            }

            return result;
        }
    }
}
