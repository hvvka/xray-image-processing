using System;
using System.Drawing;

namespace XRayImageProcessing.Models.Procesors
{
    class FloodFiller : IProcesor
    {
        public static int _percent = 25;

        public int[] Process(int[] data, int width, int height)
        {
            int[] borders = new int[data.Length];
            data.CopyTo(borders, 0);

            Array.Sort(borders);
            int limit = borders[_percent * width * height / 100];

            IterateBitmap(height, width, (h, w) =>
            {
                if (data[(h * width) + w] < limit)
                {
                    data[(h * width) + w] = Color.Transparent.ToArgb();
                }
            });

            return data;
        }

        private void IterateBitmap(int height, int width, Action<int, int> condition)
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    condition(h, w);
                }
            }
        }
    }
}
