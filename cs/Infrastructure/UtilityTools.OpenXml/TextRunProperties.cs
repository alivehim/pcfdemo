using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UtilityTools.OpenXml
{
    internal class TextRunProperties
    {
        public static readonly XName ReferenceStyleName = NamespaceConsts.WordMlNamespace + "rStyle";
        public static readonly XName RunFontsName = NamespaceConsts.WordMlNamespace + "rFonts";
        public static readonly XName BoldName = NamespaceConsts.WordMlNamespace + "b";
        public static readonly XName ComplexScriptBoldName = NamespaceConsts.WordMlNamespace + "bCs";
        public static readonly XName ItalicsName = NamespaceConsts.WordMlNamespace + "i";
        public static readonly XName ComplexScriptItalicsName = NamespaceConsts.WordMlNamespace + "iCs";
        public static readonly XName CapsName = NamespaceConsts.WordMlNamespace + "caps";
        public static readonly XName SmallCapsName = NamespaceConsts.WordMlNamespace + "smallCaps";
        public static readonly XName StrikeName = NamespaceConsts.WordMlNamespace + "strike";
        public static readonly XName DoubleStrikeName = NamespaceConsts.WordMlNamespace + "dstrike";
        public static readonly XName OutlineName = NamespaceConsts.WordMlNamespace + "outline";
        public static readonly XName ShadowName = NamespaceConsts.WordMlNamespace + "shadow";
        public static readonly XName EmbossName = NamespaceConsts.WordMlNamespace + "emboss";
        public static readonly XName ImprintName = NamespaceConsts.WordMlNamespace + "imprint";
        public static readonly XName NoProofName = NamespaceConsts.WordMlNamespace + "noProof";
        public static readonly XName SnapToGridName = NamespaceConsts.WordMlNamespace + "snapToGrid";
        public static readonly XName VanishName = NamespaceConsts.WordMlNamespace + "vanish";
        public static readonly XName WebHiddenName = NamespaceConsts.WordMlNamespace + "webHidden";
        public static readonly XName ColorName = NamespaceConsts.WordMlNamespace + "color";
        public static readonly XName SpacingName = NamespaceConsts.WordMlNamespace + "spacing";
        public static readonly XName WName = NamespaceConsts.WordMlNamespace + "w";
        public static readonly XName KernName = NamespaceConsts.WordMlNamespace + "kern";
        public static readonly XName PositionName = NamespaceConsts.WordMlNamespace + "position";
        public static readonly XName FontSizeName = NamespaceConsts.WordMlNamespace + "sz";
        public static readonly XName ComplexScriptFontSizeName = NamespaceConsts.WordMlNamespace + "szCs";
        public static readonly XName HighlightTextName = NamespaceConsts.WordMlNamespace + "highlight";
        public static readonly XName UnderlineName = NamespaceConsts.WordMlNamespace + "u";
        public static readonly XName EffectName = NamespaceConsts.WordMlNamespace + "effect";
        public static readonly XName TextBorderName = NamespaceConsts.WordMlNamespace + "bdr";
        public static readonly XName RunShadingName = NamespaceConsts.WordMlNamespace + "shd";
        public static readonly XName FitTextName = NamespaceConsts.WordMlNamespace + "fitText";
        public static readonly XName VertAlignName = NamespaceConsts.WordMlNamespace + "vertAlign";
        public static readonly XName RightToLeftTextName = NamespaceConsts.WordMlNamespace + "rtl";
        public static readonly XName ComplexScriptName = NamespaceConsts.WordMlNamespace + "cs";
        public static readonly XName EmphasisMarkName = NamespaceConsts.WordMlNamespace + "em";
        public static readonly XName LanguageName = NamespaceConsts.WordMlNamespace + "lang";
        public static readonly XName EastAsianLayoutName = NamespaceConsts.WordMlNamespace + "eastAsianLayout";
        public static readonly XName SpecVanishName = NamespaceConsts.WordMlNamespace + "specVanish";
        public static readonly XName OfficeOpenXmlMath = NamespaceConsts.WordMlNamespace + "oMath";
    }
}
