using Akka.Actor;

namespace ApiClient.MarketResearch.Services.Actors
{
    public class SystemActors
    {
        public static IActorRef ApiClient = ActorRefs.Nobody;
    }
}