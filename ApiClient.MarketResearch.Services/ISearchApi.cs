using System.Collections.Generic;

namespace ApiClient.MarketResearch.Services
{
    public interface ISearchApi
    {
        QueryResult SearchApi(int page, int pageSize, string queryFilters);
    }
}