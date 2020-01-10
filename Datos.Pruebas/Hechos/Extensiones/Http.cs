using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Datos.Extensiones;
using Xunit;

namespace Datos.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de mensajes http
  /// </summary>
  public class Http
  {
    /// <summary>
    /// Comprueba que el mensaje http
    /// no es valido para su uso
    /// </summary>
    [Fact]
    public void NoEsValida()
    {
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.InternalServerError);
      Assert.True(http.NoEsValida());
    }
    
    /// <summary>
    /// Comprueba que al mensaje http
    /// se le agrega el contenido adjunto
    /// utilizando un flujo de datos
    /// </summary>
    [Fact]
    public void AgregarAdjuntoStream()
    {
      using (FileStream fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"ConfiguracionDatos.json"))
      {
        HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.OK);
        http.AgregarAdjunto(fs, @"application/json", @"Configuracion.json");
        Assert.True(http.Content is StreamContent && http.Content.Headers != null);
      }
    }

    /// <summary>
    /// Comprueba que al mensaje http
    /// se le agrega el contenido adjunto
    /// utilizando un objeto de bytes
    /// </summary>
    [Fact]
    public void AgregarAdjuntoBytes()
    {
      byte[] bytes = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"ConfiguracionDatos.json");
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.OK);
      http.AgregarAdjunto(bytes, @"application/json", @"Configuracion.json");
      Assert.True(http.Content is ByteArrayContent && http.Content.Headers != null);
    }

    /// <summary>
    /// Comprueba que al mensaje http
    /// se le agrega el contenido adjunto
    /// utilizando un objeto de informacion
    /// de archivo
    /// </summary>
    [Fact]
    public void AgregarAdjuntoInfo()
    {
      FileInfo info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"ConfiguracionDatos.json");
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.OK);
      http.AgregarAdjunto(info, @"application/json");
      Assert.True(http.Content is StreamContent && http.Content.Headers != null);
    }

    /// <summary>
    /// Comprueba que al mensaje http
    /// se le agrega el contenido adjunto
    /// utilizando una cadena con formato de direccion
    /// </summary>
    [Fact]
    public void AgregarAdjuntoDireccion()
    {
      string direccion = AppDomain.CurrentDomain.BaseDirectory + @"ConfiguracionDatos.json";
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.OK);
      http.AgregarAdjunto(direccion, @"application/json");
      Assert.True(http.Content is StreamContent && http.Content.Headers != null);
    }
  }
}
