using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Definition;
using UtilityTools.Core;
using UtilityTools.Modules.AutomateFlow.Views;

namespace UtilityTools.Modules.AutomateFlow
{
    [ModuleDependency("Splash")]
    [ModuleOrder(3)]
    [Module(ModuleName = "AutomateFlow")]
    public class AutomateFlowModule : IModule
    {
        private const string workspaceName = "AutomateFlow";
        private readonly IRegionManager _regionManager;


        public AutomateFlowModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.Regions[RegionNames.WorkspaceRegion].Add(containerProvider.Resolve<AutomateFlowView>(), ModuleName.AutomateFlow.ToString());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AutomateFlowView>();
        }

    }
}
