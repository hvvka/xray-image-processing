using System;
namespace XRayImageProcessing.Models
{
    public class SquareDetector
    {
        private readonly int color;
        private readonly int sideLength;


        public SquareDetector(int sideLength, int color)
        {
            this.sideLength = sideLength;
            this.color = color;
        }

        private void DrawDetectedSquare(int[] dataDiff, int width, int height, int fullWidth)
        {
            for (int w = width; w < width + sideLength; w++)
            {
                for (int h = height; h < height + sideLength; h++)
                {
                    dataDiff[(h * fullWidth) + w] = Convert.ToInt32("0xFFFF0000", 16);
                }
            }
        }
        private bool DetectSquare(int[] dataDiff, int width, int height, int fullWidth)
        {

            bool detectedSquare = true;

            for (int squareWidth = width; squareWidth < width + sideLength; squareWidth++)
            {
                if (!detectedSquare)
                {
                    break;
                }
                for (int squareHeight = height; squareHeight < height + sideLength; squareHeight++)
                {
                    if ((dataDiff[(squareHeight * fullWidth) + squareWidth] != -(65536 * color + 256 * color + color)) && (dataDiff[(squareHeight * fullWidth) + squareWidth] != Convert.ToInt32("0xFFFF0000", 16)))
                    {
                        detectedSquare = false;
                        break;
                    }
                }
            }
            return detectedSquare;
        }

        public void Detect(int[] dataDiff, int width, int height)
        {
            for (int w = 0; w < width - sideLength; w++)
            {
                for (int h = 0; h < height - sideLength; h++)
                {
                    bool detectedSquare = DetectSquare(dataDiff, w, h, width);

                    if (detectedSquare)
                    {
                        DrawDetectedSquare(dataDiff, w, h, width);
                    }
                }
            }
        }
    }
}
