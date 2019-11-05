using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayImageProcessing.Models.Procesors
{
    class BorderFiller : IProcesor
    {
        private int _delta = 100;
        public void process(int[] data, int width, int height)
        {
            int[] borders = new int[2 * (_delta * width) + 2 * (_delta * (height - 2*_delta))];

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    if (h < _delta || h > height - _delta || 
                        w < _delta || w > width - _delta)
                    {
                        data[(h * width) + w] = Color.Blue.ToArgb();
                    }
                }
            }
        }
    }
}
