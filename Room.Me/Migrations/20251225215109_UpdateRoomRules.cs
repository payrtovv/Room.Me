using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Room.Me.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Rules",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "Rules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMandatory",
                table: "Rules",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "CreatedByUserId",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "IsMandatory",
                table: "Rules");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Rules",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }
    }
}
