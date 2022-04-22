namespace TVmazeScrapper.Domain.Models.Entities
{
    public record Country : Identity
    {
        public string Name { get; init; }

        public string Code { get; init; }

        public string Timezone { get; init; }
    }
}
