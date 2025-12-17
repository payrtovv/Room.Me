using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 

namespace Room.Me.Migrations
{
    /// <inheritdoc />
    public partial class AddPreferencesTables : Migration
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
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PreferenceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => new { x.UserId, x.PreferenceId });
                    table.ForeignKey(
                        name: "FK_UserPreferences_Preferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPreferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Preferences",
                columns: new[] { "Id", "Category", "Label", "Value" },
                values: new object[,]
                {
                    { 1, "personality", "Extrovertido", "extrovert" },
                    { 2, "personality", "Ambivertido", "ambivert" },
                    { 3, "personality", "Introvertido", "introvert" },
                    { 4, "schedule", "Madrugador", "early_bird" },
                    { 5, "schedule", "Horario Flexible", "flexible" },
                    { 6, "schedule", "Nocturno", "night_owl" },
                    { 7, "cleanliness", "Super Ordenado", "neat" },
                    { 8, "cleanliness", "Orden Normal", "average" },
                    { 9, "cleanliness", "Desordenado", "messy" },
                    { 10, "pets", "Tengo Mascotas", "has_pets" },
                    { 11, "pets", "Acepto Mascotas", "ok_with" },
                    { 12, "pets", "Cero Mascotas", "none" },
                    { 13, "visits", "Casa de Fiesta", "party_house" },
                    { 14, "visits", "Visitas Moderadas", "occasional" },
                    { 15, "visits", "Sin Visitas", "private" },
                    { 16, "habits", "Fumador", "smoker" },
                    { 17, "habits", "Fumo afuera", "outside_only" },
                    { 18, "habits", "No fumador", "non_smoker" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_PreferenceId",
                table: "UserPreferences",
                column: "PreferenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropTable(
                name: "Preferences");
        }
    }
}
