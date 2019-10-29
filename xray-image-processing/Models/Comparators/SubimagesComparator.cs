using System;

namespace XRayImageProcessing.Models
{
    class SubimagesComparator : IComparator
    {
        private int _power;
        public SubimagesComparator(int power)
        {
            _power = power;
        }

        private int ComputePercentageOfDifferentPixels(int[] dataBefore, int[] dataAfter, int a, int b, int c, int d, int width)
        {
            double differentPixels = 0;
            for (int w = a; w < b; w++)
            {
                for (int h = c; h < d; h++)
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

        private void FillRectangle(int[] dataDiff, int a, int b, int c, int d, int color, int width)
        {
            string hexColor = Convert.ToInt32(155 + Convert.ToDouble(color)).ToString("X");
            Console.WriteLine(hexColor);
            for (int w = a; w < b; w++)
            {
                for (int h = c; h < d; h++)
                {
                    dataDiff[(h * width) + w] = Convert.ToInt32("0x" + hexColor + "FF0000", 16);
                }
            }
        }
        public void compare(int[] dataBefore, int[] dataAfter, int[] dataDiff, int width, int height)
        {
            int widthOffset = width / _power;
            int heightOffset = height / _power;

            for (int w = 0; w < width; w = w + widthOffset)
            {
                for (int h = 0; h < height; h = h + heightOffset)
                {
                    int a = w;
                    int b = w + widthOffset;
                    if (b > width) b = width;
                    int c = h;
                    int d = h + heightOffset;
                    if (d > height) d = height;

                    int percentageOfDifferentPixels = ComputePercentageOfDifferentPixels(dataBefore, dataAfter, a, b, c, d, width);

                    if (percentageOfDifferentPixels > 0)
                    {
                        FillRectangle(dataDiff, a, b, c, d, percentageOfDifferentPixels, width);
                    }
                }
            }
        }
    }
}
