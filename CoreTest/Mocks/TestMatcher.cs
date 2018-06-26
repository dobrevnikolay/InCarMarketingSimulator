using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace CoreTest
{
    class TestMatcher : IPatternMatcher
    {
        private int _counter;
        private bool _returnFalse;

        public TestMatcher(bool returnFalse = false)
        {
            _counter = 0;
            _returnFalse = returnFalse;
        }

        public bool IsPatternInThePic(Bitmap picture, Bitmap pattern)
        {
            if (_returnFalse)
            {
                return false;
            }

            _counter++;
            if (0 == _counter % 2)
            {
                return true;
            }
            return false;
        }
    }
}
