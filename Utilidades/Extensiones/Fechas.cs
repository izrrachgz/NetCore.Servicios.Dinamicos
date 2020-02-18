using System;

namespace Utilidades.Extensiones
{
  /// <summary>
  /// Provee metodos de extensión para fechas
  /// </summary>
  public static class ExtensionesDeFechas
  {
    /// <summary>
    /// Indica si la fecha es nula o se ha iniciado en su minimo valor
    /// </summary>
    /// <param name="fecha">Fecha para comprobar</param>
    /// <returns>Verdadero o falso</returns>
    public static bool NoEsValida(this DateTime fecha)
    {
      return fecha.Equals(null) || fecha.Equals(DateTime.MinValue);
    }
  }
}