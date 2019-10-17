using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servicio.Contratos
{
  /// <summary>
  /// Contrado de Entidad
  /// </summary>
  public interface IEntidad
  {
    /// <summary>
    /// Identificador primario de la Entidad
    /// </summary>
    [Key, Column(Order = 1), Required]
    int Id { get; set; }

    /// <summary>
    /// Momento en el que se ha creado la entidad
    /// </summary>
    [/*Index(IsUnique = false, IsClustered = false),*/ Required]
    DateTime Creado { get; set; }

    /// <summary>
    /// Momento en el que se ha efectuado el ultimo cambio a la entidad
    /// </summary>
    [/*Index(IsUnique = false, IsClustered = false),*/ Required]
    DateTime Modificado { get; set; }

    /// <summary>
    /// Momento en el que se ha eliminado de manera logica la entidad
    /// </summary>
    [DefaultValue(null)]
    DateTime? Eliminado { get; set; }

    /// <summary>
    /// Nombre de las columnas en la base de datos
    /// </summary>
    [NotMapped]
    List<string> NombreColumnas { get; }

    /// <summary>
    /// Nombre de las columnas en la base de datos que pueden ser buscadas acorde palabras clave
    /// </summary>
    [NotMapped]
    List<string> ColumnasParaBuscar { get; }
  }
}