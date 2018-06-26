using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ScreenAnalyzer
    {
        //only for debug
        public static Dictionary<string, bool> AnalyzeResults;

        private IDataProvider _dataBase;
        private IPatternMatcher _patternMatcher;

        public ScreenAnalyzer(IDataProvider dataBase, IPatternMatcher patternMatcher)
        {
            //_dataBase = new MongoDB("InCarMarketing", "InCarMarketingScreen");
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

                Dictionary<string, bool> matchResults = new Dictionary<string, bool>();
                bool isMatchFound = false;
                foreach (var pattern in patterns)
                {
#if (UNIT_TEST)
                    bool result = _patternMatcher.IsPatternInThePic(null, null);
#else
                    bool result = _patternMatcher.IsPatternInThePic(screenImage, BitmapByteConverter.ConvertByteArrayToBitmap(pattern.ImageBytes));
#endif
                    if (result)
                    {
                        isMatchFound = true;
                    }
                    matchResults.Add(pattern.Name, result);
                }

                if (!isMatchFound)
                {
                    _dataBase.UploadScreen(BitmapByteConverter.ConvertBitmapToByteArray(screenImage));
                }
                else
                {
                    //Update UserActivity with just fetched data
                    var userActivity = _dataBase.GetUserActivity();

                    var keys = new List<string>(userActivity.Keys);

                    foreach (var key in keys)
                    {
                        var newValue = matchResults[key];
                        userActivity[key] = newValue;
                    }

                    //Only for debug
                    AnalyzeResults = new Dictionary<string, bool>(userActivity);

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
