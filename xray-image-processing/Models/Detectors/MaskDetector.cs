using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayImageProcessing.Models.Detectors
{
    public class MaskDetector : IMaskDetector
    {
        public void DetectMask(IList<int> dataAfter, IList<int> mask, int width, int height)
        {
            for (var w = 0; w < width; w++)
            {
                for (var h = 0; h < height; h++)
                {
                    if (dataAfter[h * width + w] != 0)
                    {
                        mask[h * width + w] = 1;
                    }
                    else
                    {
                        mask[h * width + w] = 0;
                    }
                }
            }
        }
    }
}
