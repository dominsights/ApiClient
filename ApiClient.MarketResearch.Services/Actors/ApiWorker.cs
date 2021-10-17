using System;
using Akka.Actor;

namespace ApiClient.MarketResearch.Services.Actors
{
    public class ApiWorker : UntypedActor
    {
        public record ExecuteQuery(string QueryFilters, int Page, int PageSize);

        public record QueryFailed(int Page, Exception Exception);
        
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
                        Sender.Tell(new QueryFailed(page, e));
                    }
                    break;
                default: throw new NotSupportedException();
            }
        }
    }
}