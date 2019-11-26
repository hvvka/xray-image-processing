using System;
using System.Collections.Generic;

namespace XRayImageProcessing.Models.Detectors
{
    public class SquareDetector
    {
        private readonly int _color;
        private readonly int _sideLength;

        public SquareDetector(int sideLength, int color)
        {
            _sideLength = sideLength;
            _color = color;
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
        private bool DetectSquare(int[] dataDiff, int width, int height, int fullWidth)
        {
            var detectedSquare = true;

            for (var squareWidth = width; squareWidth < width + _sideLength; squareWidth++)
            {
                if (!detectedSquare)
                {
                    break;
                }
                for (var squareHeight = height; squareHeight < height + _sideLength; squareHeight++)
                {
                    if (dataDiff[squareHeight * fullWidth + squareWidth] ==
                        -(65536 * _color + 256 * _color + _color) ||
                        dataDiff[squareHeight * fullWidth + squareWidth] == Convert.ToInt32("0xFFFF0000", 16))
                        continue;
                    detectedSquare = false;
                    break;
                }
            }
            return detectedSquare;
        }

        public void Detect(int[] dataDiff, int width, int height)
        {
            for (var w = 0; w < width - _sideLength; w++)
            {
                for (var h = 0; h < height - _sideLength; h++)
                {
                    var detectedSquare = DetectSquare(dataDiff, w, h, width);

                    if (detectedSquare)
                    {
                        DrawDetectedSquare(dataDiff, w, h, width);
                    }
                }
            }
        }
    }
}
