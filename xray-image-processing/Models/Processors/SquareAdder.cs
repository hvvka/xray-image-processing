using System;
using System.Collections.Generic;

namespace XRayImageProcessing.Models.Processors
{
    internal class SquareAdder : IProcessor
    {
        private readonly int _size;
        private readonly int? _color;

        public SquareAdder()
        {
            _size = new Random().Next(50, 90);
        }

        public SquareAdder(int size, int color)
        {
            _size = size;
            _color = color;
        }
        
        public int[] Process(int[] data, int width, int height)
        {
            var random = new Random();
            var x = random.Next(_size, width - _size);
            var y = random.Next(_size, height - _size);

            if (_color == null)
            {
                FillWithSpectralColor(x, y, data, width);
            }
            else
            {
                FillWithFixedColor(x, y, data, width);
            }
            return data;
        }

        private void FillWithSpectralColor(int x, int y, IList<int> data, int width)
        {
            for (var h = y; h < y + _size; h++)
            {
                for (var w = x; w < x + _size; w++)
                {
                    var centerDistance = Math.Sqrt(Math.Pow(x + _size / 2 - w, 2) + Math.Pow(y + _size / 2 - h, 2));
                    var c = (int)(64 * -centerDistance / _size + 180); // flat darker color spectrum
                    data[h * width + w] = -(65536 * c + 256 * c + c);
                }
            }
        }

        private void FillWithFixedColor(int x, int y, IList<int> data, int width)
        {
            var c = _color.GetValueOrDefault();
            for (var h = y; h < y + _size; h++)
            {
                for (var w = x; w < x + _size; w++)
                {
                    data[h * width + w] = -(65536 * c + 256 * c + c);
                }
            }
        }

    }
}
