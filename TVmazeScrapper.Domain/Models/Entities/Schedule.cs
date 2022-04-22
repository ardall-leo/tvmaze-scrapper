namespace TVmazeScrapper.Domain.Models.Entities
{
    public record Schedule : Identity
    {
        public string Time { get; init; }

        public string[] Days { get; init; }
    }
}
