namespace TVmazeScrapper.Domain.Models.Entities
{
    public record Network : Identity
    {
        public string Name { get; init; }

        public Country Country { get; init; }

        public string OfficialSite { get; init; }
    }
}
