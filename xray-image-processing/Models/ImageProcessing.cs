using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace XRayImageProcessing.Models
{
    public class ImageProcessor
    {
        private XRayImage _xRayBefore;
        private XRayImage _xRayAfter;

        public XRayImage XRayBefore
        {
            get { return _xRayBefore; }
            set { _xRayBefore = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public XRayImage XRayAfter
        {
            get { return _xRayAfter; }
            set { _xRayAfter = value; }
        }

        public ImageProcessor(Uri uri)
        {
            _xRayBefore = new XRayImage(uri);
            _xRayAfter = new XRayImage(uri);
        }

        public void ProcessImage(XRayImage xRayImage, IProcesor procesor)
        {
            BitmapImage originalImage = xRayImage.XRayBitmap;

            BitmapSource bitmapSource = new FormatConvertedBitmap(originalImage, PixelFormats.Pbgra32, null, 0);
            WriteableBitmap modifiedImage = new WriteableBitmap(bitmapSource);

            int height = modifiedImage.PixelHeight;
            int width = modifiedImage.PixelWidth;
            int[] pixelData = new int[width * height];
            int widthInByte = 4 * width;

            modifiedImage.CopyPixels(pixelData, widthInByte, 0);

            procesor.process(pixelData, width, height);

            modifiedImage.WritePixels(new Int32Rect(0, 0, width, height), pixelData, widthInByte, 0);

            xRayImage.XRayBitmap = modifiedImage.ToBitmapImage();
        }

        public ImageProcessor(Uri uri, XRayImage before, XRayImage after)
        {
            _xRayBefore = before;
            _xRayBefore.ChangeToUri(uri);
            _xRayAfter = after;
            _xRayAfter.ChangeToUri(uri);
        }

    }
}
