using CefSharp;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Data.Domain;

namespace UtilityTools.CEF
{
    /// <summary>
    /// Interaction logic for BaiduTranslateWindow.xaml
    /// </summary>
    public partial class BaiduTranslateWindow : Window, INotifyPropertyChanged
    {
        private string address;
        private string searchText;
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                RaisePropertyChangedEvent("Address");
            }
        }


        public BaiduTranslateWindow(string searchText)
        {
            this.searchText = searchText;
            InitializeComponent();
            DataContext = this;

            Init();
        }

        private void Init()
        {
            Address = "https://fanyi.baidu.com/#en/zh/";
            Browser.LoadingStateChanged += Browser_LoadingStateChanged;
        }

        private async void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {

                try
                {
                    //await Browser.WaitForInitialLoadAsync();
                    Browser.LoadingStateChanged -= Browser_LoadingStateChanged;
                    Browser.ExecuteScriptAsync($"document.getElementById('baidu_translate_input').value='{searchText}';");


                    var script = @"document.getElementsByClassName('trans-btn trans-btn-zh')[0].offsetLeft";
                    var script1 = @"document.getElementsByClassName('trans-btn trans-btn-zh')[0].offsetTop";

                    var xPositionResponse = await Browser.EvaluateScriptAsync(script);
                    var yPositionResponse = await Browser.EvaluateScriptAsync(script1);

                    var xpos = (int)xPositionResponse.Result;
                    var ypos = (int)yPositionResponse.Result;

                    string scriptpos1 = $@"oEvent = document.createEvent(""MouseEvents"");oEvent.initMouseEvent(""click"", true, true, document.defaultView, 0, 0, 0,{xpos + 2  }, {ypos + 2});document.getElementsByClassName('trans-btn trans-btn-zh')[0].dispatchEvent(oEvent);";
                    await Browser.EvaluateScriptAsync(scriptpos1);

                }
                catch (Exception ex)
                {

                }

            }
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
