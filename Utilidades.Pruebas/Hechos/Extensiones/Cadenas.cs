using System;
using Utilidades.Extensiones;
using Xunit;

namespace Utilidades.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de cadenas alfanumericas
  /// </summary>
  public class Cadenas
  {
    /// <summary>
    /// Comprueba que la cadena no es valida
    /// para su uso
    /// </summary>
    [Fact]
    public void NoEsValida()
    {
      Assert.True(@"".NoEsValida());
    }

    /// <summary>
    /// Comprueba que la cadena tiene un
    /// formato numerico
    /// </summary>
    [Fact]
    public void EsNumero()
    {
      Assert.True(@"1".EsNumero());
    }

    /// <summary>
    /// Comprueba que la cadena tiene un
    /// formato de telefono celular
    /// </summary>
    [Fact]
    public void EsCelular()
    {
      Assert.True(@"66235595665".EsCelular());
    }

    /// <summary>
    /// Comprueba que la cadena tiene un
    /// formato de correo electronico
    /// </summary>
    [Fact]
    public void EsCorreo()
    {
      Assert.True(@"izrra.ch@icloud.com".EsCorreo());
    }

    /// <summary>
    /// Comprueba que la cadena tiene un
    /// formato de direccion de archivo
    /// </summary>
    [Fact]
    public void EsDireccionDeArchivo()
    {
      string archivo = AppDomain.CurrentDomain.BaseDirectory + @"ConfiguracionServicio.json";
      Assert.True(archivo.EsDireccionDeArchivo());
    }

    /// <summary>
    /// Comprueba que la cadena tiene un
    /// formado de direccion web
    /// </summary>
    [Fact]
    public void EsDireccionWeb()
    {
      string url = @"http://google.com";
      Assert.True(url.EsDireccionWeb());
    }
  }
}
