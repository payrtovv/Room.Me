using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Room.Me.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncludesCleaning",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "IncludesGas",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "IncludesInternet",
                table: "Rooms");

            migrationBuilder.AddColumn<float>(
                name: "Latitud",
                table: "Rooms",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Longitud",
                table: "Rooms",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "NumOfParkingSpaces",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumOfRooms",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "NumOfParkingSpaces",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "NumOfRooms",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Rooms");

            migrationBuilder.AddColumn<bool>(
                name: "IncludesCleaning",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IncludesGas",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IncludesInternet",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
