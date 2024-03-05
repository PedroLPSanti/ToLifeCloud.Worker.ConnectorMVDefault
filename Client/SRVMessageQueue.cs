using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using ToLifeCloud.Worker.ConnectorMVDefault.Responses;
using ToLifeCloud.Worker.ConnectorMVDefault.Structs;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Client
{
    public static class SRVMessageQueue
    {
        public static T? GetMessage<T>(string urlBase, string idMessageType, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = $"{urlBase}/Message?idMessageType={idMessageType}";

                client.DefaultRequestHeaders.Add("Authorization", token);

                var response = client.GetAsync(endpoint).Result;

                var responseString = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"POST - {endpoint} - statusCode: {response.StatusCode} - reasonPhrase: {response.ReasonPhrase} - response: {responseString}");

                MessageQueueResponse result = JsonConvert.DeserializeObject<MessageQueueResponse>(responseString);

                if (result.isError) throw new Exception(result.errorDescription);

                if (result.message == null) return default(T);

                return JsonConvert.DeserializeObject<T>(result.message);
            }
        }
    }
}
