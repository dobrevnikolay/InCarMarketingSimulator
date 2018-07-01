using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Core
{
    public class MongoDataBase : IDataProvider, IDataBaseFiller
    {

        #region Private members
        
        private class Occurance
        {
            public MongoDB.Bson.ObjectId Id { get; set; }

            public string PatternName { get; set; }

            public bool IsObserved { get; set; }
        }
        

        private IMongoDatabase _database;
        private IMongoDatabase _screenDatabase;
        private IMongoClient _client;
        private GridFSBucket _bucket;
        private GridFSBucket _screenBucket;

        #endregion

        #region Constructor

        public MongoDataBase(string dataBaseName, string screenDataBaseName)
        {
            //client with a default localhost and port #27017
            _client = new MongoClient();
            _database = _client.GetDatabase(dataBaseName);
            _bucket = new GridFSBucket(_database);
            _screenDatabase = _client.GetDatabase(screenDataBaseName);
            _screenBucket = new GridFSBucket(_screenDatabase);
        }

        #endregion

        #region Public Methods

        public void AddPatterns(List<Pattern> patterns)
        {
            var collection = _database.GetCollection<Occurance>("UserActivity");

            List<string> patternNames = patterns.Select(s => s.Name).ToList();

            List<Occurance> userActivity = new List<Occurance>();

            foreach (var name in patternNames)
            {
                userActivity.Add(new Occurance()
                {
                    PatternName = name,
                    IsObserved = false
                });
            }

            collection.InsertMany(userActivity);

            foreach (var pattern in patterns)
            {
                _bucket.UploadFromBytes(pattern.Name, pattern.ImageBytes);
            }
        }

        public async Task<List<Pattern>> GetPatternsAsync()
        {
            List<Pattern> patterns = new List<Pattern>();

            var collection = _database.GetCollection<Occurance>("UserActivity");

            var documents = await collection.Find(_ => true).ToListAsync();

            //var documents = collection.AsQueryable();

            foreach (var document in documents)
            {
                try
                {
                    var bytes = _bucket.DownloadAsBytesByName(document.PatternName);
                    patterns.Add(new Pattern()
                    {
                        ImageBytes = bytes,
                        Name = document.PatternName
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine("Not found byte[] with such name in the DataBase!\n\n\n");
                    Console.WriteLine(e);
                }
            }
            return patterns;
        }

        public List<Pattern> GetPatterns()
        {
            throw new NotImplementedException();
        }

        public void UpdateUserActivity(Dictionary<string, bool> userActivity)
        {
            _database.DropCollection("UserActivity");

            var collection = _database.GetCollection<Occurance>("UserActivity");

            List<Occurance> listOfOccurance = new List<Occurance>();
            foreach (var activity in userActivity)
            {
                listOfOccurance.Add(new Occurance()
                {
                    PatternName = activity.Key,
                    IsObserved = activity.Value
                });
            }
            collection.InsertMany(listOfOccurance);
        }

        public Dictionary<string, bool> GetUserActivity()
        {
            var collection = _database.GetCollection<Occurance>("UserActivity");

            var documents = collection.Find(a => true).ToList();

            Dictionary<string, bool> userActivity = new Dictionary<string, bool>();

            foreach (var document in documents)
            {
                userActivity.Add(document.PatternName, document.IsObserved);
            }

            return userActivity;
        }

        public void UploadScreen(byte[] Image)
        {
            _screenBucket.UploadFromBytes(DateTime.Now.ToShortDateString(),Image);
        }

        #endregion

    }
}
