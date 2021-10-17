using System.Collections.Generic;

namespace ApiClient.MarketResearch.Services
{
    public interface ISearchApi
    {
        IEnumerable<Models.Object> SearchApi(int page, int pageSize, string queryFilters);
    }
}