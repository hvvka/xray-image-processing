using System;

namespace XRayImageProcessing.Models.Processors
{
    internal class CircleAdder : IProcessor
    {
        private readonly Random _random = new Random();

        public int[] Process(int[] data, int width, int height)
        {
            var radius = _random.Next(20, 70);
            var x = _random.Next(radius, width - radius);
            var y = _random.Next(radius, height - radius);

            for (var h = 0; h < height; h++)
            {
                for (var w = 0; w < width; w++)
                {
                    var originalColor = data[h * width + w];
                    var circle = Math.Pow(x - w, 2) + Math.Pow(y - h, 2);
                    double r2 = radius * radius;
                    if (!(circle < r2)) continue;
                    var ratio = circle / r2;
                    var min = (int) (originalColor - 2 * ratio);
                    var max = (int) (originalColor + 2 * ratio);
                    var c = _random.Next(min, max);
                    data[h * width + w] = -(65536 * c + 256 * c + c);
                }
            }

            return data;
        }
    }
}
