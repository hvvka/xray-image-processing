using System.Collections.Generic;
using System.Linq;

namespace XRayImageProcessing.Models
{
    class ImageInverter : IProcesor
    {
        public List<int> Process(List<int> data, int width, int height)
        {
            var result = data.Select(x => x^0x00ffffff).ToList();
            return result;
        }
    }
}
