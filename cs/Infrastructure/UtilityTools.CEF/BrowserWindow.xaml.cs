using CefSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UtilityTools.Core.Extensions;

namespace UtilityTools.CEF
{
    /// <summary>
    /// Interaction logic for BrowserWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window, INotifyPropertyChanged
    {
        private string address;

        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                RaisePropertyChangedEvent("Address");
            }
        }

        public BrowserWindow(string url)
        {
            InitializeComponent();
            DataContext = this;
            Address = url;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.CenterInScreen();
        }
    }
}
