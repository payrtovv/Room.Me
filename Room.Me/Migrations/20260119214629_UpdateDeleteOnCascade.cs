using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Room.Me.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteOnCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomFeatures_Feature_FeatureId",
                table: "RoomFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_Preferences_PreferenceId",
                table: "UserPreferences");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomFeatures_Feature_FeatureId",
                table: "RoomFeatures",
                column: "FeatureId",
                principalTable: "Feature",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_Preferences_PreferenceId",
                table: "UserPreferences",
                column: "PreferenceId",
                principalTable: "Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomFeatures_Feature_FeatureId",
                table: "RoomFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreferences_Preferences_PreferenceId",
                table: "UserPreferences");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomFeatures_Feature_FeatureId",
                table: "RoomFeatures",
                column: "FeatureId",
                principalTable: "Feature",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreferences_Preferences_PreferenceId",
                table: "UserPreferences",
                column: "PreferenceId",
                principalTable: "Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
