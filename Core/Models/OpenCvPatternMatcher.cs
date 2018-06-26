using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Core
{
    public class OpenCvPatternMatcher : IPatternMatcher
    {
        public bool IsPatternInThePic(Bitmap picture, Bitmap pattern)
        {
            Image<Bgr, Byte> screenImage = new Image<Bgr, byte>(picture);
            Image<Bgr, Byte> patterImage = new Image<Bgr, byte>(pattern);

            Image<Gray, float> imgMatch = screenImage.MatchTemplate(patterImage, TemplateMatchingType.CcoeffNormed);

            float[,,] matches = imgMatch.Data;
            for (int y = 0; y < matches.GetLength(0); ++y)
            {
                for (int x = 0; x < matches.GetLength(1); ++x)
                {
                    double matchScore = matches[y, x, 0];
                    if (0.75 < matchScore)
                    {
                        return true;

                        //Rectangle rect = new Rectangle(new Point(x, y), new Size(1, 1));
                        //imgSource.Draw(rect, new Bgr(Color.Blue), 1);
                    }
                }
            }

            return false;
        }
    }
}
