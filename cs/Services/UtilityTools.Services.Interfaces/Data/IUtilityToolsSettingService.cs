using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Data.Domain;

namespace UtilityTools.Services.Interfaces.Data
{
    public interface IUtilityToolsSettingService
    {
        //string GetAccessToken();

        //void SetAccessToken(string accessToken);

        UtilityToolsSetting Get();

        IList<UtilityToolsSetting> GetAll();

        void Save(string key, object value);

        void Update(UtilityToolsSetting utilityToolsSetting);
    }
}
