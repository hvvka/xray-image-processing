using System;
using System.Drawing;

namespace XRayImageProcessing.Models.Processors
{
    internal class BorderFiller : IProcessor
    {
        public static int Delta = 100;

        public static int Percent = 25;

        public int[] Process(int[] data, int width, int height)
        {
            var borders = new int[data.Length];

            IterateBitmap(height, width, (h, w) =>
            {
                if (IsBorder(h, w, width, height))
                {
                    borders[(h * width) + w] = data[(h * width) + w];
                }
            });

            Array.Sort(borders);
            var limit = borders[Percent * (2 * (Delta * width) + 2 * (Delta * (height - 2 * Delta))) / 100];

            IterateBitmap(height, width, condition: (h, w) =>
            {
                if (data[h * width + w] < limit && IsBorder(h, w, width, height))
                {
                    data[h * width + w] = Color.Transparent.ToArgb();
                }
            });

            return data;
        }

        private static bool IsBorder(int h, int w, int width, int height) =>
                        (h < Delta || h > height - Delta ||
                         w < Delta || w > width - Delta);

        private static void IterateBitmap(int height, int width, Action<int, int> condition)
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
