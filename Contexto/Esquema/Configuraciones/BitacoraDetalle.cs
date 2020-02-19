using Contexto.Entidades;
using Microsoft.EntityFrameworkCore;
using Utilidades.Extensiones;

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
    /// <param name="constructor">Constructor de esquema</param>
    internal static void Registrar(ModelBuilder constructor)
    {
      constructor.Entity<BitacoraDetalle>()
        .AgregarIndicesBasicos()
        .HasOne(e => e.Bitacora)
        .WithMany()
        .HasForeignKey(e => e.IdBitacora)
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
