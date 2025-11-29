using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Room.Me.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
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
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    IdPreferences = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => new { x.IdUser, x.IdPreferences });

                table.ForeignKey(
                    name: "FK_UserPreferences_Users_IdUser",
                    column: x => x.IdUser,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);

                table.ForeignKey(
                    name: "FK_UserPreferences_Preferences_IdPreferences",
                    column: x => x.IdPreferences,
                    principalTable: "Preferences", 
                    onDelete: ReferentialAction.Cascade);
     });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
