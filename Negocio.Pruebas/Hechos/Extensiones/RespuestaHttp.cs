using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Negocio.Extensiones;
using Datos.Entidades;
using Datos.Modelos;
using Xunit;

namespace Negocio.Pruebas.Hechos.Extensiones
{
  public class RespuestaHttp
  {
    [Fact]
    public void AdjuntarComoExcel()
    {
      List<Usuario> usuarios = new List<Usuario>(1)
      {
        new Usuario(){
          Nombre = @"Pruebas",
          ApellidoPaterno = @"Pruebas",
          ApellidoMaterno = @"Pruebas",
          Correo = @"Pruebas@CSharp.com",
          NumeroContacto = @"6623559566"
        },
      };
      RespuestaColeccion<Usuario> respuesta = new RespuestaColeccion<Usuario>(usuarios);
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.OK);
      http.AdjuntarComoExcel(respuesta);
      Assert.True(http.Content is StreamContent && http.Content.Headers != null);
    }

    [Fact]
    public void AdjuntarExcel()
    {
      List<Usuario> usuarios = new List<Usuario>(1)
      {
        new Usuario(){
          Nombre = @"Pruebas",
          ApellidoPaterno = @"Pruebas",
          ApellidoMaterno = @"Pruebas",
          Correo = @"Pruebas@CSharp.com",
          NumeroContacto = @"6623559566"
        },
      };
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.OK);
      http.AdjuntarExcel(usuarios.DocumentoExcel().Modelo);
      Assert.True(http.Content is StreamContent && http.Content.Headers != null);
    }
  }
}
