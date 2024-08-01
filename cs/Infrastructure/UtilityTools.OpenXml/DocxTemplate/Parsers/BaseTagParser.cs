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
    internal class BaseTagParser
    {
        public BaseTagParser(XElement startElement)
        {
            StartElement = startElement;
        }

        public XElement StartElement {  set; get; }

        protected XElement InteranlParseSdt(ITagProcessor tagProcessor, XElement sdtElement)
        {
            ITagParser? parser = null;
            switch (sdtElement.GetTagName().ToLower())
            {
                case "htmlcontent":
                    parser = new HtmlContentParser(sdtElement);
                    break;

                case "text":
                    parser = new TextParser(sdtElement);
                    break;

                case "table":
                    parser = new TableParser(sdtElement);
                    break;

                case "repeater":
                    parser = new RepeaterParser(sdtElement);
                    break;

                case "if":
                    parser = new IfParser(sdtElement);
                    break;  
            }
            return parser != null ? parser.ParseSdt(tagProcessor ) : sdtElement;
        }

        protected void DrillDown(ITagProcessor tagProcessor, XElement startelement, XElement endElement)
        {
            var tagList = new List<string> { "if", "repeater","table" };
            var innerElements = StartElement.GetInnerTagElements(endElement, tagList);


            foreach (var innerElement in innerElements)
            {
                InteranlParseSdt(tagProcessor, innerElement);
            }
        }
    }
}
