using Akka.Actor;
using Akka.DI.Core;

namespace ApiClient.MarketResearch.Services.Actors
{
    public class ApiCoordinatorFactory
    {
        public virtual IActorRef Create()
        {
            return ActorSystemRefs.ActorSystem.ActorOf(ActorSystemRefs.ActorSystem.DI().Props<ApiCoordinator>());
        }
    }
}