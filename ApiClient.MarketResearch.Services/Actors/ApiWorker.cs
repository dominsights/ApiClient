using System;
using System.Collections.Generic;
using Akka.Actor;
using ApiClient.MarketResearch.Services.Facade;

namespace ApiClient.MarketResearch.Services.Actors
{
    public class ApiWorker : UntypedActor
    {
        public record ExecuteQuery(string QueryFilters, int Page, int PageSize);
        
        private readonly ISearchApi _searchApi;

        public ApiWorker(ISearchApi searchApi)
        {
            _searchApi = searchApi;
        }
        
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case ExecuteQuery(var queryFilters, var page, var pageSize):
                    try
                    {
                        var objects = _searchApi.SearchApi(page, pageSize, queryFilters);
                        Sender.Tell(objects);
                    }
                    catch(Exception e)
                    {
                        //TODO: Implement retry
                        Sender.Tell(new Status.Failure(e));
                    }
                    break;
                default: throw new NotSupportedException();
            }
        }
    }
}