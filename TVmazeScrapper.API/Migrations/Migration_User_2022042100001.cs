using FluentMigrator;
using FluentMigrator.SqlServer;

namespace TVmazeScrapper.API.Migrations
{
    [Migration(2022042100001)]
    public class Migration_User_2022042100001 : Migration
    {
        public override void Down()
        {
            Delete.Table("Schedule");
            Delete.Table("Network");
            Delete.Table("Cast");
            Delete.Table("Character");
            Delete.Table("Person");
            Delete.Table("Country");
            Delete.Table("Images");
            Delete.Table("Genres");
            Delete.Table("ShowGenres");
            Delete.Table("Externals");
            Delete.Table("Show");
        }

        public override void Up()
        {
            Create.Table("Show")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("Url").AsString().Nullable()
                .WithColumn("Name").AsString().Nullable()
                .WithColumn("Type").AsString().Nullable()
                .WithColumn("Language").AsString().Nullable()
                .WithColumn("Status").AsString().Nullable()
                .WithColumn("Runtime").AsInt64().Nullable()
                .WithColumn("AverageRuntime").AsInt64().Nullable()
                .WithColumn("Premiered").AsDate().Nullable()
                .WithColumn("Ended").AsDate().Nullable()
                .WithColumn("OfficialSite").AsString().Nullable()
                .WithColumn("Rating").AsDecimal().Nullable()
                .WithColumn("Weight").AsInt32().Nullable()
                .WithColumn("NetworkId").AsInt64().Nullable()
                .WithColumn("WebChannel").AsString().Nullable()
                .WithColumn("DvdCountry").AsString().Nullable()
                .WithColumn("Image").AsDate().Nullable()
                .WithColumn("Summary").AsString(4001).Nullable()
                .WithColumn("Updated").AsInt64().Nullable();

            Create.Table("Images")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity(1, 1)
                .WithColumn("Medium").AsString().Nullable()
                .WithColumn("Original").AsString().Nullable();

            Create.Table("Genres")
                .WithColumn("Id").AsString().Nullable()
                .WithColumn("Genre").AsString().Nullable();

            Create.Table("ShowGenres")
                .WithColumn("Id").AsString().Nullable()
                .WithColumn("ShowId").AsString().Nullable()
                .WithColumn("GenreId").AsString().Nullable();

            Create.Table("Externals")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity(1, 1)
                .WithColumn("ShowId").AsString().Nullable()
                .WithColumn("Tvrage").AsString().Nullable()
                .WithColumn("Thetvdb").AsString().Nullable()
                .WithColumn("Imdb").AsString().Nullable();

            Create.Table("Schedule")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity(1, 1)
                .WithColumn("Time").AsString().Nullable().Unique()
                .WithColumn("Days").AsString().Nullable().Unique()
                .WithColumn("ShowId").AsInt64().ForeignKey("Show", "Id").Unique();

            Create.Table("Country")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity(1, 1)
               .WithColumn("Name").AsString().Nullable().Unique()
               .WithColumn("Code").AsString().Nullable().Unique()
               .WithColumn("Timezone").AsString().Nullable().Unique();

            Create.Table("Network")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("Name").AsString().Nullable()
                .WithColumn("CountryId").AsInt64().ForeignKey("Country", "Id")
                .WithColumn("OfficialSite").AsString().Nullable()
                .WithColumn("ShowId").AsInt64().ForeignKey("Show", "Id");

            Create.Table("Person")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("Url").AsString().Nullable()
                .WithColumn("Name").AsString().Nullable()
                .WithColumn("CountryId").AsInt64().ForeignKey("Country", "Id")
                .WithColumn("Birthday").AsDate().Nullable()
                .WithColumn("Deadday").AsDate().Nullable()
                .WithColumn("Gender").AsString().Nullable()
                .WithColumn("Image").AsInt64().Nullable()
                .WithColumn("Updated").AsInt64().Nullable();

            Create.Table("Character")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("Url").AsString().Nullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Image").AsInt64().Nullable();

            Create.Table("Cast")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity(1, 1)
                .WithColumn("ShowId").AsInt64().ForeignKey("Show", "Id").Unique()
                .WithColumn("PersonId").AsInt64().ForeignKey("Person", "Id").Unique()
                .WithColumn("CharacterId").AsInt64().ForeignKey("Character", "Id").Unique();
        }
    }
}
