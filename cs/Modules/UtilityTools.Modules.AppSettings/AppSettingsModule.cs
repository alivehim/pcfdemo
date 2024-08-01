using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Definition;
using UtilityTools.Modules.AppSettings.Views;

namespace UtilityTools.Modules.AppSettings
{

    [ModuleDependency("Splash")]
    [ModuleOrder(1)]
    [Module(ModuleName = "AppSettings")]
    public class AppSettingsModule : IModule
    {
        private const string workspaceName = "AppSettings";
        private readonly IRegionManager _regionManager;


        public AppSettingsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.Regions[RegionNames.WorkspaceRegion].Add(containerProvider.Resolve<AppSettingsView>(), ModuleName.AppSettings.ToString());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AppSettingsView>();
        }

    }
}
