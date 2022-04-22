using System.Collections.Generic;

namespace TVmazeScrapper.Domain.Models.Entities
{
    public record Cast : Identity
    {
        public Person Person { get; init; }

        public Character Character { get; init; }
    }
}
