using System;
using System.Collections.Generic;
using XRayImageProcessing.Models.Tumors;

namespace XRayImageProcessing.Models.Detectors
{
    internal class TumorDetector : IDetector
    {
        private readonly IEnumerable<Tumor> _tumors;
        private readonly double _thresholdPercentage;

        public TumorDetector(IEnumerable<Tumor> tumors, double thresholdPercentage)
        {
            _tumors = tumors;
            _thresholdPercentage = thresholdPercentage;
        }

        private static void DrawDetectedTumor(Tumor tumor,
            IList<int> tumorPixelData,
            IList<int> dataAfter,
            IList<int> dataDiff,
            int width,
            int height,
            int fullWidth)
        {
            var tumorBitmapHeight = 0;

            for (var h = height; h < height + tumor.Height; h++)
            {
                var tumorBitmapWidth = 0;

                for (var w = width; w < width + tumor.Width; w++)
                {
                    if (tumorPixelData[tumorBitmapHeight * tumor.Width + tumorBitmapWidth] == tumor.ComputedColor)
                    {
                        if (dataAfter[h * fullWidth + w] == tumor.ComputedColor)
                        {
                            dataDiff[(h * fullWidth) + w] = Convert.ToInt32("0xFFFF0000", 16);
                        }
                    }
                    tumorBitmapWidth++;
                }
                tumorBitmapHeight++;
            }
        }

        private static int CountPixelsForTumor(Tumor tumor)
        {
            var pixelCounter = 0;
            var tumorPixelData = tumor.GetTumorAsBitsTable();

            for (var h = 0; h < tumor.Height; h++)
            {
                for (var w = 0; w < tumor.Width; w++)
                {
                    if (tumorPixelData[h * tumor.Width + w] == tumor.ComputedColor)
                    {
                        pixelCounter++;
                    }
                }
            }

            return pixelCounter;
        }
        private static bool DetectTumor(Tumor tumor,
            IList<int> tumorPixelData,
            int maxFalsePixels,
            IList<int> dataAfter,
            int width,
            int height,
            int fullWidth)
        {
            var detectedTumor = true;
            var falsePixels = 0;
            var tumorBitmapHeight = 0;

            for (var tumorHeight = height; tumorHeight < height + tumor.Height; tumorHeight++)
            {
                if (!detectedTumor)
                {
                    break;
                }

                var tumorBitmapWidth = 0;

                for (var tumorWidth = width; tumorWidth < width + tumor.Width; tumorWidth++)
                {
                    if (tumorPixelData[tumorBitmapHeight * tumor.Width + tumorBitmapWidth] == tumor.ComputedColor)
                    {
                        if (dataAfter[tumorHeight * fullWidth + tumorWidth] != tumor.ComputedColor)
                        {
                            falsePixels++;

                            if (falsePixels > maxFalsePixels)
                            {
                                detectedTumor = false;
                                break;
                            }
                        }
                    }
                    tumorBitmapWidth++;
                }
                tumorBitmapHeight++;
            }
            return detectedTumor;
        }


        public void Detect(IList<int> dataAfter, IList<int> dataDiff, int width, int height)
        {
            foreach (var tumor in _tumors)
            {
                var maxFalsePixelsForATumor = Convert.ToInt32(0.01 * (100 - _thresholdPercentage) * CountPixelsForTumor(tumor));
                var tumorPixelData = tumor.GetTumorAsBitsTable();

                for (var w = 0; w < width - tumor.Width; w++)
                {
                    for (var h = 0; h < height - tumor.Height; h++)
                    {
                        if (DetectTumor(tumor, tumorPixelData, maxFalsePixelsForATumor, dataAfter, w, h, width))
                        {
                            DrawDetectedTumor(tumor, tumorPixelData, dataAfter, dataDiff, w, h, width);
                        }
                    }
                }
            }
        }
    }
}
