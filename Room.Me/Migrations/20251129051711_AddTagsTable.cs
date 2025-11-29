using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Room.Me.Migrations
{
    /// <inheritdoc />
    public partial class AddTagsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Personality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Routine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cleanliness = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pets = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Visits = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Smoking = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
