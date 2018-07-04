using FaceDetection.WPF.ViewModels;
using System.Windows;

namespace FaceDetection.WPF.Services
{
    public interface IViewModelService
    {
        void ShowViewModel(IViewModel viewModel);
        void ShowMessage(IMessageViewModel viewModel);
        void ShowMessage(string messageBoxText, string caption, MessageBoxButton button);
    }
}