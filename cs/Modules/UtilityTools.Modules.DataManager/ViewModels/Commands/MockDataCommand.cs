using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Services.Interfaces.D365;

namespace UtilityTools.Modules.DataManager.ViewModels.Commands
{
    public class MockDataCommand : ICommand
    {
        private DataManagerViewModel _viewModel;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public MockDataCommand(DataManagerViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }


        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            var values = (object[])parameter;
            var token = (string)values[0];
            var description = (RequestDescription)values[1];
            if (!string.IsNullOrEmpty(token))
            {
                _viewModel.IsWaiting = true;
                description.TaskStage = Core.Models.TaskStage.Doing;
                var violaService = ToolsContext.Current.UnityContainer.ResolveService<IViolaService>();
                var eventAggregator = ToolsContext.Current.UnityContainer.ResolveService<IEventAggregator>();

                Task.Run(async () =>
                {
                    await violaService.UpdateSubmissDate(description.FormId, token);
                    await violaService.MockTimeLineDate(description.WFRequestId, token);

                }).ContinueWith((result) =>
                {
                    _viewModel.IsWaiting = false;
                    description.TaskStage = Core.Models.TaskStage.Done;
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
    }
}
