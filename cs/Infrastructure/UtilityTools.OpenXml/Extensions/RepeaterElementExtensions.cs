using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.OpenXml.DocxTemplate.Models;

namespace UtilityTools.OpenXml.Extensions
{
    internal static class RepeaterElementExtensions
    {
        public static bool IsRepeaterItem(this RepeaterInnerElement repeaterElement)
        {
            return repeaterElement.Element.IsTextItem();
        }
        public static string GetExpression(this RepeaterInnerElement repeaterElement)
        {
            return repeaterElement.Element.GetExpression();
        }

        public static void Remove(this RepeaterInnerElement repeaterElement)
        {
            repeaterElement.Element.Remove();
        }
    }
}
