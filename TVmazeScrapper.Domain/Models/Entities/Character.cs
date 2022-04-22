namespace TVmazeScrapper.Domain.Models.Entities
{
    public record Character : Identity
    {
        public string Url { get; init; }

        public string Name { get; init; }

        public Image Image { get; init; }
    }
}
