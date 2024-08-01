using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UtilityTools.OpenXml.DocxTemplate.Processes
{
    internal class DocumentProcessor : BaseProcess, ITagProcessor
    {
        public DocumentProcessor() : base(null, null)
        {

        }

        public DocumentProcessor(XElement startElement, XElement endElement) : base(startElement, endElement)
        {
        }

        public void Process(DataReader dataReader)
        {
            SetDataReader(dataReader);
            base.Process();
        }
    }
}
