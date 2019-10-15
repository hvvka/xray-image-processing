using Caliburn.Micro;
using System;
using System.Windows;
using XRayImageProcessing.Models;

namespace XRayImageProcessing.ViewModels
{
    public class ShellViewModel : Screen
    {
        private ImageProcessor _imageProcessor;

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
                string filename = openFileDialog.FileName;
                OpenNewImage(filename);
            }
        }

        public ShellViewModel()
        {
            // Todo
            string path = @"C:\Users\Zofia\Documents\projects\xray-image-processing\xray-image-processing\Resources\samples\00030636_017.png";
            OpenNewImage(path);
        }

        private void OpenNewImage(string path)
        {
            _imageProcessor = new ImageProcessor(new Uri(path));
        }
    }
}
