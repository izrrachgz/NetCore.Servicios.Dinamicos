using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using Utilidades.Configuraciones;
using Utilidades.Extensiones;
using Utilidades.Modelos;
using Xunit;

namespace Utilidades.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de extensiones de listas
  /// </summary>
  public class Listas
  {
    /// <summary>
    /// Comprueba que la lista no es valida
    /// para su uso
    /// </summary>
    [Fact]
    public void NoEsValida()
    {
      List<string> lista = new List<string>(0);
      Assert.True(lista.NoEsValida());
    }

    /// <summary>
    /// Comprueba que de un listado de configuracion
    /// puede obtenerse un elemento de configuracion
    /// interpretado siempre y cuando exista
    /// </summary>
    [Fact]
    public void ObenerElementoDeConfiguracion()
    {
      List<ElementoConfiguracion> configuraciones = new List<ElementoConfiguracion>(1)
      {
        new ElementoConfiguracion()
        {
          Clave = @"Nombre",
          Valor = @"Israel Ch"
        }
      };
      Assert.True(!configuraciones.Obtener<string>(@"Nombre").NoEsValida());
    }

    /// <summary>
    /// Comprueba que una lista de elementos
    /// puede convertirse en una respuesta
    /// de coleccion
    /// </summary>
    [Fact]
    public void ConvertirEnRespuestaColeccion()
    {
      List<string> nombres = Enumerable.Repeat(@"Israel Ch", 10).ToList();
      Assert.True(nombres.Convertir().Correcto);
    }

    /// <summary>
    /// Comprueba que se puede obtener
    /// el contenido en buffer de una lista
    /// de elementos
    /// </summary>
    [Fact]
    public void ObtenerBytes()
    {
      List<string> nombres = Enumerable.Repeat(@"Israel Ch", 10).ToList();
      Assert.True(!nombres.ObtenerBytes().NoEsValido());
    }

    /// <summary>
    /// Comprueba que el listado de nombres
    /// puede ser interpretado por un documento
    /// excel
    /// </summary>
    [Fact]
    public void DocumentoExcel()
    {
      List<string> nombres = Enumerable.Repeat(@"Israel Ch", 10).ToList();
      RespuestaModelo<SpreadsheetDocument> guardado = nombres.DocumentoExcel();
      Assert.True(guardado.Correcto);
    }
  }
}
