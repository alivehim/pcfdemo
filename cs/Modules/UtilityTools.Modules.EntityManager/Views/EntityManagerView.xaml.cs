using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UtilityTools.Modules.EntityManager.ViewModels;

namespace UtilityTools.Modules.EntityManager.Views
{
    /// <summary>
    /// Interaction logic for EntityManagerView.xaml
    /// </summary>
    public partial class EntityManagerView : UserControl
    {
        public EntityManagerView()
        {
            InitializeComponent();

            fieldFilter.AddHandler(TextBox.MouseRightButtonDownEvent, new MouseButtonEventHandler(this.fileFilter_MouseRightButtonDown), true);

        }

        private void fileFilter_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            IDataObject data = Clipboard.GetDataObject();

            // 将数据与指定的格式进行匹配，返回bool
            if (data.GetDataPresent(DataFormats.Text))
            {
                // GetData检索数据并指定一个格式
                string test = (string)data.GetData(DataFormats.Text);

                this.fieldFilter.Text = test;
                e.Handled = true;
            }
            else
            {
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            var db = this.DataContext as EntityManagerViewModel;
            Observable.FromEventPattern<TextChangedEventArgs>(this.fileFilter, "TextChanged").Throttle(TimeSpan.FromSeconds(1)).Subscribe(x =>
                this.Dispatcher.Invoke(() =>
                {
                    var filterText = this.fileFilter.Text;
                    db.NameFilter = filterText;
                }));

            Observable.FromEventPattern<TextChangedEventArgs>(this.fieldFilter, "TextChanged").Throttle(TimeSpan.FromSeconds(1)).Subscribe(x =>
               this.Dispatcher.Invoke(() =>
               {
                   var filterText = this.fieldFilter.Text;
                   db.FieldNameFilter = filterText;
               }));

        }
    }
}
