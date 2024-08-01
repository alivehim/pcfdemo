using System.Windows;
using System.Windows.Media;

namespace UtilityTools.Controls.Ribbon.UI
{
    /// <summary>
    /// Interaction logic for UtilityWindow.xaml
    /// </summary>
    public  class UtilityWindow : Window
    {
        public Brush AccentBrush
        {
            get { return (Brush)GetValue(AccentBrushProperty); }
            set { SetValue(AccentBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AccentBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AccentBrushProperty =
            DependencyProperty.Register("AccentBrush", typeof(Brush), typeof(UtilityWindow), new PropertyMetadata(null));

        public Brush HoverBrush
        {
            get { return (Brush)GetValue(HoverBrushProperty); }
            set { SetValue(HoverBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverBrushProperty =
            DependencyProperty.Register("HoverBrush", typeof(Brush), typeof(UtilityWindow), new PropertyMetadata(null));


        public UtilityWindow()
        {
            
        }
    }
}
