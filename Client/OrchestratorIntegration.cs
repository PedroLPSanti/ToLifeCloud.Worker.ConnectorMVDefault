using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using ToLifeCloud.Worker.ConnectorMVDefault.Requests;
using ToLifeCloud.Worker.ConnectorMVDefault.Responses;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Client
{
    public static class OrchestratorIntegration
    {
        public static EpisodeResponse CreateEpisode(string urlBase, PostEpisodeRequest body, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = $"{urlBase}/api/v1/Episode";

                string jsonContent = JsonConvert.SerializeObject(body);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Add("Authorization", token);

                var response = client.PostAsync(endpoint,content).Result;

                var responseString = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"POST - {endpoint} - statusCode: {response.StatusCode} - reasonPhrase: {response.ReasonPhrase} - response: {responseString}");

                EpisodeResponse result = JsonConvert.DeserializeObject<EpisodeResponse>(responseString);

                return result;
            }
        }
    }
}
