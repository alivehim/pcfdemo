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
    public class FilterChangedFilesCommand : ICommand
    {
        private WebResourceManagerViewModel _viewModel;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public FilterChangedFilesCommand(WebResourceManagerViewModel viewModel)
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

                var gitSerivce = ToolsContext.Current.UnityContainer.ResolveService<IGitService>();

                Task.Run(() =>
               {
                   return gitSerivce.GetChangedFiles(_viewModel.Url);

               }).ContinueWith(files =>
               {
                   if (files.IsFaulted) throw files.Exception;
                   _viewModel.FileChangeList = files.Result;
                   _viewModel.IsWaiting = false;

               }, TaskScheduler.FromCurrentSynchronizationContext());

            }

        }
    }
}
