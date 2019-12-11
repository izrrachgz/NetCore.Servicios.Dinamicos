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
    /// Crea un documento de excel a partir de la coleccion
    /// de elementos contenida en una lista.
    /// </summary>
    /// <param name="lista">Instancia valida de respuesta coleccion</param>
    /// <param name="columnas">Columnas opcionales que deberan aparecer como encabezados</param>
    /// <returns>Documento Excel</returns>
    public static RespuestaModelo<SpreadsheetDocument> ReporteExcel<T>(this List<T> lista, List<string> columnas)
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
      //Verificar que la respuesta de coleccion sea correcta
      if (lista == null || !lista.Any())
      {
        return new RespuestaModelo<SpreadsheetDocument>()
        {
          Correcto = false,
          Mensaje = @"La lista proporcionada no es valida."
        };
      }
      T entidad = lista.First();
      //Verificar las columnas proporcionadas
      if (columnas == null || !columnas.Any())
      {
        return new RespuestaModelo<SpreadsheetDocument>()
        {
          Correcto = false,
          Mensaje = @"Las columnas que has proporcionado no son validas."
        };
      }
      SpreadsheetDocument documento;
      RespuestaModelo<SpreadsheetDocument> resultado;
      using (documento = SpreadsheetDocument.CreateFromTemplate(direccionPlantilla))
      {
        try
        {
          Sheet hojaPrincipal = documento.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault();
          //Agregar el contenido a la hoja principal del documento
          if (hojaPrincipal != null)
          {
            List<Row> filas = hojaPrincipal.Descendants<Row>().ToList();
            //Si no contiene encabezados se agregan unos nuevos
            if (!filas.Any()) filas.Add(new Row());
            Row encabezados = filas.ElementAt(0);
            List<Cell> celdas = hojaPrincipal.Descendants<Cell>().ToList();
            //Decidir que titulos se mostraranen los encabezados del reporte
            columnas.ForEach(t =>
            {
              Cell celda = new Cell()
              {
                CellValue = new CellValue(t),
                DataType = new EnumValue<CellValues>(CellValues.String)
              };
              celdas.Add(celda);
              encabezados.AppendChild(celda);
            });
            PropertyInfo[] propiedadesEntidad = entidad.GetType().GetProperties();
            //Agregar el contenido de la respuesta como contenido del reporte
            lista.ForEach(c =>
            {
              Row fila = new Row();
              columnas.ForEach(t =>
              {
                Cell celda = new Cell()
                {
                  CellValue = new CellValue($"{propiedadesEntidad.First(p => p.Name.Equals(t)).GetValue(c)}"),
                  DataType = new EnumValue<CellValues>(CellValues.String)
                };
                fila.AppendChild(celda);
              });
              filas.Add(fila);
            });
          }
          //Guardar todos los cambios
          documento.Save();
          documento.Close();
          resultado = new RespuestaModelo<SpreadsheetDocument>(documento);
        }
        catch (Exception ex)
        {
          resultado = new RespuestaModelo<SpreadsheetDocument>(ex);
        }
      }
      return resultado;
    }
  }
}
