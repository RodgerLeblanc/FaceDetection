using System.Windows;

namespace FaceDetection.WPF.ViewModels
{
    public interface IMessageViewModel
    {
        string MessageBoxText { get; }
        string Caption { get; }
        MessageBoxButton Button { get; }
        MessageBoxImage Icon { get; }
        MessageBoxResult DefaultResult { get; }
        MessageBoxOptions Options { get; }
    }
}
