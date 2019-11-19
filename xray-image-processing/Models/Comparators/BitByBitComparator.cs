using System;
using System.Collections.Generic;

namespace XRayImageProcessing.Models
{
    class BitByBitComparator : IComparator
    {
        public void Compare(IList<int> dataBefore, IList<int> dataAfter, IList<int> dataDiff, int width, int height)
        {
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
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
