using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Room.Me.Migrations
{
    /// <inheritdoc />
    public partial class RoomandRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    IdRoom = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    M2Space = table.Column<float>(type: "real", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NearTransport = table.Column<bool>(type: "bit", nullable: false),
                    NearCollege = table.Column<bool>(type: "bit", nullable: false),
                    IncludesElectricity = table.Column<bool>(type: "bit", nullable: false),
                    IncludesWater = table.Column<bool>(type: "bit", nullable: false),
                    IncludesInternet = table.Column<bool>(type: "bit", nullable: false),
                    IncludesGas = table.Column<bool>(type: "bit", nullable: false),
                    IncludesCleaning = table.Column<bool>(type: "bit", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    IdUserHost = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.IdRoom);
                    table.ForeignKey(
                        name: "FK_Rooms_Users_IdUserHost",
                        column: x => x.IdUserHost,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    RuleId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomRules_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "IdRoom",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomRules_Rules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomRules_RoomId",
                table: "RoomRules",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomRules_RuleId",
                table: "RoomRules",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_IdUserHost",
                table: "Rooms",
                column: "IdUserHost");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomRules");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Rules");
        }
    }
}
