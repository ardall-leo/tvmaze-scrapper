using System;
using TVmazeScrapper.Domain.Enums;

namespace TVmazeScrapper.Domain.Models.Entities
{
    public record Person : Identity
    {
        public string Url { get; init; }

        public string Name { get; init; }

        public Country Country { get; init; }

        public DateTime Birthday { get; init; }

        public DateTime Deadday { get; init; }

        public Gender Gender { get; init; }

        public Image Image { get; init; }

        public long Updated { get; init; }
    }
}
