namespace AzureApiHelper.Constants
{
    public class AzureApiConstants
    {
        public static string SubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
        public static string SubscriptionKey = "[YOUR_API_KEY]";
        public static string UriBase = "https://eastus.api.cognitive.microsoft.com/face/v1.0/detect";
        public static string DefaultRequestParameters =
            "returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";
    }
}
