using System;
using Datos.Extensiones;
using Xunit;

namespace Datos.Pruebas.Hechos.Extensiones
{
  public class Cadenas
  {
    [Fact]
    public void NoEsValida()
    {
      Assert.True(@"".NoEsValida());
    }

    [Fact]
    public void EsNumero()
    {
      Assert.True(@"1".EsNumero());
    }

    [Fact]
    public void EsCelular()
    {
      Assert.True(@"66235595665".EsCelular());
    }

    [Fact]
    public void EsCorreo()
    {
      Assert.True(@"izrra.ch@icloud.com".EsCorreo());
    }

    [Fact]
    public void EsDireccionDeArchivo()
    {
      string archivo = AppDomain.CurrentDomain.BaseDirectory + @"ConfiguracionServicio.json";
      Assert.True(archivo.EsDireccionDeArchivo());
    }

    [Fact]
    public void EsDireccionWeb()
    {
      string url = @"http://google.com";
      Assert.True(url.EsDireccionWeb());
    }
  }
}
