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
using UtilityTools.Modules.DataViewer.Views;

namespace UtilityTools.Modules.DataViewer
{
    [ModuleDependency("Splash")]
    [ModuleOrder(3)]
    [Module(ModuleName = "DataViewer")]
    public class DataViewerModule : IModule
    {
        private const string workspaceName = "DataViewer";
        private readonly IRegionManager _regionManager;


        public DataViewerModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.Regions[RegionNames.WorkspaceRegion].Add(containerProvider.Resolve<DataView>(), ModuleName.DataViewer.ToString());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<DataView>();
        }

    }
}
