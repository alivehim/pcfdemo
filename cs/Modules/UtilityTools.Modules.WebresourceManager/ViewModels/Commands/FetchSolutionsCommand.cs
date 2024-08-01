using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Services.Interfaces.D365;

namespace UtilityTools.Modules.WebresourceManager.ViewModels.Commands
{
    public class FetchSolutionsCommand: ICommand
    {
        private WebResourceManagerViewModel _viewModel;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public FetchSolutionsCommand(WebResourceManagerViewModel viewModel)
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
            _viewModel.IsWaiting = true;

            var webresourceservice = ToolsContext.Current.UnityContainer.ResolveService<IWebresourceServers>();

            Task.Run(async () =>
            {
                return await webresourceservice.GetSolutionAsync(Settings.Current.D365AccessToken);

            }).ContinueWith(token =>
            {
                _viewModel.IsWaiting = false;
                if (token.IsFaulted) throw token.Exception;
                _viewModel.InitSolution(token.Result);

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
