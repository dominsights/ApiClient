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
                // execute first query
                // get page count
                // execute all other queries - 1 in parallel
                // group data and return
                // implement retry if time is enough
                
                var httpClient1 = httpClientFactory.CreateClient();
                var httpClient2 = httpClientFactory.CreateClient();
                var response1 = httpClient1.GetAsync("https://localhost:44364/account");
                var response2 = httpClient2.GetAsync("https://localhost:44364/account");
            });
        }
    }
}