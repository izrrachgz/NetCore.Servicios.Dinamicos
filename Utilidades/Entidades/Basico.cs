using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utilidades.Contratos;

namespace Utilidades.Entidades
{
  public class EntidadBase : IEntidad
  {
    [Key, Column(Order = 1), Required]
    public long Id { get; set; }

    [Required]
    public DateTime Creado { get; set; }

    [Required]
    public DateTime Modificado { get; set; }

    [DefaultValue(null)]
    public DateTime? Eliminado { get; set; }
  }
}