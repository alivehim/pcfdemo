using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core;
using UtilityTools.Core.Definition;
using UtilityTools.Modules.DataManager.Views;

namespace UtilityTools.Modules.DataManager
{
    [ModuleDependency("WebResouce")]
    [Module(ModuleName = "DataManager")]
    public class DataManagerModule : IModule
    {
        private const string workspaceName = "DataManager";
        private readonly IRegionManager _regionManager;


        public DataManagerModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.Regions[RegionNames.WorkspaceRegion].Add(containerProvider.Resolve<DataManagerView>(), ModuleName.DataManager.ToString());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<DataManagerView>();
        }
    }
}
