using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilidades.Contratos
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
    long Id { get; set; }

    /// <summary>
    /// Momento en el que se ha creado la entidad
    /// </summary>
    [Required]
    DateTime Creado { get; set; }

    /// <summary>
    /// Momento en el que se ha efectuado el ultimo cambio a la entidad
    /// </summary>
    [Required]
    DateTime Modificado { get; set; }

    /// <summary>
    /// Momento en el que se ha eliminado de manera logica la entidad
    /// </summary>
    [DefaultValue(null)]
    DateTime? Eliminado { get; set; }
  }
}