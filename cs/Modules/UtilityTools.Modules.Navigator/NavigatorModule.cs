using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core;
using UtilityTools.Modules.Navigator.Views;

namespace UtilityTools.Modules.Navigator
{
    [Module(ModuleName = "Navigator")]
    [ModuleDependency("WebResouce")]
    [ModuleDependency("EntityManager")]
    //[ModuleDependency("DataManager")]
    [ModuleDependency("AppSettings")]
    [ModuleDependency("WebResouce")]
    [ModuleDependency("Splash")]
    public class NavigatorModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public NavigatorModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.NavigatorRegion, "NavigatorView");

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigatorView>();
        }
    }
}
