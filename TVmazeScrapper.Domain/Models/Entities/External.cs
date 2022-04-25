using Newtonsoft.Json;

namespace TVmazeScrapper.Domain.Models.Entities
{
    public record External : Identity
    {
        [JsonIgnore]
        public override long? Id { get; init; }

        public long Tvrage { get; init; }

        public long Thetvdb { get; init; }

        public string Imdb { get; init; }
    }
}
