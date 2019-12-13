using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Servicio.Modelos;

namespace Negocio.Extensiones
{
  public static class ExtensionesDeListas
  {
    /// <summary>
    /// Determina si un valor forma parte
    /// de los tipos de datos del sistema
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    /// <param name="e">Valor de entidad</param>
    /// <returns>Verdadero o falso</returns>
    private static bool EsUnidimencional<T>(T e)
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
    /// Guarda un archivo excel simple en el directorio
    /// de ejecucion de la aplicacion, o en un directorio
    /// si es proporcionada una ruta y tiene permisos de escritura.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    /// <param name="lista">Coleccion de entidades</param>
    /// <param name="directorio">Directorio para guardar el archivo</param>
    /// <returns>Directorio para acceder al archivo</returns>
    public static RespuestaModelo<string> GuardarComoExcel<T>(this List<T> lista, string directorio = null)
    {
      string directorioBase = AppDomain.CurrentDomain.BaseDirectory;
      //Verificar que la lista sea valida
      if (!lista.Any())
      {
        return new RespuestaModelo<string>()
        {
          Correcto = false,
          Mensaje = @"La lista proporcionada no es valida."
        };
      }
      directorio = directorio ?? directorioBase;
      //Verificar que exista el directorio
      if (!Directory.Exists(directorio))
      {
        return new RespuestaModelo<string>()
        {
          Correcto = false,
          Mensaje = @"El directorio de destino no es valido."
        };
      }
      string direccionPlantilla = $@"{directorioBase}\Plantillas\Reportes\RespuestaColeccion.xlsx";
      FileInfo infoPlantilla = new FileInfo(direccionPlantilla);
      //Verificar que exista la plantilla de reporte
      if (!infoPlantilla.Exists)
      {
        return new RespuestaModelo<string>()
        {
          Correcto = false,
          Mensaje = @"No se ha encontrado la plantilla para generar el reporte."
        };
      }
      string directorioSalida = $@"{directorio}{Guid.NewGuid():N}.xlsx";
      using (SpreadsheetDocument documento = SpreadsheetDocument.CreateFromTemplate(direccionPlantilla))
      {
        //Agregar un libro nuevo al documento
        WorkbookPart libro = documento.AddWorkbookPart();
        libro.Workbook = new Workbook();
        //Agregar el apartado de trabajo
        WorksheetPart espacioDeTrabajo = libro.AddNewPart<WorksheetPart>();
        espacioDeTrabajo.Worksheet = new Worksheet(new SheetData());
        //Agregar una hoja de trabajo
        Sheets hojas = documento.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
        //Agregar la primera hoja de trabajo
        Sheet sheet = new Sheet()
        {
          Id = documento.WorkbookPart.GetIdOfPart(espacioDeTrabajo),
          SheetId = 1,
          Name = "Reporte"
        };
        hojas.Append(sheet);
        //Obtener la referencia a los datos contenidos en la primera hoja del espacio de trabajo
        SheetData sheetData = espacioDeTrabajo.Worksheet.GetFirstChild<SheetData>();
        //Agregar una fila nueva
        Row encabezados = new Row() { RowIndex = 1 };
        sheetData.Append(encabezados);
        T entidad = lista.ElementAt(0);
        //Obtener la informacion de las propiedades de la entidad
        PropertyInfo[] propiedades = entidad.GetType().GetProperties();
        //Si el tipo de entidad es un tipo de variable, se interpreta como lista de entidades de sistema
        if (EsUnidimencional(entidad))
        {
          //Agregar todas las entidades como una fila
          for (uint i = 0; i < lista.Count; i++)
          {
            //Crear una fila nueva
            Row fila = new Row() { RowIndex = encabezados.RowIndex + i };
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
            Row fila = new Row() { RowIndex = encabezados.RowIndex + i };
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
        //Cerrar el documento
        documento.SaveAs(directorioSalida);
      }
      return new RespuestaModelo<string>(directorioSalida);
    }

    /// <summary>
    /// Convierte una coleccion de entidades en un
    /// documento de excel
    /// </summary>
    /// <typeparam name="T">Tipo de entidad</typeparam>
    /// <param name="lista">Coleccion de entidades</param>
    /// <returns>Directorio para acceder al archivo</returns>
    public static RespuestaModelo<SpreadsheetDocument> ObtenerComoExcel<T>(this List<T> lista)
    {
      string directorioBase = AppDomain.CurrentDomain.BaseDirectory;
      string direccionPlantilla = $@"{directorioBase}\Plantillas\Reportes\RespuestaColeccion.xlsx";
      FileInfo infoPlantilla = new FileInfo(direccionPlantilla);
      //Verificar que exista la plantilla de reporte
      if (!infoPlantilla.Exists)
      {
        return new RespuestaModelo<SpreadsheetDocument>()
        {
          Correcto = false,
          Mensaje = @"No se ha encontrado la plantilla para generar el reporte."
        };
      }
      //Verificar que la lista sea valida
      if (!lista.Any())
      {
        return new RespuestaModelo<SpreadsheetDocument>()
        {
          Correcto = false,
          Mensaje = @"La lista proporcionada no es valida."
        };
      }
      SpreadsheetDocument documento;
      using (documento = SpreadsheetDocument.CreateFromTemplate(direccionPlantilla))
      {
        //Agregar un libro nuevo al documento
        WorkbookPart libro = documento.WorkbookPart ?? documento.AddWorkbookPart();
        libro.Workbook = new Workbook();
        //Agregar el apartado de trabajo
        WorksheetPart espacioDeTrabajo = libro.AddNewPart<WorksheetPart>();
        espacioDeTrabajo.Worksheet = new Worksheet(new SheetData());
        //Agregar una hoja de trabajo
        Sheets hojas = documento.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
        //Agregar la primera hoja de trabajo
        Sheet sheet = new Sheet()
        {
          Id = documento.WorkbookPart.GetIdOfPart(espacioDeTrabajo),
          SheetId = 1,
          Name = "Reporte"
        };
        hojas.Append(sheet);
        //Obtener la referencia a los datos contenidos en la primera hoja del espacio de trabajo
        SheetData sheetData = espacioDeTrabajo.Worksheet.GetFirstChild<SheetData>();
        //Agregar una fila nueva
        Row encabezados = new Row() { RowIndex = 1 };
        sheetData.Append(encabezados);
        T entidad = lista.ElementAt(0);
        //Obtener la informacion de las propiedades de la entidad
        PropertyInfo[] propiedades = entidad.GetType().GetProperties();
        //Si el tipo de entidad es un tipo de variable, se interpreta como lista de entidades de sistema
        if (EsUnidimencional(entidad))
        {
          //Agregar todas las entidades como una fila
          for (uint i = 0; i < lista.Count; i++)
          {
            //Crear una fila nueva
            Row fila = new Row() { RowIndex = encabezados.RowIndex + i };
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
            Row fila = new Row() { RowIndex = encabezados.RowIndex + i };
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
        //Cerrar el documento
        documento.Close();
      }
      return new RespuestaModelo<SpreadsheetDocument>(documento);
    }
  }
}
