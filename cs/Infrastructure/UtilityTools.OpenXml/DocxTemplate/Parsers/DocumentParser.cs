using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.OpenXml.DocxTemplate.Processes;
using UtilityTools.OpenXml.Extensions;

namespace UtilityTools.OpenXml.DocxTemplate.Parsers
{
    internal class DocumentParser : BaseTagParser
    {

        public DocumentParser(XElement startElement):base(startElement)
        {
        }

        public void AddProcessor(ITagProcessor processor)
        {
        }

        public DocumentProcessor Parse()
        {
            var documentProcessor = new DocumentProcessor(StartElement,null);
            //get the first element with name std
            var sdtElement = StartElement.GetSdtElements().FirstOrDefault();
            while (sdtElement != null)
            {
                sdtElement = InteranlParseSdt(documentProcessor, sdtElement);
                sdtElement = sdtElement.GetNextTagElements().FirstOrDefault();
            }
            return documentProcessor;
        }

    }
}
