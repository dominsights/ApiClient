using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using ApiClient.MarketResearch.Services.Actors;
using ApiClient.MarketResearch.Services.Models;
using Object = ApiClient.MarketResearch.Services.Models.Object;

namespace ApiClient.MarketResearch.Services {
    public class Makelaar : IMakelaar {
        private readonly ApiCoordinatorFactory _apiCoordinatorFactory;
        public event EventHandler<MakelaarDataReceivedEventArgs> OnMakelaarDataReceived;

        public Makelaar(ApiCoordinatorFactory apiCoordinatorFactory)
        {
            _apiCoordinatorFactory = apiCoordinatorFactory;
        }

        public async void RequestMakelaarData(int pageSize, string queryFilters)
        {
            try
            {
                var apiCoordinator = _apiCoordinatorFactory.Create();
                IEnumerable<Object> objects = await apiCoordinator
                    .Ask(new ApiCoordinator.SearchObjects(pageSize, queryFilters), TimeSpan.FromSeconds(30))
                    .ContinueWith(task =>
                    {
                        var result = task.Result;
                        return result switch
                        {
                            IEnumerable<Object> objects => new List<Object>(objects),
                            _ => null //TODO: find a better way to handle error case and send to view
                        };
                    });

                var makelaarsData = objects
                    .GroupBy(o => o.MakelaarId)
                    .OrderByDescending(g => g.Count())
                    .Select(g => new Models.Makelaar(g.Key, g.First().MakelaarNaam, g.Count()))
                    .Take(10);

                OnMakelaarDataReceived?.Invoke(this, new MakelaarDataReceivedEventArgs(makelaarsData));
            }
            catch
            {
                OnMakelaarDataReceived?.Invoke(this, new MakelaarDataReceivedEventArgs(null));
            }
        }
    }
}