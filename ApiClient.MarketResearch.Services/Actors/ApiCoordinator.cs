using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.Event;
using Akka.Routing;
using Akka.Util.Internal;

namespace ApiClient.MarketResearch.Services.Actors
{
    public class ApiCoordinator : ReceiveActor
    {
        public record SearchObjects(int PageSize, string QueryFilters);

        private IEnumerable<Models.Object> _objects;
        private SearchObjects _search;
        private readonly List<int> _requests;
        private IActorRef _sender;
        private readonly ILoggingAdapter _log = Logging.GetLogger(Context);

        public ApiCoordinator(ISearchApi searchApi)
        {
            _requests = new List<int>();
            
            Receive<SearchObjects>(search =>
            {
                _search = search;
                _sender = Sender;
                var workerActor = Context.ActorOf(Props.Create(() => new ApiWorker(searchApi)));
                workerActor.Tell(new ApiWorker.ExecuteQuery(search.QueryFilters, 1, search.PageSize));
            });

            Receive<QueryResult>(result =>
            {
                Context.Stop(Sender);
                _objects = result.Objects;
                Become(Initialized);
                var workerActor = Context.ActorOf(Props.Create(() => new ApiWorker(searchApi)).WithRouter(new RoundRobinPool(10, new DefaultResizer(5, result.PageCount))));
                Enumerable.Range(2, result.PageCount - 1)
                    .ForEach(i =>
                    {
                        _log.Info($"Sending execute query for page {i}");
                        _requests.Add(i);
                        workerActor.Tell(new ApiWorker.ExecuteQuery(_search.QueryFilters, i, _search.PageSize));
                    });
            });
            
            Receive<ApiWorker.QueryFailed>(failure =>
            {
                //TODO: Implement retry up to N times
                _log.Error(failure.Exception, "Query failed!");
                _sender.Tell(new Status.Failure(failure.Exception));
                Context.Stop(Self);
            });
        }
        
        private void Initialized()
        {
            Receive<QueryResult>(msg =>
            {
                _objects = _objects.Concat(msg.Objects);
                _requests.Remove(msg.Page);

                if (!_requests.Any())
                {
                    _log.Info("[QueryResult] All results received!");
                    _sender.Tell(_objects);
                    Context.Stop(Self);
                }
            });

            Receive<ApiWorker.QueryFailed>(failure =>
            {
                //TODO: Implement retry up to N times
                _log.Error(failure.Exception, "Query failed!");
                _sender.Tell(new Status.Failure(failure.Exception));
                Context.Stop(Self);
            });
        }
    }
}