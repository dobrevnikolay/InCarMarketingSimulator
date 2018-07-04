using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Assert = NUnit.Framework.Assert;
using TestContext = NUnit.Framework.TestContext;

namespace CoreTest
{
    [TestFixture]
    public class TestScreenAnalyzer
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int memcmp(byte[] b1, byte[] b2, long count);
        static bool ByteArrayCompare(byte[] b1, byte[] b2)
        {
            // Validate buffers are the same length.
            // This also ensures that the count does not exceed the length of either buffer.  
            return b1.Length == b2.Length && memcmp(b1, b2, b1.Length) == 0;
        }


        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ScreenAnalyzer_InvalidScreen_ExceptionThrown()
        {
            IDataProvider dataBase = new DataProvider();
            var matcher = new TestMatcher();
            ScreenAnalyzer sa = new ScreenAnalyzer(dataBase, matcher);
            var projectDir = TestContext.CurrentContext.TestDirectory;

            int end = projectDir.LastIndexOf("bin");

            projectDir = projectDir.Substring(0, end);

            var picturePath = projectDir + @"\TestResources\2.jpg";

            Bitmap pic = new Bitmap(picturePath);

            Task task = sa.AnalyzeScreenAsync(null)
                .ContinueWith(innerTask =>
                {

                    // ... assertions here ...
                });

        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ScreenAnalyzer_InvalidDatabase_ExceptionThrown()
        {
            
            var matcher = new TestMatcher();
            ScreenAnalyzer sa = new ScreenAnalyzer(null, matcher);

            var projectDir = TestContext.CurrentContext.TestDirectory;

            int end = projectDir.LastIndexOf("bin");

            projectDir = projectDir.Substring(0, end);

            var picturePath = projectDir + @"\TestResources\2.jpg";

            Bitmap pic = new Bitmap(picturePath);

            Task task = sa.AnalyzeScreenAsync(null)
                .ContinueWith(innerTask =>
                {

                    // ... assertions here ...
                });

        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ScreenAnalyzer_InvalidPictureMatcher_ExceptionThrown()
        {
            IDataProvider dataBase = new DataProvider();
            ScreenAnalyzer sa = new ScreenAnalyzer(dataBase, null);

            var projectDir = TestContext.CurrentContext.TestDirectory;

            int end = projectDir.LastIndexOf("bin");

            projectDir = projectDir.Substring(0, end);

            var picturePath = projectDir + @"\TestResources\2.jpg";

            Bitmap pic = new Bitmap(picturePath);

            Task task = sa.AnalyzeScreenAsync(null)
                .ContinueWith(innerTask =>
                {

                    // ... assertions here ...
                });

        }


        //The environment could not handle that exception
        //its under investigation
        [Test, ExpectedException(typeof(System.ArgumentException))]
        public void ScreenAnalyzer_WrongImagePath_BitmapThrowsException()
        {
            IDataProvider dataBase = new DataProvider();
            ScreenAnalyzer sa = new ScreenAnalyzer(dataBase, null);

            Bitmap pic = new Bitmap(@"arGUI\CoreTest\TestResources\2.jpg");

            sa.AnalyzeScreenAsync(pic);

        }

        [Test]
        public void ScreenAnalyzer_ValidData_RyanAirMatched()
        {
            var matcher = new TestMatcher();
            IDataProvider dataBase = new DataProvider();
            ScreenAnalyzer sa = new ScreenAnalyzer(dataBase, matcher);

            var projectDir = TestContext.CurrentContext.TestDirectory;

            int end = projectDir.LastIndexOf("bin");

            projectDir = projectDir.Substring(0, end);

            var picturePath = projectDir + @"\TestResources\2.jpg";

            Bitmap pic = new Bitmap(picturePath);

            var expected = new Dictionary<string, bool>()
            {
                {"bmw", false },
                {"ryanair",true }
            };

            matcher._counter = 1;
            matcher._indexOfPattern = 1;

            sa.AnalyzeScreenAsync(pic);

            var result = dataBase.GetUserActivity();

            var dict3 = result.Where(entry => expected[entry.Key] != entry.Value)
                .ToDictionary(entry => entry.Key, entry => entry.Value);

            //if it is 0 there is match between both dictionaries
            Assert.That(dict3.Count, Is.Zero);

        }

        [Test]
        public void ScreenAnalyzer_NoPatternMatch_ScreenIsUploaded()
        {
            var matcher = new TestMatcher(true);
            
            var dataBase = new DataProvider();
            ScreenAnalyzer sa = new ScreenAnalyzer(dataBase, matcher);

            var projectDir = TestContext.CurrentContext.TestDirectory;

            int end = projectDir.LastIndexOf("bin");

            projectDir = projectDir.Substring(0, end);

            var picturePath = projectDir + @"\TestResources\2.jpg";

            Bitmap pic = new Bitmap(picturePath);

            var expected = new Dictionary<string, bool>()
            {
                {"bmw", false },
                {"ryanair", false }
            };

            sa.AnalyzeScreenAsync(pic);

            var result = dataBase.GetUserActivity();

            var dict3 = result.Where(entry => expected[entry.Key] != entry.Value)
                .ToDictionary(entry => entry.Key, entry => entry.Value);

            //if it is 0 there is match between both dictionaries
            Assert.That(dict3.Count, Is.Zero);

            var comparisonResult = ByteArrayCompare(BitmapByteConverter.ConvertBitmapToByteArray(pic), dataBase.getScreen());

            Assert.That(comparisonResult, Is.True);

        }

    }
}
