using System;
using System.Collections.Generic;
using System.Net.Http;
using DocumentFormat.OpenXml.Packaging;
using Servicio.Extensiones;
using Servicio.Modelos;

namespace Negocio.Extensiones
{
  /// <summary>
  /// Provee metodos de extension para las respuestas tipo http
  /// </summary>
  public static class ExtensionesDeRespuestaHttp
  {
    /// <summary>
    /// Adjunta un documento excel a partir de una coleccion
    /// a un mensaje de respuesta
    /// </summary>
    /// <typeparam name="T">Tipo de entidad en la respuesta coleccion</typeparam>
    /// <param name="http">Referencia a la respuesta</param>
    /// <param name="respueta">Referencia a la respuesta coleccion</param>
    /// <returns>Respuesta web con el documento como adjunto</returns>
    public static void AdjuntarComoExcel<T>(this HttpResponseMessage http, RespuestaColeccion<T> respueta)
    {
      if (!respueta.Correcto) return;
      RespuestaModelo<SpreadsheetDocument> resultado = respueta.Coleccion.DocumentoExcel();
      if (!resultado.Correcto) return;
      ExtensionesHttp.AgregarAdjunto(http, resultado.Modelo.Stream(), $@"Reporte {DateTime.Now:s}.xlsx");
    }

    /// <summary>
    /// Adjunta un documento excel a partir de una coleccion
    /// a un mensaje de respuesta
    /// </summary>
    /// <typeparam name="T">Tipo de entidad en la coleccion</typeparam>
    /// <param name="http">Referencia a la respuesta</param>
    /// <param name="lista">Referencia a la coleccion</param>
    /// <returns>Respuesta web con el documento como adjunto</returns>
    public static void AdjuntarComoExcel<T>(this HttpResponseMessage http, List<T> lista)
    {
      if (lista.NoEsValida()) return;
      RespuestaModelo<SpreadsheetDocument> resultado = lista.DocumentoExcel();
      if (!resultado.Correcto) return;
      ExtensionesHttp.AgregarAdjunto(http, resultado.Modelo.Stream(), $@"Reporte {DateTime.Now:s}.xlsx");
    }

    /// <summary>
    /// Adjunta un documento excel a un mensaje de respuesta
    /// </summary>
    /// <param name="http">Referencia a la respuesta</param>
    /// <param name="documento">Referencia al documento</param>
    /// <returns>Respuesta web con el documento como adjunto</returns>
    public static void AdjuntarExcel(this HttpResponseMessage http, SpreadsheetDocument documento)
    {
      if (documento.NoEsValido()) return;
      ExtensionesHttp.AgregarAdjunto(http, documento.Stream());
    }
  }
}
