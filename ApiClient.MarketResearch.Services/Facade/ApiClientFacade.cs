using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ApiClient.MarketResearch.Services.Facade
{
    public class ApiClientFacade : ISearchApi
    {
        private readonly HttpClient _httpClient;

        public ApiClientFacade(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
        }
        public IEnumerable<Models.Object> SearchApi(int page, int pageSize, string queryFilters)
        {
            // take url
            // form url with filters
            // send request using client
            // return parsed response
            throw new NotImplementedException();
        }
    }
}