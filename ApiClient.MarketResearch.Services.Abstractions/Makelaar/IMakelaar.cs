using System.Threading.Tasks;
using System.Collections.Generic;

namespace ApiClient.MarketResearch.Services.Makelaar {

    public delegate Task OnMessageReceivedHandler(IEnumerable<Models.Makelaar> makelaars);
    public interface IMakelaar {
        event OnMessageReceivedHandler OnMakelaarDataReceived;
        void RequestMakelaarData();
    }
}
