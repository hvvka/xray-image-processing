using System;
using XRayImageProcessing.Models.Tumors;

namespace XRayImageProcessing.Models.Processors
{
    public class TumorAdder : IProcessor
    {
        private readonly Tumor _tumor;
        public TumorAdder(Tumor tumor)
        {
            _tumor = tumor;
        }

        public int[] Process(int[] data, int width, int height)
        {
            var tumorPixelData = _tumor.GetTumorAsBitsTable();

            var random = new Random();
            var x = random.Next(0, width - _tumor.Width);
            var y = random.Next(0, height - _tumor.Height);

            var tumorColor = -(65536 * _tumor.Color + 256 * _tumor.Color + _tumor.Color);

            var tumorWidth = 0;
            var tumorHeight = 0;

            for (var h = y; h < y + _tumor.Height; h++)
            {
                tumorWidth = 0;
                for (var w = x; w < x + _tumor.Width; w++)
                {
                    if (tumorPixelData[tumorHeight * _tumor.Width + tumorWidth] == tumorColor)
                    {
                        data[h * width + w] = tumorColor;
                    }
                    tumorWidth++;
                }
                tumorHeight++;
            }

            return data;
        }
    }
}
