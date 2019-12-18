using System;
using Datos.Extensiones;
using Xunit;

namespace Datos.Pruebas.Hechos.Extensiones
{
  public class Fechas
  {
    [Fact]
    public void NoEsValida()
    {
      Assert.True(DateTime.MinValue.NoEsValida());
    }
  }
}
