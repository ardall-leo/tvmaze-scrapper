using System;
using System.Collections.Generic;

namespace TVmazeScrapper.Domain.Models.Entities
{
    public record Show : Identity
    {
        public string Url { get; init; }

        public string Name { get; init; }

        public string Type { get; init; }

        public string Language { get; init; }

        public string[] Genres { get; init; }

        public string Status { get; init; }

        public int Runtime { get; init; }

        public int AverageRuntime { get; init; }

        public DateTime Premiered { get; init; }

        public DateTime? Ended { get; init; }

        public string OfficialSite { get; init; }

        public Schedule Schedule { get; init; }

        public Rating Rating { get; init; }

        public int Weight { get; init; }

        public Network Network { get; init; }

        public string WebChannel { get; init; }

        public string DvdCountry { get; init; }

        public External Externals { get; init; }

        public Image Image { get; init; }

        public string Summary { get; init; }

        public long Updated { get; init; }

        public List<Cast> Cast { get; set; } = new();
    }
}
