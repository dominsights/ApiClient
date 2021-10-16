using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using ApiClient.MarketResearch.Services.Actors;
using ApiClient.MarketResearch.Services.Models;

namespace ApiClient.MarketResearch.Services {
    public class Makelaar : IMakelaar {
        public event EventHandler<MakelaarDataReceivedEventArgs> OnMakelaarDataReceived;

        public async void RequestMakelaarData(int pageSize)
        {
            IEnumerable<Models.Object> objects = await SystemActors.ApiClient.Ask(new Actors.ApiClient.SearchObjects(), TimeSpan.FromMinutes(1))
                .ContinueWith(task =>
                {
                    var result = task.Result;
                    return result switch
                    {
                        IEnumerable<Models.Object> objects => new List<Models.Object>(objects),
                        _ => new List<Models.Object>()
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