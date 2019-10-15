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
    }
}
