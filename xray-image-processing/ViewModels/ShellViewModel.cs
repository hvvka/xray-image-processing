using Caliburn.Micro;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using XRayImageProcessing.Models;
using XRayImageProcessing.Models.Procesors;

namespace XRayImageProcessing.ViewModels
{
    public class ShellViewModel : Screen, INotifyPropertyChanged
    {
        private ImageProcessor _imageProcessor;
        private string _chosenPath = "default";
        private string _powerForImageDivision = "20";

        public event PropertyChangedEventHandler PropertyChanged;

        public string PowerForImageDivision
        {
            get { return _powerForImageDivision; }
            set { _powerForImageDivision = value; }
        }
        public string ChosenPath
        {
            get { return _chosenPath; }
            set
            {
                _chosenPath = value;
                OnPropertyChanged("ChosenPath");
            }
        }

        public ImageProcessor ImageProcessor
        {
            get { return _imageProcessor; }
            set { _imageProcessor = value; }
        }
        public void ChooseFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".png",
                Filter = "Png files (.png)|*.png"
            };

            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                ChosenPath = openFileDialog.FileName;
                OpenNewImage(ChosenPath);
            }
        }
        public void SaveFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                FileName = Path.GetFileNameWithoutExtension(_chosenPath) + "_" + DateTime.Now.ToString("yyyy-dd-MM_HH-mm-ss") + ".png",
                DefaultExt = ".png",
                Filter = "Png files (.png)|*.png"
            };

            Nullable<bool> result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string path = saveFileDialog.FileName;
                _imageProcessor.XRayAfter.Save(path);
            }
        }

        public ShellViewModel()
        {
            // TODO: Relative path
            ChosenPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\samples\00030636_017.png"));
            _imageProcessor = new ImageProcessor(new Uri(ChosenPath));
            OpenNewImage(ChosenPath);
        }

        private void OpenNewImage(string path)
        {
            _imageProcessor = new ImageProcessor(new Uri(path), _imageProcessor.XRayBefore, _imageProcessor.XRayAfter, _imageProcessor.XRayImagesDiff);
        }

        private void OnPropertyChanged([CallerMemberName]string caller = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        public void InvertColours()
        {
            _imageProcessor.ProcessImage(_imageProcessor.XRayAfter, new ImageInverter());
        }

        public void AddCircle()
        {
            _imageProcessor.ProcessImage(_imageProcessor.XRayAfter, new CircleAdder());
        }

        public void AddSquare()
        {
            _imageProcessor.ProcessImage(_imageProcessor.XRayAfter, new SquareAdder());
        }

        public void FloodFill()
        {
            _imageProcessor.FloodFill(_imageProcessor.XRayAfter);
        }

        public void FillBorders()
        {
            _imageProcessor.ProcessImage(_imageProcessor.XRayAfter, new BorderFiller());
        }

        public void CompareBitByBit()
        {
            _imageProcessor.CompareImages(_imageProcessor.XRayBefore, _imageProcessor.XRayAfter, _imageProcessor.XRayImagesDiff, new BitByBitComparator());
        }

        public void CompareSubimages()
        {
            int power;
            if (int.TryParse(_powerForImageDivision, out power))
            {
                _imageProcessor.CompareImages(_imageProcessor.XRayBefore, _imageProcessor.XRayAfter, _imageProcessor.XRayImagesDiff, new SubimagesComparator(power));
            }
        }
    }
}
