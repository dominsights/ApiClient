using System;
using System.Collections.Generic;
using System.Linq;
using Object = ApiClient.MarketResearch.Services.Models.Object;

namespace ApiClient.MarketResearch.Services.UnitTests.Makelaar
{
    public class MakelaarFixture
    {
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
            ObjectsObtained = Enumerable.Range(0, makelaarSize)
                .SelectMany(i => Enumerable.Range(0, makelaarSize - i)
                    .OrderByDescending(_ => _)
                    .Select(_ => new Models.Object(Guid.NewGuid(), i + 1, $"Makelaar {i + 1}")));
        }

        public IEnumerable<Models.Makelaar> ExpectedTop10Makelaars { get; }
        public IEnumerable<Models.Object> ObjectsObtained { get; }
    }
}