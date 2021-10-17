using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ApiClient.MarketResearch.Services.Facade
{
    public class ApiClientFacade
    {
        private readonly HttpClient _httpClient;

        public ApiClientFacade(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IEnumerable<Models.Object> SearchApi(int page, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}