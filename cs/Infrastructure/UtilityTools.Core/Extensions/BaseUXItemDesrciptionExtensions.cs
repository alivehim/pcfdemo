using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Core.Mvvm;

namespace UtilityTools.Core.Extensions
{
    public static class BaseUXItemDesrciptionExtensions
    {
        public static void Done(this BaseUXItemDescription baseUXItemDesrciption)
        {
            baseUXItemDesrciption.TaskStage = TaskStage.Done;
        }

        public static void Error(this BaseUXItemDescription baseUXItemDesrciption)
        {
            baseUXItemDesrciption.TaskStage = TaskStage.Error;
        }

        public static void Doing(this BaseUXItemDescription baseUXItemDesrciption)
        {
            baseUXItemDesrciption.TaskStage = TaskStage.Doing;
        }
    }
}
