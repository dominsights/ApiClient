using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using ApiClient.MarketResearch.Services.Actors;
using ApiClient.MarketResearch.Services.Models;
using ApiClient.MarketResearch.Services.UnitTests.Makelaar;
using Xunit;
using Moq;
using Moq.Protected;

namespace ApiClient.MarketResearch.Services.UnitTests.Actors
{
    public class ApiCoordinatorTests : TestKit, IClassFixture<MakelaarFixture>
    {
        private readonly MakelaarFixture _fixture;

        public ApiCoordinatorTests(MakelaarFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public void test()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(_fixture.ObjectsObtained) //TODO: return only 20 for each request
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            
            var subject = Sys.ActorOf(Props.Create(() => new ApiCoordinator(mockFactory.Object)));
            subject.Tell(new ApiCoordinator.SearchObjects(20));
            var objects = ExpectMsg<IEnumerable<Object>>();
            Assert.Equal(_fixture.ObjectsObtained, objects);
        }
    }
}