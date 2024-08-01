using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Infrastructure
{
    public class SettingContainer
    {
        private readonly UtilityToolsSetting utilityToolsSetting;
        public SettingContainer(UtilityToolsSetting utilityToolsSetting)
        {
            this.utilityToolsSetting = utilityToolsSetting;
        }

        public object this[string key]
        {
            get
            {
                var type = typeof(UtilityToolsSetting);
                var prop = type.GetProperty(key);

                return prop.GetValue(utilityToolsSetting, null);
            }   
        }
    }
}
