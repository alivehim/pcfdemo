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
using UtilityTools.Modules.Canvas.ViewModels;
using UtilityTools.Modules.Canvas.Views;
using UtilityTools.Modules.Canvas.Views.Components;

namespace UtilityTools.Modules.Canvas
{
    [ModuleDependency("Splash")]
    [Module(ModuleName = "Canvas")]
    public class CanvasModule : IModule
    {
        private const string workspaceName = "Canvas";
        private readonly IRegionManager _regionManager;


        public CanvasModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.Regions[RegionNames.WorkspaceRegion].Add(containerProvider.Resolve<CanvasView>(), ModuleName.Canvas.ToString());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CanvasViewModel>();

            containerRegistry.RegisterDialog<CanvasOnenoteSectionDialog, CanvasOnenoteSectionDialogViewModel>();


            containerRegistry.RegisterForNavigation<XmlWidgetView>();
            containerRegistry.RegisterForNavigation<AutomateFlowWidgetView>();
            containerRegistry.RegisterForNavigation<DictionaryWidgetView>();
            containerRegistry.RegisterForNavigation<TexttoSpeechWidgetView>();
            containerRegistry.RegisterForNavigation<TextFormatWidgeView>();
            containerRegistry.RegisterForNavigation<DocxWidgetView>();

        }


    }
}
