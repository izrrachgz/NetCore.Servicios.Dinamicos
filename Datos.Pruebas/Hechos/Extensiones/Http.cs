using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Datos.Extensiones;
using Xunit;

namespace Datos.Pruebas.Hechos.Extensiones
{
  public class Http
  {
    [Fact]
    public void NoEsValida()
    {
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.InternalServerError);
      Assert.True(http.NoEsValida());
    }

    [Fact]
    public void AgregarAdjuntoStream()
    {
      using (FileStream fs = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"ConfiguracionServicio.json"))
      {
        HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.OK);
        http.AgregarAdjunto(fs, @"application/json", @"Configuracion.json");
        Assert.True(http.Content is StreamContent && http.Content.Headers != null);
      }
    }

    [Fact]
    public void AgregarAdjuntoBytes()
    {
      byte[] bytes = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + @"ConfiguracionServicio.json");
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.OK);
      http.AgregarAdjunto(bytes, @"application/json", @"Configuracion.json");
      Assert.True(http.Content is ByteArrayContent && http.Content.Headers != null);
    }

    [Fact]
    public void AgregarAdjuntoInfo()
    {
      FileInfo info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"ConfiguracionServicio.json");
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.OK);
      http.AgregarAdjunto(info, @"application/json");
      Assert.True(http.Content is StreamContent && http.Content.Headers != null);
    }

    [Fact]
    public void AgregarAdjuntoDireccion()
    {
      string direccion = AppDomain.CurrentDomain.BaseDirectory + @"ConfiguracionServicio.json";
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.OK);
      http.AgregarAdjunto(direccion, @"application/json");
      Assert.True(http.Content is StreamContent && http.Content.Headers != null);
    }
  }
}
