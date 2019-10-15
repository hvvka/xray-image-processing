using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace XRayImageProcessing.Models
{
    public class XRayImage : INotifyPropertyChanged
    {
        private BitmapImage _xRayBitmap;

        public event PropertyChangedEventHandler PropertyChanged;

        public BitmapImage XRayBitmap
        {
            get { return _xRayBitmap; }
            set
            {
                _xRayBitmap = value;
                OnPropertyChanged("XRayBitmap");
            }
        }

        private void OnPropertyChanged([CallerMemberName]string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        public XRayImage(Uri uri)
        {
            _xRayBitmap = new BitmapImage(uri);
            OnPropertyChanged("XRayBitmap");
        }

        public void ChangeToUri(Uri uri)
        {
            _xRayBitmap = new BitmapImage(uri);
            OnPropertyChanged("XRayBitmap");
        }

        public void Save(string path)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(_xRayBitmap));

            using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }
    }
}
