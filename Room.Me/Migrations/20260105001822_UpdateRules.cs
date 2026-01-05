using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Room.Me.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomRules");

            migrationBuilder.DeleteData(
                table: "Rules",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rules",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rules",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rules",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rules",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "IsMandatory",
                table: "Rules");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Rules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rules_RoomId",
                table: "Rules",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Rooms_RoomId",
                table: "Rules",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "IdRoom",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Rooms_RoomId",
                table: "Rules");

            migrationBuilder.DropIndex(
                name: "IX_Rules_RoomId",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Rules");

            migrationBuilder.AddColumn<bool>(
                name: "IsMandatory",
                table: "Rules",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.InsertData(
                table: "Rules",
                columns: new[] { "Id", "CreatedByUserId", "IsMandatory", "Name" },
                values: new object[,]
                {
                    { 1, null, true, "Fumar" },
                    { 2, null, true, "Mascotas" },
                    { 3, null, true, "Visitas" },
                    { 4, null, true, "Reuniones" },
                    { 5, null, true, "Alcohol" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomRules_RoomId",
                table: "RoomRules",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomRules_RuleId",
                table: "RoomRules",
                column: "RuleId");
        }
    }
}
