using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UtilityTools.Core;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Definition;
using UtilityTools.Modules.WebresourceManager.Views;

namespace UtilityTools.Modules.WebresouceManager
{
    [ModuleOrder(2)]
    [Module(ModuleName = "WebResouce")]
    public class WebresouceManagerModule : IModule
    {
        private const string workspaceName = "WebResouce";
        private readonly IRegionManager _regionManager;


        public WebresouceManagerModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //_regionManager.AddToRegion(RegionNames.WorkspaceRegion, containerProvider.Resolve<WebresouceManagerView>());

            _regionManager.Regions[RegionNames.WorkspaceRegion].Add(containerProvider.Resolve<WebResourceManagerView>(), ModuleName.WebResouce.ToString());

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<WebResourceManagerView>();
        }

    }
}
