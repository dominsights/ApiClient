using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Xunit;
using Akka.TestKit.Xunit2;
using ApiClient.MarketResearch.Services.Actors;
using ApiClient.MarketResearch.Services.Models;
using Moq;
using static ApiClient.MarketResearch.Services.UnitTests.Makelaar.MakelaarFixture;

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
            var factory = new Mock<ApiCoordinatorFactory>();
            factory.Setup(f => f.Create()).Returns(testProbe.Ref);
            var makelaarService = new Services.Makelaar(factory.Object);
            var result = await Assert.RaisesAsync<MakelaarDataReceivedEventArgs>(h => makelaarService.OnMakelaarDataReceived += h,
                h => makelaarService.OnMakelaarDataReceived -= h,
                 async () =>
                {
                    makelaarService.RequestMakelaarData(PageSize, QueryFilters);
                    testProbe.ExpectMsg<Services.Actors.ApiCoordinator.SearchObjects>();
                    testProbe.Reply(new List<Models.Object>(_fixture.ObjectsObtained));
                    await Task.Delay(TimeSpan.FromSeconds(1));
                });
            
            Assert.Equal(_fixture.ExpectedTop10Makelaars, result.Arguments.MakelaarsData);
        }
        
        [Fact]
        public async Task Should_fire_event_when_status_is_failure()
        {
            var testProbe = CreateTestProbe();
            var factory = new Mock<ApiCoordinatorFactory>();
            factory.Setup(f => f.Create()).Returns(testProbe.Ref);
            var makelaarService = new Services.Makelaar(factory.Object);
            makelaarService.RequestMakelaarData(PageSize, QueryFilters);
            
            var result = await Assert.RaisesAsync<MakelaarDataReceivedEventArgs>(h => makelaarService.OnMakelaarDataReceived += h,
                h => makelaarService.OnMakelaarDataReceived -= h,
                async () =>
                {
                    makelaarService.RequestMakelaarData(PageSize, QueryFilters);
                    testProbe.ExpectMsg<Services.Actors.ApiCoordinator.SearchObjects>();
                    testProbe.Reply(new Status.Failure(new Exception()));
                    await Task.Delay(TimeSpan.FromSeconds(1));
                });
        
            Assert.False(result.Arguments.MakelaarsData.Any());
        }
    }
}