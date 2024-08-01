using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.OpenXml
{
    internal class StyleConsts
    {
        public const string

           // Run properties elements
           BoldElement = "b",
           ItalicElement = "i",
           UnderlineElement = "u",
           StrikeElement = "strike",
           DoubleStrikeElement = "dstrike",
           VerticalAlignmentElement = "vertAlign",
           ColorElement = "color",
           HighlightElement = "highlight",
           FontElement = "rFonts",
           FontSizeElement = "sz",
           RightToLeftTextElement = "rtl",

           // Paragraph properties elements
           AlignmentElement = "jc",
           PageBreakBeforeElement = "pageBreakBefore",
           SpacingElement = "spacing",
           IndentationElement = "ind",
           ShadingElement = "shd",

           // Attributes
           IdAttribute = "id",
           ValueAttribute = "val",
           ColorAttribute = "color",
           AsciiFontFamily = "ascii",
           SpacingAfterAttribute = "after",
           SpacingBeforeAttribute = "before",
           LeftIndentationAttribute = "left",
           RightIndentationAttribute = "right",
           HangingIndentationAttribute = "hanging",
           FirstLineIndentationAttribute = "firstLine",
           FillAttribute = "fill";
    }
}
