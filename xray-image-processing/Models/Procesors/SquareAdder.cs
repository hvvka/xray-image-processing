using System;

namespace XRayImageProcessing.Models
{
    class SquareAdder : IProcesor
    {
        private readonly int size;
        private readonly int? color = null;

        public SquareAdder()
        {
            size = new Random().Next(50, 90);
        }

        public SquareAdder(int size, int color)
        {
            this.size = size;
            this.color = color;
        }
        public void process(int[] data, int width, int height)
        {
            Random random = new Random();
            int x = random.Next(size, width - size);
            int y = random.Next(size, height - size);

            if (color == null)
            {
                FillWithSpectralColor(x, y, data, width);
            }
            else
            {
                FillWithFixedColor(x, y, data, width);
            }
        }

        private void FillWithSpectralColor(int x, int y, int[] data, int width)
        {
            for (int h = y; h < y + size; h++)
            {
                for (int w = x; w < x + size; w++)
                {
                    double center_distance = Math.Sqrt(Math.Pow(x + size / 2 - w, 2) + Math.Pow(y + size / 2 - h, 2));
                    int c = (int)(64 * -center_distance / size + 180); // flat darker color spectrum
                    data[(h * width) + w] = -(65536 * c + 256 * c + c);
                }
            }
        }

        private void FillWithFixedColor(int x, int y, int[] data, int width)
        {
            int c = color.GetValueOrDefault();

            for (int h = y; h < y + size; h++)
            {
                for (int w = x; w < x + size; w++)
                {
                    data[(h * width) + w] = -(65536 * c + 256 * c + c);
                }
            }
        }

    }
}
