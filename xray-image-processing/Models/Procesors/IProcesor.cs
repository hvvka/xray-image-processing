namespace XRayImageProcessing.Models
{
    public interface IProcesor
    {
        void process(List<int> data, int width, int height);
    }
}
