using System;
using System.Collections.Generic;

namespace XRayImageProcessing.Models
{
    class CircleAdder : IProcesor
    {
        private readonly Random random = new Random();

        public List<int> Process(List<int> data, int width, int height)
        {
            int radius = random.Next(20, 70);
            int x = random.Next(radius, width - radius);
            int y = random.Next(radius, height - radius);

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    int originalColour = data[(h * width) + w];
                    double circle = (Math.Pow(x - w, 2) + Math.Pow(y - h, 2));
                    double r2 = radius * radius;
                    if (circle < r2)
                    {
                        double ratio = circle / r2;
                        int min = (int) (originalColour - 2 * ratio);
                        int max = (int) (originalColour + 2 * ratio);
                        int c = random.Next(min, max);
                        Console.WriteLine(c);
                        data[(h * width) + w] = -(65536 * c + 256 * c + c);
                    }
                }
            }

            return data;
        }
    }
}
