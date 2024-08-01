﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UtilityTools.OpenXml
{
    internal class NamespaceConsts
    {
        public const string DocumentRelationshipType =
    "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument";

        public const string StylesRelationshipType =
          "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles";
        public const string WordMlNamespaceString =
          "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        public static readonly XNamespace WordMlNamespace = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        public static readonly XName Style = WordMlNamespace + "style";
        public static readonly XName StyleId = WordMlNamespace + "styleId";
        public static readonly XName tblPr = WordMlNamespace + "tblPr";

        public static readonly XName BodyName = WordMlNamespace + "body";
        public static readonly XName IdName = WordMlNamespace + "id";
        public static readonly XName ParagraphName = WordMlNamespace + "p";
        public static readonly XName ParagraphPropertiesName = WordMlNamespace + "pPr";
        public static readonly XName SdtContentName = WordMlNamespace + "sdtContent";
        public static readonly XName SdtName = WordMlNamespace + "sdt";
        public static readonly XName SdtPrName = WordMlNamespace + "sdtPr";
        public static readonly XName TableName = WordMlNamespace + "tbl";
        public static readonly XName TableCellName = WordMlNamespace + "tc";
        public static readonly XName TableRowName = WordMlNamespace + "tr";
        public static readonly XName TableCellPropertiesName = WordMlNamespace + "tcPr";
        public static readonly XName TableCellWidthName = WordMlNamespace + "tcW";
        public static readonly XName TablePropertiesName = WordMlNamespace + "tblPr";
        public static readonly XName TableWidthName = WordMlNamespace + "tblW";
        public static readonly XName TableAlignmentName = WordMlNamespace + "jc";
        public static readonly XName TypeAttribute = WordMlNamespace + "type";
        public static readonly XName WidthAttributeName = WordMlNamespace + "w";
        public static readonly XName TagName = WordMlNamespace + "tag";
        public static readonly XName ValAttributeName = WordMlNamespace + "val";
        public static readonly XName TextRunName = WordMlNamespace + "r";
        public static readonly XName TextRunPropertiesName = WordMlNamespace + "rPr";
        public static readonly XName TextName = WordMlNamespace + "t";
        public static readonly XName CustomXmlName = WordMlNamespace + "customXml";
        public static readonly XName HyperlinkName = WordMlNamespace + "hyperlink";
        public static readonly XName FieldSimpleName = WordMlNamespace + "fldSimple";
        public static readonly XName SmartTagName = WordMlNamespace + "smartTag";
        public static readonly XName InsertName = WordMlNamespace + "ins";
        public static readonly XName DeleteName = WordMlNamespace + "del";
        public static readonly XName MoveFromName = WordMlNamespace + "moveFrom";
        public static readonly XName MoveToName = WordMlNamespace + "moveTo";
        public static readonly XName ProofingErrorAnchorName = WordMlNamespace + "proofErr";
        public static readonly XName BookmarkStartName = WordMlNamespace + "bookmarkStart";
        public static readonly XName AltChunkName = WordMlNamespace + "altChunk";
        public static readonly XName ParagraphStyleName = WordMlNamespace + "pStyle";

        public static readonly XName ShadingName = WordMlNamespace + "shd";
        public static readonly XName ColorName = WordMlNamespace + "color";
        public static readonly XName FillName = WordMlNamespace + "fill";

        public static readonly XName RsidRName = WordMlNamespace + "rsidR";
        public static readonly XName RsidRPropertiesName = WordMlNamespace + "rsidRPr";
        public static readonly XName RsidRDefaultName = WordMlNamespace + "rsidRDefault";
        public static readonly XName RsidPName = WordMlNamespace + "rsidP";

    }
}
