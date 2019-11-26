using System;
using System.Drawing;

namespace XRayImageProcessing.Models.Processors
{
    internal class FloodFiller : IProcessor
    {
        public static int Percent = 25;

        public int[] Process(int[] data, int width, int height)
        {
            var sortedData = new int[data.Length];
            data.CopyTo(sortedData, 0);

            Array.Sort(sortedData);
            var limit = sortedData[Percent * width * height / 100];

            IterateBitmap(height, width, (h, w) =>
            {
                if (data[h * width + w] < limit)
                {
                    data[h * width + w] = Color.Transparent.ToArgb();
                }
            });

            return data;
        }

        public static void IterateBitmap(int height, int width, Action<int, int> condition)
        {
            for (var h = 0; h < height; h++)
            {
                for (var w = 0; w < width; w++)
                {
                    condition(h, w);
                }
            }
        }
    }
}
