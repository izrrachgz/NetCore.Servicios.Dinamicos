using System;
using Datos.Modelos;

namespace Negocio.Utilidades
{
  internal static class Archivo
  {
    public static RespuestaBasica GuardarComoJson(object contenido)
    {
      if (contenido is string) return new RespuestaBasica(false, @"El contenido proporcionado no es valido.");
      RespuestaBasica respuesta;
      try
      {
        respuesta = new RespuestaBasica();
      }
      catch (Exception ex)
      {
        respuesta = new RespuestaBasica(ex);
      }
      return respuesta;
    }
  }
}
