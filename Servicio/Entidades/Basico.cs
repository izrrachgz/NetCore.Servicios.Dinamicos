using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Servicio.Contratos;

namespace Servicio.Entidades
{
  public class EntidadBase : IEntidad
  {
    [Key, Column(Order = 1), Required]
    public int Id { get; set; }

    [/*Index(IsUnique = false, IsClustered = false),*/ Required]
    public DateTime Creado { get; set; }

    [/*Index(IsUnique = false, IsClustered = false),*/ Required]
    public DateTime Modificado { get; set; }

    [DefaultValue(null)]
    public DateTime? Eliminado { get; set; }
  }
}