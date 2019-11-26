using System;
using System.Collections.Generic;

namespace XRayImageProcessing.Models.Comparators
{
    class SubimagesComparator : IComparator
    {
        private readonly int _power;
        public SubimagesComparator(int power)
        {
            _power = power;
        }

        private static int ComputePercentageOfDifferentPixels(IReadOnlyList<int> dataBefore, IReadOnlyList<int> dataAfter, int a, int b, int c, int d, int width)
        {
            double differentPixels = 0;
            for (var w = a; w < b; w++)
            {
                for (var h = c; h < d; h++)
                {
                    if (dataBefore[(h * width) + w] != dataAfter[(h * width) + w])
                    {
                        differentPixels += 1;
                    }
                }
            }

            double totalPixels = (b - a) * (d - c);
            return Convert.ToInt32(differentPixels * 100 / totalPixels);
        }

        private static void FillRectangle(IList<int> dataDiff, int a, int b, int c, int d, int color, int width)
        {
            var hexColor = Convert.ToInt32(155 + Convert.ToDouble(color)).ToString("X");
            for (var w = a; w < b; w++)
            {
                for (var h = c; h < d; h++)
                {
                    dataDiff[(h * width) + w] = Convert.ToInt32("0x" + hexColor + "FF0000", 16);
                }
            }
        }

        public void Compare(int[] dataBefore, int[] dataAfter, int[] dataDiff, int width, int height)
        {
            var widthOffset = width / _power;
            var heightOffset = height / _power;

            for (var w = 0; w < width; w += widthOffset)
            {
                for (var h = 0; h < height; h += heightOffset)
                {
                    var a = w;
                    var b = w + widthOffset;
                    if (b > width) b = width;
                    var c = h;
                    var d = h + heightOffset;
                    if (d > height) d = height;

                    var percentageOfDifferentPixels = ComputePercentageOfDifferentPixels(dataBefore, dataAfter, a, b, c, d, width);

                    if (percentageOfDifferentPixels > 0)
                    {
                        FillRectangle(dataDiff, a, b, c, d, percentageOfDifferentPixels, width);
                    }
                }
            }
        }
    }
}
