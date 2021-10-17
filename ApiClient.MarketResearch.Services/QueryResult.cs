using System.Collections.Generic;

namespace ApiClient.MarketResearch.Services
{
    public record QueryResult(int PageCount, IEnumerable<Models.Object> Objects);
}