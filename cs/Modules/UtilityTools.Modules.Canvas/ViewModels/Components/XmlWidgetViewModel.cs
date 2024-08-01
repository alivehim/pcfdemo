using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using UtilityTools.Core.Mvvm;
using UtilityTools.Services.Interfaces.D365;

namespace UtilityTools.Modules.Canvas.ViewModels.Components
{
    public class XmlWidgetViewModel : BindableBase
    {
        private readonly IDynamicsService dynamicsService;


        private bool isShowJson;
        public bool IsShowJson
        {
            get
            {
                return isShowJson;
            }
            set
            {
                isShowJson = value;
                RaisePropertyChanged("IsShowJson");
            }
        }

        private string xmlContent;
        public string XmlContent
        {
            get
            {
                return xmlContent;
            }
            set
            {
                xmlContent = value;
                RaisePropertyChanged("XmlContent");
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

        private string outputJson;
        public string OutputJson
        {
            get
            {
                return outputJson;
            }
            set
            {
                outputJson = value;
                RaisePropertyChanged("OutputJson");
            }
        }


        public ICommand TransferCommand => new DelegateCommand((obj) =>
        {
            OutputContent = XmlContent.Replace("\"", "\\\"").Replace("\r\n", "");

            Clipboard.SetDataObject(OutputContent);
            IsShowJson = false;
        });

        public ICommand ExcuteCommand => new DelegateCommand(async (obj) =>
        {
            try
            {
                var result = await dynamicsService.FetchXml(XmlContent);
                Clipboard.SetDataObject(result);
                OutputJson = result;
                IsShowJson = true;
            }
            catch
            {
                OutputJson = string.Empty;
                IsShowJson = true;
            }
        });
        public ICommand ExcuteODataCommand => new DelegateCommand(async (obj) =>
        {
            try
            {
                var result = await dynamicsService.FetchOdata(XmlContent);
                Clipboard.SetDataObject(result);
                OutputJson = result;
                IsShowJson = true;
            }
            catch
            {
                OutputJson = string.Empty;
                IsShowJson = true;
            }
           
        });

 
        public XmlWidgetViewModel(IDynamicsService dynamicsService)
        {
            this.dynamicsService = dynamicsService;
        }
    }
}
