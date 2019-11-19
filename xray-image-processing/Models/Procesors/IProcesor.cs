namespace XRayImageProcessing.Models
{
    public interface IProcesor
    {
        int[] Process(int[] data, int width, int height);
    }
}
