using FaceDetection.WPF.ViewModels;
using System.Windows;

namespace FaceDetection.WPF.Services
{
    public class ViewModelService : IViewModelService
    {
        public void ShowViewModel(IViewModel viewModel)
        {
            Window window = new Window { Content = viewModel };
            window.Show();
        }

        public void ShowMessage(IMessageViewModel viewModel)
        {
            MessageBox.Show(
                viewModel.MessageBoxText,
                viewModel.Caption,
                viewModel.Button,
                viewModel.Icon,
                viewModel.DefaultResult,
                viewModel.Options);
        }

        public void ShowMessage(string messageBoxText, string caption, MessageBoxButton button)
        {
            ShowMessage(new MessageViewModel(messageBoxText, caption, button));
        }
    }
}
