using AzureApiHelper;
using AzureApiHelper.Models;
using FaceDetection.WPF.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FaceDetection.WPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private IViewModelService _viewModelService;

        public MainWindowViewModel(IViewModelService viewModelService) : base()
        {
            _viewModelService = viewModelService;
            CascadeClassifierFileName = @"haarcascade_frontalface_alt_tree.xml";
        }

        public string CascadeClassifierFileName { get; set; }

        public byte[] FaceDetected { get; set; }

        public bool IsLoading { get; set; }

        protected override async void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(sender, e);

            if (e.PropertyName == nameof(FaceDetected) && FaceDetected != null)
            {
                try
                {
                    IsLoading = true;
                    RaisePropertyChanged(nameof(IsLoading));

                    IEnumerable<FaceModel> faces = await Task.Run(async () => await AzureApi.GetFacesAsync(FaceDetected));

                    foreach (FaceModel face in faces)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine($"Age: {face.FaceAttributes.Age}");
                        builder.AppendLine($"Gender: {face.FaceAttributes.Gender}");
                        string hairColor = face.FaceAttributes.Hair.HairColor.OrderByDescending(hc => hc.Confidence).FirstOrDefault().Color;
                        builder.AppendLine($"HairColor: {hairColor}");
                        builder.AppendLine($"Beard: {face.FaceAttributes.FacialHair.Beard}");
                        builder.AppendLine($"Moustache: {face.FaceAttributes.FacialHair.Moustache}");
                        builder.AppendLine($"Smile: {face.FaceAttributes.Smile}");

                        builder.AppendLine();

                        builder.AppendLine("--EMOTIONS--");
                        builder.AppendLine($"Anger: {face.FaceAttributes.Emotion.Anger}");
                        builder.AppendLine($"Contempt: {face.FaceAttributes.Emotion.Contempt}");
                        builder.AppendLine($"Disgust: {face.FaceAttributes.Emotion.Disgust}");
                        builder.AppendLine($"Fear: {face.FaceAttributes.Emotion.Fear}");
                        builder.AppendLine($"Happiness: {face.FaceAttributes.Emotion.Happiness}");
                        builder.AppendLine($"Neutral: {face.FaceAttributes.Emotion.Neutral}");
                        builder.AppendLine($"Sadness: {face.FaceAttributes.Emotion.Sadness}");
                        builder.AppendLine($"Surprise: {face.FaceAttributes.Emotion.Surprise}");

                        _viewModelService.ShowMessage(builder.ToString(), "Face informations", MessageBoxButton.OK);
                    }
                }
                finally
                {
                    IsLoading = false;
                    RaisePropertyChanged(nameof(IsLoading));
                }
            }
        }
    }
}