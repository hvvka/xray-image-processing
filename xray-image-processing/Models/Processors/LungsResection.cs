using System;
using System.Collections.Generic;

namespace XRayImageProcessing.Models.Processors
{
    internal class LungsResection : IProcessor
    {
        public static int SquareNumberBorder = 256;
        public static int SquareNumber = (int) Math.Pow(SquareNumberBorder, 2);

        public int[] Process(int[] data, int width, int height)
        {
            var lungs = new int[data.Length]; // /2
            Array.Copy(data, 0, lungs, 0, data.Length); // /2
            var squaredData = GetSquaredData(lungs, width, height);
            //return squaredData;

            var expandedSquaredData = GetExpandedSquaredData(squaredData, width, height);
            return expandedSquaredData;
            
            // TODO: Invert
            var noLungs = new FloodFiller().Process(expandedSquaredData, width, height);
            return noLungs; // lungs
        }

        private static int[] GetExpandedSquaredData(int[] squaredData, int width, int height)
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
            var extendedSquaredLungs = new int[lung.Count];
            var squaredLung = new int[SquareNumber];
            var squareWidth = width / SquareNumberBorder;
            var squareHeight = height / SquareNumberBorder;

            FloodFiller.IterateBitmap(SquareNumberBorder, SquareNumberBorder, (y, x) =>
            {
                var averageColor = 0;
                for (var h = y * squareHeight; h < (y + 1) * squareHeight; h++)
                {
                    for (var w = x * squareWidth; w < (x + 1) * squareWidth; w++)
                    {
                        averageColor += lung[h * width + w];
                    }
                }
                averageColor /= squareHeight * squareWidth;
                Console.WriteLine($"Square ({x}, {y}): {averageColor}");
                squaredLung[y * SquareNumberBorder + x] = averageColor;

                for (var h = y * squareHeight; h < (y + 1) * squareHeight; h++)
                {
                    for (var w = x * squareWidth; w < (x + 1) * squareWidth; w++)
                    {
                        extendedSquaredLungs[h * width + w] = averageColor;
                    }
                }
            });
            return squaredLung;
        }
    }
}
