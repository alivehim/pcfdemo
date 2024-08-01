using CefSharp;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UtilityTools.CEF.Handlers;

namespace UtilityTools.Modules.PowerPlatform.Views
{
    /// <summary>
    /// Interaction logic for PowerPlatformView.xaml
    /// </summary>
    public partial class PowerPlatformView : UserControl
    {
        public PowerPlatformView()
        {
            InitializeComponent();

            Browser.FrameLoadEnd += WebBrowserFrameLoadEnded;

            Browser.RequestHandler = new MonitorNetworkRequestHandler();
        }

        private void Browser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {

                Browser.LoadingStateChanged -= Browser_LoadingStateChanged;




            }
        }

        private async void WebBrowserFrameLoadEnded(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                //Browser.ViewSource();
                await Browser.GetSourceAsync().ContinueWith(taskHtml =>
                {
                    var html = taskHtml.Result;
                });



            }
        }
    }
}
