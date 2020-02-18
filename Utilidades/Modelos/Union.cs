using System;
using System.Collections.Generic;
using Utilidades.Enumerados;

namespace Utilidades.Modelos
{
  /// <summary>
  /// Provee el modelo de datos para unir dos entidades
  /// </summary>
  public class Union
  {
    /// <summary>
    /// Tablas
    /// </summary>
    public Tuple<string, string> Tablas { get; }

    /// <summary>
    /// Tipo : interno, izquierda, derecha.
    /// </summary>
    public TipoUnion Tipo { get; }

    /// <summary>
    /// Columnas que indican la unión
    /// </summary>
    public Tuple<List<string>, List<string>> Uniones { get; }

    /// <summary>
    /// Columnas a seleccionar
    /// </summary>
    public Tuple<List<string>, List<string>> Seleccion { get; }

    public Union(Tuple<string, string> tablas, Tuple<List<string>, List<string>> uniones, TipoUnion tipo = TipoUnion.Interna, Tuple<List<string>, List<string>> seleccion = null)
    {
      Tablas = tablas;
      Uniones = uniones;
      Tipo = tipo;
      Seleccion = seleccion;
    }
  }
}
