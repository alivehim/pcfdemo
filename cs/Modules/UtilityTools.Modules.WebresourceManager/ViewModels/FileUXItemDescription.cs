using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Mvvm;
using UtilityTools.Modules.WebresourceManager.ViewModels.Commands;

namespace UtilityTools.Modules.WebresourceManager.ViewModels
{
    public class FileUXItemDescription : BaseUXItemDescription
    {
        public string FileName { get; set; }
        public string FullName { get; set; }
        public string ObjectId { get; set; }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged("Name");

            }
        }

        private string displayName { get; set; }
        public string DisplayName {
            get
            {
                return displayName;
            }
            set
            {
                displayName = value;
                RaisePropertyChanged("DisplayName");

            }
        }
        public ICommand UploadCommand { get; set; }
        public ICommand PublishCommand { get; set; }
        public FileUXItemDescription()
        {
            UploadCommand = new UploadCommand(this);
            PublishCommand = new PublishCommand(this);
        }

    }
}
