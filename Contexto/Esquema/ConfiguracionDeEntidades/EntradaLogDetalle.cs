using Contexto.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Contexto.Esquema.ConfiguracionDeEntidades
{
  /// <summary>
  /// Provee un metodo para registrar la configuracion
  /// de la entidad
  /// </summary>
  internal static class EntradaLogDetalleConfiguracion
  {
    /// <summary>
    /// Permite registrar la configuracion
    /// de esquema asociado a la entidad
    /// </summary>
    /// <param name="model"></param>
    internal static void Registrar(ModelBuilder model)
    {
      model.Entity<EntradaLogDetalle>()
        .HasOne(e => e.EntradaLog)
        .WithMany()
        .HasForeignKey(e => e.IdEntradaLog)
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
