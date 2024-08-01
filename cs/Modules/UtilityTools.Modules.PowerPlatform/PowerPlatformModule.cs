using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Definition;
using UtilityTools.Core;
using UtilityTools.Modules.PowerPlatform.Views;

namespace UtilityTools.Modules.PowerPlatform
{
    [ModuleDependency("Splash")]
    [Module(ModuleName = "PowerPlatform")]
    public class PowerPlatformModule : IModule
    {
        private const string workspaceName = "PowerPlatform";
        private readonly IRegionManager _regionManager;


        public PowerPlatformModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.Regions[RegionNames.WorkspaceRegion].Add(containerProvider.Resolve<PowerPlatformView>(), ModuleName.PowerPlatform.ToString());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PowerPlatformView>();

        }
    }
}
