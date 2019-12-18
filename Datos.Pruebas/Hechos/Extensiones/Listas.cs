using System.Collections.Generic;
using Datos.Extensiones;
using Xunit;

namespace Datos.Pruebas.Hechos.Extensiones
{
  public class Listas
  {
    [Fact]
    public void NoEsValida()
    {
      List<string> lista = new List<string>(0);
      Assert.True(lista.NoEsValida());
    }
  }
}
