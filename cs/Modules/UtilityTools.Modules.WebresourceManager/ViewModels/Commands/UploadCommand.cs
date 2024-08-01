using System;
using System.Windows.Input;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Extensions;
using UtilityTools.Services.Interfaces.D365;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Modules.WebresourceManager.ViewModels.Commands
{
    public class UploadCommand : ICommand
    {
        private readonly FileUXItemDescription fileUXItemDescription;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public UploadCommand(FileUXItemDescription fileUXItemDescription)
        {
            this.fileUXItemDescription = fileUXItemDescription;
        }

        public bool CanExecute(object parameter)
        {
            return !fileUXItemDescription.IsWaiting;
        }


        /// <summary>
        /// upload webresource file to server
        /// </summary>
        /// <param name="parameter">token</param>
        public void Execute(object parameter)
        {
            var webresourceservice = ToolsContext.Current.UnityContainer.ResolveService<IWebresourceServers>();
            var logService = ToolsContext.Current.UnityContainer.ResolveService<IMessageStreamProvider<IUXMessage>>();
            if (string.IsNullOrEmpty(fileUXItemDescription.ObjectId))
            {
                logService.Error(fileUXItemDescription, "can not find objectid");
                fileUXItemDescription.Error();
                return;
            }
            fileUXItemDescription.TaskStage = Core.Models.TaskStage.Doing;


            fileUXItemDescription.IsWaiting = true;
            Task.Run(async () =>
            {
                //publish 
                await webresourceservice.UploadFileAsync(fileUXItemDescription, fileUXItemDescription.ObjectId, Settings.Current.D365AccessToken, fileUXItemDescription.FullName);
                await webresourceservice.PublishAsync(fileUXItemDescription, fileUXItemDescription.ObjectId, Settings.Current.D365AccessToken);

            }).ContinueWith(token =>
            {
                if (token.IsFaulted) {

                    logService.Error(fileUXItemDescription, token.Exception.ToString());
                    fileUXItemDescription.Error();
                    fileUXItemDescription.IsWaiting = false;
                    return;
                }
                fileUXItemDescription.Done();
                fileUXItemDescription.IsWaiting = false;

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
