using System;
using System.Windows.Media.Imaging;

namespace XRayImageProcessing.Models
{
    public interface IComparator
    {
        void compare(int[] dataBefore, int[] dataAfter, int[] dataDiff, int width, int height);
    }
}
