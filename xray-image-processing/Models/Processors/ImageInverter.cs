using System.Drawing;

namespace XRayImageProcessing.Models.Processors
{
    internal class ImageInverter : IProcessor
    {
        public int[] Process(int[] data, int width, int height)
        {
            for (var i = 0; i < data.Length; i++)
            {
                if (data[i] != Color.Transparent.ToArgb())
                    data[i] ^= 0x00ffffff;
            }
            return data;
        }
    }
}
