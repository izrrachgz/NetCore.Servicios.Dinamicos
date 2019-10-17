using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Servicio.Migrations
{
    public partial class TablaUsuarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Creado = table.Column<DateTime>(nullable: false),
                    Modificado = table.Column<DateTime>(nullable: false),
                    Eliminado = table.Column<DateTime>(nullable: true),
                    Nombre = table.Column<string>(maxLength: 128, nullable: false),
                    ApellidoPaterno = table.Column<string>(maxLength: 64, nullable: false),
                    ApellidoMaterno = table.Column<string>(maxLength: 64, nullable: false),
                    Correo = table.Column<string>(maxLength: 192, nullable: false),
                    NumeroContacto = table.Column<string>(maxLength: 18, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
