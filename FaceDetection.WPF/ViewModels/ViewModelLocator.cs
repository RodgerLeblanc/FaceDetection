using CommonServiceLocator;
using FaceDetection.WPF.Services;
using GalaSoft.MvvmLight.Ioc;

namespace FaceDetection.WPF.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<IViewModelService, ViewModelService>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get => ServiceLocator.Current.GetInstance<MainWindowViewModel>();
        }

        public static void Cleanup()
        {
            //TODO Clear the ViewModels
        }
    }
}