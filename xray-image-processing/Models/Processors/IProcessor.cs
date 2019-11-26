namespace XRayImageProcessing.Models.Processors
{
    public interface IProcessor
    {
        int[] Process(int[] data, int width, int height);
    }
}
