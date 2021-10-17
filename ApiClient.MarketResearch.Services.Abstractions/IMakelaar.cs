using System;
using ApiClient.MarketResearch.Services.Models;

namespace ApiClient.MarketResearch.Services {
    public interface IMakelaar {
        event EventHandler<MakelaarDataReceivedEventArgs> OnMakelaarDataReceived;
        void RequestMakelaarData(int pageSize, string queryFilters);
    }
}
