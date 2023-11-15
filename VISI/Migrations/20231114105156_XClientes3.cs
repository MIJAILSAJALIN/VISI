using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VISI.Migrations
{
    /// <inheritdoc />
    public partial class XClientes3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "VISI_Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nif = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telefono = table.Column<int>(type: "int", nullable: true),
                    Cp = table.Column<int>(type: "int", nullable: true),
                    Iban = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Formapago = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    AdministradorId = table.Column<int>(type: "int", nullable: true),
                    Contacto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VISI_Clientes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VISI_Clientes");

        }
    }
}
