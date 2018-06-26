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

        public List<string> CreateAd(string GPS)
        {

            var userActivity = _dataBase.GetUserActivity();

            var keys = new List<string>(userActivity.Keys);

            List<string> results = new List<string>();

            foreach (var key in keys)
            {
                string ad= string.Empty;
                if (_geoInfoProvider.IsNearby(key, GPS, out ad))
                {
                    results.Add(ad);
                    userActivity[key] = false;
                }
            }

            _dataBase.UpdateUserActivity(userActivity);

            return results;
        }
    }
}
