using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UtilityTools.OpenXml
{
    internal class ParagraphProperties
    {
        public static readonly XName ReferenceStyleName = NamespaceConsts.WordMlNamespace + "pStyle";

        public static readonly XName KeepNextName = NamespaceConsts.WordMlNamespace + "keepNext";

        public static readonly XName KeepLineName = NamespaceConsts.WordMlNamespace + "keepLines";

        public static readonly XName PageBreakName = NamespaceConsts.WordMlNamespace + "pageBreakBefore";

        public static readonly XName TextFrameName = NamespaceConsts.WordMlNamespace + "framePr";

        public static readonly XName WidowControlName = NamespaceConsts.WordMlNamespace + "widowControl";

        public static readonly XName NumberingDefinitionName = NamespaceConsts.WordMlNamespace + "numPr";

        public static readonly XName SuppressLineNumbersName = NamespaceConsts.WordMlNamespace + "suppressLineNumbers";

        public static readonly XName BordersName = NamespaceConsts.WordMlNamespace + "pBdr";

        public static readonly XName ShadingName = NamespaceConsts.WordMlNamespace + "shd";

        public static readonly XName CustomTabName = NamespaceConsts.WordMlNamespace + "tabs";

        public static readonly XName SuppressHyphenationName = NamespaceConsts.WordMlNamespace + "suppressAutoHyphens";

        public static readonly XName KinsokuName = NamespaceConsts.WordMlNamespace + "kinsoku";

        public static readonly XName WordWrapName = NamespaceConsts.WordMlNamespace + "wordWrap";

        public static readonly XName OverflowPunctuationName = NamespaceConsts.WordMlNamespace + "overflowPunct";

        public static readonly XName TopLinePunctuationName = NamespaceConsts.WordMlNamespace + "topLinePunct";

        public static readonly XName AutoSpaceDEName = NamespaceConsts.WordMlNamespace + "autoSpaceDE";

        public static readonly XName AutoSpaceDNName = NamespaceConsts.WordMlNamespace + "autoSpaceDN";

        public static readonly XName RightToLeftLayoutName = NamespaceConsts.WordMlNamespace + "bidi";

        public static readonly XName AdjuctRightIndentName = NamespaceConsts.WordMlNamespace + "adjustRightInd";

        public static readonly XName SnapToGridName = NamespaceConsts.WordMlNamespace + "snapToGrid";

        public static readonly XName SpacingBetweenLineName = NamespaceConsts.WordMlNamespace + "spacing";

        public static readonly XName IndentationName = NamespaceConsts.WordMlNamespace + "ind";

        public static readonly XName ContextualSpacingName = NamespaceConsts.WordMlNamespace + "contextualSpacing";

        public static readonly XName MirrorIndentsName = NamespaceConsts.WordMlNamespace + "mirrorIndents";

        public static readonly XName SuppressOverlapName = NamespaceConsts.WordMlNamespace + "suppressOverlap";

        public static readonly XName AlignmentName = NamespaceConsts.WordMlNamespace + "jc";

        public static readonly XName TextDirectionName = NamespaceConsts.WordMlNamespace + "textDirection";

        public static readonly XName TextAlignmentName = NamespaceConsts.WordMlNamespace + "textAlignment";

        public static readonly XName TextboxTightWrapName = NamespaceConsts.WordMlNamespace + "textboxTightWrap";

        public static readonly XName OutlineLevelName = NamespaceConsts.WordMlNamespace + "outlineLvl";

        public static readonly XName DivIdName = NamespaceConsts.WordMlNamespace + "divId";

        public static readonly XName ConditionalFormattingStyleName = NamespaceConsts.WordMlNamespace + "cnfStyle";

        public static readonly XName RunPropertiesName = NamespaceConsts.WordMlNamespace + "rPr";

        public static readonly XName SectionPropertiesName = NamespaceConsts.WordMlNamespace + "sectPr";

        public static readonly XName RevisionPropertiesInformationName = NamespaceConsts.WordMlNamespace + "pPrChange";
    }
}
