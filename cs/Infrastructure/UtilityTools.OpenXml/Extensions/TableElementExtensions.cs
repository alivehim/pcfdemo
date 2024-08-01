using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using UtilityTools.OpenXml.DocxTemplate;
using UtilityTools.OpenXml.DocxTemplate.Models;

namespace UtilityTools.OpenXml.Extensions
{
    internal static class TableElementExtensions
    {
        public static XElement GetTableElement(this XElement xElement, XElement endElement)
        {
            return xElement.GetInnerElements(endElement).First(p => p.Name == NamespaceConsts.TableName);
        }

        public static IList<XElement> GetRows(this XElement tableElement)
        {
            return tableElement.Elements(NamespaceConsts.TableRowName).ToList();

        }

        public static XElement GetDynamicRow(this XElement tableElement,int index)
        {
            return tableElement.GetRows().ElementAt(index);
        }

        public static bool IsTextItem(this TableInnerElement  tableInnerElement)
        {
            return tableInnerElement.Element.IsTextItem();
        }
        public static string GetExpression(this TableInnerElement tableInnerElement)
        {
            return tableInnerElement.Element.GetExpression();
        }

    }
}
