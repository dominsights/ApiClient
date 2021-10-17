using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ApiClient.MarketResearch.Services.Facade
{


    public record SearchResult
    {
        [JsonProperty("AccountStatus")]
        public long AccountStatus { get; set; }

        [JsonProperty("EmailNotConfirmed")]
        public bool EmailNotConfirmed { get; set; }

        [JsonProperty("ValidationFailed")]
        public bool ValidationFailed { get; set; }

        [JsonProperty("ValidationReport")]
        public object ValidationReport { get; set; }

        [JsonProperty("Website")]
        public long Website { get; set; }

        [JsonProperty("Metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("Objects")]
        public Object[] Objects { get; set; }

        [JsonProperty("Paging")]
        public Paging Paging { get; set; }

        [JsonProperty("TotaalAantalObjecten")]
        public long TotaalAantalObjecten { get; set; }
    }

    public record Metadata
    {
        [JsonProperty("ObjectType")]
        public string ObjectType { get; set; }

        [JsonProperty("Omschrijving")]
        public string Omschrijving { get; set; }

        [JsonProperty("Titel")]
        public string Titel { get; set; }
    }

    public record Object
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("MakelaarId")]
        public int MakelaarId { get; set; }

        [JsonProperty("MakelaarNaam")]
        public string MakelaarNaam { get; set; }
    }

    public record Paging
    {
        [JsonProperty("AantalPaginas")]
        public int AantalPaginas { get; set; }

        [JsonProperty("HuidigePagina")]
        public long HuidigePagina { get; set; }

        [JsonProperty("VolgendeUrl")]
        public string VolgendeUrl { get; set; }

        [JsonProperty("VorigeUrl")]
        public object VorigeUrl { get; set; }
    }
}
