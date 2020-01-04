using System.Collections.Generic;

namespace Datos.Modelos
{
  /// <summary>
  /// Provee un modelo de datos para representar la fila de una tabla
  /// </summary>
  public class FilaDeTabla
  {
    /// <summary>
    /// Indice de la fila
    /// </summary>
    public int Indice { get; }

    /// <summary>
    /// Columnas de la fila
    /// </summary>
    public List<ColumnaDeTabla> Columnas { get; set; }

    public FilaDeTabla() { }

    public FilaDeTabla(int indice, List<ColumnaDeTabla> columnas)
    {
      Indice = indice;
      Columnas = columnas;
    }
  }
}
