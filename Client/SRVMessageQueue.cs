using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using ToLifeCloud.Worker.ConnectorMVDefault.Responses;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Client
{
    public static class SRVMessageQueue
    {
        public static MessageQueueResponse GetMessage(string urlBase, string idMessageType, string token)
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

                return result;
            }
        }
    }
}
