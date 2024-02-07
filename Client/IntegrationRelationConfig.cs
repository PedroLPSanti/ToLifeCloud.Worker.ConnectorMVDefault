using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using ToLifeCloud.Worker.ConnectorMVDefault.Requests;
using ToLifeCloud.Worker.ConnectorMVDefault.Responses;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Client
{
    public static class IntegrationRelationConfig
    {
        public static IntegrationVariableResponse GetVariables(string urlBase, long idHealthUnit, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = $"{urlBase}/ConfigHash/Variables/idHealthUnit/{idHealthUnit}/idIntegrationType/7";

                client.DefaultRequestHeaders.Add("Authorization", token);

                var response = client.GetAsync(endpoint).Result;

                var responseString = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"POST - {endpoint} - statusCode: {response.StatusCode} - reasonPhrase: {response.ReasonPhrase} - response: {responseString}");

                IntegrationVariableResponse result = JsonConvert.DeserializeObject<IntegrationVariableResponse>(responseString);

                return result;
            }
        }

        public static void SendKeepAlive(string urlBase, KeepAliveRequest body, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = $"{urlBase}/KeepAlive";

                client.DefaultRequestHeaders.Add("Authorization", token);

                string jsonContent = JsonConvert.SerializeObject(body);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = client.PostAsync(endpoint, content).Result;

                var responseString = response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
