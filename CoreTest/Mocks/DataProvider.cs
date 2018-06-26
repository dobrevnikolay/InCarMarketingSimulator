using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace CoreTest
{
    public class DataProvider : IDataProvider, IUnitTestDataProvider
    {
        private Dictionary<string, bool> UserActivity { get; set; }

        private byte[] ScreenBytes { get; set; }

        public DataProvider()
        {
            UserActivity = new Dictionary<string, bool>()
            {
                {"bmw", false },
                {"ryanair",false }
            };
        }

        public Task<List<Pattern>> GetPatternsAsync()
        {
            throw new NotImplementedException();
        }

        public List<Pattern> GetPatterns()
        {
            return new List<Pattern>()
            {
                new Pattern(){ImageBytes = new byte []{12,5,1,4}, Name = "bmw"},
                new Pattern(){ImageBytes = new byte[]{5,1,5,7}, Name = "ryanair"}
            };
        }

        public void UpdateUserActivity(Dictionary<string, bool> userActivity)
        {
            UserActivity = userActivity;
        }

        public Dictionary<string, bool> GetUserActivity()
        {
            return UserActivity;
        }

        public void UploadScreen(byte[] Image)
        {
            ScreenBytes = new byte[Image.Length];
            ScreenBytes = Image;
        }

        public byte[] getScreen()
        {
            return ScreenBytes;
        }
    }

}
