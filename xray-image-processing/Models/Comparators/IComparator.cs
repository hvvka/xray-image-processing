namespace XRayImageProcessing.Models.Comparators
{
    public interface IComparator
    {
        void Compare(int[] dataBefore, int[] dataAfter, int[] dataDiff, int width, int height);
    }
}
