using TVmazeScrapper.Domain.Enums;

namespace TVmazeScrapper.Domain.Models.Entities
{
    public record Image : Identity
    {
        public string Medium { get; init; }

        public string Original { get; init; }

        public ImageType Type { get; init; }
    }
}
