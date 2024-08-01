
using SharpCompress.Compressors.Xz;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Xml;
using System.Xml.Linq;

namespace UtilityTools.OpenXml
{
    public partial class DocxReader
    {
        //private Package? package = null;
        private PackagePart? mainDocumentPart;

        private TextElement current;

        private FlowDocument? document = null;
        public FlowDocument? Document
        {
            get { return this.document; }
        }

        private Stream mainPartStream;

        private Stream? styleStream;
        //private Stream origianlFileStream;

        public Stream MainPartStream => mainPartStream;

        //public Stream OrigianlFileStream =>

        private MemoryStream? ReadStyleStream(Package package, string namespaces)
        {
            //PackageRelationship? docPackageRelationship =
            //      package.GetRelationshipsByType(namespaces).FirstOrDefault();
            //if (docPackageRelationship != null)
            //{
            //    Uri documentUri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative),
            //      docPackageRelationship.TargetUri);
            //    mainDocumentPart = package.GetPart(documentUri);

            //    var ms = mainDocumentPart.GetStream();

            //    var nm = new MemoryStream();
            //    ms.CopyTo(nm);

            //    return nm;
            //}
            //return null;

            Uri stylesUri = new Uri("/word/styles.xml", UriKind.Relative);

            if (package.PartExists(stylesUri))
            {
                PackagePart stylesPart = package.GetPart(stylesUri);

                var ms = stylesPart.GetStream();
                var nm = new MemoryStream();
                ms.CopyTo(nm);

                return nm;

                          }

            return null;

        }

        //https://learn.microsoft.com/en-us/dotnet/standard/linq/example-outputs-office-open-xml-document-parts
        public string Read(string filePath)
        {

            using (var package = Package.Open(filePath, FileMode.Open, FileAccess.Read))
            {


                styleStream = ReadStyleStream(package, NamespaceConsts.StylesRelationshipType);

                PackageRelationship? docPackageRelationship =
    package.GetRelationshipsByType(NamespaceConsts.DocumentRelationshipType).FirstOrDefault();
                if (docPackageRelationship != null)
                {
                    Uri documentUri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative),
                      docPackageRelationship.TargetUri);
                    mainDocumentPart = package.GetPart(documentUri);

                    //XDocument partXml;
                    //using (var reader = XmlReader.Create(mainDocumentPart.GetStream()))
                    //    partXml = XDocument.Load(reader);


                    var ms = mainDocumentPart.GetStream();


                    mainPartStream = new MemoryStream();
                    ms.CopyTo(mainPartStream);
                    mainPartStream.Seek(0, SeekOrigin.Begin);
                    StreamReader reader = new StreamReader(mainPartStream);


                    string text = reader.ReadToEnd();

                    return text;
                }
            }



            return string.Empty;
        }


        public string Read(Stream docxFileStream)
        {

            using (var package = Package.Open(docxFileStream, FileMode.Open, FileAccess.Read))
            {


                styleStream = ReadStyleStream(package, NamespaceConsts.StylesRelationshipType);

                PackageRelationship? docPackageRelationship =
    package.GetRelationshipsByType(NamespaceConsts.DocumentRelationshipType).FirstOrDefault();
                if (docPackageRelationship != null)
                {
                    Uri documentUri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative),
                      docPackageRelationship.TargetUri);
                    mainDocumentPart = package.GetPart(documentUri);

                    //XDocument partXml;
                    //using (var reader = XmlReader.Create(mainDocumentPart.GetStream()))
                    //    partXml = XDocument.Load(reader);


                    var ms = mainDocumentPart.GetStream();


                    mainPartStream = new MemoryStream();
                    ms.CopyTo(mainPartStream);
                    mainPartStream.Seek(0, SeekOrigin.Begin);
                    StreamReader reader = new StreamReader(mainPartStream);


                    string text = reader.ReadToEnd();

                    return text;
                }
            }



            return string.Empty;
        }

        public string ReadDocumentStream(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(stream);


            string text = reader.ReadToEnd();

            return text;
        }

        public XDocument? GetDocument()
        {
            //if (mainDocumentPart == null)
            //{
            //    return null;
            //}
            using (var reader = XmlReader.Create(mainPartStream))
            {

                var partXml = XDocument.Load(reader);


                return partXml;
            }
        }

        public XDocument GetStyleDocument()
        {
            //if (mainDocumentPart == null)
            //{
            //    return null;
            //}

            styleStream?.Seek(0, System.IO.SeekOrigin.Begin);
            using (var reader = XmlReader.Create(styleStream))
            {

                var partXml = XDocument.Load(reader);


                return partXml;
            }
        }

        public void ParseStyle()
        {
            var xdoc = GetStyleDocument();

            //var ns = NamespaceConsts.WordMlNamespace + "/style";
            var elements =  xdoc.Descendants(NamespaceConsts.Style);


            var result = (from m in elements
                          where m.Attribute(NamespaceConsts.StyleId)?.Value == "a3"
                          select m).ToList().FirstOrDefault();

            foreach (var element in result?.Elements())
            {
                if(element.Name == NamespaceConsts.tblPr)
                {
                    var descendants = element.Descendants();

                    foreach(var  descendant in descendants)
                    {
                        if(descendant.Name == NamespaceConsts.tblPr)
                        {

                        }
                    }
                }
            }
            Console.WriteLine(result);
        }

        public void Render()
        {
            if (mainDocumentPart == null)
            {
                return;
            }

            this.document = new FlowDocument();
            this.document.BeginInit();
            this.document.ColumnWidth = double.NaN;


            mainPartStream.Seek(0, SeekOrigin.Begin);
            using (var reader = XmlReader.Create(mainPartStream, new XmlReaderSettings()
            {
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true
            }))
                ReadMainDocument(reader);

            this.document.EndInit();
        }

        public void Render(Stream stream,bool overrided=false)
        {
            stream.Seek(0, SeekOrigin.Begin);
            if (overrided)
            {
                if (mainPartStream != null)
                {
                    mainPartStream.Dispose();

                    mainPartStream = null;

                    mainPartStream = new MemoryStream();

                    stream.CopyTo(mainPartStream);

                }
                else
                {
                    mainPartStream = new MemoryStream();
                    stream.CopyTo(mainPartStream);
                }


                this.document = new FlowDocument();
                this.document.BeginInit();
                this.document.ColumnWidth = double.NaN;


                mainPartStream.Seek(0, SeekOrigin.Begin);
                using (var reader = XmlReader.Create(mainPartStream, new XmlReaderSettings()
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true
                }))
                    ReadMainDocument(reader);

                this.document.EndInit();
            }
            else
            {
                this.document = new FlowDocument();
                this.document.BeginInit();
                this.document.ColumnWidth = double.NaN;


                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = XmlReader.Create(stream, new XmlReaderSettings()
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true
                }))
                    ReadMainDocument(reader);

                this.document.EndInit();
            }
        }

        private void ReadMainDocument(XmlReader reader)
        {
            while (reader.Read())
                if (reader.NodeType == XmlNodeType.Element && reader.NamespaceURI == NamespaceConsts.WordMlNamespace && reader.LocalName == DocumentConsts.DocumentElement)
                {
                    ReadXmlSubtree(reader, this.ReadDocument);
                    break;
                }
        }

        protected void ReadDocument(XmlReader reader)
        {
            while (reader.Read())
                if (reader.NodeType == XmlNodeType.Element && reader.NamespaceURI == NamespaceConsts.WordMlNamespace && reader.LocalName == DocumentConsts.BodyElement)
                {
                    ReadXmlSubtree(reader, this.ReadBody);
                    break;
                }
        }

        private void ReadBody(XmlReader reader)
        {
            while (reader.Read())
                this.ReadBlockLevelElement(reader);
        }

        private void ReadBlockLevelElement(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                Action<XmlReader> action = null;

                if (reader.NamespaceURI == NamespaceConsts.WordMlNamespace)
                    switch (reader.LocalName)
                    {
                        case DocumentConsts.ParagraphElement:
                            action = this.ReadParagraph;
                            break;

                        case DocumentConsts.TableElement:
                            action = this.ReadTable;
                            break;
                    }

                ReadXmlSubtree(reader, action);
            }
        }

        #region Core
        protected virtual void ReadParagraph(XmlReader reader)
        {
            using (this.SetCurrent(new Paragraph()))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.NamespaceURI == NamespaceConsts.WordMlNamespace
                        && reader.LocalName == DocumentConsts.ParagraphPropertiesElement)
                        ReadXmlSubtree(reader, this.ReadParagraphProperties);
                    else
                        this.ReadInlineLevelElement(reader);


                }
            }

        }





        protected void ReadParagraphProperties(XmlReader reader)
        {
            while (reader.Read())
                if (reader.NodeType == XmlNodeType.Element && reader.NamespaceURI == NamespaceConsts.WordMlNamespace)
                {
                    var paragraph = (Paragraph)this.current;
                    switch (reader.LocalName)
                    {
                        case StyleConsts.AlignmentElement:
                            var textAlignment = ConvertTextAlignment(GetValueAttribute(reader));
                            if (textAlignment.HasValue)
                                paragraph.TextAlignment = textAlignment.Value;
                            break;
                        case StyleConsts.PageBreakBeforeElement:
                            paragraph.BreakPageBefore = GetOnOffValueAttribute(reader);
                            break;
                        case StyleConsts.SpacingElement:
                            paragraph.Margin = GetSpacing(reader, paragraph.Margin);
                            break;
                        case StyleConsts.IndentationElement:
                            SetParagraphIndent(reader, paragraph);
                            break;
                        case StyleConsts.ShadingElement:
                            var background = GetShading(reader);
                            if (background != null)
                                paragraph.Background = background;
                            break;
                    }
                }
        }

        private void ReadInlineLevelElement(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                Action<XmlReader> action = null;

                if (reader.NamespaceURI == NamespaceConsts.WordMlNamespace)
                    switch (reader.LocalName)
                    {
                        //case DocumentConsts.SimpleFieldElement:
                        //    action = this.ReadSimpleField;
                        //    break;

                        //case DocumentConsts.HyperlinkElement:
                        //    action = this.ReadHyperlink;
                        //    break;

                        case DocumentConsts.RunElement: // <w:r>
                            action = this.ReadRun;
                            break;
                    }

                ReadXmlSubtree(reader, action);
            }
        }

        protected void ReadRun(XmlReader reader)
        {
            using (this.SetCurrent(new Span()))  // add new span
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element
                        && reader.NamespaceURI == NamespaceConsts.WordMlNamespace
                        && reader.LocalName == DocumentConsts.RunPropertiesElement)
                        //<w:rPr>
                        ReadXmlSubtree(reader, this.ReadRunProperties);
                    else
                        this.ReadRunContentElement(reader);
                }
            }
        }

        protected void ReadRunProperties(XmlReader reader)
        {
            while (reader.Read())
                if (reader.NodeType == XmlNodeType.Element && reader.NamespaceURI == NamespaceConsts.WordMlNamespace)
                {
                    var inline = (Inline)this.current;
                    switch (reader.LocalName)
                    {
                        case StyleConsts.BoldElement:
                            inline.FontWeight = GetOnOffValueAttribute(reader) ? FontWeights.Bold : FontWeights.Normal;
                            break;
                        case StyleConsts.ItalicElement:
                            inline.FontStyle = GetOnOffValueAttribute(reader) ? FontStyles.Italic : FontStyles.Normal;
                            break;
                        case StyleConsts.UnderlineElement:
                            var underlineTextDecorations = GetUnderlineTextDecorations(reader, inline);
                            if (underlineTextDecorations != null)
                                inline.TextDecorations.Add(underlineTextDecorations);
                            break;
                        case StyleConsts.StrikeElement:
                            if (GetOnOffValueAttribute(reader))
                                inline.TextDecorations.Add(TextDecorations.Strikethrough);
                            break;
                        case StyleConsts.DoubleStrikeElement:
                            if (GetOnOffValueAttribute(reader))
                            {
                                inline.TextDecorations.Add(new TextDecoration() { Location = TextDecorationLocation.Strikethrough, PenOffset = this.current.FontSize * 0.015 });
                                inline.TextDecorations.Add(new TextDecoration() { Location = TextDecorationLocation.Strikethrough, PenOffset = this.current.FontSize * -0.015 });
                            }
                            break;
                        case StyleConsts.VerticalAlignmentElement:
                            var baselineAlignment = GetBaselineAlignment(GetValueAttribute(reader));
                            if (baselineAlignment.HasValue)
                            {
                                inline.BaselineAlignment = baselineAlignment.Value;
                                if (baselineAlignment.Value == BaselineAlignment.Subscript || baselineAlignment.Value == BaselineAlignment.Superscript)
                                    inline.FontSize *= 0.65; //MS Word 2002 size: 65% http://en.wikipedia.org/wiki/Subscript_and_superscript
                            }
                            break;
                        case StyleConsts.ColorElement:
                            var color = GetColor(GetValueAttribute(reader));
                            if (color.HasValue)
                                inline.Foreground = new SolidColorBrush(color.Value);
                            break;
                        case StyleConsts.HighlightElement:
                            var highlight = GetHighlightColor(GetValueAttribute(reader));
                            if (highlight.HasValue)
                                inline.Background = new SolidColorBrush(highlight.Value);
                            break;
                        case StyleConsts.FontElement:
                            var fontFamily = reader[StyleConsts.AsciiFontFamily, NamespaceConsts.WordMlNamespaceString];
                            if (!string.IsNullOrEmpty(fontFamily))
                                inline.FontFamily = (FontFamily)new FontFamilyConverter().ConvertFromString(fontFamily);
                            break;
                        case StyleConsts.FontSizeElement:
                            var fontSize = reader[StyleConsts.ValueAttribute, NamespaceConsts.WordMlNamespaceString];
                            if (!string.IsNullOrEmpty(fontSize))
                                // Attribute Value / 2 = Points
                                // Points * (96 / 72) = Pixels
                                inline.FontSize = uint.Parse(fontSize) * 0.6666666666666667;
                            break;
                        case StyleConsts.RightToLeftTextElement:
                            inline.FlowDirection = (GetOnOffValueAttribute(reader)) ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
                            break;
                    }
                }

        }
        private void ReadRunContentElement(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                Action<XmlReader> action = null;

                if (reader.NamespaceURI == NamespaceConsts.WordMlNamespace)
                    switch (reader.LocalName)
                    {
                        case DocumentConsts.BreakElement:
                            action = this.ReadBreak;
                            break;

                        case DocumentConsts.TabCharacterElement:
                            action = this.ReadTabCharacter;
                            break;

                        case DocumentConsts.TextElement:
                            action = this.ReadText;
                            break;
                    }

                ReadXmlSubtree(reader, action);
            }
        }


        protected void ReadText(XmlReader reader)
        {
            this.AddChild(new Run(reader.ReadString()));
        }
        protected void ReadBreak(XmlReader reader)
        {
            this.AddChild(new LineBreak());
        }

        protected void ReadTabCharacter(XmlReader reader)
        {
            this.AddChild(new Run("\t"));
        }
        #endregion


        #region basic methods

        private static void ReadXmlSubtree(XmlReader reader, Action<XmlReader> action)
        {
            using (var subtreeReader = reader.ReadSubtree())
            {
                // Position on the first node.
                subtreeReader.Read();

                if (action != null)
                    action(subtreeReader);
            }
        }

        #endregion

        #region style methods

        private static bool GetOnOffValueAttribute(XmlReader reader)
        {
            var value = GetValueAttribute(reader);

            switch (value)
            {
                case null:
                case "1":
                case "on":
                case "true":
                    return true;
                default:
                    return false;
            }
        }

        private static string GetValueAttribute(XmlReader reader)
        {
            return reader[StyleConsts.ValueAttribute, NamespaceConsts.WordMlNamespaceString];
        }

        private static Color? GetColor(string colorString)
        {
            if (string.IsNullOrEmpty(colorString) || colorString == "auto")
                return null;

            return (Color)ColorConverter.ConvertFromString('#' + colorString);
        }

        private static Color? GetHighlightColor(string highlightString)
        {
            if (string.IsNullOrEmpty(highlightString) || highlightString == "auto")
                return null;

            return (Color)ColorConverter.ConvertFromString(highlightString);
        }

        private static BaselineAlignment? GetBaselineAlignment(string verticalAlignmentString)
        {
            switch (verticalAlignmentString)
            {
                case "baseline":
                    return BaselineAlignment.Baseline;
                case "subscript":
                    return BaselineAlignment.Subscript;
                case "superscript":
                    return BaselineAlignment.Superscript;
                default:
                    return null;
            }
        }

        private static double? ConvertTwipsToPixels(string twips)
        {
            if (string.IsNullOrEmpty(twips))
                return null;
            else
                return ConvertTwipsToPixels(double.Parse(twips, CultureInfo.InvariantCulture));
        }

        private static double ConvertTwipsToPixels(double twips)
        {
            return 96d / (72 * 20) * twips;
        }

        private static TextAlignment? ConvertTextAlignment(string value)
        {
            switch (value)
            {
                case "both":
                    return TextAlignment.Justify;
                case "left":
                    return TextAlignment.Left;
                case "right":
                    return TextAlignment.Right;
                case "center":
                    return TextAlignment.Center;
                default:
                    return null;
            }
        }

        private static Thickness GetSpacing(XmlReader reader, Thickness margin)
        {
            var after = ConvertTwipsToPixels(reader[StyleConsts.SpacingAfterAttribute, NamespaceConsts.WordMlNamespaceString]);
            if (after.HasValue)
                margin.Bottom = after.Value;

            var before = ConvertTwipsToPixels(reader[StyleConsts.SpacingBeforeAttribute, NamespaceConsts.WordMlNamespaceString]);
            if (before.HasValue)
                margin.Top = before.Value;

            return margin;
        }

        private static void SetParagraphIndent(XmlReader reader, Paragraph paragraph)
        {
            var margin = paragraph.Margin;

            var left = ConvertTwipsToPixels(reader[StyleConsts.LeftIndentationAttribute, NamespaceConsts.WordMlNamespaceString]);
            if (left.HasValue)
                margin.Left = left.Value;

            var right = ConvertTwipsToPixels(reader[StyleConsts.RightIndentationAttribute, NamespaceConsts.WordMlNamespaceString]);
            if (right.HasValue)
                margin.Right = right.Value;

            paragraph.Margin = margin;

            var firstLine = ConvertTwipsToPixels(reader[StyleConsts.FirstLineIndentationAttribute, NamespaceConsts.WordMlNamespaceString]);
            if (firstLine.HasValue)
                paragraph.TextIndent = firstLine.Value;

            var hanging = ConvertTwipsToPixels(reader[StyleConsts.HangingIndentationAttribute, NamespaceConsts.WordMlNamespaceString]);
            if (hanging.HasValue)
                paragraph.TextIndent -= hanging.Value;
        }

        private static Brush GetShading(XmlReader reader)
        {
            var color = GetColor(reader[StyleConsts.FillAttribute, NamespaceConsts.WordMlNamespaceString]);
            return color.HasValue ? new SolidColorBrush(color.Value) : null;
        }

        private static TextDecorationCollection GetUnderlineTextDecorations(XmlReader reader, Inline inline)
        {
            TextDecoration textDecoration;
            Brush brush;
            var color = GetColor(reader[StyleConsts.ColorAttribute, NamespaceConsts.WordMlNamespaceString]);

            if (color.HasValue)
                brush = new SolidColorBrush(color.Value);
            else
                brush = inline.Foreground;

            var textDecorations = new TextDecorationCollection()
            {
                (textDecoration = new TextDecoration()
                {
                    Location = TextDecorationLocation.Underline,
                    Pen = new Pen()
                    {
                        Brush = brush
                    }
                })
            };

            switch (GetValueAttribute(reader))
            {
                case "single":
                    break;
                case "double":
                    textDecoration.PenOffset = inline.FontSize * 0.05;
                    textDecoration = textDecoration.Clone();
                    textDecoration.PenOffset = inline.FontSize * -0.05;
                    textDecorations.Add(textDecoration);
                    break;
                case "dotted":
                    textDecoration.Pen.DashStyle = DashStyles.Dot;
                    break;
                case "dash":
                    textDecoration.Pen.DashStyle = DashStyles.Dash;
                    break;
                case "dotDash":
                    textDecoration.Pen.DashStyle = DashStyles.DashDot;
                    break;
                case "dotDotDash":
                    textDecoration.Pen.DashStyle = DashStyles.DashDotDot;
                    break;
                case "none":
                default:
                    // If underline type is none or unsupported then it will be ignored.
                    return null;
            }

            return textDecorations;
        }


        #endregion

        private void AddChild(TextElement textElement)
        {
            if (this.current is Table table)
            {
                if (table.RowGroups.Contains(textElement))
                {
                    return;
                }
            }

            ((IAddChild)this.current ?? this.document).AddChild(textElement);
        }


        private IDisposable SetCurrent(TextElement current)
        {
            return new CurrentHandle(this, current);
        }

        private struct CurrentHandle : IDisposable
        {
            private readonly DocxReader converter;
            private readonly TextElement previous;

            public CurrentHandle(DocxReader converter, TextElement current)
            {
                this.converter = converter;
                this.converter.AddChild(current);
                this.previous = this.converter.current;
                this.converter.current = current;
            }

            public void Dispose()
            {
                this.converter.current = this.previous;
            }
        }
    }

}
