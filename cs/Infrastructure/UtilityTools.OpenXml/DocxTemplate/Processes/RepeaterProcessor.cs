using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.OpenXml.DocxTemplate.Core;
using UtilityTools.OpenXml.DocxTemplate.Models;
using UtilityTools.OpenXml.Extensions;

namespace UtilityTools.OpenXml.DocxTemplate.Processes
{
    internal class RepeaterProcessor : BaseProcess, ITagProcessor
    {
        public RepeaterProcessor(XElement startElement, XElement endElement) : base(startElement, endElement)
        {
        }

        public override void Process()
        {
            base.Process();

            var dataReaders = DataReader?.GetReaders(StartElement.GetExpression());

            if (dataReaders == null)
            {
                throw new Exception("can not get the reader data");
            }

            var taglist = new List<string> { "itemtext" };
            var elements = StartElement.GetInnerElements(EndElement);

            var repeatElements = GetRepeaterElements(elements);

            XElement current = StartElement;
            foreach (var dataReader in dataReaders)
            {

                current= ProcessElements(repeatElements, dataReader,null, current);

            }

            foreach (var element in repeatElements)
            {
                element.Remove();
            }



            ClearUpSelf();
        }

        private IList<RepeaterInnerElement> GetRepeaterElements(IList<XElement> xElements)
        {
            IList < RepeaterInnerElement > result = new List<RepeaterInnerElement>();
            foreach (var xElement in xElements)
            {
                var repeaterElement = new RepeaterInnerElement(xElement);

                repeaterElement.ChildElements = GetRepeaterElements(xElement.Elements().ToList());
                result.Add(repeaterElement);
            }


            return result;
        }

        private XElement ProcessElements(IList<RepeaterInnerElement> repeatElements, DataReader dataReader, XElement? parent, XElement startElement, bool nested = false)
        {
            var previous = startElement;

            XElement currentElement = null;
            foreach (var element in repeatElements)
            {

                if (element.IsRepeaterItem())
                {
                    var textValue = dataReader.ReadText(element.GetExpression());

                    currentElement =  ItemTextProcess(element, textValue);
                }
                else
                {
                    var newElement = new XElement(element.Element);
                    newElement.RemoveNodes();
                    currentElement = newElement;

                    if (element.HasChildElements)
                    {
                        ProcessElements(element.ChildElements, dataReader, newElement, previous, true);
                    }
                    else
                    {
                        newElement.Value = element.Element.Value;
                    }
                }

                if (currentElement != null)
                {
                    if (!nested)
                    {
                        previous.AddAfterSelf(currentElement);
                        previous = currentElement;
                    }
                    else
                    {
                        parent?.Add(currentElement);
                    }
                }

            }

            return currentElement ?? previous;

        }

        private XElement ItemTextProcess(RepeaterInnerElement xElement, string textValue)
        {
            //create a new paragraph before the xelement
            var newXElement = DocxHelper.CreateTextElement(xElement.Element, xElement.Element?.Parent, textValue);

            return newXElement;
        }
    }
}
