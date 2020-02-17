using Contexto.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Contexto.Esquema.Configuraciones
{
  /// <summary>
  /// Provee un metodo para registrar la configuracion
  /// de la entidad
  /// </summary>
  internal static class EsquemaBitacoraDetalle
  {
    /// <summary>
    /// Permite registrar la configuracion
    /// de esquema asociado a la entidad
    /// </summary>
    /// <param name="model"></param>
    internal static void Registrar(ModelBuilder model)
    {
      model.Entity<BitacoraDetalle>()
        .HasOne(e => e.Bitacora)
        .WithMany()
        .HasForeignKey(e => e.IdBitacora)
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
