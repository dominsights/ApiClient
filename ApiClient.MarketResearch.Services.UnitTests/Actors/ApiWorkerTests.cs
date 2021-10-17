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
using Moq;
using Moq.Protected;
using Xunit;
using static ApiClient.MarketResearch.Services.UnitTests.Makelaar.MakelaarFixture;

namespace ApiClient.MarketResearch.Services.UnitTests.Actors
{
    public class ApiWorkerTests : TestKit, IClassFixture<MakelaarFixture>
    {
        private readonly MakelaarFixture _fixture;

        public ApiWorkerTests(MakelaarFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Theory]
        [InlineData(0,1)]
        [InlineData(20,2)]
        public void Should_return_data_accordingly_to_page(int skip, int page)
        {
            // check url is well formed?
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(); //TODO: move to fixture
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(MockApiResults(_fixture.ApiResult, PageSize))
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            var apiFacade = new ApiClientFacade(client);
            var subject = Sys.ActorOf(Props.Create(() => new ApiWorker(apiFacade)));
            subject.Tell(new ApiWorker.QueryData("type=koop&zo=/amsterdam/tuin", page, PageSize));
            var objects = ExpectMsg<IEnumerable<Models.Object>>();
            Assert.Equal(_fixture.ObjectsObtained.Skip(skip).Take(20), objects);
        }
        
        private int count = 0;
        /// <summary>
        /// Tries to mock the pagination from server side.
        /// </summary>
        /// <param name="searchResult">All objects expected to return from server side.</param>
        /// <param name="pageSize">Page size to simulate multiple pages.</param>
        /// <returns></returns>
        private SearchResult MockApiResults(SearchResult searchResult, int pageSize) //TODO: move to fixture
        {
            int skip = pageSize * count;
            var newObjects = searchResult.Objects.Skip(skip).Take(pageSize);
            count++;
            return searchResult with {Objects = newObjects.ToList(), Paging = searchResult.Paging with { HuidigePagina = count}};
        }
    }
}