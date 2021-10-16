using System;

namespace ApiClient.MarketResearch.Services.Models
{
    public record Object(Guid Id, int MakelaarId, string MakelaarNaam);
}