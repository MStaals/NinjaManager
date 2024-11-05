using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NinjaManagerProg5.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false),
                    Intelligence = table.Column<int>(type: "int", nullable: false),
                    Agility = table.Column<int>(type: "int", nullable: false),
                    GoldValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ninja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gold = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ninja", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NinjaEquipments",
                columns: table => new
                {
                    NinjaId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NinjaEquipments", x => new { x.NinjaId, x.EquipmentId });
                    table.ForeignKey(
                        name: "FK_NinjaEquipments_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NinjaEquipments_Ninja_NinjaId",
                        column: x => x.NinjaId,
                        principalTable: "Ninja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "Id", "Agility", "Category", "GoldValue", "Intelligence", "Name", "Strength" },
                values: new object[,]
                {
                    { 1, 3, "Hand", 50, 0, "Sword", 5 },
                    { 2, 0, "Head", 30, 1, "Helmet", 2 },
                    { 3, 2, "Chest", 80, 0, "Armor", 10 }
                });

            migrationBuilder.InsertData(
                table: "Ninja",
                columns: new[] { "Id", "Gold", "Name" },
                values: new object[,]
                {
                    { 1, 100, "Ninja A" },
                    { 2, 150, "Ninja B" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NinjaEquipments_EquipmentId",
                table: "NinjaEquipments",
                column: "EquipmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NinjaEquipments");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Ninja");
        }
    }
}
