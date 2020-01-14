using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XRayImageProcessing.Models.Tumors;

namespace XRayImageProcessing.Models.Processors
{
    class LungTumorAdder : IProcessor
    {
        private readonly Tumor _tumor;
        private readonly int[] _potentialPoints;

        public LungTumorAdder(Tumor tumor, int[] potentialPoints)

        {
            _tumor = tumor;
            _potentialPoints = potentialPoints;
        }

        public int[] Process(int[] data, int width, int height)
        {
            var tumorPixelData = _tumor.GetTumorAsBitsTable();
            var potentialPointsPixelData = _potentialPoints;

            var x = 0;
            var y = 0;

            // populate lists
            var xList = Enumerable.Range(0, width - _tumor.Width).ToList();
            var yList = Enumerable.Range(0, height - _tumor.Height).ToList();
            var random = new Random();

            do
            {
                xList.Remove(x);
                yList.Remove(y);

                var randomXIndex = random.Next(xList.Count);
                var randomYIndex = random.Next(yList.Count);

                x = xList[randomXIndex];
                y = yList[randomYIndex];
            } while (potentialPointsPixelData[y * width + x] == 0 && xList.Count > 1 && yList.Count > 1);

            var tumorColor = -(65536 * _tumor.Color + 256 * _tumor.Color + _tumor.Color);

            var tumorHeight = 0;
            for (var h = y; h < y + _tumor.Height; h++)
            {
                var tumorWidth = 0;
                for (var w = x; w < x + _tumor.Width; w++)
                {
                    if (tumorPixelData[tumorHeight * _tumor.Width + tumorWidth] == tumorColor)
                    {
                        data[h * width + w] = tumorColor;
                    }
                    tumorWidth++;
                }
                tumorHeight++;
            }

            return data;
        }

        public static int[] GetBitmapAsBitsTable(BitmapImage bitmap)
        {
            var originalImage = bitmap;

            BitmapSource bitmapSource = new FormatConvertedBitmap(originalImage, PixelFormats.Pbgra32, null, 0);
            var writableBitmap = new WriteableBitmap(bitmapSource);

            var height = writableBitmap.PixelHeight;
            var width = writableBitmap.PixelWidth;

            var pixelData = new int[width * height];
            var widthInByte = 4 * width;

            writableBitmap.CopyPixels(pixelData, widthInByte, 0);

            return pixelData;
        }
    }
}
