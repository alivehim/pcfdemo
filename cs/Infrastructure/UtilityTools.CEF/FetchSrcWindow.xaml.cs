using CefSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using UtilityTools.CEF.Handlers;
using UtilityTools.Core.Models.TaskSchedule;

namespace UtilityTools.CEF
{
    /// <summary>
    /// Interaction logic for FetchSrcWindow.xaml
    /// </summary>
    public partial class FetchSrcWindow : Window, INotifyPropertyChanged, IImageContainer
    {
        private string address;

        public string HtmlSource = string.Empty;
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                RaisePropertyChangedEvent("Address");
            }
        }

        public IDictionary<string, byte[]> ImageSources { get; set; } = new Dictionary<string, byte[]>();

        public FetchSrcWindow(string url)
        {
            InitializeComponent();
            DataContext = this;
            Address = url;
            Browser.RequestHandler = new JAVRequestHandler(this);
            Browser.LoadingStateChanged += Browser_LoadingStateChanged;

            Browser.LoadError += OnWebBrowserLoadError;
        }

        private  async void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {

                try
                {
                    //await Browser.WaitForInitialLoadAsync();
                    HtmlSource = await Browser.GetSourceAsync();

                    var dispatcher = Application.Current.Dispatcher;

                    if(HtmlSource== "<html><head></head><body></body></html>")
                    {
                        return;
                    }
                    if (!HtmlSource.Contains("Sign in"))
                    {
                        //Browser.LoadingStateChanged -= Browser_LoadingStateChanged;

                        //await dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                        //{
                        //    this.Close();
                        //}));
                    }
                  
                }
                catch 
                {
                    HtmlSource = string.Empty;
                }

            }
        }

        private void OnWebBrowserLoadError(object sender, LoadErrorEventArgs args)
        {
             Console.WriteLine($"{args.ErrorCode}--{args.ErrorText}");
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

        private  void Window_Closed(object sender, EventArgs e)
        {
        }
    }
}
