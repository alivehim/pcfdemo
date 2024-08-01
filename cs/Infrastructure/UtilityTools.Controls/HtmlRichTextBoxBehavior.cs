using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using UtilityTools.Controls.HtmlConverter;

namespace UtilityTools.Controls
{
    public class HtmlRichTextBoxBehavior : DependencyObject
    {
        public static readonly DependencyProperty TextProperty =
                DependencyProperty.RegisterAttached("Text", typeof(string),
                typeof(HtmlRichTextBoxBehavior), new UIPropertyMetadata(null, OnValueChanged));

        public static string GetText(RichTextBox o) { return (string)o.GetValue(TextProperty); }

        public static void SetText(RichTextBox o, string value) { o.SetValue(TextProperty, value); }

        private static void OnValueChanged(DependencyObject dependencyObject,
                DependencyPropertyChangedEventArgs e)
        {
            var richTextBox = (RichTextBox)dependencyObject;
            var text = (e.NewValue ?? string.Empty).ToString();
            var xaml = HtmlToXamlConverter.ConvertHtmlToXaml(text, true);
            var flowDocument = XamlReader.Parse(xaml) as FlowDocument;
            HyperlinksSubscriptions(flowDocument);
            richTextBox.Document = flowDocument;

            var parent = FindParent<HtmlRenderControl>(dependencyObject);
            if (parent != null)
            {

                //https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/how-to-extract-the-text-content-from-a-richtextbox?view=netframeworkdesktop-4.8
                TextRange textRange = new TextRange(
                    // TextPointer to the start of content in the RichTextBox.
                    richTextBox.Document.ContentStart,
                    // TextPointer to the end of content in the RichTextBox.
                    richTextBox.Document.ContentEnd
                );

                // The Text property on a TextRange object returns a string
                // representing the plain text content of the TextRange.


                parent.SetValue(HtmlRenderControl.RawTextProperty, textRange.Text);
            }
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }


        private static void HyperlinksSubscriptions(FlowDocument flowDocument)
        {
            if (flowDocument == null) return;
            GetVisualChildren(flowDocument).OfType<Hyperlink>().ToList().ForEach(i => i.RequestNavigate += HyperlinkNavigate);
        }

        private static IEnumerable<DependencyObject> GetVisualChildren(DependencyObject root)
        {
            foreach (var child in LogicalTreeHelper.GetChildren(root).OfType<DependencyObject>())
            {
                yield return child;
                foreach (var descendants in GetVisualChildren(child)) yield return descendants;
            }
        }

        private static void HyperlinkNavigate(object sender,
            System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }


        //public static string GetRawText(DependencyObject obj)
        //{
        //          //return (string)obj.GetValue(RawTextProperty);
        //          var richTextBox = (RichTextBox)obj;
        //          richTextBox.SelectAll();
        //          return richTextBox.Selection.Text;
        //      }

        //public static void SetRawText(DependencyObject obj, string value)
        //{
        //	obj.SetValue(RawTextProperty, value);
        //}

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...

    }
}

