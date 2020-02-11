using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Contexto.Entidades;
using Negocio.Extensiones;
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
    public void AdjuntarRespuestaColeccionComoExcel()
    {
      List<EntradaLog> coleccion = new List<EntradaLog>(1)
      {
        new EntradaLog(){
          Nombre = @"Prueba",
          Descripcion = @"Descripcion de prueba"
        },
      };
      RespuestaColeccion<EntradaLog> respuesta = new RespuestaColeccion<EntradaLog>(coleccion);
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.OK);
      http.AdjuntarComoExcel(respuesta);
      Assert.True(http.Content is StreamContent && http.Content.Headers != null);
    }

    /// <summary>
    /// Comprueba que al mensaje http
    /// se le puede adjuntar un documento
    /// interpretado como excel a partir
    /// de una coleccion de datos
    /// </summary>
    [Fact]
    public void AdjuntarListaComoExcel()
    {
      List<EntradaLog> coleccion = new List<EntradaLog>(1)
      {
        new EntradaLog(){
          Nombre = @"Pruebas",
          Descripcion = @"Descripcion de prueba"
        },
      };
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.OK);
      http.AdjuntarComoExcel(coleccion);
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
      List<EntradaLog> coleccion = new List<EntradaLog>(1)
      {
        new EntradaLog(){
          Nombre = @"Pruebas",
          Descripcion = @"Descripcion de prueba"
        },
      };
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.OK);
      http.AdjuntarComoJson(coleccion);
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
      List<EntradaLog> coleccion = new List<EntradaLog>(1)
      {
        new EntradaLog(){
          Nombre = @"Pruebas",
          Descripcion = @"Descripcion de prueba"
        },
      };
      HttpResponseMessage http = new HttpResponseMessage(HttpStatusCode.OK);
      http.AdjuntarExcel(coleccion.DocumentoExcel().Modelo);
      Assert.True(http.Content is StreamContent && http.Content.Headers != null);
    }
  }
}
