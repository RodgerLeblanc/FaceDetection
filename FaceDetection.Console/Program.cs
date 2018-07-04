using AzureApiHelper;
using AzureApiHelper.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceDetection.Console
{
    class Program
    {
        static void Main()
        {
            System.Console.WriteLine("Trois-Rivières Police Department");
            System.Console.Write("Enter the path to the image of the suspect: ");
            string imageFilePath = System.Console.ReadLine();

            if (String.IsNullOrEmpty(imageFilePath))
                imageFilePath = @"C:\Users\roger\Downloads\331181_10150553537532873_1945643508_o.jpg";

            if (File.Exists(imageFilePath))
            {
                System.Console.WriteLine("\nWait a moment while we look at the suspect(s)...\n");
                AnalyzeSuspect(imageFilePath).Wait();
            }
            else
            {
                System.Console.WriteLine("\nInvalid file path");
            }

            System.Console.WriteLine("That's all we know. Press enter to exit...");
            System.Console.ReadLine();
        }

        static async Task AnalyzeSuspect(string imageFilePath)
        {
            byte[] byteData = GetImageAsByteArray(imageFilePath);
            IEnumerable<FaceModel> faces = await AzureApi.GetFacesAsync(byteData);

            if (!faces.Any())
            {
                System.Console.WriteLine("No suspect found.");
                System.Console.WriteLine();
                return;
            }

            int suspectNumber = 1;
            foreach (FaceModel face in faces)
            {
                PrintFaceDetails(face, suspectNumber++);
            }
        }

        private static void PrintFaceDetails(FaceModel face, int suspectNumber)
        {
            if (!(face.FaceAttributes is FaceAttributes faceAttributes))
                return;

            int age = Convert.ToInt32(faceAttributes?.Age ?? 115);
            string gender = faceAttributes?.Gender ?? "transgender";

            string hair = faceAttributes.Hair == null || faceAttributes.Hair.Invisible ?
                "no" :
                faceAttributes.Hair.HairColor?.OrderByDescending(h => h.Confidence).FirstOrDefault()?.Color ?? "no";

            string accessories = null;
            if (faceAttributes.Accessories?.Any() == true)
            {
                IList<string> accessoryList = faceAttributes.Accessories
                    .OrderByDescending(a => a.Confidence)
                    .Select(a => a.Type)
                    .ToList();

                accessories = String.Join(",", accessoryList);
            }


            bool moustache = faceAttributes.FacialHair?.Moustache > 0.5;
            bool beard = faceAttributes.FacialHair?.Beard > 0.5;
            bool sideburns = faceAttributes.FacialHair?.Sideburns > 0.5;

            StringBuilder facialHair = new StringBuilder();
            if (moustache)
                facialHair.Append("moustache");

            if (beard)
                facialHair.Append(facialHair.Length > 0 ? ", beard" : "beard");

            if (sideburns)
                facialHair.Append(facialHair.Length > 0 ? ", sideburns" : "sideburns");

            if (facialHair.Length == 0)
                facialHair.Append("no facial hair");

            bool eyeMakeup = faceAttributes.Makeup?.EyeMakeup ?? false;
            bool lipMakeup = faceAttributes.Makeup?.LipMakeup ?? false;

            StringBuilder makeup = new StringBuilder();
            if (eyeMakeup)
                makeup.Append("eye makeup");

            if (lipMakeup)
                makeup.Append(makeup.Length > 0 ? " and lip makeup" : "lip makeup");

            if (makeup.Length == 0)
                makeup.Append("no makeup");

            string emotion = null;
            if (faceAttributes.Emotion != null)
            {
                IList<KeyValuePair<double, string>> emotions = new List<KeyValuePair<double, string>>();
                emotions.Add(new KeyValuePair<double, string>(faceAttributes.Emotion.Anger, nameof(faceAttributes.Emotion.Anger)));
                emotions.Add(new KeyValuePair<double, string>(faceAttributes.Emotion.Contempt, nameof(faceAttributes.Emotion.Contempt)));
                emotions.Add(new KeyValuePair<double, string>(faceAttributes.Emotion.Disgust, nameof(faceAttributes.Emotion.Disgust)));
                emotions.Add(new KeyValuePair<double, string>(faceAttributes.Emotion.Fear, nameof(faceAttributes.Emotion.Fear)));
                emotions.Add(new KeyValuePair<double, string>(faceAttributes.Emotion.Happiness, nameof(faceAttributes.Emotion.Happiness)));
                emotions.Add(new KeyValuePair<double, string>(faceAttributes.Emotion.Neutral, nameof(faceAttributes.Emotion.Neutral)));
                emotions.Add(new KeyValuePair<double, string>(faceAttributes.Emotion.Sadness, nameof(faceAttributes.Emotion.Sadness)));
                emotions.Add(new KeyValuePair<double, string>(faceAttributes.Emotion.Surprise, nameof(faceAttributes.Emotion.Surprise)));

                emotion = emotions.OrderByDescending(kv => kv.Key).FirstOrDefault().Value.ToLower();
            }
            else
            {
                emotion = "no emotion";
            }

            bool isMale = gender == "male";
            string heShe = isMale ? "He" : "She";
            string hisHer = isMale ? "his" : "her";

            System.Console.WriteLine($"The suspect #{suspectNumber} is a {age} year old {gender}, with {hair} hair, {facialHair.ToString()}.");
            System.Console.WriteLine($"{heShe} is also wearing {makeup.ToString()}, we could read {emotion} on {hisHer} face.");

            if (!String.IsNullOrEmpty(accessories))
                System.Console.WriteLine($"{heShe} is also wearing those accessories : {accessories}.");

            System.Console.WriteLine();
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}
