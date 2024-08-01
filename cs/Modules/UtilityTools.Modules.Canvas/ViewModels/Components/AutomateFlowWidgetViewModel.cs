using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using UtilityTools.Core.Mvvm;

namespace UtilityTools.Modules.Canvas.ViewModels.Components
{
    public class AutomateFlowWidgetViewModel : BindableBase
    {
        private string caption;
        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
                RaisePropertyChanged("Caption");
            }
        }


        private string outputContent;
        public string OutputContent
        {
            get
            {
                return outputContent;
            }
            set
            {
                outputContent = value;
                RaisePropertyChanged("OutputContent");
            }
        }


        public ICommand TransferCommand => new DelegateCommand((obj) =>
        {
            OutputContent = Caption.Replace(" ", "_");

            Clipboard.SetDataObject(OutputContent);
        });

        public ICommand StyleVariableCommand => new DelegateCommand((obj) =>
        {
            OutputContent = $"variables('{Caption.Replace(" ", "_")}')";
            Clipboard.SetDataObject(OutputContent);
        });

        public ICommand StyleOutputCommand => new DelegateCommand((obj) =>
        {
            OutputContent = $"outputs('{Caption.Replace(" ", "_")}')";
            Clipboard.SetDataObject(OutputContent);
        });
        public ICommand StyleListOutputCommand => new DelegateCommand((obj) =>
               {
                   OutputContent = $"outputs('{Caption.Replace(" ", "_")}')?['body/value']";
                   Clipboard.SetDataObject(OutputContent);
               });

        public AutomateFlowWidgetViewModel()
        {

        }
    }
}
