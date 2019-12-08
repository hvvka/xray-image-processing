using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using XRayImageProcessing.Models.Algorithms;
using XRayImageProcessing.Models.Comparators;
using XRayImageProcessing.Models.Detectors;
using XRayImageProcessing.Models.Processors;
using XRayImageProcessing.Models.Util;

namespace XRayImageProcessing.Models
{
    public class ImageProcessor
    {
        public XRayImage XRayBefore { get; set; }

        public XRayImage XRayAfter { get; set; }

        public XRayImage XRayImagesDiff { get; set; }

        public Stack<BitmapImage> ImageHistory { get; set; }

        public ImageProcessor(Uri uri)
        {
            XRayBefore = new XRayImage(uri);
            XRayAfter = new XRayImage(uri);
            XRayImagesDiff = new XRayImage(uri);
            ImageHistory = new Stack<BitmapImage>();
        }

        public ImageProcessor(Uri uri, XRayImage before, XRayImage after, XRayImage diff)
        {
            XRayBefore = before;
            XRayBefore.ChangeToUri(uri);
            XRayAfter = after;
            XRayAfter.ChangeToUri(uri);
            XRayImagesDiff = diff;
            XRayImagesDiff.ChangeToUri(uri);
            ImageHistory = new Stack<BitmapImage>();
        }

        public void ProcessImage(XRayImage xRayImage, IProcessor processor)
        {
            ImageHistory.Push(xRayImage.XRayBitmap);
            var originalImage = xRayImage.XRayBitmap;

            BitmapSource bitmapSource = new FormatConvertedBitmap(originalImage, PixelFormats.Pbgra32, null, 0);
            var modifiedImage = new WriteableBitmap(bitmapSource);

            var height = modifiedImage.PixelHeight;
            var width = modifiedImage.PixelWidth;
            var pixelData = new int[width * height];
            var widthInByte = 4 * width;

            modifiedImage.CopyPixels(pixelData, widthInByte, 0);
            var processedPixelData = processor.Process(pixelData, width, height);
            modifiedImage.WritePixels(new Int32Rect(0, 0, width, height), processedPixelData, widthInByte, 0);
            xRayImage.XRayBitmap = modifiedImage.ToBitmapImage();
        }

        public void FloodFill(XRayImage xRayImage)
        {
            ImageHistory.Push(xRayImage.XRayBitmap);
            var originalImage = xRayImage.XRayBitmap;
            using (var bitmap = ImageUtils.BitmapImage2Bitmap(originalImage))
            {
                var floodFill = new FloodFill();
                // top & bottom borders
                for (var x = 0; x < bitmap.Width; x += 100)
                {
                    floodFill.Fill(bitmap, new System.Drawing.Point(x, 0), System.Drawing.Color.Black, System.Drawing.Color.Transparent);
                    floodFill.Fill(bitmap, new System.Drawing.Point(x, bitmap.Height - 1), System.Drawing.Color.Black, System.Drawing.Color.Transparent);
                }
                // left & right borders
                for (var y = 0; y < bitmap.Height; y += 100)
                {
                    floodFill.Fill(bitmap, new System.Drawing.Point(0, y), System.Drawing.Color.Black, System.Drawing.Color.Transparent);
                    floodFill.Fill(bitmap, new System.Drawing.Point(bitmap.Width - 1, y), System.Drawing.Color.Black, System.Drawing.Color.Transparent);
                }

                xRayImage.XRayBitmap = bitmap.Bitmap2BitmapImage();
            }
        }

        public void Undo()
        {
            if (ImageHistory.Count > 0)
            {
                XRayAfter.XRayBitmap = ImageHistory.Pop();
            }
        }

        public void CompareImages(XRayImage xRayImageBefore, XRayImage xRayImageAfter, XRayImage imagesDiff, IComparator comparator)
        {
            var imageBefore = xRayImageBefore.XRayBitmap;
            var imageAfter = xRayImageAfter.XRayBitmap;

            if (imageBefore.PixelWidth != imageAfter.PixelWidth || imageBefore.PixelHeight != imageAfter.PixelHeight)
            {
                throw new System.Exception("Bitmaps have different dimensions");
            }

            BitmapSource bitmapSourceBefore = new FormatConvertedBitmap(imageBefore, PixelFormats.Pbgra32, null, 0);
            BitmapSource bitmapSourceAfter = new FormatConvertedBitmap(imageAfter, PixelFormats.Pbgra32, null, 0);
            var imageBeforeWritable = new WriteableBitmap(bitmapSourceBefore);
            var imageAfterWritable = new WriteableBitmap(bitmapSourceAfter);
            var imagesDiffWritable = new WriteableBitmap(bitmapSourceAfter);

            var width = imageAfter.PixelWidth;
            var height = imageAfter.PixelHeight;

            var pixelDataBefore = new int[width * height];
            var pixelDataAfter = new int[width * height];
            var pixelDataDiff = new int[width * height];
            var widthInByte = 4 * imageAfter.PixelWidth;

            imageBeforeWritable.CopyPixels(pixelDataBefore, widthInByte, 0);
            imageAfterWritable.CopyPixels(pixelDataAfter, widthInByte, 0);
            imagesDiffWritable.CopyPixels(pixelDataDiff, widthInByte, 0);

            comparator.Compare(pixelDataBefore, pixelDataAfter, pixelDataDiff, width, height);

            imagesDiffWritable.WritePixels(new Int32Rect(0, 0, width, height), pixelDataDiff, widthInByte, 0);
            imagesDiff.XRayBitmap = imagesDiffWritable.ToBitmapImage();
        }

        public void DetectChanges(XRayImage xRayImageAfter, XRayImage imagesDiff, IDetector detector)
        {
            BitmapImage imageAfter = xRayImageAfter.XRayBitmap;

            BitmapSource bitmapSourceAfter = new FormatConvertedBitmap(imageAfter, PixelFormats.Pbgra32, null, 0);
            WriteableBitmap imagesAfterWritable = new WriteableBitmap(bitmapSourceAfter);
            WriteableBitmap imagesDiffWritable = new WriteableBitmap(bitmapSourceAfter);

            var width = imageAfter.PixelWidth;
            var height = imageAfter.PixelHeight;

            var pixelDataAfter = new int[width * height];
            var pixelDataDiff = new int[width * height];
            var widthInByte = 4 * imageAfter.PixelWidth;

            imagesAfterWritable.CopyPixels(pixelDataAfter, widthInByte, 0);
            imagesDiffWritable.CopyPixels(pixelDataDiff, widthInByte, 0);

            detector.Detect(pixelDataAfter, pixelDataDiff, width, height);

            imagesDiffWritable.WritePixels(new Int32Rect(0, 0, width, height), pixelDataDiff, widthInByte, 0);
            imagesDiff.XRayBitmap = imagesDiffWritable.ToBitmapImage();
        }

    }
}
