using System;

namespace XRayImageProcessing.Models
{
    class CircleAdder : IProcesor
    {
        public void process(int[] data, int width, int height)
        {
            // TODO: Select those randomly
            int radius = 100;
            int x = width / 2;
            int y = height / 2;

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    if ((Math.Pow(x - w, 2) + Math.Pow(y - h, 2)) < radius * radius)
                    {
                        data[h * width + w] = 0x00ffffff;
                    }
                }
            }
        }
    }
}
