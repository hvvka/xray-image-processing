namespace XRayImageProcessing.Models
{
    class ImageInverter : IProcesor
    {
        public int[] Process(int[] data, int width, int height)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= 0x00ffffff;
            }
            return data;
        }
    }
}
