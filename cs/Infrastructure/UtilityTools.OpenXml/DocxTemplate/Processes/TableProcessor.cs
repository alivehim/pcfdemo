using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.OpenXml.DocxTemplate.Core;
using UtilityTools.OpenXml.DocxTemplate.Models;
using UtilityTools.OpenXml.Extensions;

namespace UtilityTools.OpenXml.DocxTemplate.Processes
{
    internal class TableProcessor : BaseProcess, ITagProcessor
    {
        private XElement TableElement { get; set; }
        private int DynamicRows { get; set; }
        public TableProcessor(XElement startElement, XElement endElement, XElement tableElement, int dynamicRows) : base(startElement, endElement)
        {
            TableElement = tableElement;
            DynamicRows = dynamicRows;
        }

        public override void Process()
        {
            base.Process();

            var dataReaders = DataReader?.GetReaders(StartElement.GetExpression());

            if (dataReaders == null)
            {
                throw new Exception("can not get the reader data");
            }

            var dynamicRow = TableElement.GetDynamicRow(DynamicRows - 1);
            //var elements = StartElement.GetInnerElements(EndElement);


            var repeaterElements = GetRepeaterElements(dynamicRow);
            XElement current = dynamicRow;
            foreach (var dataReader in dataReaders)
            {

                current = ProcessElements(repeaterElements, dataReader, null, current);

            }

            dynamicRow.Remove();

            ClearUpSelf();
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

                    currentElement = ItemTextProcess(element, textValue);
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

        //private XElement ProcessElements(XElement rowElements, DataReader dataReader)
        //{
        //    //create a new row
        //    var currentRow = new XElement(rowElements);

        //    var allDynamicElements = GetTableInnerElement(currentRow);

        //    foreach (var dynamicElement in allDynamicElements)
        //    {


        //    }

        //    rowElements.AddBeforeSelf(currentRow);


        //    return currentRow;
        //}

        //private XElement ProcessElements(TableInnerElement tableInnerElement, DataReader dataReader)
        //{

        //    if (tableInnerElement.IsTextItem())
        //    {

        //    }
        //}

        private XElement ItemTextProcess(RepeaterInnerElement xElement, string textValue)
        {
            //create a new paragraph before the xelement
            var newXElement = DocxHelper.CreateTextElement(xElement.Element, xElement.Element?.Parent, textValue);

            return newXElement;
        }

        private IList<RepeaterInnerElement> GetRepeaterElements(XElement rowElements)
        {
            IList<RepeaterInnerElement> result = new List<RepeaterInnerElement>();
            var repeaterElement = new RepeaterInnerElement(rowElements);

            repeaterElement.ChildElements = GetRepeaterElements(rowElements.Elements().ToList());
            result.Add(repeaterElement);

            return result;
        }

        private IList<RepeaterInnerElement> GetRepeaterElements(IList<XElement> xElements)
        {
            IList<RepeaterInnerElement> result = new List<RepeaterInnerElement>();
            foreach (var xElement in xElements)
            {
                var repeaterElement = new RepeaterInnerElement(xElement);

                repeaterElement.ChildElements = GetRepeaterElements(xElement.Elements().ToList());
                result.Add(repeaterElement);
            }


            return result;
        }

        /// <summary>
        /// get all the elements with tag under the specific element
        /// </summary>
        /// <param name="rowElements"></param>
        /// <returns></returns>
        private IList<TableInnerElement> GetTableInnerElement(XElement rowElements)
        {
            IList<TableInnerElement> result = new List<TableInnerElement>();

            var elements = rowElements.Descendants(NamespaceConsts.SdtName);

            foreach (var element in elements)
            {
                var newElement = new TableInnerElement(element);
            }
            return result;
        }
    }
}
