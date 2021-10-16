using Akka.Actor;

namespace ApiClient.MarketResearch.Services.Actors
{
    public class ApiClient : UntypedActor
    {
        public record SearchObjects();
        
        protected override void OnReceive(object message)
        {
            throw new System.NotImplementedException();
        }
    }
}