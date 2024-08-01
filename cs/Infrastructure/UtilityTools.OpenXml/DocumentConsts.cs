using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.OpenXml
{
    internal class DocumentConsts
    {
        public static readonly string DocumentElement = "document";

        public static readonly string BodyElement = "body";

        public  const   string ParagraphElement = "p";
        public const  string TableElement = "tbl";

        // Inline-Level elements
        public const string SimpleFieldElement = "fldSimple";
        public const string HyperlinkElement = "hyperlink";
        public const string RunElement = "r";

        public const string ParagraphPropertiesElement = "pPr";
        public const string RunPropertiesElement = "rPr";

        public const string BreakElement = "br";
        public const string TabCharacterElement = "tab";
        public const string TextElement = "t";

        // Table elements
        public const string TableRowElement = "tr";
        public const string TableCellElement = "tc";

        public const string TablePropertiesElement = "tblPr";

        public const string TableGridElement = "tblGrid";

        public const string TableGridColomnElemnt = "gridCol";

        public const string TableCellPropertiesElement = "tcPr";
    }
}
