using Newtonsoft.Json;

namespace TVmazeScrapper.Domain.Models.Entities
{
    public record Schedule : Identity
    {
        [JsonIgnore]
        public override long? Id { get; init; }

        public string Time { get; init; }

        public string[] Days { get; init; }
    }
}
