using Newtonsoft.Json;

namespace TVmazeScrapper.Domain.Models.Entities
{
    public record Country : Identity
    {
        [JsonIgnore]
        public override long? Id { get; init; }

        public string Name { get; init; }

        public string Code { get; init; }

        public string Timezone { get; init; }
    }
}
