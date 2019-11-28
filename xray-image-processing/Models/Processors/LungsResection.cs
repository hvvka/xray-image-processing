using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace XRayImageProcessing.Models.Processors
{
    internal class LungsResection : IProcessor
    {
        public static int SquareNumberBorder = 128;

        private static readonly int SquareNumber = (int) Math.Pow(SquareNumberBorder, 2);

        public int[] Process(int[] data, int width, int height)
        {
            var lungs = new int[data.Length];
            Array.Copy(data, 0, lungs, 0, data.Length);
            var squaredData = GetSquaredData(lungs, width, height);
            var expandedSquaredData = GetExpandedSquaredData(squaredData, width, height);

            FloodFiller.Percent = 60;
            var invertedData = new ImageInverter().Process(expandedSquaredData, width, height);
            var noLungs = new FloodFiller().Process(invertedData, width, height);
            var finalData = new ImageInverter().Process(noLungs, width, height);
            BorderFiller.Delta = 180;
            BorderFiller.Percent = 45;
            var noBorder = new BorderFiller().Process(finalData, width, height);
            var desquaredLungs = DesquareData(data, noBorder, width, height);
            return desquaredLungs;
        }

        private int[] DesquareData(IReadOnlyList<int> originalData, IReadOnlyList<int> noBorder, int width, int height)
        {
            var lungs = Enumerable.Repeat(Color.Transparent.ToArgb(), originalData.Count).ToArray();
            FloodFiller.IterateBitmap(height, width, (h, w) =>
            {
                if (noBorder[h * width + w] != Color.Transparent.ToArgb())
                {
                    lungs[h * width + w] = originalData[h * width + w];
                }
            });
            return lungs;
        }

        private static int[] GetExpandedSquaredData(IReadOnlyList<int> squaredData, int width, int height)
        {
            var expandedSquaredData = new int[width * height];
            var squareWidth = width / SquareNumberBorder;
            var squareHeight = height / SquareNumberBorder;

            FloodFiller.IterateBitmap(height, width, (h, w) =>
            {
                expandedSquaredData[h * width + w] = 
                    squaredData[h / squareHeight * SquareNumberBorder + w / squareWidth];
            });
            return expandedSquaredData;
        }

        private static int[] GetSquaredData(IReadOnlyList<int> lung, int width, int height)
        {
            var squaredLung = new int[SquareNumber];
            var squareWidth = width / SquareNumberBorder;
            var squareHeight = height / SquareNumberBorder;

            FloodFiller.IterateBitmap(SquareNumberBorder, SquareNumberBorder, (y, x) =>
            {
                // TODO: store in array
                //IList<int> averageRgb = new List<int>();
                var averageRed = 0;
                var averageBlue = 0;
                var averageGreen = 0;
                for (var h = y * squareHeight; h < (y + 1) * squareHeight; h++)
                {
                    for (var w = x * squareWidth; w < (x + 1) * squareWidth; w++)
                    {
                        var color = lung[h * width + w];
                        averageRed += Color.FromArgb(color).R;
                        averageBlue += Color.FromArgb(color).B;
                        averageGreen += Color.FromArgb(color).G;
                    }
                }
                averageRed /= squareHeight * squareWidth;
                averageBlue /= squareHeight * squareWidth;
                averageGreen /= squareHeight * squareWidth;
                var averageColor = Color.FromArgb(255, averageRed, averageGreen, averageBlue);

                squaredLung[y * SquareNumberBorder + x] = averageColor.ToArgb();
            });
            return squaredLung;
        }
    }
}
