using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Data;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Services.Data
{
    public class UtilityToolsSettingService : IUtilityToolsSettingService
    {

        private IRepository<UtilityToolsSetting> repository;

        public UtilityToolsSettingService(IRepository<UtilityToolsSetting> repository)
        {
            this.repository = repository;
        }

        public UtilityToolsSetting Get()
        {
            return repository.Table.FirstOrDefault();
        }

        public IList<UtilityToolsSetting> GetAll()
        {
            return repository.Table.ToList();
        }

        public void Save(string key, object value)
        {
            if(value!=null)
            {
                var setting = repository.Table.FirstOrDefault(p => p.Key == key);
                if (setting != null)
                {
                    setting.Value = value.ToString();
                    repository.Update(setting);
                }
                else
                {
                    repository.Insert(new UtilityToolsSetting { Key = key, Value = value.ToString() });
                }
            }
           
        }

        public void Update(UtilityToolsSetting utilityToolsSetting)
        {
            repository.Update(utilityToolsSetting);
        }



        //public string GetAccessToken()
        //{
        //    var setting = repository.Table.FirstOrDefault(p => !string.IsNullOrEmpty(p.D365AccessToken));

        //    if (setting != null)
        //    {
        //        return setting.D365AccessToken;
        //    }

        //    return null;
        //}

        //public void SetAccessToken(string accessToken)
        //{
        //    var setting = repository.Table.FirstOrDefault(p => !string.IsNullOrEmpty(p.D365AccessToken));

        //    if (setting != null)
        //    {
        //        setting.D365AccessToken = accessToken;

        //        repository.Update(setting);
        //    }
        //    else
        //    {
        //        repository.Insert(new UtilityToolsSetting { D365AccessToken = accessToken });
        //    }
        //}
    }
}
