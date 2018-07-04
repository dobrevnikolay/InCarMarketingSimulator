using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.CvEnum;

namespace Core
{
    public class ScreenAnalyzer
    {

        private IDataProvider _dataBase;
        private IPatternMatcher _patternMatcher;

        public ScreenAnalyzer(IDataProvider dataBase, IPatternMatcher patternMatcher)
        {
            //_dataBase = new MongoDataBase("InCarMarketing", "InCarMarketingScreen");
            _dataBase = dataBase;
            _patternMatcher = patternMatcher;
        }

        public async Task AnalyzeScreenAsync(Bitmap screenImage)
        {
            if (null == _dataBase || null == _patternMatcher)
            {
                throw new ArgumentNullException();
            }

            if (null != screenImage)
            {
#if (UNIT_TEST)
                var patterns = _dataBase.GetPatterns();
#else
                var patterns = await _dataBase.GetPatternsAsync();
#endif
                
                var match = _patternMatcher.FindMatch(patterns, BitmapByteConverter.ConvertBitmapToByteArray(screenImage));

                if (null == match)
                {
                    _dataBase.UploadScreen(BitmapByteConverter.ConvertBitmapToByteArray(screenImage));
                }
                else
                {
                    //Update UserActivity with just fetched data
                    var userActivity = _dataBase.GetUserActivity();

                    userActivity[match.Name] = true;

                    _dataBase.UpdateUserActivity(userActivity);
                }
            }
            else
            {
                throw new ArgumentNullException();
            }

        }
    }
}
