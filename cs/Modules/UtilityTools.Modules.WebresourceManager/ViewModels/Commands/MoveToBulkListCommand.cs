using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Infrastructure;

namespace UtilityTools.Modules.WebresourceManager.ViewModels.Commands
{
    public class MoveToBulkListCommand : ICommand
    {
        private WebResourceManagerViewModel _viewModel;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public MoveToBulkListCommand(WebResourceManagerViewModel viewModel)
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
            _viewModel.PublishingTaskList.Clear();

            foreach(var item in _viewModel.PageList.Where(p=>_viewModel.MoveToTaskListFilter(p)))
            {
                _viewModel.PublishingTaskList.Add(item);
            }
         }
    }
}
