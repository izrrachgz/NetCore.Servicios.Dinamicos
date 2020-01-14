using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Negocio.Extensiones;
using Datos.Entidades;
using Datos.Modelos;
using Xunit;

namespace Negocio.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de extensiones http
  /// </summary>
  public class RespuestaHttp
  {
    /// <summary>
    /// Comprueba que al mensaje http
    /// se le puede adjuntar un documento
    /// interpretado como excel a partir
    /// de una respuesta de coleccion
    /// </summary>
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

    /// <summary>
    /// Comprueba que al mensaje http
    /// se le puede adjuntar un documento
    /// json
    /// </summary>
    [Fact]
    public void AdjuntarComoJson()
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
      http.AdjuntarComoJson(usuarios);
      Assert.True(http.Content is StringContent && http.Content.Headers != null);
    }

    /// <summary>
    /// Comprueba que al mensaje http
    /// se le puede adjuntar un documento
    /// interpretado como excel a partir
    /// de una coleccion de datos
    /// </summary>
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
