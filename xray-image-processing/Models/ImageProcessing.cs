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
        private XRayImage _xRayImagesDiff;

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

        public XRayImage XRayImagesDiff
        {
            get { return _xRayImagesDiff; }
            set { _xRayImagesDiff = value; }
        }

        public ImageProcessor(Uri uri)
        {
            _xRayBefore = new XRayImage(uri);
            _xRayAfter = new XRayImage(uri);
            _xRayImagesDiff = new XRayImage(uri);

        }

        public ImageProcessor(Uri uri, XRayImage before, XRayImage after, XRayImage diff)
        {
            _xRayBefore = before;
            _xRayBefore.ChangeToUri(uri);
            _xRayAfter = after;
            _xRayAfter.ChangeToUri(uri);
            _xRayImagesDiff = diff;
            _xRayImagesDiff.ChangeToUri(uri);
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

        public void CompareImages(XRayImage xRayImageBefore, XRayImage xRayImageAfter, XRayImage imagesDiff, IComparator comparator)
        {
            BitmapImage imageBefore = xRayImageBefore.XRayBitmap;
            BitmapImage imageAfter = xRayImageAfter.XRayBitmap;

            if (imageBefore.PixelWidth != imageAfter.PixelWidth || imageBefore.PixelHeight != imageAfter.PixelHeight)
            {
                throw new System.Exception("Bitmaps have different dimensions");
            }

            BitmapSource bitmapSourceBefore = new FormatConvertedBitmap(imageBefore, PixelFormats.Pbgra32, null, 0);
            BitmapSource bitmapSourceAfter = new FormatConvertedBitmap(imageAfter, PixelFormats.Pbgra32, null, 0);
            WriteableBitmap imageBeforeWritable = new WriteableBitmap(bitmapSourceBefore);
            WriteableBitmap imageAfterWritable = new WriteableBitmap(bitmapSourceAfter);
            WriteableBitmap imagesDiffWritable = new WriteableBitmap(bitmapSourceAfter);

            int width = imageAfter.PixelWidth;
            int height = imageAfter.PixelHeight;

            int[] pixelDataBefore = new int[width * height];
            int[] pixelDataAfter = new int[width * height];
            int[] pixelDataDiff = new int[width * height];
            int widthInByte = 4 * imageAfter.PixelWidth;

            imageBeforeWritable.CopyPixels(pixelDataBefore, widthInByte, 0);
            imageAfterWritable.CopyPixels(pixelDataAfter, widthInByte, 0);
            imagesDiffWritable.CopyPixels(pixelDataDiff, widthInByte, 0);

            comparator.compare(pixelDataBefore, pixelDataAfter, pixelDataDiff, width, height);

            imagesDiffWritable.WritePixels(new Int32Rect(0, 0, width, height), pixelDataDiff, widthInByte, 0);
            imagesDiff.XRayBitmap = imagesDiffWritable.ToBitmapImage();
        }

        public void DetectSquares(XRayImage xRayImageAfter, XRayImage imagesDiff, SquareDetector squareDetector)
        {
            BitmapImage imageAfter = xRayImageAfter.XRayBitmap;

            BitmapSource bitmapSourceAfter = new FormatConvertedBitmap(imageAfter, PixelFormats.Pbgra32, null, 0);
            WriteableBitmap imagesDiffWritable = new WriteableBitmap(bitmapSourceAfter);

            int width = imageAfter.PixelWidth;
            int height = imageAfter.PixelHeight;

            int[] pixelDataDiff = new int[width * height];
            int widthInByte = 4 * imageAfter.PixelWidth;

            imagesDiffWritable.CopyPixels(pixelDataDiff, widthInByte, 0);

            squareDetector.Detect(pixelDataDiff, width, height);

            imagesDiffWritable.WritePixels(new Int32Rect(0, 0, width, height), pixelDataDiff, widthInByte, 0);
            imagesDiff.XRayBitmap = imagesDiffWritable.ToBitmapImage();
        }
    }
}
