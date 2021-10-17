using System;
using Akka.Actor;
using ApiClient.MarketResearch.Services.Facade;

namespace ApiClient.MarketResearch.Services.Actors
{
    public class ApiWorker : UntypedActor
    {
        public record QueryData(string QueryFilters, int Page, int PageSize);
        
        private readonly ApiClientFacade _apiClientFacade;

        public ApiWorker(ApiClientFacade apiClientFacade)
        {
            _apiClientFacade = apiClientFacade;
        }
        
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case QueryData query: throw new NotImplementedException();
                default: throw new NotSupportedException();
            }
            
            throw new System.NotImplementedException();
        }
    }
}