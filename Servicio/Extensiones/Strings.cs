using System.Linq;

namespace Servicio.Extensiones
{
  /// <summary>
  /// Provee metodos de extensión para cadenas de caracteres
  /// </summary>
  public static class StringExtension
  {
    /// <summary>
    /// Indica si una cadena es nula o al unirla sin espacios esta vacia
    /// </summary>
    /// <param name="caracteres">Cadena para comprobar</param>
    /// <returns>Verdadero o falso</returns>
    public static bool NoEsValida(this string caracteres)
    {
      return caracteres == null || caracteres.Trim().Length.Equals(0);
    }

    /// <summary>
    /// Indica si una cadena de la coleccón es nula o al unirla sin espacios esta vacia
    /// </summary>
    /// <param name="cadenas"></param>
    /// <returns></returns>
    public static bool NoEsValida(this string[] cadenas)
    {
      return cadenas.Any(c => c.NoEsValida());
    }
  }
}