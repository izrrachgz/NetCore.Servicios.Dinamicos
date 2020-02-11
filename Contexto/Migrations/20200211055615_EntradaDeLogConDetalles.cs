using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Contexto.Migrations
{
  public partial class EntradaDeLogConDetalles : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "EntradaLog",
          columns: table => new
          {
            Id = table.Column<int>(nullable: false)
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
            table.PrimaryKey("PK_EntradaLog", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "EntradaLogDetalle",
          columns: table => new
          {
            Id = table.Column<int>(nullable: false)
                  .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            Creado = table.Column<DateTime>(nullable: false),
            Modificado = table.Column<DateTime>(nullable: false),
            Eliminado = table.Column<DateTime>(nullable: true),
            IdEntradaLog = table.Column<int>(nullable: false),
            Valor = table.Column<string>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_EntradaLogDetalle", x => x.Id);
            table.ForeignKey(
                      name: "FK_EntradaLogDetalle_EntradaLog_IdEntradaLog",
                      column: x => x.IdEntradaLog,
                      principalTable: "EntradaLog",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_EntradaLogDetalle_IdEntradaLog",
          table: "EntradaLogDetalle",
          column: "IdEntradaLog");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "EntradaLogDetalle");

      migrationBuilder.DropTable(
          name: "EntradaLog");
    }
  }
}
