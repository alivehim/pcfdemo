using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilityTools.OpenXml.Extensions;

namespace UtilityTools.OpenXml.DocxTemplate.Core
{
    internal class DocxHelper
    {
        private static readonly HashSet<XName> ValidTextRunContainers = new HashSet<XName>
                                                                                {
                                                                                    NamespaceConsts.ParagraphName,
                                                                                    NamespaceConsts.CustomXmlName,
                                                                                    NamespaceConsts.HyperlinkName,
                                                                                    NamespaceConsts.MoveFromName,
                                                                                    NamespaceConsts.MoveToName,
                                                                                    NamespaceConsts.SdtContentName,
                                                                                    NamespaceConsts.DeleteName,
                                                                                    NamespaceConsts.InsertName,
                                                                                    NamespaceConsts.SmartTagName,
                                                                                    NamespaceConsts.FieldSimpleName
                                                                                };

        private static readonly HashSet<XName> TextPropertiesNames = new HashSet<XName>
                                                                                {
                                                                                    TextRunProperties.BoldName,
                                                                                    TextRunProperties.RunFontsName,
                                                                                    TextRunProperties.ReferenceStyleName,
                                                                                    TextRunProperties.ComplexScriptBoldName,
                                                                                    TextRunProperties.ItalicsName,
                                                                                    TextRunProperties.CapsName,
                                                                                    TextRunProperties.SmallCapsName,
                                                                                    TextRunProperties.StrikeName,
                                                                                    TextRunProperties.DoubleStrikeName,
                                                                                    TextRunProperties.OutlineName,
                                                                                    TextRunProperties.ShadowName,
                                                                                    TextRunProperties.EmbossName,
                                                                                    TextRunProperties.ImprintName,
                                                                                    TextRunProperties.SnapToGridName,
                                                                                    TextRunProperties.VanishName,
                                                                                    TextRunProperties.WebHiddenName,
                                                                                    TextRunProperties.ColorName,
                                                                                    TextRunProperties.SpacingName,
                                                                                    TextRunProperties.WName,
                                                                                    TextRunProperties.KernName,
                                                                                    TextRunProperties.PositionName,
                                                                                    TextRunProperties.FontSizeName,
                                                                                    TextRunProperties.ComplexScriptFontSizeName,
                                                                                    TextRunProperties.HighlightTextName,
                                                                                    TextRunProperties.UnderlineName,
                                                                                    TextRunProperties.EffectName,
                                                                                    TextRunProperties.TextBorderName,
                                                                                    TextRunProperties.RunShadingName,
                                                                                    TextRunProperties.FitTextName,
                                                                                    TextRunProperties.VertAlignName,
                                                                                    TextRunProperties.RightToLeftTextName,
                                                                                    TextRunProperties.ComplexScriptName,
                                                                                    TextRunProperties.EmphasisMarkName,
                                                                                    TextRunProperties.EastAsianLayoutName,
                                                                                    TextRunProperties.SpecVanishName,
                                                                                    TextRunProperties.OfficeOpenXmlMath,
                                                                                };

        private static readonly HashSet<XName> CellPropertiesNames = new HashSet<XName>
                                                                         {
                                                                             TableCellProperties.BorderCellName,
                                                                             TableCellProperties.ConditionFormattingCellName,
                                                                             TableCellProperties.DeletionCellName,
                                                                             TableCellProperties.FitTextCellName,
                                                                             TableCellProperties.GridSpanCellName,
                                                                             TableCellProperties.HorizontallyMergedCellName,
                                                                             TableCellProperties.IgnoreEndMarkerCellName,
                                                                             TableCellProperties.InsertionCellName,
                                                                             TableCellProperties.NoWrapCellName,
                                                                             TableCellProperties.PreferredWidthCellName,
                                                                             TableCellProperties.RevisionPropertiesInformationCellName,
                                                                             TableCellProperties.ShadingCellName,
                                                                             TableCellProperties.SingleMarginsCellName,
                                                                             TableCellProperties.TextFlowDirectionCellName,
                                                                             TableCellProperties.VerticalAlignmentCellName,
                                                                             TableCellProperties.VerticallyMergedCellName,
                                                                             TableCellProperties.VerticallyMergedSplitCellName
                                                                         };

        private static readonly HashSet<XName> ParagraphPropertiesNames = new HashSet<XName>
                                                                              {
                                                                                  ParagraphProperties.ReferenceStyleName,
                                                                                  ParagraphProperties.KeepNextName,
                                                                                  ParagraphProperties.KeepLineName,
                                                                                  ParagraphProperties.PageBreakName,
                                                                                  ParagraphProperties.TextFrameName,
                                                                                  ParagraphProperties.WidowControlName,
                                                                                  ParagraphProperties.NumberingDefinitionName,
                                                                                  ParagraphProperties.SuppressLineNumbersName,
                                                                                  ParagraphProperties.BordersName,
                                                                                  ParagraphProperties.ShadingName,
                                                                                  ParagraphProperties.CustomTabName,
                                                                                  ParagraphProperties.SuppressHyphenationName,
                                                                                  ParagraphProperties.KinsokuName,
                                                                                  ParagraphProperties.WordWrapName,
                                                                                  ParagraphProperties.OverflowPunctuationName,
                                                                                  ParagraphProperties.TopLinePunctuationName,
                                                                                  ParagraphProperties.AutoSpaceDEName,
                                                                                  ParagraphProperties.AutoSpaceDNName,
                                                                                  ParagraphProperties.RightToLeftLayoutName,
                                                                                  ParagraphProperties.AdjuctRightIndentName,
                                                                                  ParagraphProperties.SnapToGridName,
                                                                                  ParagraphProperties.SpacingBetweenLineName,
                                                                                  ParagraphProperties.IndentationName,
                                                                                  ParagraphProperties.ContextualSpacingName,
                                                                                  ParagraphProperties.MirrorIndentsName,
                                                                                  ParagraphProperties.SuppressOverlapName,
                                                                                  ParagraphProperties.AlignmentName,
                                                                                  ParagraphProperties.TextDirectionName,
                                                                                  ParagraphProperties.TextAlignmentName,
                                                                                  ParagraphProperties.TextboxTightWrapName,
                                                                                  ParagraphProperties.OutlineLevelName,
                                                                                  ParagraphProperties.DivIdName,
                                                                                  ParagraphProperties.ConditionalFormattingStyleName,
                                                                                  ParagraphProperties.RunPropertiesName,
                                                                                  ParagraphProperties.SectionPropertiesName,
                                                                                  ParagraphProperties.RevisionPropertiesInformationName
                                                                              };

        public static XElement CreateTextElement(XElement self, XElement parent, string text)
        {
            return CreateTextElement(self, parent,  text, new XElement(NamespaceConsts.ParagraphName),string.Empty);
        }

        public static XElement CreateTextElement(XElement self, XElement parent, string text, XElement wrapTo, string styleName, bool wrapParagraphs = false)
        {
            var result = new XElement(NamespaceConsts.TextRunName, new XElement(NamespaceConsts.TextName, text));
            IEnumerable<XElement>? paragraphProperties = null;
            if (self.IsSdt())
            {
                var properties = self.Element(NamespaceConsts.SdtContentName)
                    .Descendants(NamespaceConsts.TextRunPropertiesName)
                    .Elements()
                    .Where(e => TextPropertiesNames.Contains(e.Name))
                    .Distinct(new NameComparer());

                //create text run properties
                var textRunProperties = new XElement(NamespaceConsts.TextRunPropertiesName, properties);

                if (!(styleName.Equals(string.Empty) || properties.Any(p => p.Name.Equals(NamespaceConsts.ParagraphStyleName))))
                {
                    textRunProperties.Add(new XElement(TextRunProperties.ReferenceStyleName, new XAttribute(NamespaceConsts.ValAttributeName, styleName + "0")));
                }

                //add text run properties
                result.AddFirst(textRunProperties);



                if (self.Element(NamespaceConsts.SdtContentName).Elements(NamespaceConsts.ParagraphName).Any())
                {
                    paragraphProperties =
                        self.Element(NamespaceConsts.SdtContentName)
                            .Descendants(NamespaceConsts.ParagraphPropertiesName)
                            .Elements()
                            .Where(e => ParagraphPropertiesNames.Contains(e.Name))
                            .Distinct(new NameComparer());
                }
            }
            if (paragraphProperties == null && (parent.Elements(NamespaceConsts.ParagraphName).Any() || (parent.Name == NamespaceConsts.ParagraphName)))
            {
                paragraphProperties =
                    parent.DescendantsAndSelf(NamespaceConsts.ParagraphPropertiesName)
                          .Elements()
                          .Where(e => ParagraphPropertiesNames.Contains(e.Name))
                          .Distinct(new NameComparer());
            }

            if (paragraphProperties != null)
            {
                var paragraphPropertiesElement = new XElement(NamespaceConsts.ParagraphPropertiesName, paragraphProperties);

                if (!(styleName.Equals(string.Empty) || paragraphProperties.Any(p => p.Name.Equals(NamespaceConsts.ParagraphStyleName))))
                {
                    paragraphPropertiesElement.Add(new XElement(NamespaceConsts.ParagraphStyleName, new XAttribute(NamespaceConsts.ValAttributeName, styleName)));
                }

                wrapTo.AddFirst(paragraphPropertiesElement);
            }
            if ((!ValidTextRunContainers.Any(name => name.Equals(parent.Name))) || (wrapParagraphs && !parent.Elements().Any(el => el.Name.Equals(NamespaceConsts.TextRunName))))
            {
                wrapTo.Add(result);
                result = wrapTo;
            }
            return result;
        }
        private class NameComparer : IEqualityComparer<XElement>
        {
            public bool Equals(XElement x, XElement y)
            {
                return x.Name.Equals(y.Name);
            }

            public int GetHashCode(XElement obj)
            {
                return obj.Name.GetHashCode();
            }
        }
    }
}
