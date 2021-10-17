using System;
using System.Collections.Generic;
using System.Linq;
using ApiClient.MarketResearch.Services.Facade;

namespace ApiClient.MarketResearch.Services.UnitTests.Makelaar
{
    public class MakelaarFixture
    {
        public IEnumerable<Models.Makelaar> ExpectedTop10Makelaars { get; }
        public IEnumerable<Models.Object> ObjectsObtained { get; }
        public SearchResult ApiResult { get; }
        public const int PageSize = 20;

        public MakelaarFixture()
        {
            ExpectedTop10Makelaars = new List<Models.Makelaar>()
            {
                new(1, "Makelaar 1", 20),
                new(2, "Makelaar 2", 19),
                new(3, "Makelaar 3", 18),
                new(4, "Makelaar 4", 17),
                new(5, "Makelaar 5", 16),
                new(6, "Makelaar 6", 15),
                new(7, "Makelaar 7", 14),
                new(8, "Makelaar 8", 13),
                new(9, "Makelaar 9", 12),
                new(10, "Makelaar 10", 11)
            };

            const int makelaarSize = 20;
            // Creates a list of tuples with the expected values and the values returned by the api
            var mockObjects = Enumerable.Range(0, makelaarSize)
                .SelectMany(i => Enumerable.Range(0, makelaarSize - i)
                    .OrderByDescending(_ => _)
                    .Select(_ =>
                    {
                        var objectId = Guid.NewGuid();
                        int makelaarId = i + 1;
                        string makelaarNaam = $"Makelaar {makelaarId}";
                        var expectedObject = new Models.Object(objectId, makelaarId, makelaarNaam);
                        var objectApiResult = new Facade.Object { Id = objectId.ToString(), MakelaarId = makelaarId, MakelaarNaam = makelaarNaam};
                        return (expectedObject, objectApiResult);
                    }));

            ObjectsObtained = mockObjects.Select(x => x.Item1);

            var apiObjects = mockObjects.Select(x => x.Item2);
            int aantalPagines = (int)Math.Ceiling((double) apiObjects.Count() / PageSize);
            ApiResult = new SearchResult() { Objects = apiObjects.ToList(), Paging = new Paging { AantalPaginas = aantalPagines } };
        }
    }
}