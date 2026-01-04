using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Room.Me.Migrations
{
    /// <inheritdoc />
    public partial class FeaturesSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomFeature_Feature_FeatureId",
                table: "RoomFeature");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomFeature_Rooms_RoomId",
                table: "RoomFeature");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomFeature",
                table: "RoomFeature");

            migrationBuilder.RenameTable(
                name: "RoomFeature",
                newName: "RoomFeatures");

            migrationBuilder.RenameIndex(
                name: "IX_RoomFeature_FeatureId",
                table: "RoomFeatures",
                newName: "IX_RoomFeatures_FeatureId");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Feature",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Feature",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomFeatures",
                table: "RoomFeatures",
                columns: new[] { "RoomId", "FeatureId" });

            migrationBuilder.InsertData(
                table: "Feature",
                columns: new[] { "Id", "Category", "Key", "Name" },
                values: new object[,]
                {
                    { 1, "Servicios y Conectividad", "wifi", "Internet / Wifi" },
                    { 2, "Servicios y Conectividad", "agua_caliente", "Agua caliente" },
                    { 3, "Servicios y Conectividad", "servicios_basicos", "Servicios básicos incluidos (Luz/Agua/Gas)" },
                    { 4, "Servicios y Conectividad", "limpieza_comun", "Limpieza de áreas comunes" },
                    { 5, "Servicios y Conectividad", "seguridad_24h", "Portería / Seguridad 24h" },
                    { 6, "Servicios y Conectividad", "ascensor", "Ascensor" },
                    { 7, "Servicios y Conectividad", "bodega", "Bodega" },
                    { 8, "Áreas Sociales y Bienestar", "terraza", "Terraza / Rooftop" },
                    { 9, "Áreas Sociales y Bienestar", "bbq", "Zona BBQ / Parrilla" },
                    { 10, "Áreas Sociales y Bienestar", "piscina", "Piscina" },
                    { 11, "Áreas Sociales y Bienestar", "sauna", "Sauna / Turco" },
                    { 12, "Áreas Sociales y Bienestar", "jacuzzi", "Hidromasaje / Jacuzzi" },
                    { 13, "Áreas Sociales y Bienestar", "canchas", "Canchas deportivas" },
                    { 14, "Áreas Sociales y Bienestar", "sala_eventos", "Sala comunal / Eventos" },
                    { 15, "Áreas Sociales y Bienestar", "gimnasio", "Gimnasio" },
                    { 16, "Áreas Sociales y Bienestar", "sala_tv", "Sala de TV / Cine" },
                    { 17, "Áreas Sociales y Bienestar", "jardin", "Jardín interior" },
                    { 18, "Áreas Sociales y Bienestar", "suite_huespedes", "Suite de Huéspedes" },
                    { 19, "Equipamiento del Hogar", "linea_blanca", "Línea Blanca Completa" },
                    { 20, "Equipamiento del Hogar", "microondas", "Microondas" },
                    { 21, "Equipamiento del Hogar", "lavadora", "Lavadora" },
                    { 22, "Equipamiento del Hogar", "secadora", "Secadora" },
                    { 23, "Equipamiento del Hogar", "pequenos_electro", "Pequeños Electrodomésticos" },
                    { 24, "Equipamiento del Hogar", "menaje_cocina", "Menaje de Cocina" },
                    { 25, "Equipamiento del Hogar", "tv_sala", "TV en sala (Smart TV)" },
                    { 26, "Equipamiento del Hogar", "sala_comedor", "Sala y Comedor amoblados" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RoomFeatures_Feature_FeatureId",
                table: "RoomFeatures",
                column: "FeatureId",
                principalTable: "Feature",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomFeatures_Rooms_RoomId",
                table: "RoomFeatures",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "IdRoom",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomFeatures_Feature_FeatureId",
                table: "RoomFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomFeatures_Rooms_RoomId",
                table: "RoomFeatures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomFeatures",
                table: "RoomFeatures");

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Feature",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Feature");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Feature");

            migrationBuilder.RenameTable(
                name: "RoomFeatures",
                newName: "RoomFeature");

            migrationBuilder.RenameIndex(
                name: "IX_RoomFeatures_FeatureId",
                table: "RoomFeature",
                newName: "IX_RoomFeature_FeatureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomFeature",
                table: "RoomFeature",
                columns: new[] { "RoomId", "FeatureId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoomFeature_Feature_FeatureId",
                table: "RoomFeature",
                column: "FeatureId",
                principalTable: "Feature",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomFeature_Rooms_RoomId",
                table: "RoomFeature",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "IdRoom",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
