using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Infrastructure;

namespace UtilityTools.Modules.WebresourceManager.ViewModels.Commands
{
    public class ApppendToTaskListCommand : ICommand
    {
        private WebResourceManagerViewModel _viewModel;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ApppendToTaskListCommand(WebResourceManagerViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(Settings.Current.D365AccessToken);
        }


        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            if (!_viewModel.PublishingTaskList.Any(p => p == parameter))
            {
                _viewModel.PublishingTaskList.Add(parameter as FileUXItemDescription);
            }
        }
    }
}
