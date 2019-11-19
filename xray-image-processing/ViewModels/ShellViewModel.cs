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
        private string _chosenPath = "default";

        public event PropertyChangedEventHandler PropertyChanged;

        public int BorderWidth { get; set; } = 140;
        public int PercentCovered { get; set; } = 35;
        public string PowerForImageDivision { get; set; } = "20";
        public string ChosenPath
        {
            get { return _chosenPath; }
            set
            {
                _chosenPath = value;
                OnPropertyChanged("ChosenPath");
            }
        }

        public ImageProcessor ImageProcessor { get; set; }
        public void ChooseFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".png",
                Filter = "Png files (.png)|*.png"
            };

            bool? result = openFileDialog.ShowDialog();

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

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string path = saveFileDialog.FileName;
                ImageProcessor.XRayAfter.Save(path);
            }
        }

        public ShellViewModel()
        {
            // TODO: Relative path
            ChosenPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\samples\00030636_017.png"));
            ImageProcessor = new ImageProcessor(new Uri(ChosenPath));
            OpenNewImage(ChosenPath);
        }

        private void OpenNewImage(string path) => ImageProcessor = new ImageProcessor(new Uri(path), ImageProcessor.XRayBefore, ImageProcessor.XRayAfter, ImageProcessor.XRayImagesDiff);

        private void OnPropertyChanged([CallerMemberName]string caller = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));

        public void InvertColours() => ImageProcessor.ProcessImage(ImageProcessor.XRayAfter, new ImageInverter());

        public void AddCircle() => ImageProcessor.ProcessImage(ImageProcessor.XRayAfter, new CircleAdder());

        public void AddSquare() => ImageProcessor.ProcessImage(ImageProcessor.XRayAfter, new SquareAdder());

        public void FloodFill()
        {
            FloodFiller._percent = 5;
            ImageProcessor.ProcessImage(ImageProcessor.XRayAfter, new FloodFiller());
        }

        public void Undo() => ImageProcessor.Undo();

        public void AddFixedSquare()
        {
            _imageProcessor.ProcessImage(_imageProcessor.XRayAfter, new SquareAdder(70, 145));
        }
        
        public void FillBorders()
        {
            BorderFiller._delta = BorderWidth;
            BorderFiller._percent = PercentCovered;
            ImageProcessor.ProcessImage(ImageProcessor.XRayAfter, new BorderFiller());
        }

        public void CompareBitByBit() => ImageProcessor.CompareImages(ImageProcessor.XRayBefore, ImageProcessor.XRayAfter, ImageProcessor.XRayImagesDiff, new BitByBitComparator());

        public void CompareSubimages()
        {
            if (int.TryParse(PowerForImageDivision, out int power))
            {
                ImageProcessor.CompareImages(ImageProcessor.XRayBefore, ImageProcessor.XRayAfter, ImageProcessor.XRayImagesDiff, new SubimagesComparator(power));
            }
        }

        public void DetectFixedSquares()
        {
            _imageProcessor.DetectSquares(_imageProcessor.XRayAfter, _imageProcessor.XRayImagesDiff, new SquareDetector(70, 145));
        }
    }
}
