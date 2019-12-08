using System;
using System.Collections.Generic;
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

            IList<Tuple<int, int>> ranges = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(Color.Black.ToArgb(), limit)
            };
            FloodFiller.FillRanges(data, width, height, ranges, Color.Transparent.ToArgb());

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

        public static int[] FillRanges(int[] data, int width, int height, IList<Tuple<int, int>> ranges,
            int replacement)
        {
            foreach (var (lowerBound, upperBound) in ranges)
            {
                IterateBitmap(height, width, (h, w) =>
                {
                    if (data[h * width + w] >= lowerBound && data[h * width + w] <= upperBound)
                    {
                        data[h * width + w] = replacement;
                    }
                });
            }

            return data;
        }

        public static int[] FillEverythingExceptColors(int[] data, int width, int height, IList<int> colors,
            int replacement)
        {
            IterateBitmap(height, width, (h, w) =>
            {
                if (!colors.Contains(data[h * width + w]))
                {
                    data[h * width + w] = replacement;
                }
            });
            return data;
        }
    }
}