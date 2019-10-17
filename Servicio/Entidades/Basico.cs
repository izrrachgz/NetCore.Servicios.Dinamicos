using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using Servicio.Contratos;
using Servicio.Extensiones;

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

    [JsonIgnore]
    public List<string> NombreColumnas => GetType()
      .GetProperties()
      .Where(p => !p.Name.Equals(@"NombreColumnas"))
      .Where(p => p.CustomAttributes
        .Select(a => a.AttributeType)
        .All(a => a != typeof(NotMappedAttribute) && a != typeof(JsonIgnoreAttribute))
      )
      .Select(p =>
      {
        string n = p.Name;
        if (p.CustomAttributes.Any(a => a.AttributeType == typeof(ColumnAttribute)))
        {
          string nombre = p.CustomAttributes
                            .Where(a => a.AttributeType == typeof(ColumnAttribute))
                            .Where(a => a.ConstructorArguments.Any(t => t.ArgumentType == typeof(string)))
                            .SelectMany(a => a.ConstructorArguments)
                            .Select(a => a.Value)
                            .FirstOrDefault()?.ToString() ?? @"";
          n = nombre.NoEsValida() ? n : nombre;
        }
        return n;
      })
      .ToList();

    [JsonIgnore]
    public List<string> ColumnasParaBuscar => GetType()
      .GetProperties()
      .Where(p => !p.Name.Equals(@"NombreColumnas") && !p.Name.Equals(@"ColumnasParaBuscar") && p.PropertyType.Name.Equals(@"String"))
      .Where(p => p.CustomAttributes
        .Select(a => a.AttributeType)
        .All(a => a != typeof(NotMappedAttribute) && a != typeof(JsonIgnoreAttribute))
      )
      .Select(p =>
      {
        string n = p.Name;
        if (p.CustomAttributes.Any(a => a.AttributeType == typeof(ColumnAttribute)))
        {
          string nombre = p.CustomAttributes
                            .Where(a => a.AttributeType == typeof(ColumnAttribute))
                            .Where(a => a.ConstructorArguments.Any(t => t.ArgumentType == typeof(string)))
                            .SelectMany(a => a.ConstructorArguments)
                            .Select(a => a.Value)
                            .FirstOrDefault()?.ToString() ?? @"";
          n = nombre.NoEsValida() ? n : nombre;
        }
        return n;
      })
      .ToList();
  }
}