using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using UtilityTools.Core.Extensions;

namespace UtilityTools.Controls.XML
{
    /// <summary>
    /// Interaction logic for XMLViewer.xaml
    /// </summary>
    public partial class XMLViewer : UserControl
    {
        private XmlDocument _xmldocument;

        public static readonly DependencyProperty XMLContentProperty =
               DependencyProperty.RegisterAttached("XMLContent", typeof(string), typeof(XMLViewer), new PropertyMetadata(String.Empty, OnTextChanged));

        public string XMLContent
        {
            get { return (string)GetValue(XMLContentProperty); }
            set { SetValue(XMLContentProperty, value); }
        }

        private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var viewer = sender as XMLViewer;

            XmlDocument doc = new XmlDocument();
            if (e.NewValue != null && !string.IsNullOrEmpty(e.NewValue.ToString()))
            {
                doc.LoadXml(e.NewValue.ToString());
                viewer.xmlDocument = doc;
            }
         
        }

        public XMLViewer()
        {
            InitializeComponent();
        }

        public XmlDocument xmlDocument
        {
            get { return _xmldocument; }
            set
            {
                _xmldocument = value;
                BindXMLDocument();
            }
        }

        private void BindXMLDocument()
        {
            if (_xmldocument == null)
            {
                xmlTree.ItemsSource = null;
                return;
            }

            XmlDataProvider provider = new XmlDataProvider();
            provider.Document = _xmldocument;
            Binding binding = new Binding();
            binding.Source = provider;
            binding.XPath = "child::node()";
            xmlTree.SetBinding(TreeView.ItemsSourceProperty, binding);
        }
    }
}
