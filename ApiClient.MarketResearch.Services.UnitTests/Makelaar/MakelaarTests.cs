using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Xunit;
using Akka.TestKit.Xunit2;
using ApiClient.MarketResearch.Services.Actors;
using ApiClient.MarketResearch.Services.Models;

namespace ApiClient.MarketResearch.Services.UnitTests.Makelaar {
    public class MakelaarTests : TestKit, IClassFixture<MakelaarFixture> {
        private readonly MakelaarFixture _fixture;

        public MakelaarTests(MakelaarFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public async Task Should_return_makelaar_list_when_request_is_success()
        {
            var testProbe = CreateTestProbe();
            SystemActors.ApiClient = testProbe;
            
            var makelaarService = new Services.Makelaar();
            var result = await Assert.RaisesAsync<MakelaarDataReceivedEventArgs>(h => makelaarService.OnMakelaarDataReceived += h,
                h => makelaarService.OnMakelaarDataReceived -= h,
                 async () =>
                {
                    makelaarService.RequestMakelaarData(20);
                    testProbe.ExpectMsg<Actors.ApiClient.SearchObjects>();
                    testProbe.Reply(new List<Models.Object>(_fixture.ObjectsObtained));
                    await Task.Delay(TimeSpan.FromSeconds(1));
                });
            
            Assert.Equal(_fixture.ExpectedTop10Makelaars, result.Arguments.MakelaarsData);
        }
        
        [Fact]
        public async Task Should_fire_event_when_status_is_failure()
        {
            var testProbe = CreateTestProbe();
            SystemActors.ApiClient = testProbe;
            
            var makelaarService = new Services.Makelaar();
            makelaarService.RequestMakelaarData(20);
            
            var result = await Assert.RaisesAsync<MakelaarDataReceivedEventArgs>(h => makelaarService.OnMakelaarDataReceived += h,
                h => makelaarService.OnMakelaarDataReceived -= h,
                async () =>
                {
                    makelaarService.RequestMakelaarData(20);
                    testProbe.ExpectMsg<Actors.ApiClient.SearchObjects>();
                    testProbe.Reply(new Status.Failure(new Exception()));
                    await Task.Delay(TimeSpan.FromSeconds(1));
                });
        
            Assert.False(result.Arguments.MakelaarsData.Any());
        }
    }
}