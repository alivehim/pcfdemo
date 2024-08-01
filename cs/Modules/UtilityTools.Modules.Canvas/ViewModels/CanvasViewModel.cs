using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UtilityTools.Core;
using UtilityTools.Core.Definition;
using UtilityTools.Core.Events;
using UtilityTools.Core.Mvvm;

namespace UtilityTools.Modules.Canvas.ViewModels
{
    public class CanvasViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        public CanvasViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;

            eventAggregator.GetEvent<WidgeModuleChange>().Subscribe(RegionChanged);

        }

        private void RegionChanged(string region)
        {
            ActivateView(region);
        }

        public ICommand OpenXmlRegionCommand => new DelegateCommand((obj) =>
        {
            ActivateView(WidgetModuleName.XmlRegion);
        });

        public ICommand OpenAutomateFlowRegionCommand => new DelegateCommand((obj) =>
        {
            ActivateView(WidgetModuleName.AutomateFlowRegion);
        });

        public ICommand OpenDictionaryCommand => new DelegateCommand((obj) =>
        {
            ActivateView(WidgetModuleName.DictionaryRegion);
        });
        public ICommand OpenTexttoSpeechCommand => new DelegateCommand((obj) =>
        {
            ActivateView(WidgetModuleName.TexttoSpeech);
        });
        public ICommand OpenTextFormatCommand => new DelegateCommand((obj) =>
        {
            ActivateView(WidgetModuleName.TextFormat);
        });
        public ICommand OpenFlowHistoryCommand => new DelegateCommand((obj) =>
               {
                   ActivateView(WidgetModuleName.FlowHistory);
               });

   public ICommand OpenDocxCommand => new DelegateCommand((obj) =>
               {
                   ActivateView(WidgetModuleName.Docx);
               });

        public IRegionManager RegionManager => regionManager;

        public void ActivateView(WidgetModuleName module)
        {

            // Deactivate current view
            IRegion workspaceRegion = RegionManager.Regions[RegionNames.WidgetRegion];
            var views = workspaceRegion.Views;
            foreach (var view in views)
            {
                workspaceRegion.Deactivate(view);
            }

            // Activate named view
            var viewToActivate = RegionManager.Regions[RegionNames.WidgetRegion].GetView(module.ToString());
            RegionManager.Regions[RegionNames.WidgetRegion].Activate(viewToActivate);

            //this.activeWorkspace = viewName;
        }

        public void ActivateView(string region)
        {

            // Deactivate current view
            IRegion workspaceRegion = RegionManager.Regions[RegionNames.WidgetRegion];
            var views = workspaceRegion.Views;
            foreach (var view in views)
            {
                workspaceRegion.Deactivate(view);
            }

            // Activate named view
            var viewToActivate = RegionManager.Regions[RegionNames.WidgetRegion].GetView(region);
            if (viewToActivate != null)
            {

                RegionManager.Regions[RegionNames.WidgetRegion].Activate(viewToActivate);
            }

            //this.activeWorkspace = viewName;
        }
    }
}
