using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.TaskSchedule;

namespace UtilityTools.Modules.DataManager.ViewModels.Commands
{
    public class LoadCommand:ICommand
    {
        private DataManagerViewModel _viewModel;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public LoadCommand(DataManagerViewModel viewModel)
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
            var authorityToken = parameter as string;
            if (!string.IsNullOrEmpty(authorityToken))
            {
                _viewModel.PageList.Clear();
                _viewModel.IsWaiting = true;

                var taskContext = new TaskContext(_viewModel.CancelTokenSource, _viewModel.GetMessageOwner(),_viewModel.ModuleId)
                {
                    Key = authorityToken,
                };

                _viewModel.TaskManager.StartNew(taskContext);
            }
        }
    }
}
