﻿using Caliburn.Micro;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using XRayImageProcessing.Models;
using XRayImageProcessing.Models.Comparators;
using XRayImageProcessing.Models.Detectors;
using XRayImageProcessing.Models.Processors;

namespace XRayImageProcessing.ViewModels
{
    public class ShellViewModel : Screen
    {
        private string _chosenPath = "default";

        public override event PropertyChangedEventHandler PropertyChanged;

        public int BorderWidth { get; set; } = 150;
        public int PercentCovered { get; set; } = 40;
        public int SquareNumberBorder { get; set; } = 128;
        public string PowerForImageDivision { get; set; } = "20";
        public string ChosenPath
        {
            get => _chosenPath;
            set
            {
                _chosenPath = value;
                OnPropertyChanged();
            }
        }

        public ImageProcessor ImageProcessor { get; set; }
        public void ChooseFile(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".png",
                Filter = "Png files (.png)|*.png"
            };

            var result = openFileDialog.ShowDialog();

            if (result != true) return;
            ChosenPath = openFileDialog.FileName;
            OpenNewImage(ChosenPath);
        }
        public void SaveFile(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                FileName = Path.GetFileNameWithoutExtension(_chosenPath) + "_" + DateTime.Now.ToString("yyyy-dd-MM_HH-mm-ss") + ".png",
                DefaultExt = ".png",
                Filter = "Png files (.png)|*.png"
            };

            var result = saveFileDialog.ShowDialog();

            if (result != true) return;
            var path = saveFileDialog.FileName;
            ImageProcessor.XRayAfter.Save(path);
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

        public void InvertColors() => ImageProcessor.ProcessImage(ImageProcessor.XRayAfter, new ImageInverter());

        public void AddCircle() => ImageProcessor.ProcessImage(ImageProcessor.XRayAfter, new CircleAdder());

        public void AddSquare() => ImageProcessor.ProcessImage(ImageProcessor.XRayAfter, new SquareAdder());
        
        public void CutLungs()
        {
            LungsResection.SquareNumberBorder = SquareNumberBorder;
            ImageProcessor.ProcessImage(ImageProcessor.XRayAfter, new LungsResection());
        }

        public void FloodFill()
        {
            FloodFiller.Percent = 5;
            ImageProcessor.ProcessImage(ImageProcessor.XRayAfter, new FloodFiller());
        }

        public void Undo() => ImageProcessor.Undo();

        public void AddFixedSquare()
        {
            ImageProcessor.ProcessImage(ImageProcessor.XRayAfter, new SquareAdder(70, 145));
        }
        
        public void FillBorders()
        {
            BorderFiller.Delta = BorderWidth;
            BorderFiller.Percent = PercentCovered;
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
            ImageProcessor.DetectSquares(ImageProcessor.XRayAfter, ImageProcessor.XRayImagesDiff, new SquareDetector(70, 145));
        }
    }
}
