using System.Collections.Specialized;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UtilityTools.Controls.Ribbon.UI;
using UtilityTools.Services.Interfaces;

namespace UtilityTools.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UtilityWindow, IShell
    {
        //#region denpency object 

        //public Brush AccentBrush
        //{
        //    get { return (Brush)GetValue(AccentBrushProperty); }
        //    set { SetValue(AccentBrushProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for AccentBrush.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty AccentBrushProperty =
        //    DependencyProperty.Register("AccentBrush", typeof(Brush), typeof(MainWindow), new PropertyMetadata(null));

        //public Brush HoverBrush
        //{
        //    get { return (Brush)GetValue(HoverBrushProperty); }
        //    set { SetValue(HoverBrushProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for HoverBrush.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty HoverBrushProperty =
        //    DependencyProperty.Register("HoverBrush", typeof(Brush), typeof(MainWindow), new PropertyMetadata(null));


        //#endregion

        public MainWindow()
        {
            this.WindowState = System.Windows.WindowState.Maximized;
            InitializeComponent();

            ((INotifyCollectionChanged)loglist.Items).CollectionChanged += MainWindow_CollectionChanged;

            //https://www.cnblogs.com/choumengqizhigou/p/15739993.html
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (_, __) => { SystemCommands.CloseWindow(this); }));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (_, __) => { SystemCommands.MinimizeWindow(this); }));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (_, __) => { SystemCommands.MaximizeWindow(this); }));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (_, __) => { SystemCommands.RestoreWindow(this); }));
            CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));


        }

        private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            if (element == null)
                return;

            var position = WindowState == WindowState.Maximized ? new Point(0, element.ActualHeight)
                : new Point(Left + BorderThickness.Left, element.ActualHeight + Top + BorderThickness.Top);
            position = element.TransformToAncestor(this).Transform(position);
            SystemCommands.ShowSystemMenu(this, position);
        }

        private void MainWindow_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //if (VisualTreeHelper.GetChildrenCount(loglist) > 0)
            //{
            //    FrameworkElement border = (FrameworkElement)VisualTreeHelper.GetChild(loglist, 0);
            //    if (border != null)
            //    {
            //        if (VisualTreeHelper.GetChild(border, 0) is ScrollViewer)
            //        {
            //            ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
            //            scrollViewer.ScrollToBottom();

            //        }
            //    }

            //}
            //loglist.SelectedIndex = loglist.Items.Count - 1;
            //loglist.ScrollIntoView(loglist.SelectedItem);

            //https://stackoverflow.com/questions/2337822/wpf-listbox-scroll-to-end-automatically

            ListBoxAutomationPeer svAutomation = (ListBoxAutomationPeer)ScrollViewerAutomationPeer.CreatePeerForElement(loglist);

            IScrollProvider scrollInterface = (IScrollProvider)svAutomation.GetPattern(PatternInterface.Scroll);
            System.Windows.Automation.ScrollAmount scrollVertical = System.Windows.Automation.ScrollAmount.LargeIncrement;
            System.Windows.Automation.ScrollAmount scrollHorizontal = System.Windows.Automation.ScrollAmount.NoAmount;
            //If the vertical scroller is not available, the operation cannot be performed, which will raise an exception. 
            if (scrollInterface.VerticallyScrollable)
                scrollInterface.Scroll(scrollHorizontal, scrollVertical);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //double controlsize = ((SystemParameters.PrimaryScreenWidth / 12) / 3 * 2) / 5 * 0.7;
            //System.Windows.Application.Current.Resources.Remove("ControlFontSize");
            //System.Windows.Application.Current.Resources.Add("ControlFontSize", controlsize);

            System.Windows.Application.Current.Resources.Remove("ControlFontSize");
            System.Windows.Application.Current.Resources.Add("ControlFontSize", 16d);
        }

    }
}
