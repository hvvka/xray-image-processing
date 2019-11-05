using System;
using System.Drawing;

namespace XRayImageProcessing.Models.Procesors
{
    class BorderFiller : IProcesor
    {
        private readonly int _delta = 100;

        private readonly int _percent = 25;

        public void process(int[] data, int width, int height)
        {
            int[] borders = new int[data.Length];

            IterateBitmap(height, width, (h, w) =>
            {
                if (IsBorder(h, w, width, height))
                {
                    borders[(h * width) + w] = data[(h * width) + w];
                }
            });

            Array.Sort(borders);
            int limit = borders[_percent * (2 * (_delta * width) + 2 * (_delta * (height - 2 * _delta))) / 100];

            IterateBitmap(height, width, (h, w) =>
            {
                if (data[(h * width) + w] < limit && IsBorder(h, w, width, height))
                {
                    data[(h * width) + w] = Color.Black.ToArgb();
                }
            });
        }

        private bool IsBorder(int h, int w, int width, int height) =>
                        (h < _delta || h > height - _delta ||
                         w < _delta || w > width - _delta);

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
