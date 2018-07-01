using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.CvEnum;

namespace Core
{
    public interface IPatternMatcher
    {
        Pattern FindMatch(List<Pattern> patterns, byte[] screen);
    }
}
