using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Services.Interfaces.D365;
using UtilityTools.Core.Extensions;

namespace UtilityTools.Modules.EntityManager.ViewModels.Commands
{
    public class FetchTokenCommand : ICommand
    {
        private EntityManagerViewModel _viewModel;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public FetchTokenCommand(EntityManagerViewModel viewModel)
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
            _viewModel.IsWaiting = true;

            var webresourceservice = ToolsContext.Current.UnityContainer.ResolveService<IWebresourceServers>();

            Task.Run(async () =>
            {
                return await webresourceservice.GetTokenAsync();

            }).ContinueWith(token =>
            {
                if (token.IsFaulted) throw token.Exception;
                _viewModel.Token = token.Result;
                _viewModel.IsWaiting = false;

            }, TaskScheduler.FromCurrentSynchronizationContext());


        }
    }
}
