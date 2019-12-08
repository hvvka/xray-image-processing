using System;
using System.Collections.Generic;

namespace XRayImageProcessing.Models.Detectors
{
    internal class SquareDetector : IDetector
    {
        private readonly int _color;
        private readonly int _sideLength;
        private readonly int _maxFalsePixels;

        public SquareDetector(int sideLength, int color, double thresholdPercentage)
        {
            _sideLength = sideLength;
            _color = color;
            _maxFalsePixels = Convert.ToInt32(0.01 * (100 - thresholdPercentage) * _sideLength * _sideLength);
        }

        private void DrawDetectedSquare(IList<int> dataDiff, int width, int height, int fullWidth)
        {
            for (var w = width; w < width + _sideLength; w++)
            {
                for (var h = height; h < height + _sideLength; h++)
                {
                    dataDiff[(h * fullWidth) + w] = Convert.ToInt32("0xFFFF0000", 16);
                }
            }
        }
        private bool DetectSquare(IList<int> dataAfter, int width, int height, int fullWidth)
        {
            var detectedSquare = true;
            var falsePixels = 0;

            for (var squareWidth = width; squareWidth < width + _sideLength; squareWidth++)
            {
                if (!detectedSquare)
                {
                    break;
                }

                for (var squareHeight = height; squareHeight < height + _sideLength; squareHeight++)
                {
                    if (dataAfter[squareHeight * fullWidth + squareWidth] ==
                        -(65536 * _color + 256 * _color + _color))
                    {
                        continue;
                    }
                    else
                    {
                        falsePixels++;

                        if (falsePixels <= _maxFalsePixels) continue;
                        detectedSquare = false;
                        break;
                    }
                }
            }
            return detectedSquare;
        }

        public void Detect(IList<int> dataAfter, IList<int> dataDiff, int width, int height)
        {
            for (var w = 0; w < width - _sideLength; w++)
            {
                for (var h = 0; h < height - _sideLength; h++)
                {
                    if (DetectSquare(dataAfter, w, h, width))
                    {
                        DrawDetectedSquare(dataDiff, w, h, width);
                    }
                }
            }
        }
    }
}
