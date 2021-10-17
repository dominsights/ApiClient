using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ApiClient.MarketResearch.Services.Facade
{
    public class ApiClientFacade : ISearchApi
    {
        private readonly HttpClient _httpClient;

        public ApiClientFacade(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IEnumerable<Models.Object> SearchApi(int page, int pageSize, string queryFilters)
        {
            throw new NotImplementedException();
        }
    }
}