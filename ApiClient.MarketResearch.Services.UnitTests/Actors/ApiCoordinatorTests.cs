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
using FluentAssertions;

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
        public void Should_return_expected_makelaar_data_when_api_result_is_ok()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(); //TODO: move to fixture
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(MockApiResults(_fixture.ApiResult, MakelaarFixture.PageSize))
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            var apiFacade = new ApiClientFacade(mockFactory.Object, new ApiConfig("key", "https://validurl.org"));

            var subject = Sys.ActorOf(Props.Create(() => new ApiCoordinator(apiFacade)));
            subject.Tell(new ApiCoordinator.SearchObjects(MakelaarFixture.PageSize, "type=koop&zo=/amsterdam/tuin"));

            var objects = ExpectMsg<IEnumerable<Object>>();
            // Assert
            _fixture.ObjectsObtained.Should().BeEquivalentTo(objects);
        }

        private int _count = 0;
        private readonly object _myLock = new object();
        /// <summary>
        /// Tries to mock the pagination from server side.
        /// </summary>
        /// <param name="searchResult">All objects expected to return from server side.</param>
        /// <param name="pageSize">Page size to simulate multiple pages.</param>
        /// <returns></returns>
        private SearchResult MockApiResults(SearchResult searchResult, int pageSize) //TODO: move to fixture
        {
            lock (_myLock)
            {
                int skip = pageSize * _count;
                var newObjects = searchResult.Objects.Skip(skip).Take(pageSize);
                _count++;
                return searchResult with {Objects = newObjects.ToArray(), Paging = searchResult.Paging with { HuidigePagina = _count}};
            }
        }
    }
}