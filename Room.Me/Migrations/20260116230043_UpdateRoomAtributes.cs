using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Room.Me.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomAtributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "NearCollege",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "NearTransport",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Rooms",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "NumOfRooms",
                table: "Rooms",
                newName: "ParkingSpaces");

            migrationBuilder.RenameColumn(
                name: "NumOfParkingSpaces",
                table: "Rooms",
                newName: "Bedrooms");

            migrationBuilder.RenameColumn(
                name: "NumOfBathrooms",
                table: "Rooms",
                newName: "Bathrooms");

            migrationBuilder.RenameColumn(
                name: "M2Space",
                table: "Rooms",
                newName: "Surface");

            migrationBuilder.RenameColumn(
                name: "Longitud",
                table: "Rooms",
                newName: "Lng");

            migrationBuilder.RenameColumn(
                name: "Latitud",
                table: "Rooms",
                newName: "Lat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "address",
                table: "Rooms",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "Surface",
                table: "Rooms",
                newName: "M2Space");

            migrationBuilder.RenameColumn(
                name: "ParkingSpaces",
                table: "Rooms",
                newName: "NumOfRooms");

            migrationBuilder.RenameColumn(
                name: "Lng",
                table: "Rooms",
                newName: "Longitud");

            migrationBuilder.RenameColumn(
                name: "Lat",
                table: "Rooms",
                newName: "Latitud");

            migrationBuilder.RenameColumn(
                name: "Bedrooms",
                table: "Rooms",
                newName: "NumOfParkingSpaces");

            migrationBuilder.RenameColumn(
                name: "Bathrooms",
                table: "Rooms",
                newName: "NumOfBathrooms");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "NearCollege",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NearTransport",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
