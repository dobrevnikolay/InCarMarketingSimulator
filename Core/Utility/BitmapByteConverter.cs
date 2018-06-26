using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BitmapByteConverter
    {

        /// <summary>
        /// Covert a bitmap to a byte array
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns>
        /// byte array, when bitmap could be converted
        /// null, when bitmap is null
        /// null, when bitmap could not be converted to byte array
        /// </returns>
        public static byte[] ConvertBitmapToByteArray(Bitmap bitmap)
        {
            byte[] result = null;
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, bitmap.RawFormat);
                result = stream.ToArray();
            }
            return result;
        }

        /// <summary>
        /// Back convertion from array to Bitmap image
        /// </summary>
        /// <param name="inputBytes">Bitmap image input bytes</param>
        /// <returns></returns>
        public static Bitmap ConvertByteArrayToBitmap(byte[] inputBytes)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(inputBytes))
            {
                bmp = new Bitmap(ms);
            }
            return bmp;
        }
    }
}
