using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UtilityTools.OpenXml
{
    internal class TableCellProperties
    {
        public static readonly XName BorderCellName = NamespaceConsts.WordMlNamespace + "tcBorders";

        public static readonly XName ConditionFormattingCellName = NamespaceConsts.WordMlNamespace + "cnfStyle";

        public static readonly XName DeletionCellName = NamespaceConsts.WordMlNamespace + "cellDel";

        public static readonly XName FitTextCellName = NamespaceConsts.WordMlNamespace + "tcFitText";

        public static readonly XName GridSpanCellName = NamespaceConsts.WordMlNamespace + "gridSpan";

        public static readonly XName HorizontallyMergedCellName = NamespaceConsts.WordMlNamespace + "hMerge";

        public static readonly XName IgnoreEndMarkerCellName = NamespaceConsts.WordMlNamespace + "hideMark";

        public static readonly XName InsertionCellName = NamespaceConsts.WordMlNamespace + "cellIns";

        public static readonly XName NoWrapCellName = NamespaceConsts.WordMlNamespace + "noWrap";

        public static readonly XName PreferredWidthCellName = NamespaceConsts.WordMlNamespace + "tcW";

        public static readonly XName RevisionPropertiesInformationCellName = NamespaceConsts.WordMlNamespace + "tcPrChange";

        public static readonly XName ShadingCellName = NamespaceConsts.WordMlNamespace + "shd";

        public static readonly XName SingleMarginsCellName = NamespaceConsts.WordMlNamespace + "tcMar";

        public static readonly XName TextFlowDirectionCellName = NamespaceConsts.WordMlNamespace + "textDirection";

        public static readonly XName VerticalAlignmentCellName = NamespaceConsts.WordMlNamespace + "vAlign";

        public static readonly XName VerticallyMergedCellName = NamespaceConsts.WordMlNamespace + "vMerge";

        public static readonly XName VerticallyMergedSplitCellName = NamespaceConsts.WordMlNamespace + "cellMerge";
    }
}
