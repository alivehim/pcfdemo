using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Events;
using UtilityTools.Modules.Splash.ViewModels;
using UtilityTools.Modules.Splash.Views;
using UtilityTools.Services.Interfaces;

namespace UtilityTools.Modules.Splash
{
    [ModuleOrder(1)]
    [Module(ModuleName = "Splash")]
    public class SplashModule : IModule
    {
        private const string workspaceName = "Splash";
        private readonly IRegionManager _regionManager;
        private readonly IContainerProvider Container;
        private readonly IEventAggregator eventAggregator;
        //private IShell Shell;
        private AutoResetEvent WaitForCreation { get; set; }
        public SplashModule(IRegionManager regionManager, IContainerProvider containerProvider, IEventAggregator eventAggregator/*,IShell shell*/)
        {
            _regionManager = regionManager;
            Container = containerProvider;
            //Shell = shell;
            this.eventAggregator = eventAggregator;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //_regionManager.Regions[RegionNames.WorkspaceRegion].Add(containerProvider.Resolve<DataView>(), ModuleName.DataViewer.ToString());

            Dispatcher.CurrentDispatcher.BeginInvoke(
       (Action)(() =>
       {
           //Shell.Show();
           eventAggregator.GetEvent<CloseSplashEvent>().Publish();
       }));

            WaitForCreation = new AutoResetEvent(false);

            ThreadStart showSplash =
              () =>
              {
                  Dispatcher.CurrentDispatcher.BeginInvoke(
                    (Action)(() =>
                    {
                       
                        var splash = Container.Resolve<SplashView>();
                        eventAggregator.GetEvent<CloseSplashEvent>().Subscribe(
                            () => splash.Dispatcher.BeginInvoke(()=> { 
                                splash.Close();
                            }),
                             ThreadOption.PublisherThread, true);

                        splash.Show();

                        WaitForCreation.Set();
                    }));

                  Dispatcher.Run();
              };

            var thread = new Thread(showSplash) { Name = "Splash Thread", IsBackground = true };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            WaitForCreation.WaitOne();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SplashView>();
        }
    }
}
