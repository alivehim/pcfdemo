using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Services.Interfaces.D365;

namespace UtilityTools.Modules.WebresourceManager.ViewModels.Commands
{
    public class BulkPublishCommand : ICommand
    {
        private WebResourceManagerViewModel _viewModel;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public BulkPublishCommand(WebResourceManagerViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(Settings.Current.D365AccessToken) && _viewModel.PublishingTaskList.Count != 0;
        }


        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {


            //fileUXItemDescription.TaskStage = Core.Models.TaskStage.Doing;
            var webresourceservice = ToolsContext.Current.UnityContainer.ResolveService<IWebresourceServers>();

            Task.Run(async () =>
            {

                foreach (var item in _viewModel.PublishingTaskList)
                {

                    item.TaskStage = TaskStage.Doing;
                    item.IsWaiting = true;
                    await webresourceservice.UploadFileAsync(item, item.ObjectId, Settings.Current.D365AccessToken, item.FullName);
                    await webresourceservice.PublishAsync(item, item.ObjectId, Settings.Current.D365AccessToken);
                    item.TaskStage = TaskStage.Done;
                    item.IsWaiting = false;
                }

            }).ContinueWith(token =>
            {
                if (token.IsFaulted)
                {
                    foreach (var item in _viewModel.PublishingTaskList)
                    {
                        item.TaskStage = TaskStage.Error;
                        item.IsWaiting = false;
                    }

                    throw token.Exception;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
