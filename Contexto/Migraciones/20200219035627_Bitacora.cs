using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Contexto.Migraciones
{
  public partial class Bitacora : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Bitacora",
          columns: table => new
          {
            Id = table.Column<long>(nullable: false)
                  .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            Creado = table.Column<DateTime>(nullable: false),
            Modificado = table.Column<DateTime>(nullable: false),
            Eliminado = table.Column<DateTime>(nullable: true),
            Nombre = table.Column<string>(maxLength: 128, nullable: false),
            Descripcion = table.Column<string>(maxLength: 512, nullable: true),
            Tipo = table.Column<int>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Bitacora", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "BitacoraDetalle",
          columns: table => new
          {
            Id = table.Column<long>(nullable: false)
                  .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            Creado = table.Column<DateTime>(nullable: false),
            Modificado = table.Column<DateTime>(nullable: false),
            Eliminado = table.Column<DateTime>(nullable: true),
            IdBitacora = table.Column<long>(nullable: false),
            Valor = table.Column<string>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_BitacoraDetalle", x => x.Id);
            table.ForeignKey(
                      name: "FK_BitacoraDetalle_Bitacora_IdBitacora",
                      column: x => x.IdBitacora,
                      principalTable: "Bitacora",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_Bitacora_Creado_Modificado",
          table: "Bitacora",
          columns: new[] { "Creado", "Modificado" });

      migrationBuilder.CreateIndex(
          name: "IX_BitacoraDetalle_IdBitacora",
          table: "BitacoraDetalle",
          column: "IdBitacora");

      migrationBuilder.CreateIndex(
          name: "IX_BitacoraDetalle_Creado_Modificado",
          table: "BitacoraDetalle",
          columns: new[] { "Creado", "Modificado" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "BitacoraDetalle");

      migrationBuilder.DropTable(
          name: "Bitacora");
    }
  }
}
