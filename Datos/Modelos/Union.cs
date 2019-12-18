using System;
using System.Collections.Generic;

namespace Servicio.Modelos
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

  /// <summary>
  /// Tipo de union
  /// </summary>
  public enum TipoUnion
  {
    /// <summary>
    /// Unión interna (inner join)
    /// </summary>
    Interna = 0,

    /// <summary>
    /// Unión a la izqueirda (left join)
    /// </summary>
    Izquierda = 1,

    /// <summary>
    /// Unión a la derecha (right join)
    /// </summary>
    Derecha = 2,
  }

  /// <summary>
  /// Provee el modelo de dato
  /// que representa una fila resultante de una unión
  /// </summary>
  public class FilaUnida
  {
    /// <summary>
    /// Indice de la columna
    /// </summary>
    public int Indice { get; }

    /// <summary>
    /// Nombre de la columna
    /// </summary>
    public string Columna { get; }

    /// <summary>
    /// Valor de la celda
    /// </summary>
    public object Valor { get; }

    public FilaUnida() { }

    public FilaUnida(int indice, string columna, object valor)
    {
      Indice = indice;
      Columna = columna;
      Valor = valor;
    }
  }
}
