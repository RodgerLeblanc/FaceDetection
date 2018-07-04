using System.Windows;

namespace FaceDetection.WPF.ViewModels
{
    public class MessageViewModel : IMessageViewModel
    {
        public MessageViewModel(string messageBoxText)
            : this(messageBoxText, messageBoxText)
        {
        }

        public MessageViewModel(string messageBoxText, string caption)
            : this(messageBoxText, caption, MessageBoxButton.OKCancel)
        {
        }

        public MessageViewModel(string messageBoxText, string caption, MessageBoxButton button)
            : this(messageBoxText, caption, button, MessageBoxImage.None)
        {
        }

        public MessageViewModel(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
            : this(messageBoxText, caption, button, icon, MessageBoxResult.None)
        {
        }

        public MessageViewModel(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
            : this(messageBoxText, caption, button, icon, defaultResult, MessageBoxOptions.None)
        {
        }

        public MessageViewModel(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options)
        {
            MessageBoxText = messageBoxText;
            Caption = caption;
            Button = button;
            Icon = icon;
            DefaultResult = defaultResult;
            Options = options;
        }

        public string MessageBoxText { get; }
        public string Caption { get; }
        public MessageBoxButton Button { get; }
        public MessageBoxImage Icon { get; }
        public MessageBoxResult DefaultResult { get; }
        public MessageBoxOptions Options { get; }
    }
}
