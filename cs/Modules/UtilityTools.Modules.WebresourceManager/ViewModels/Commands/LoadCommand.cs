using System;
using System.Windows.Input;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models.TaskSchedule;

namespace UtilityTools.Modules.WebresourceManager.ViewModels.Commands
{
    public class LoadCommand : ICommand
    {
        private WebResourceManagerViewModel _viewModel;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public LoadCommand(WebResourceManagerViewModel viewModel)
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
            if (!string.IsNullOrEmpty(_viewModel.Url))
            {
                _viewModel.PageList.Clear();
                _viewModel.IsWaiting = true;

                _viewModel.SearchHistory2DB(_viewModel.Url);

                var taskContext = new TaskContext(_viewModel.CancelTokenSource, Core.Models.MessageOwner.WebresourceManager, _viewModel.ModuleId)
                {
                    Key= _viewModel.Url
                };

                _viewModel.TaskManager.StartNew(taskContext);
            }
        }
    }
}
