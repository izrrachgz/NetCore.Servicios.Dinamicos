using System;
using Servicio.Extensiones;
using Xunit;

namespace Servicio.Pruebas.Hechos.Extensiones
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
