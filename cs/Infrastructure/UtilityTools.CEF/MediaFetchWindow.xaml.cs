using CefSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
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
using UtilityTools.Services.Interfaces;

namespace UtilityTools.CEF
{
    /// <summary>
    /// Interaction logic for MediaFectchWindow.xaml
    /// </summary>
    public partial class MediaFetchWindow : Window, ICEFCallback, INotifyPropertyChanged
    {
        private string address;

        private IStreamUXItemDescription streamUXItemDescription;
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

        public MediaFetchWindow(IStreamUXItemDescription streamUXItemDescription)
        {
            InitializeComponent();

            this.streamUXItemDescription = streamUXItemDescription;
            DataContext = this;
            Address = streamUXItemDescription.StreamUri;
            Browser.RequestHandler = new NoodlemagazineHandler(this);
            Browser.LoadingStateChanged += Browser_LoadingStateChanged;

            Browser.LoadError += OnWebBrowserLoadError;
        }


        private async void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {

                try
                {
                    //await Browser.WaitForInitialLoadAsync();
                    Browser.LoadingStateChanged -= Browser_LoadingStateChanged;

                    HtmlSource = await Browser.GetSourceAsync();

                   
                }
                catch (Exception ex)
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

        public async void Action(string url)
        {
            var dispatcher = Application.Current.Dispatcher;
            await dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                streamUXItemDescription.StreamUri = url;
                streamUXItemDescription.TaskStage = Core.Models.TaskStage.Prepared;
                this.Close();
            }));

        }
    }
}
