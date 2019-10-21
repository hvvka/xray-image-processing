namespace XRayImageProcessing.Models
{
    public interface IProcesor
    {
        void process(int[] data, int width, int height);
    }
}
