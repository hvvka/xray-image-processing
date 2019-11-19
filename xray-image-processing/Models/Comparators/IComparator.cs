using System.Collections.Generic;

namespace XRayImageProcessing.Models
{
    public interface IComparator
    {
        void Compare(IList<int> dataBefore, IList<int> dataAfter, IList<int> dataDiff, int width, int height);
    }
}
