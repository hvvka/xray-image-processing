using System;

namespace XRayImageProcessing.Models
{
    class SquareAdder : IProcesor
    {
        private readonly Random random = new Random();

        public void process(List<int> data, int width, int height)
        {
            int size = random.Next(50, 90);
            int x = random.Next(size, width - size);
            int y = random.Next(size, height - size);

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
    }
}
