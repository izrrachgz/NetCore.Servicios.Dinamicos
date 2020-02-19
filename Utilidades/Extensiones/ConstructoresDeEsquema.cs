using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Utilidades.Contratos;

namespace Utilidades.Extensiones
{
  /// <summary>
  /// Provee metodos de extension para constructores
  /// de esquemas de entidades
  /// </summary>
  public static class ExtensionesDeModelBuilder
  {
    /// <summary>
    /// Permite establecer los indices basicos de una entidad
    /// </summary>
    /// <typeparam name="T">Entidad</typeparam>
    /// <param name="constructor">Constructor de Esquema</param>
    /// <returns>Constructor de Esquema</returns>
    public static EntityTypeBuilder<T> AgregarIndicesBasicos<T>(this EntityTypeBuilder<T> constructor) where T : class, IEntidad
    {
      constructor
        .HasIndex(e => new { e.Creado, e.Modificado });
      return constructor;
    }
  }
}
