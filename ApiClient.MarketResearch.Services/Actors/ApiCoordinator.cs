using System;
using System.Collections.Generic;
using System.Net.Http;
using Akka.Actor;

namespace ApiClient.MarketResearch.Services.Actors
{
    public class ApiCoordinator : ReceiveActor
    {
        public record SearchObjects(int PageSize, string QueryFilters);

        public ApiCoordinator(ISearchApi searchApi)
        {
            Receive<SearchObjects>(search =>
            {
                // execute first query
                var workerActor = Context.ActorOf(Props.Create(() => new ApiWorker(searchApi)));
                workerActor.Tell(new ApiWorker.ExecuteQuery(search.QueryFilters, 1, search.PageSize));
                // get page count
                // execute all other queries - 1 in parallel
                // group data and return
                // implement retry if time is enough

            });

            Receive<QueryResult>(msg =>
            {
                // needs page count
            });
        }
    }
}