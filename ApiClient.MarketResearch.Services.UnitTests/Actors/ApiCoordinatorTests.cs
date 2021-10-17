using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using ApiClient.MarketResearch.Services.Actors;
using ApiClient.MarketResearch.Services.Facade;
using ApiClient.MarketResearch.Services.UnitTests.Makelaar;
using Xunit;
using Moq;
using Moq.Protected;
using Object = ApiClient.MarketResearch.Services.Models.Object;

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
                .ReturnsAsync(() => new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(MockApiResults(_fixture.ApiResult, MakelaarFixture.PageSize)) //TODO: return only 20 for each request
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            
            var subject = Sys.ActorOf(Props.Create(() => new ApiCoordinator(mockFactory.Object)));
            subject.Tell(new ApiCoordinator.SearchObjects(MakelaarFixture.PageSize));
            var objects = ExpectMsg<IEnumerable<Object>>();
            Assert.Equal(_fixture.ObjectsObtained, objects);
        }

        private int count = 0;
        /// <summary>
        /// Tries to mock the pagination from server side.
        /// </summary>
        /// <param name="searchResult"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private SearchResult MockApiResults(SearchResult searchResult, int pageSize)
        {
            int skip = pageSize * count;
            var newObjects = searchResult.Objects.Skip(skip).Take(pageSize);
            count++;
            return searchResult with {Objects = newObjects.ToList(), Paging = searchResult.Paging with { HuidigePagina = count}};
        }
    }
}