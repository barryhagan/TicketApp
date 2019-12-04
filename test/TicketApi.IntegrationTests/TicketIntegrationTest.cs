using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TicketCore.Exceptions;
using Xunit;

namespace TicketApi.IntegrationTests
{
    public class TicketIntegrationTest : IDisposable, IClassFixture<TicketTestHost>
    {
        private readonly HttpClient testClient;

        public TicketIntegrationTest(TicketTestHost testHost)
        {
            testClient = testHost.CreateClient();
        }

        public void Dispose()
        {
            testClient?.Dispose();
        }

        protected async Task<T> GraphRequestAsync<T>(string gqlQuery)
        {
            var graphResult = await SendGraphRequestAsync(gqlQuery);

            if (graphResult?.data is JArray array)
            {
                return array.ToObject<T>();
            }

            if (graphResult?.data is JObject obj)
            {
                return obj.ToObject<T>();
            }

            return default(T);
        }

        private async Task<GraphResult> SendGraphRequestAsync(string gqlQuery)
        {
            return await GetGraphResultAsync(await SendRequestAsync(gqlQuery));
        }

        private async Task<HttpResponseMessage> SendRequestAsync(string gqlQuery)
        {
            var requestMethod = new HttpMethod("POST");
            var request = new HttpRequestMessage(requestMethod, "/graphql");

            var body = new StringContent(JsonConvert.SerializeObject(new GraphRequest { query = gqlQuery }), Encoding.UTF8, "application/json");
            request.Content = body;

            return await testClient.SendAsync(request);
        }

        private async Task<GraphResult> GetGraphResultAsync(HttpResponseMessage response)
        {
            GraphResult graphResult = JsonConvert.DeserializeObject<GraphResult>(await response.Content.ReadAsStringAsync());
            if (!response.IsSuccessStatusCode)
            {
                if (graphResult != null && graphResult.errors.Any())
                {
                    throw new TicketAppException($"Error loading graph data from server. {graphResult.errors.First().message}");
                }
                else
                {
                    throw new TicketAppException($"The server returned status {(int)response.StatusCode}");
                }
            }

            if (graphResult?.errors?.Any() ?? false)
            {
                throw new TicketAppException($"Error response from GraphQL server. {graphResult.errors.First().message}");
            }

            return graphResult;
        }

        private class GraphRequest
        {
            public string query { get; set; }           
        }

        private class GraphResult
        {
            public object data { get; set; }
            public List<GraphError> errors { get; set; }
        }

        private class GraphError
        {
            public string message { get; set; }
        }
    }
}
