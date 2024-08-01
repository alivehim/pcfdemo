using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.OpenXml.DocxTemplate.Tags;
using UtilityTools.OpenXml.Extensions;

namespace UtilityTools.OpenXml.DocxTemplate.Processes
{
    internal class IfProcessor : BaseProcess, ITagProcessor
    {
        public IfProcessor(XElement startElement, XElement endElement) : base(startElement, endElement)
        {
        }


        public override void Process()
        {
            base.Process();

            if (bool.TryParse(this.DataReader?.ReadText(StartElement.GetExpression()), out bool result))
            {
                if (!result)
                {
                    //delete the elements between start element and end element

                    CleanUp();
                }

                ClearUpSelf();
            }
        }
    }
}
