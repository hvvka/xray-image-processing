using System;

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
        public ImageProcessor(Uri uri, XRayImage before, XRayImage after)
        {
            _xRayBefore = before;
            _xRayBefore.ChangeToUri(uri);
            _xRayAfter = after;
            _xRayAfter.ChangeToUri(uri);
        }

    }
}
