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
        public void AdCreator_ActiveUserActivity_TwoResults()
        {
            var geo = new MockGeoProvider(true);
            var database = new DataProvider();

            AdsCreator adCreator = new AdsCreator(geo, database);

            var result = adCreator.CreateAd("GPS");

            Assert.That(result.Count, Is.Not.Zero);
        }


        [Test]
        public void AdCreator_ActiveUserActivity_ValidString()
        {
            var geo = new MockGeoProvider(true);
            var database = new DataProvider();

            var compareWith = "Hey you have interest in...";

            geo.SetResult(compareWith);

            AdsCreator adCreator = new AdsCreator(geo, database);

            var result = adCreator.CreateAd("GPS");

            Assert.That(result.Count, Is.Not.Zero);

            var equality = result[0].Equals(compareWith);

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

            userAct["bmw"] = false;
            userAct["ryanair"] = false;

            var compareWith = "Hey you have interest in...";

            geo.SetResult(compareWith);

            AdsCreator adCreator = new AdsCreator(geo, database);

            var result = adCreator.CreateAd("GPS");

            Assert.That(result.Count, Is.Not.Zero);

            var equality = result[0].Equals(compareWith);

            var updatedUserAct = database.GetUserActivity();

            var dict3 = updatedUserAct.Where(entry => userAct[entry.Key] != entry.Value)
                .ToDictionary(entry => entry.Key, entry => entry.Value);

            Assert.That(equality, Is.True);

            Assert.That(dict3.Count, Is.Zero);

        }
    }
}
