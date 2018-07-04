using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class AdsCreator
    {
        private IGeoInfoProvider _geoInfoProvider;
        private IDataProvider _dataBase;
        public AdsCreator(IGeoInfoProvider geoInfoProvider, IDataProvider dataBase)
        {
            _geoInfoProvider = geoInfoProvider;
            _dataBase = dataBase;
        }

        public string CreateAd(string GPS)
        {

            var userActivity = _dataBase.GetUserActivity();

            var keys = new List<string>(userActivity.Keys);

            string result = string.Empty;

            foreach (var key in keys)
            {
                
                if (userActivity[key])
                {
                    if (_geoInfoProvider.IsNearby(key, GPS, out result))
                    {
                        userActivity[key] = false;
                        break;

                    }
                }

            }

            _dataBase.UpdateUserActivity(userActivity);

            return result;
        }
    }
}
