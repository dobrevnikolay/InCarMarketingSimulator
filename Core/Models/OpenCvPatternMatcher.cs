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
using Emgu.CV.UI;

namespace Core
{
    public class OpenCvPatternMatcher : IPatternMatcher
    {
        private Dictionary<TemplateMatchingType, double> Thresholds;

        public OpenCvPatternMatcher()
        {
            Thresholds = new Dictionary<TemplateMatchingType, double>()
            {
                {TemplateMatchingType.CcoeffNormed, 0.599999},
                {TemplateMatchingType.CcorrNormed, 0.901000}
            };
        }

        public Pattern FindMatch(List<Pattern> patterns, byte[] screen)
        {

            List<Pattern> matches = new List<Pattern>();

            foreach (var pattern in patterns)
            {
                
                bool matchDetected = this.MatchPatternInPicture(BitmapByteConverter.ConvertByteArrayToBitmap(screen),
                    BitmapByteConverter.ConvertByteArrayToBitmap(pattern.ImageBytes), TemplateMatchingType.CcoeffNormed);
                if (matchDetected)
                {
                    matches.Add(pattern);
                }
                
            }

            if (1 == matches.Count)
            {
                return matches[0];
            }
            else if (1 < matches.Count)
            {
                //perform another check with different matching type
                foreach (var pattern in matches)
                {
                    bool matchDetected = this.MatchPatternInPicture(
                        BitmapByteConverter.ConvertByteArrayToBitmap(screen),
                        BitmapByteConverter.ConvertByteArrayToBitmap(pattern.ImageBytes),
                        TemplateMatchingType.CcorrNormed);
                    if (!matchDetected)
                    {
                        matches.Remove(pattern);
                    }
                }
            }
            else
            {
                return null;
            }

            //If there is still more than one match get first one
            return matches[0];
        }


        private bool MatchPatternInPicture(Bitmap picture, Bitmap pattern, TemplateMatchingType matchingType)
        {

            Image<Gray, float> screenImage = new Image<Gray, float>(picture);
            Image<Gray, float> patterImage = new Image<Gray, float>(pattern);
            

            using (Image<Gray, float> result = screenImage.MatchTemplate(patterImage, matchingType))
            {
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;
                result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
                
                if (Thresholds[matchingType] < maxValues[0] )
                {
                    
                    return true;
                }
            }

            return false;
        }
    }
}
