using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VISI.Migrations
{
    /// <inheritdoc />
    public partial class albaranes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VISI_albaranes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    ClienteNombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeImprime = table.Column<bool>(type: "bit", nullable: false),
                    NumFactura = table.Column<int>(type: "int", nullable: false),
                    TipoIva = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BaseImponible = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VISI_albaranes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "VISI_Albaranes_detalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlbaranNum = table.Column<int>(type: "int", nullable: false),
                    LineaNum = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VISI_Albaranes_detalle", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VISI_albaranes");

            migrationBuilder.DropTable(
                name: "VISI_Albaranes_detalle");
        }
    }
}
