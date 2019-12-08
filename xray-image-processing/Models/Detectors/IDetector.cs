using System.Collections.Generic;

namespace XRayImageProcessing.Models.Detectors
{
    public interface IDetector
    {
        void Detect(IList<int> dataAfter, IList<int> dataDiff, int width, int height);
    }
}
