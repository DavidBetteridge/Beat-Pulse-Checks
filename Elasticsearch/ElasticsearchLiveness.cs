using BeatPulse.Core;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BeatPulse.Elasticsearch
{
    public class ElasticsearchLiveness : IBeatPulseLiveness
    {
        readonly HttpClient _httpClient;
        public ElasticsearchLiveness(string endpoint)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(endpoint)
            };

            _httpClient.DefaultRequestHeaders
                   .Accept
                   .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<LivenessResult> IsHealthy(LivenessExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var response = await _httpClient.GetAsync("_cluster/health");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var results = JsonConvert.DeserializeObject<ClusterHealth>(json);

                switch (results.status)
                {
                    case ClusterStatus.Green:
                        return LivenessResult.Healthy("Green");

                    case ClusterStatus.Yellow:
                        return LivenessResult.Healthy("Yellow (no replicas)");

                    case ClusterStatus.Red:
                        return LivenessResult.UnHealthy("Red");

                    default:
                        return LivenessResult.UnHealthy("Unknown status - " + results.status);
                }

            }
            catch (Exception ex)
            {
                return LivenessResult.UnHealthy(ex);
            }
        }

        class ClusterStatus
        {
            public const string Green = "green";
            public const string Yellow = "yellow";
            public const string Red = "red";
        }


    }
}
