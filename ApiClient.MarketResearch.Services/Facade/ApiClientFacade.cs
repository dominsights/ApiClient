using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace ApiClient.MarketResearch.Services.Facade
{
    public class ApiClientFacade : ISearchApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly string _apiUrl;

        public ApiClientFacade(IHttpClientFactory httpClientFactory, string apiKey, string apiUrl)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = apiKey;
            _apiUrl = apiUrl;
        }
        public QueryResult SearchApi(int page, int pageSize, string queryFilters)
        {
            string url = $"{_apiUrl}/{_apiKey}/?{queryFilters}/&page={page}&pagesize={pageSize}";
            var httpClient = _httpClientFactory.CreateClient();
            var result = httpClient.GetAsync(url).Result;
            var content = result.Content.ReadAsStringAsync().Result;
            var deserialized = JsonConvert.DeserializeObject<SearchResult>(content); 
            var objects = deserialized.Objects.Select(o => new Models.Object(Guid.Parse(o.Id), o.MakelaarId, o.MakelaarNaam));
            return new QueryResult(deserialized.Paging.AantalPaginas, objects);
        }
    }
}