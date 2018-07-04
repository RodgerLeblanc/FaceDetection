using GalaSoft.MvvmLight;
using System.ComponentModel;

namespace FaceDetection.WPF.ViewModels
{
    public class BaseViewModel : ViewModelBase, IViewModel
    {
        public BaseViewModel() : base()
        {
            PropertyChanged += BaseViewModel_PropertyChanged;
        }

        private void BaseViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(sender, e);
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
