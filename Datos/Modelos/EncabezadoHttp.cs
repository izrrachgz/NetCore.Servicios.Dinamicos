namespace Datos.Modelos
{
  /// <summary>
  /// Provee un modelo de datos para representar
  /// un encabezado de solicitud http
  /// </summary>
  public class EncabezadoHttp
  {
    /// <summary>
    /// Nombre del encabezado
    /// </summary>
    public string Nombre { get; }

    /// <summary>
    /// Valor asociado al encabezado
    /// </summary>
    public string Valor { get; }

    public EncabezadoHttp(string nombre, string valor)
    {
      Nombre = nombre;
      Valor = valor;
    }
  }
}
