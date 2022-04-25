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
            Delete.Table("Cast");
            Delete.Table("Character");
            Delete.Table("Person");
            Delete.Table("Show");
            Delete.Table("Network");
            Delete.Table("Country");
            Delete.Table("Images");
            Delete.Table("Externals");
        }

        public override void Up()
        {
            Create.Table("Images")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity(1, 1)
                .WithColumn("Medium").AsString().Nullable()
                .WithColumn("Original").AsString().Nullable()
                .WithColumn("Type").AsString().NotNullable()
                .WithColumn("OwnerId").AsInt64().NotNullable();

            Create.UniqueConstraint("ImagesTypeAndOwnerId")
                .OnTable("Images")
                .Columns("Type", "OwnerId");

            Create.Table("Externals")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity(1, 1)
                .WithColumn("Tvrage").AsInt64().Nullable()
                .WithColumn("Thetvdb").AsInt64().Nullable()
                .WithColumn("Imdb").AsString().Nullable();

            Create.Table("Country")
               .WithColumn("Id").AsInt64().PrimaryKey().Identity(1, 1)
               .WithColumn("Name").AsString().Nullable().Unique()
               .WithColumn("Code").AsString().Nullable().Unique()
               .WithColumn("Timezone").AsString().Nullable().Unique();

            Create.Table("Network")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("Name").AsString().Nullable()
                .WithColumn("CountryId").AsInt64().ForeignKey("Country", "Id")
                .WithColumn("OfficialSite").AsString().Nullable();

            Create.Table("Show")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("Url").AsString().Nullable()
                .WithColumn("Name").AsString().Nullable()
                .WithColumn("Type").AsString().Nullable()
                .WithColumn("Language").AsString().Nullable()
                .WithColumn("Genres").AsString().Nullable()
                .WithColumn("Status").AsString().Nullable()
                .WithColumn("Runtime").AsInt64().Nullable()
                .WithColumn("AverageRuntime").AsInt64().Nullable()
                .WithColumn("Premiered").AsDate().Nullable()
                .WithColumn("Ended").AsDate().Nullable()
                .WithColumn("OfficialSite").AsString().Nullable()
                .WithColumn("Rating.Average").AsDecimal().Nullable()
                .WithColumn("Weight").AsInt32().Nullable()
                .WithColumn("NetworkId").AsInt64().Nullable().ForeignKey("Network", "Id")
                .WithColumn("WebChannel").AsString().Nullable()
                .WithColumn("DvdCountry").AsString().Nullable()
                .WithColumn("ImageId").AsInt64().ForeignKey("Images", "Id")
                .WithColumn("ExternalId").AsInt64().ForeignKey("Externals", "Id")
                .WithColumn("Summary").AsString(4001).Nullable()
                .WithColumn("Updated").AsInt64().Nullable();

            Create.Table("Schedule")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity(1, 1)
                .WithColumn("Time").AsString().Nullable()
                .WithColumn("Days").AsString().Nullable()
                .WithColumn("ShowId").AsInt64().ForeignKey("Show", "Id");

            Create.UniqueConstraint("ScheduleTimeDays")
              .OnTable("Schedule")
              .Columns("Time", "Days", "ShowId");

            Create.Table("Person")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("Url").AsString().Nullable()
                .WithColumn("Name").AsString().Nullable()
                .WithColumn("CountryId").AsInt64().Nullable().ForeignKey("Country", "Id")
                .WithColumn("Birthday").AsDate().Nullable()
                .WithColumn("Deadday").AsDate().Nullable()
                .WithColumn("Gender").AsString().Nullable()
                .WithColumn("ImageId").AsInt64().Nullable().ForeignKey("Images", "Id")
                .WithColumn("Updated").AsInt64().Nullable();

            Create.Table("Character")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("Url").AsString().Nullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("ImageId").AsInt64().Nullable().ForeignKey("Images", "Id");

            Create.Table("Cast")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity(1, 1)
                .WithColumn("ShowId").AsInt64().ForeignKey("Show", "Id")
                .WithColumn("PersonId").AsInt64().ForeignKey("Person", "Id")
                .WithColumn("CharacterId").AsInt64().Nullable().ForeignKey("Character", "Id");

            Create.UniqueConstraint("CastShowPersonCharacter")
              .OnTable("Cast")
              .Columns("ShowId", "PersonId", "CharacterId");
        }
    }
}
