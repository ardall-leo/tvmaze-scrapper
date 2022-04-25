using System.Collections.Generic;
using Newtonsoft.Json;

namespace TVmazeScrapper.Domain.Models.Entities
{
    public record Cast : Identity
    {
        [JsonIgnore]
        public override long? Id { get; init; }

        public Person Person { get; init; }

        public Character Character { get; init; }
    }
}
