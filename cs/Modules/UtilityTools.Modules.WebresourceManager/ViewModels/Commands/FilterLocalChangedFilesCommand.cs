using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Modules.WebresourceManager.ViewModels.Commands
{
    public class FilterLocalChangedFilesCommand : ICommand
    {
        private WebResourceManagerViewModel _viewModel;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public FilterLocalChangedFilesCommand(WebResourceManagerViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(Settings.Current.D365AccessToken) && _viewModel.PageList.Count != 0;
        }


        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            if (_viewModel.FileChangeList != null)
            {
                _viewModel.FileChangeList = null;
            }
            else
            {
                _viewModel.IsWaiting = true;

                var changedList = _viewModel.PageList
                    .Where(p => p.TaskStage == Core.Models.TaskStage.Prepared)
                    .Select(p => p.Name)
                    .ToList();
                _viewModel.FileChangeList = changedList;
                _viewModel.IsWaiting = false;

            }

        }
    }
}
