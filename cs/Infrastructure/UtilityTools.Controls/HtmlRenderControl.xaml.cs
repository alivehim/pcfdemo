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

namespace UtilityTools.Controls
{
    /// <summary>
    /// Interaction logic for PopupUserControl.xaml
    /// </summary>
    public partial class HtmlRenderControl : UserControl
    {

        public static readonly DependencyProperty RawTextProperty =
                DependencyProperty.RegisterAttached("RawText", typeof(string), typeof(HtmlRenderControl), new PropertyMetadata(String.Empty, OnTextChanged));

        public string RawText
        {
            get
            {
                PopupText.SelectAll();
                return PopupText.Selection.Text;

            }
            set { SetValue(RawTextProperty, value); }
        }

        private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public HtmlRenderControl()
        {
            InitializeComponent();
        }

       
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var aa = PopupText.Selection.Text;
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            PopupText.SelectAll();

            PopupText.Copy();
        }
    }
}
