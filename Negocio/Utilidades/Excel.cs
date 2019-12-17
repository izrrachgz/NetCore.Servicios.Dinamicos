using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Negocio.Modelos;
using Newtonsoft.Json;
using Servicio.Extensiones;
using Servicio.Modelos;

namespace Negocio.Utilidades
{
  internal static class Excel
  {
    /// <summary>
    /// Verifica si existe un archivo
    /// en la direccion proporcionada
    /// </summary>
    /// <param name="direccion">Direccion absoluta al archivo</param>
    /// <returns>Verdadero o falso</returns>
    private static RespuestaBasica ExisteArchivo(string direccion)
    {
      if (direccion.NoEsValida())
      {
        return new RespuestaBasica(false, @"La direccion proporcionada no es valida.");
      }
      FileInfo info = new FileInfo(direccion);
      return new RespuestaBasica()
      {
        Correcto = info.Exists,
        Mensaje = info.Exists ? @"Archivo encontrado" : @"No se ha encontrado la plantilla para generar el reporte."
      };
    }

    /// <summary>
    /// Determina si un valor forma parte
    /// de los tipos de datos del sistema
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    /// <param name="e">Valor de entidad</param>
    /// <returns>Verdadero o falso</returns>
    private static bool EsEntidadDeSistema<T>(T e)
    {
      string ns = e.GetType().Namespace;
      return ns != null && ns.ToLowerInvariant().Contains("system");
    }

    /// <summary>
    /// Inicializa una celda con el valor
    /// de entidad proporcionado
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    /// <param name="e">Valor de entidad</param>
    /// <returns>Celda</returns>
    private static Cell InicializarCelda<T>(T e)
    {
      object valor = e;
      if (e is IEnumerable<T>) valor = JsonConvert.SerializeObject(e);
      Cell celda = new Cell()
      {
        CellValue = new CellValue($"{valor}"),
        DataType = new EnumValue<CellValues>(CellValues.String),
      };
      return celda;
    }

    /// <summary>
    /// Guarda el contenido en el documento proporcionado
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    /// <param name="documento">Referencia al documento</param>
    /// <param name="lista">Coleccion de entidades</param>
    /// <param name="configuracion">Configuracion del documento</param>
    /// <returns>Documento con el contenido</returns>
    public static RespuestaModelo<SpreadsheetDocument> GuardarContenidoDeLista<T>(List<T> lista, ConfiguracionReporteExcel configuracion = null)
    {
      string directorioBase = AppDomain.CurrentDomain.BaseDirectory;
      string direccionPlantilla = $@"{directorioBase}Plantillas\Reportes\RespuestaColeccion.xlsx";
      RespuestaBasica existePlantilla = ExisteArchivo(direccionPlantilla);
      if (!existePlantilla.Correcto)
      {
        return new RespuestaModelo<SpreadsheetDocument>()
        {
          Correcto = false,
          Mensaje = existePlantilla.Mensaje
        };
      }
      //Verificar el contenido que se incluira en el documento
      if (lista.NoEsValida())
      {
        return new RespuestaModelo<SpreadsheetDocument>()
        {
          Correcto = false,
          Mensaje = @"La lista de contenido no es valida."
        };
      }
      RespuestaModelo<SpreadsheetDocument> respuesta;
      SpreadsheetDocument documento = SpreadsheetDocument.CreateFromTemplate(direccionPlantilla);
      try
      {
        //Comprimir al maximo el archivo
        documento.CompressionOption = CompressionOption.Maximum;
        //Asociar el libro del documento si no agregar uno nuevo
        WorkbookPart libro = documento.WorkbookPart ?? documento.AddWorkbookPart();
        //Referencia al espacio de trabajo
        WorksheetPart espacioDeTrabajo;
        //Referencia a las hojas
        Sheets hojas;
        //Referencia a la primera hoja
        Sheet hoja;
        //Verificar los valores de plantilla
        if (libro.Workbook == null || libro.WorksheetParts.Count().Equals(0))
        {
          //Agregar nuevo libro y todas sus partes
          libro.Workbook = new Workbook();
          //Agregar un nuevo espacio de trabajo
          espacioDeTrabajo = libro.AddNewPart<WorksheetPart>();
          //Agregar una nueva hoja con nuevos datos
          espacioDeTrabajo.Worksheet = new Worksheet(new SheetData());
          //Agregar una nueva coleccion de hojas y sus datos
          hojas = libro.Workbook.AppendChild<Sheets>(new Sheets());
          //Agregar una nueva hoja a la coleccion de hojas
          hoja = new Sheet()
          {
            Id = libro.GetIdOfPart(espacioDeTrabajo),
            SheetId = 1,
            Name = @"Reporte"
          };
          hojas.Append(hoja);
          //Agregar proteccion por clave a las entidades del documento
          if (!configuracion.Clave.NoEsValida())
          {
            espacioDeTrabajo.Worksheet.AppendChild(new SheetProtection()
            {
              DeleteRows = true,
              DeleteColumns = true,
              InsertRows = true,
              InsertColumns = true,
              Password = new HexBinaryValue(configuracion.Clave)
            });
          }
        }
        else
        {
          //Buscar el espacio de trabajo
          espacioDeTrabajo = libro.WorksheetParts.First();
          //Buscar la coleccion de hojas
          hojas = libro.Workbook.GetFirstChild<Sheets>() ?? documento.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
          //Buscar la primera hoja de la coleccion
          hoja = hojas.GetFirstChild<Sheet>();
          if (hoja == null)
          {
            //Agregar una nueva hoja a la coleccion de hojas
            hoja = new Sheet()
            {
              Id = libro.GetIdOfPart(espacioDeTrabajo),
              SheetId = 1,
              Name = @"Reporte"
            };
            hojas.Append(hoja);
          }
        }
        //Establecer instancia de configuracion
        configuracion = configuracion ?? new ConfiguracionReporteExcel();
        //Actualizar el titulo de la hoja
        hoja.Name = configuracion.Titulo ?? @"Listado";
        //Obtener la referencia a los datos contenidos en la primera hoja del espacio de trabajo
        SheetData datos = espacioDeTrabajo.Worksheet.GetFirstChild<SheetData>();
        //Determinar la ultima fila para establecer el indice donde debe comenzar a escribir
        uint indiceComenzar = datos.Descendants<Row>().LastOrDefault()?.RowIndex.Value ?? 0;
        indiceComenzar++;
        //Agregar encabezados a partir de la ultima fila escrita en la plantilla
        if (configuracion.Encabezados != null && !configuracion.Encabezados.NoEsValida())
        {
          //Crear una fila nueva
          Row fila = new Row() { RowIndex = indiceComenzar };
          //Agregar todas las entidades como una fila
          for (uint i = 0; i < configuracion.Encabezados.Length; i++)
          {
            //Obtener el encabezado en turno
            Cell celda = InicializarCelda(configuracion.Encabezados[i]);
            fila.AppendChild(celda);
          }
          //Agregar la fila a los datos de la hoja
          datos.Append(fila);
          indiceComenzar++;
        }
        T entidad = lista.ElementAt(0);
        //Obtener la informacion de las propiedades de la entidad
        PropertyInfo[] propiedades = entidad.GetType().GetProperties();
        //Si el tipo de entidad es un tipo de variable, se interpreta como lista de entidades de sistema
        if (EsEntidadDeSistema(entidad))
        {
          //Agregar todas las entidades como una fila
          for (uint i = 0; i < lista.Count; i++)
          {
            //Crear una fila nueva
            Row fila = new Row() { RowIndex = indiceComenzar + i };
            //Obtener la entidad en turno
            T e = lista.ElementAt((int)i);
            Cell celda = InicializarCelda(e);
            fila.AppendChild(celda);
            //Agregar la fila a los datos de la hoja
            datos.Append(fila);
          }
        }
        else
        {
          //Agregar todas las entidades como una fila
          for (uint i = 0; i < lista.Count; i++)
          {
            //Crear una fila nueva
            Row fila = new Row() { RowIndex = indiceComenzar + i };
            //Obtener la entidad en turno
            T e = lista.ElementAt((int)i);
            //Agregar todas las columnas de la entidad
            foreach (PropertyInfo info in propiedades)
            {
              object valor = info.GetValue(e);
              Cell celda = InicializarCelda(valor);
              fila.AppendChild(celda);
            }
            //Agregar la fila a los datos de la hoja
            datos.Append(fila);
          }
        }
        //Agregar propiedades de documento
        documento.PackageProperties.Title = @"Reporte de Listado";
        documento.PackageProperties.Creator = @"Israel Ch";
        documento.PackageProperties.Category = @"Reporte";
        documento.PackageProperties.Description = $@"Listado de entidades tipo {entidad.GetType()}";
        documento.PackageProperties.LastModifiedBy = @"Israel Ch";
        //Agregar propiedades extendidas
        if (documento.ExtendedFilePropertiesPart == null) documento.AddExtendedFilePropertiesPart();
        documento.ExtendedFilePropertiesPart.Properties = new Properties();
        documento.ExtendedFilePropertiesPart.Properties.Company = new Company(@"izrra.ch");
        documento.ExtendedFilePropertiesPart.Properties.Application = new Application("NetCore.Servicios.Dinamicos");
        //Guardar todos los cambios en el documento
        documento.Save();
        //Guardar el documento en la direccion especificada
        if (configuracion.DirectorioDeSalida.EsDireccionDeDirectorio())
        {
          documento.SaveAs(configuracion.DirectorioDeSalida + $@"{Guid.NewGuid():N}.xlsx");
          documento.Close();
          documento.Dispose();
        }
        respuesta = new RespuestaModelo<SpreadsheetDocument>(documento);
      }
      catch (Exception ex)
      {
        respuesta = new RespuestaModelo<SpreadsheetDocument>(ex);
      }
      return respuesta;
    }
  }
}
