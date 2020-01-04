using System;
using System.Collections.Generic;

namespace Datos.Modelos
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
  /// Provee un modelo de datos para representar una celda de una tabla
  /// </summary>
  public class ColumnaDeTabla
  {
    /// <summary>
    /// Indice de la columna
    /// </summary>
    public int Indice { get; }

    /// <summary>
    /// Nombre de la columna
    /// </summary>
    public string Nombre { get; }

    /// <summary>
    /// Valor de la celda
    /// </summary>
    public object Celda { get; }

    public ColumnaDeTabla() { }

    public ColumnaDeTabla(int indice, string nombre, object valor)
    {
      Indice = indice;
      Nombre = nombre;
      Celda = valor == DBNull.Value ? null : valor;
    }
  }

  /// <summary>
  /// Provee un modelo de datos para representar la fila de una tabla
  /// </summary>
  public class FilaDeTabla
  {
    /// <summary>
    /// Indice de la fila
    /// </summary>
    public int Indice { get; }

    /// <summary>
    /// Columnas de la fila
    /// </summary>
    public List<ColumnaDeTabla> Columnas { get; set; }

    public FilaDeTabla() { }

    public FilaDeTabla(int indice, List<ColumnaDeTabla> columnas)
    {
      Indice = indice;
      Columnas = columnas;
    }
  }
}
