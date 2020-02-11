using Contexto.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contexto.Esquema.Configuraciones
{
  /// <summary>
  /// Configuración de una entidad
  /// </summary>
  internal class EntradaLogDetalleConfig : IEntityTypeConfiguration<EntradaLogDetalle>
  {

    /// <summary>
    /// registra la configuración de la entidad 
    /// </summary>
    /// <param name="model"></param>
    public void Configure(EntityTypeBuilder<EntradaLogDetalle> model)
    {
      model.HasOne(e => e.EntradaLog)
        .WithMany()
        .HasForeignKey(e => e.IdEntradaLog)
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}