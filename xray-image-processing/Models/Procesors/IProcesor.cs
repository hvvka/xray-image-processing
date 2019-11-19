using System.Collections.Generic;

namespace XRayImageProcessing.Models
{
    public interface IProcesor
    {
        List<int> Process(List<int> data, int width, int height);
    }
}
