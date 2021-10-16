using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Akka.TestKit.Xunit2;
using ApiClient.MarketResearch.Services.Actors;

namespace ApiClient.MarketResearch.Services.UnitTests.Makelaar {
    public class MakelaarTests : TestKit, IClassFixture<MakelaarFixture> {
        private readonly MakelaarFixture _fixture;
        private bool _eventIsFired;

        public MakelaarTests(MakelaarFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public void Should_return_makelaar_list_when_request_is_success()
        {
            var testProbe = CreateTestProbe();
            SystemActors.ApiClient = testProbe;
            
            var makelaarService = new Services.Makelaar();
            makelaarService.RequestMakelaarData(20);
            makelaarService.OnMakelaarDataReceived += makelaars =>
            {
                Assert.Equal(_fixture.ExpectedTop10Makelaars, makelaars);
                _eventIsFired = true;
                return Task.CompletedTask;
            };

            testProbe.ExpectMsg<Actors.ApiClient.SearchObjects>();
            testProbe.Reply(new List<Models.Object>(_fixture.ObjectsObtained));
            Assert.True(_eventIsFired);
        }
    }
}