using Caliburn.Micro;
using System.Windows;
using XRayImageProcessing.ViewModels;

namespace XRayImageProcessing
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
