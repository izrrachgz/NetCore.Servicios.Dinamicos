using System.Collections.Generic;
using Utilidades.Extensiones;
using Xunit;

namespace Utilidades.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de objetos
  /// </summary>
  public class Objetos
  {
    public Objetos()
    {

    }

    /// <summary>
    /// Comprueba que el objeto indicado contenga una lista
    /// de valores alfanumericos
    /// </summary>
    [Fact]
    public void EsColeccionDeCaracteres()
    {
      object lista = new List<string>(2) { @"1", @"2" };
      Assert.True(lista.EsColeccionDeCaracteres());
    }

    /// <summary>
    /// Comprueba que el objeto indicado contenga una lista
    /// de valores numericos
    /// </summary>
    [Fact]
    public void EsColeccionNumerica()
    {
      object lista = new List<int>(2) { 1, 2 };
      Assert.True(lista.EsColeccionNumerica());
    }
  }
}
