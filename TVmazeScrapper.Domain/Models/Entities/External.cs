namespace TVmazeScrapper.Domain.Models.Entities
{
    public record External
    {
        public long Tvrage { get; init; }

        public long Thetvdb { get; init; }

        public string Imdb { get; init; }
    }
}
