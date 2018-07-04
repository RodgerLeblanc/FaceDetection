using AzureApiHelper.Constants;
using AzureApiHelper.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AzureApiHelper
{
    public class AzureApi
    {
        public static async Task<IEnumerable<FaceModel>> GetFacesAsync(byte[] byteData)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(AzureApiConstants.SubscriptionKeyHeader, AzureApiConstants.SubscriptionKey);

                string uri = AzureApiConstants.UriBase + "?" + AzureApiConstants.DefaultRequestParameters;

                HttpResponseMessage response;
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);
                }

                return FaceModel.FromJson(await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }

            return null;
        }
    }
}
