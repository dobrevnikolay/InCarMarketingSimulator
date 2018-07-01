using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Emgu.CV.CvEnum;

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

        public Pattern FindMatch(List<Pattern> patterns, byte[] screen)
        {
            if (0 == patterns.Capacity)
            {
                return null;
            }

            if (_returnFalse)
            {
                return null;
            }

            _counter++;
            if (0 == _counter % 2)
            {
                return patterns[0];
            }
            return null;
        }
    }
}
