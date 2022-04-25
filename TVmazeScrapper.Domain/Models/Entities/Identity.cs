using Newtonsoft.Json;

namespace TVmazeScrapper.Domain.Models.Entities
{
    public record Identity
    {
        [JsonProperty(Order = -1)]
        public virtual long? Id { get; init; }
    }
}
