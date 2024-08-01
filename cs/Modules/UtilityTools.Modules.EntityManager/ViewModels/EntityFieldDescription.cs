using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UtilityTools.Core.Mvvm;
using UtilityTools.Modules.EntityManager.ViewModels.Commands;

namespace UtilityTools.Modules.EntityManager.ViewModels
{
    public class EntityFieldDescription : BaseUXItemDescription
    {
        public EntityFieldDescription()
        {
            CopyCommand = new CopyFieldCommand(this);
        }

        public string EntityName { get; set; }
        public string Name { get; set; }

        public string ColumnType { get; set; }

        public string DisplayName { get; set; }

        public ICommand CopyCommand { get; set; }


    }
}
