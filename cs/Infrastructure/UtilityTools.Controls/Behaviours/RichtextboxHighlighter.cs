using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using System.Reflection;
using SharpCompress.Common;

namespace UtilityTools.Controls.Behaviours
{
    public class RichtextboxHighlighter
    {
        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(RichtextboxHighlighter),
                new PropertyMetadata(new PropertyChangedCallback(TextChanged)));


        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            if (!(d is RichTextBox)) throw new InvalidOperationException("Only valid for TextBlock");

            var control = d as RichTextBox;

            var doc = control.Document as FlowDocument;
            doc.Blocks.Clear();
            string text = e.NewValue.ToString();

            string highlightText = "test";
            if (string.IsNullOrEmpty(highlightText)) return;

            int index = text.IndexOf(highlightText, StringComparison.CurrentCultureIgnoreCase);
            
            Brush selectionColor = (Brush)d.GetValue(HighlightColorProperty);
            Brush forecolor = (Brush)d.GetValue(ForecolorProperty);

            var paragraph = new Paragraph();

            if (index < 0)
            {
                paragraph.Inlines.Add(new Run(text));
            }
            else
            {
                while (true)
                {
                    paragraph.Inlines.AddRange(new Inline[] {
                    new Run(text.Substring(0, index)),
                    new Run(text.Substring(index, highlightText.Length)) {Background = selectionColor,
                        Foreground = forecolor}
                });

                    text = text.Substring(index + highlightText.Length);
                    index = text.IndexOf(highlightText, StringComparison.CurrentCultureIgnoreCase);

                    if (index < 0)
                    {
                        paragraph.Inlines.Add(new Run(text));
                        break;
                    }
                }

            }


            doc.Blocks.Add(paragraph);
        }

        public static Brush GetHighlightColor(DependencyObject obj)
        {
            return (Brush)obj.GetValue(HighlightColorProperty);
        }

        public static void SetHighlightColor(DependencyObject obj, Brush value)
        {
            obj.SetValue(HighlightColorProperty, value);
        }

        public static readonly DependencyProperty HighlightColorProperty =
            DependencyProperty.RegisterAttached("HighlightColor", typeof(Brush), typeof(RichtextboxHighlighter)
               );


        public static Brush GetForecolor(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ForecolorProperty);
        }

        public static void SetForecolor(DependencyObject obj, Brush value)
        {
            obj.SetValue(ForecolorProperty, value);
        }

        public static readonly DependencyProperty ForecolorProperty =
            DependencyProperty.RegisterAttached("Forecolor", typeof(Brush), typeof(RichtextboxHighlighter)
                );
    }
}
