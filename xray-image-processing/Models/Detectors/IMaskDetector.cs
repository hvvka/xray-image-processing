using System;
using System.Collections.Generic;

namespace XRayImageProcessing.Models.Detectors
{
    public interface IMaskDetector
    {
        void DetectMask(IList<int> dataAfter, IList<int> mask, int width, int height);
    }
}
