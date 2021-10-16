using System;
using System.Net.Http;
using Akka.Actor;

namespace ApiClient.MarketResearch.Services.Actors
{
    public class ApiCoordinator : ReceiveActor
    {
        public record SearchObjects(int PageSize);

        public ApiCoordinator(IHttpClientFactory httpClientFactory)
        {
            Receive<SearchObjects>(_ =>
            {
                var httpClient = httpClientFactory.CreateClient();
                var response = httpClient.GetAsync("https://localhost:44364/account");
            });
        }
    }
}