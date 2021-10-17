using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ApiClient.MarketResearch.Services.Facade
{
    public class ApiClientFacade : ISearchApi
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl;

        public ApiClientFacade(HttpClient httpClient, string apiKey, string apiUrl)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
            _apiUrl = apiUrl;
        }
        public IEnumerable<Models.Object> SearchApi(int page, int pageSize, string queryFilters)
        {
            string url = $"{_apiUrl}/{_apiKey}/?{queryFilters}/&page={page}&pagesize={pageSize}";
            var result = _httpClient.GetAsync(url).Result;
            var content = result.Content.ReadAsStringAsync().Result;
            var deserialized = JsonConvert.DeserializeObject<SearchResult>(content); 
            return deserialized.Objects.Select(o => new Models.Object(Guid.Parse(o.Id), o.MakelaarId, o.MakelaarNaam));
        }
    }
}