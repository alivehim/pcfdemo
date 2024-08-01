using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UtilityTools.Modules.Navigator.ViewModels.Commands
{
    public class MenuClickCommand : ICommand
    {
        private readonly NavigatorViewModel _viewModel;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public MenuClickCommand(NavigatorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            var workspaceName = parameter as string;
            return _viewModel.ActiveWorkspace != workspaceName;
        }

        public void Execute(object parameter)
        {
            var workspaceName = parameter as string;

            _viewModel.ActivateView(workspaceName);
        }
    }
}
