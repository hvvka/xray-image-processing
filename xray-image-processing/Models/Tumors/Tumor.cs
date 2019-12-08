using Newtonsoft.Json;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace XRayImageProcessing.Models.Tumors
{
    public class Tumor
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public int Color { get; set; }

        public int ComputedColor { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public BitmapImage TumorBitmap { get; set; }

        public void ComputeColor()
        {
            ComputedColor = -(65536 * Color + 256 * Color + Color);
        }
        public void LoadBitmap(Uri uri)
        {
            TumorBitmap = new BitmapImage(uri);

            BitmapSource bitmapSource = new FormatConvertedBitmap(TumorBitmap, PixelFormats.Pbgra32, null, 0);
            var writeableBitmap = new WriteableBitmap(bitmapSource);
            Height = writeableBitmap.PixelHeight;
            Width = writeableBitmap.PixelWidth;
        }

        public int[] GetTumorAsBitsTable()
        {
            var originalImage = TumorBitmap;

            BitmapSource bitmapSource = new FormatConvertedBitmap(originalImage, PixelFormats.Pbgra32, null, 0);
            var writeableBitmap = new WriteableBitmap(bitmapSource);

            var height = writeableBitmap.PixelHeight;
            var width = writeableBitmap.PixelWidth;

            var pixelData = new int[width * height];
            var widthInByte = 4 * width;

            writeableBitmap.CopyPixels(pixelData, widthInByte, 0);

            return pixelData;
        }
    }
}
