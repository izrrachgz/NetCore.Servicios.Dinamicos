using System;
using Contexto.Entidades;
using Contexto.Enumerados;
using Xunit;

namespace Negocio.Pruebas.Teorias
{
  /// <summary>
  /// Pruebas teoricas de rendimiento
  /// </summary>
  public class Rendimiento
  {
    private Bitacora Modelo { get; }

    public Rendimiento()
    {
      Modelo = new Bitacora()
      {
        Nombre = @"Death Note",
        Descripcion = @"6:40",
        Tipo = BitacoraTipo.Advertencia,
        Creado = DateTime.Now,
        Modificado = DateTime.Now
      };
    }
  }
}
