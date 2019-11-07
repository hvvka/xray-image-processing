namespace XRayImageProcessing.Models
{
    class ImageInverter : IProcesor
    {
        public IEnumerable<int> process(IEnumerable<int> data, int width, int height)
        {
            var result = data.Select(x => x^0x00ffffff);
            return result;
            // for (int i = 0; i < data.Length; i++)
            // {
            //     data[i] ^= 0x00ffffff;
            // }
        }
    }
}
