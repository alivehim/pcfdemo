using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.OpenXml.DocxTemplate.Models;
using UtilityTools.OpenXml.DocxTemplate.Processes;
using UtilityTools.OpenXml.Extensions;

namespace UtilityTools.OpenXml.DocxTemplate.Parsers
{
    internal class TableParser : BaseTagParser, ITagParser
    {
        public TableParser(XElement startElement) : base(startElement)
        {
        }

        public XElement FindEndTag()
        {
            var nextElements = StartElement.GetNextTagElements(SdtTag.EndTable);

            if (!nextElements.Any())
            {
                throw new Exception("table end tag can not be matched");
            }
            return nextElements.First();
        }

        public XElement ParseSdt(ITagProcessor parentProcessor)
        {
            var endTag = FindEndTag();


           

            var tableElement = StartElement.GetTableElement(endTag);

            //var rowElements =  StartElement.GetInnerElements(endTag)
            //    .Descendants(NamespaceConsts.TableRowName).Where(p=>p.Parent==tableElement);

            var rowElements = tableElement
               .Descendants(NamespaceConsts.TableRowName).Where(p => p.Parent == tableElement);

            int dynamicRows = 0;
            foreach(var rowElement in rowElements)
            {
                //find dynamic row

                if(rowElement.Descendants().Any(p=> p.IsTextItem()))
                {
                    dynamicRows++;
                }
            }


            var process = new TableProcessor(StartElement, endTag, tableElement, dynamicRows);

            DrillDown(process, StartElement, endTag);

            parentProcessor.AddProcessor(process);
            return endTag;
        }
    }
}
