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
using UtilityTools.Modules.EntityManager.Views;

namespace UtilityTools.Modules.EntityManager
{
    [ModuleDependency("Splash")]
    [ModuleDependency("WebResouce")]
    [ModuleOrder(3)]
    [Module(ModuleName = "EntityManager")]
    public class EntityManagerModule : IModule
    {
        private const string workspaceName = "EntityManager";
        private readonly IRegionManager _regionManager;


        public EntityManagerModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.Regions[RegionNames.WorkspaceRegion].Add(containerProvider.Resolve<EntityManagerView>(), ModuleName.EntityManager.ToString());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<EntityManagerView>();
        }

    }
}
