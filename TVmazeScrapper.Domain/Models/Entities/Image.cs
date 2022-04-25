using Newtonsoft.Json;
using TVmazeScrapper.Domain.Enums;

namespace TVmazeScrapper.Domain.Models.Entities
{
    public record Image : Identity
    {
        [JsonIgnore]
        public override long? Id { get; init; }

        public string Medium { get; init; }

        public string Original { get; init; }

        [JsonIgnore]
        public ImageType Type { get; init; }
    }
}
