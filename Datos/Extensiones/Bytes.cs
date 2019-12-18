
namespace Datos.Extensiones
{
  /// <summary>
  /// Provee metodos de extension para los arreglos de bytes
  /// </summary>
  public static class ExtensionesDeBytes
  {
    /// <summary>
    /// Indica si el arreglo de bytes es nulo o no tiene
    /// un espacio definido
    /// </summary>
    /// <param name="bytes">Referencia al arreglo</param>
    /// <returns>Verdadeo o falso</returns>
    public static bool NoEsValido(this byte[] bytes)
    {
      return bytes == null || bytes.Length.Equals(0);
    }
  }
}
