using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiClient.MarketResearch.Services {

    public delegate Task OnMessageReceivedHandler(IEnumerable<Models.Makelaar> makelaars);
    public interface IMakelaar {
        event OnMessageReceivedHandler OnMakelaarDataReceived;
        void RequestMakelaarData();
    }
}
