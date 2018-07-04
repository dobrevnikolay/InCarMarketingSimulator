using System.Collections.Generic;
using System.Linq;
using Core.Models;
using NUnit.Framework;

namespace CoreTest
{
    [TestFixture]
    public class TestAdCreator
    {
        [Test]
        public void AdCreator_NoUserActivity_EmptyList()
        {
            var geo = new MockGeoProvider(false);
            var database = new DataProvider();

            AdsCreator adCreator = new AdsCreator(geo, database);

            var result = adCreator.CreateAd("GPS");

            Assert.That(result.Count, Is.Zero);

        }


        [Test]
        public void AdCreator_ActiveUserActivity_ValidString()
        {
            var geo = new MockGeoProvider(true);
            var database = new DataProvider();

            var compareWith = "There is bmw dealership in 1km";

            geo.SetResult(compareWith);

            AdsCreator adCreator = new AdsCreator(geo, database);

            database.GetUserActivity()["bmw"] = true;

            var result = adCreator.CreateAd("GPS");

            Assert.That(result, Is.Not.Null);

            var equality = result.Equals(compareWith);

            Assert.That(equality, Is.True);

        }

        [Test]
        public void AdCreator_ActiveUserActivity_ConsumedUserActivity()
        {
            var geo = new MockGeoProvider(true);
            var database = new DataProvider();

            var userAct = new Dictionary<string, bool>()
            {
                { "bmw", true },
                { "ryanair",true }
            };

            database.UpdateUserActivity(userAct);

            var compareWith = "Hey you have interest in...";

            geo.SetResult(compareWith);

            AdsCreator adCreator = new AdsCreator(geo, database);

            var result = adCreator.CreateAd("GPS");

            Assert.That(result, Is.Not.Empty);

            userAct["bmw"] = false;
            userAct["ryanair"] = false;

            var equality = result.Equals(compareWith);

            var updatedUserAct = database.GetUserActivity();

            var dict3 = updatedUserAct.Where(entry => userAct[entry.Key] != entry.Value)
                .ToDictionary(entry => entry.Key, entry => entry.Value);

            Assert.That(equality, Is.True);

            Assert.That(dict3.Count, Is.Zero);

        }
    }
}
