using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using ApiClient.MarketResearch.Services.Actors;
using ApiClient.MarketResearch.Services.Models;
using Object = ApiClient.MarketResearch.Services.Models.Object;

namespace ApiClient.MarketResearch.Services {
    public class Makelaar : IMakelaar {
        public event EventHandler<MakelaarDataReceivedEventArgs> OnMakelaarDataReceived;

        public async void RequestMakelaarData(int pageSize, string queryFilters)
        {
            IEnumerable<Object> objects = await SystemActors.ApiClient.Ask(new ApiCoordinator.SearchObjects(pageSize, queryFilters), TimeSpan.FromSeconds(30))
                .ContinueWith(task =>
                {
                    var result = task.Result;
                    return result switch
                    {
                        IEnumerable<Object> objects => new List<Object>(objects),
                        _ => new List<Object>()
                    };
                });

            var makelaarsData = objects
                .GroupBy(o => o.MakelaarId)
                .OrderByDescending(g => g.Count())
                .Select(g => new Models.Makelaar(g.Key, g.First().MakelaarNaam, g.Count()))
                .Take(10);

            OnMakelaarDataReceived?.Invoke(this,new MakelaarDataReceivedEventArgs(makelaarsData));
        }
    }
}