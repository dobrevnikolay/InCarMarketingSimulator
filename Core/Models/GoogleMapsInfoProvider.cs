using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class GoogleMapsInfoProvider : IGeoInfoProvider
    {
        private readonly Dictionary<string, string> InfoAboutPlace;

        public GoogleMapsInfoProvider()
        {
            InfoAboutPlace = new Dictionary<string, string>()
            {
                {"bmw","There is bmw dealership in 1km"},
                {"ryanair","We've seen you are looking for flights, there is a cheap flight to Barcelona" },
                {"spotify","Beyonce just launched a new single. You can find it at spotify. Just press on the ad." }
            };
        }

        public bool IsNearby(string patternName, string GPS, out string Result)
        {
            Result = InfoAboutPlace[patternName];
            return true;
        }
    }
}
