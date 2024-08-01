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
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.CEF
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ShoushuWpfWinfow : Window, INotifyPropertyChanged
    {

        //private AutoResetEvent islogined = new AutoResetEvent(false);
        private string address;
        private ResourceUser user;

        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                RaisePropertyChangedEvent("Address");
            }
        }

        public ShoushuWpfWinfow(ResourceUser resourceUser)
        {
            InitializeComponent();
            DataContext = this;
            user = resourceUser;
            Init();
        }

        public ShoushuWpfWinfow(string address)
        {
            InitializeComponent();
            DataContext = this;

            Address = address;
        }


        private void Init()
        {
            //Address = @"https://ledaidai.erenebenshu.com";

            var mediaSymbolService = ToolsContext.Current.ResolveService<IMediaSymbolDBService>();
            var symbol = mediaSymbolService.GetMediaSymbol(Core.Models.MediaSymbolType.Soushu);

            Address = symbol.Address;

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
                    //if (Address.Contains(Settings.Current.ShoushuUrl))
                    {
                        Console.WriteLine("模拟 登录 ......");
                        Browser.ExecuteScriptAsync($"document.getElementById('ls_username').value='{user.Name}';");
                        Browser.ExecuteScriptAsync($"document.getElementById('ls_password').value='{user.Password}';");

                        var script = @"document.getElementsByClassName('pn vm')[0].offsetLeft";
                        var script1 = @"document.getElementsByClassName('pn vm')[0].offsetTop";

                        var xPositionResponse = await Browser.EvaluateScriptAsync(script);
                        var yPositionResponse = await Browser.EvaluateScriptAsync(script1);

                        var xpos = (int)xPositionResponse.Result;
                        var ypos = (int)yPositionResponse.Result;

                        string scriptpos1 = $@"oEvent = document.createEvent(""MouseEvents"");oEvent.initMouseEvent(""click"", true, true, document.defaultView, 0, 0, 0,{xpos + 2}, {ypos + 2});document.getElementsByClassName('pn vm')[0].dispatchEvent(oEvent);";
                        await Browser.EvaluateScriptAsync(scriptpos1);
                        Console.WriteLine("等待 登录 结果 ......");

                    }
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