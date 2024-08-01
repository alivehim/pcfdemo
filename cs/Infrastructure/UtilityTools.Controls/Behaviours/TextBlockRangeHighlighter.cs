using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using UtilityTools.Core.Infrastructure;


namespace UtilityTools.Controls.Behaviours
{
    public static class TextBlockRangeHighlighter
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
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(TextBlockRangeHighlighter),
                new PropertyMetadata(new PropertyChangedCallback(TextChanged)));


        private  static string changeValue="";
        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (e.NewValue != null && !string.IsNullOrEmpty(e.NewValue.ToString()))
            {
                var control = d as RichTextBox;
                if (control != null)
                {

                    var doc = control.Document as FlowDocument;
                 
                    string value = e.NewValue.ToString();

                    if (!string.IsNullOrEmpty(changeValue) && changeValue.Trim() == value.Trim())
                        return;

                    doc.Blocks.Clear();
                    changeValue = value;
                    Brush selectionColor = (Brush)d.GetValue(HighlightColorProperty);
                    Brush forecolor = (Brush)d.GetValue(ForecolorProperty);


                    var paragraph = new Paragraph();


                    if (value.Length > Settings.Current.TTSSelectCount)
                    {
                        paragraph.Inlines.AddRange(new Inline[] {
                            new Run(value.Substring(0, Settings.Current.TTSSelectCount)) {Background = selectionColor,Foreground = forecolor},
                            //new Run(value.Substring(10000, 10000)) {Background = new SolidColorBrush(Colors.Beige), Foreground = forecolor},
                            //new Run(value.Substring(20000,10000))  {Background = new SolidColorBrush(Colors.AliceBlue), Foreground = forecolor },
                            new Run(value.Substring(Settings.Current.TTSSelectCount))
                        });

                        doc.Blocks.Add(paragraph);
                    }
                    //else
                    //if (value.Length > 16000)
                    //{
                    //    paragraph.Inlines.AddRange(new Inline[] {
                    //         new Run(value.Substring(0, 8000)) {Background = selectionColor,
                    //            Foreground = forecolor},
                    //        new Run(value.Substring(8000, 8000)) {Background = new SolidColorBrush(Colors.Beige),
                    //            Foreground = forecolor},
                    //        new Run(value.Substring(16000))  
                    //    });
                    //    doc.Blocks.Add(paragraph);
                    //}
                    //else if (value.Length > 8000)
                    //{
                    //    paragraph.Inlines.AddRange(new Inline[] {
                    //         new Run(value.Substring(0, 8000)) {Background = selectionColor,
                    //            Foreground = forecolor},
                    //        new Run(value.Substring(8000))  
                    //    });
                    //    doc.Blocks.Add(paragraph);
                    //}
                    else
                    {
                        paragraph.Inlines.AddRange(new Inline[] {
                            new Run(value)
                        });

                        doc.Blocks.Add(paragraph);
                    }

                }
            }


            //if (d == null) return;
            //if (!(d is TextBlock)) throw new InvalidOperationException("Only valid for TextBlock");

            //TextBlock txtBlock = d as TextBlock;
            //string text = txtBlock.Text;
            //if (string.IsNullOrEmpty(text)) return;

            //string highlightText = txtBlock.Text.Substring(0, 1000);
            //if (string.IsNullOrEmpty(highlightText)) return;


            //int index = text.IndexOf(highlightText, StringComparison.CurrentCultureIgnoreCase);
            //if (index < 0) return;

            //int index = 1000;

            //Brush selectionColor = (Brush)d.GetValue(HighlightColorProperty);
            //Brush forecolor = (Brush)d.GetValue(ForecolorProperty);

            //txtBlock.Inlines.Clear();
            //while (true)
            //{
            //    txtBlock.Inlines.AddRange(new Inline[] {
            //        new Run(text.Substring(0, index)) {Background = selectionColor,
            //            Foreground = forecolor},
            //        new Run(text.Substring(index)) {Background = selectionColor,
            //            Foreground = forecolor}
            //    });

            //    //text = text.Substring(index + highlightText.Length);
            //    //index = text.IndexOf(highlightText, StringComparison.CurrentCultureIgnoreCase);

            //    //if (index < 0)
            //    //{
            //    //    txtBlock.Inlines.Add(new Run(text));
            //    //    break;
            //    //}
            //}
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
            DependencyProperty.RegisterAttached("HighlightColor", typeof(Brush), typeof(TextBlockRangeHighlighter)
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
            DependencyProperty.RegisterAttached("Forecolor", typeof(Brush), typeof(TextBlockRangeHighlighter)
                );

    }
}
