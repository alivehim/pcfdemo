using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.OpenXml.Extensions;

namespace UtilityTools.OpenXml.DocxTemplate.Processes
{
    internal abstract class BaseProcess : ITagProcessor
    {
        public XElement StartElement { get; set; }
        public XElement EndElement { get; set; }
        private IList<ITagProcessor> Processes { get; set; } = new List<ITagProcessor>();
        public virtual DataReader? DataReader { get; set; }


        protected BaseProcess(XElement startElement, XElement endElement)
        {
            StartElement = startElement;
            EndElement = endElement;
        }



     

        public void SetDataReader(DataReader  dataReader)
        {
            DataReader = dataReader;
        }
        public void AddProcessor(ITagProcessor processor)
        {
            Processes.Add(processor);
        }

        public virtual void Process()
        {
            if(DataReader == null)
            {
                throw new Exception(" data reader is null");
            }

            foreach (ITagProcessor processor in Processes)
            {
                processor.SetDataReader(DataReader);
                processor.Process();
            }
        }

        protected void CleanUp()
        {
            StartElement.GetInnerElements(EndElement).Remove();
        }


        protected void ClearUpSelf()
        {
            StartElement?.Remove();
            EndElement?.Remove();
        }
    }
}
