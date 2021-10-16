using System;
using System.Collections.Generic;

namespace ApiClient.MarketResearch.Services.Models
{
    public class MakelaarDataReceivedEventArgs : EventArgs
    {
        public MakelaarDataReceivedEventArgs(IEnumerable<Makelaar> makelaarsData)
        {
            MakelaarsData = makelaarsData;
        }

        public IEnumerable<Models.Makelaar> MakelaarsData { get;  }
    }
}