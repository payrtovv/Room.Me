using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Room.Me.Migrations
{
    /// <inheritdoc />
    public partial class AddPreferencesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Preferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetFriendly = table.Column<bool>(type: "bit", nullable: false),
                    AllowSmoking = table.Column<bool>(type: "bit", nullable: false),
                    AllowGuests = table.Column<bool>(type: "bit", nullable: false),
                    AllowParties = table.Column<bool>(type: "bit", nullable: false),
                    LikesMusic = table.Column<bool>(type: "bit", nullable: true),
                    IsOrganized = table.Column<bool>(type: "bit", nullable: true),
                    WakesUpEarly = table.Column<bool>(type: "bit", nullable: true),
                    IsQuiet = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetFriendly = table.Column<bool>(type: "bit", nullable: false),
                    AllowSmoking = table.Column<bool>(type: "bit", nullable: false),
                    AllowGuests = table.Column<bool>(type: "bit", nullable: false),
                    AllowParties = table.Column<bool>(type: "bit", nullable: false),
                    LikesMusic = table.Column<bool>(type: "bit", nullable: true),
                    IsOrganized = table.Column<bool>(type: "bit", nullable: true),
                    WakesUpEarly = table.Column<bool>(type: "bit", nullable: true),
                    IsQuiet = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Preferences");

            migrationBuilder.DropTable(
                name: "UserPreferences");
        }
    }
}
