using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml;

namespace UtilityTools.OpenXml
{
    public partial class DocxReader
    {
        protected void ReadTable(XmlReader reader)
        {
            using (this.SetCurrent(new Table()
            {
                //BorderBrush = Brushes.Black,
                //BorderThickness = new System.Windows.Thickness(2, 2, 2, 2)
            }))
            {
                var group = new TableRowGroup();
                while (reader.Read())
                    if (reader.NodeType == XmlNodeType.Element
                        && reader.NamespaceURI == NamespaceConsts.WordMlNamespace
                        && reader.LocalName == DocumentConsts.TablePropertiesElement)
                    {

                        ReadXmlSubtree(reader, this.ReadTableProperties);
                    }
                    else if (reader.NodeType == XmlNodeType.Element
                            && reader.NamespaceURI == NamespaceConsts.WordMlNamespace
                            && reader.LocalName == DocumentConsts.TableGridElement)
                    {
                        ReadXmlSubtree(reader, this.ReadTableGrid);
                    }

                    else if (reader.NodeType == XmlNodeType.Element
                            && reader.NamespaceURI == NamespaceConsts.WordMlNamespace
                            && reader.LocalName == DocumentConsts.TableRowElement)
                    {
                        using (this.SetCurrent(group))
                        {
                            ReadXmlSubtree(reader, this.ReadTableRow);
                        }
                    }

            }

        }

        protected void ReadTableProperties(XmlReader reader)
        {

        }

        protected void ReadTableGrid(XmlReader reader)
        {

            while (reader.Read())
                if (reader.NodeType == XmlNodeType.Element && reader.NamespaceURI == NamespaceConsts.WordMlNamespace)
                {
                    var table = (Table)this.current;
                    switch (reader.LocalName)
                    {
                        case DocumentConsts.TableGridColomnElemnt:
                            table.Columns.Add(new TableColumn());
                            break;

                    }
                }
        }

        protected void ReadTableRow(XmlReader reader)
        {
            using (this.SetCurrent(new TableRow()
            {
                //Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x90, 255))

            }))
            {
                while (reader.Read())
                    if (reader.NodeType == XmlNodeType.Element
                        && reader.NamespaceURI == NamespaceConsts.WordMlNamespace
                        && reader.LocalName == DocumentConsts.TableCellElement)
                        ReadXmlSubtree(reader, this.ReadTableCell);
            }



        }
        protected void ReadTableCell(XmlReader reader)
        {
            using (this.SetCurrent(new TableCell() { BorderBrush = Brushes.Black, BorderThickness = new System.Windows.Thickness(2, 2, 2, 2) }))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element
                                           && reader.NamespaceURI == NamespaceConsts.WordMlNamespace
                                           && reader.LocalName == DocumentConsts.TableCellPropertiesElement)
                    {
                        ReadXmlSubtree(reader, this.ReadTableCellProperties);
                    }
                    else if (reader.NodeType == XmlNodeType.Element
                                           && reader.NamespaceURI == NamespaceConsts.WordMlNamespace
                                           && reader.LocalName == DocumentConsts.ParagraphElement)
                    {
                        ReadXmlSubtree(reader, this.ReadParagraph);
                    }
                }
            }

        }

        protected void ReadTableCellProperties(XmlReader reader)
        {

        }

    }


}
