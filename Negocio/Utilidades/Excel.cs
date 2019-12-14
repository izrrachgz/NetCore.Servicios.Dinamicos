using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Negocio.Extensiones;
using Negocio.Modelos;
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
    public static RespuestaBasica ExisteArchivo(string direccion)
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
      CellValues tipoDeValor = CellValues.String;
      if (e is bool) tipoDeValor = CellValues.Boolean;
      if (e is byte) tipoDeValor = CellValues.Number;
      if (e is short) tipoDeValor = CellValues.Number;
      if (e is int) tipoDeValor = CellValues.Number;
      if (e is long) tipoDeValor = CellValues.Number;
      if (e is double) tipoDeValor = CellValues.Number;
      if (e is decimal) tipoDeValor = CellValues.Number;
      if (e is DateTime) tipoDeValor = CellValues.Date;
      if (e is TimeSpan) tipoDeValor = CellValues.Date;
      Cell celda = new Cell()
      {
        CellValue = new CellValue($"{e}"),
        DataType = new EnumValue<CellValues>(tipoDeValor),
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
    public static RespuestaModelo<SpreadsheetDocument> GuardarContenidoDeLista<T>(SpreadsheetDocument documento, List<T> lista, ConfiguracionReporteExcel configuracion = null)
    {
      //Verificar el documento
      if (documento.NoEsValido())
      {
        return new RespuestaModelo<SpreadsheetDocument>()
        {
          Correcto = false,
          Mensaje = @"El documento proporcionado no es valido."
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
      try
      {
        //Agregar un libro nuevo al documento
        WorkbookPart libro = documento.WorkbookPart ?? documento.AddWorkbookPart();
        libro.Workbook = new Workbook();
        //Agregar el apartado de trabajo
        WorksheetPart espacioDeTrabajo = libro.AddNewPart<WorksheetPart>();
        espacioDeTrabajo.Worksheet = new Worksheet(new SheetData());
        //Agregar proteccion por clave a las entidades del documento
        configuracion = configuracion ?? new ConfiguracionReporteExcel();
        if (!configuracion.Clave.NoEsValida())
        {
          espacioDeTrabajo.Worksheet.AppendChild(new SheetProtection()
          {
            Sheet = true,
            DeleteRows = true,
            DeleteColumns = true,
            InsertRows = true,
            InsertColumns = true,
            InsertHyperlinks = true,
            Password = new HexBinaryValue(configuracion.Clave)
          });
        }
        //Agregar una hoja de trabajo
        Sheets hojas = documento.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
        //Agregar la primera hoja de trabajo
        Sheet sheet = new Sheet()
        {
          Id = documento.WorkbookPart.GetIdOfPart(espacioDeTrabajo),
          SheetId = 1,
          Name = configuracion.Titulo ?? @"Reporte"
        };
        if (!hojas.Any()) hojas.Append(sheet);
        //Obtener la referencia a los datos contenidos en la primera hoja del espacio de trabajo
        SheetData sheetData = espacioDeTrabajo.Worksheet.GetFirstChild<SheetData>();
        //Determinar la primera fila con contenido
        uint indiceComenzar = sheetData.GetFirstChild<Row>()?.RowIndex ?? 1;
        //Agregar encabezados
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
          sheetData.Append(fila);
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
            sheetData.Append(fila);
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
            sheetData.Append(fila);
          }
        }
        documento.Save();
        //Guardar el documento en la direccion especificada
        if (configuracion.DirectorioDeSalida.EsDireccionDeDirectorio())
        {
          documento.SaveAs(configuracion.DirectorioDeSalida + $@"{Guid.NewGuid():N}.xlsx");
          documento.Close();
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
