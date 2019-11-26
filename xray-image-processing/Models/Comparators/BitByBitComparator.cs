using System;

namespace XRayImageProcessing.Models.Comparators
{
    class BitByBitComparator : IComparator
    {
        public void Compare(int[] dataBefore, int[] dataAfter, int[] dataDiff, int width, int height)
        {
            for (var w = 0; w < width; w++)
            {
                for (var h = 0; h < height; h++)
                {
                    if (dataBefore[(h * width) + w] != dataAfter[(h * width) + w])
                    {
                        dataDiff[(h * width) + w] = Convert.ToInt32("0xFFFF0000", 16);
                    }
                }
            }
        }
    }
}
