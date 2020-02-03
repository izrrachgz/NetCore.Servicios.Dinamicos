using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Datos.Enumerados;
using Datos.Extensiones;
using Datos.Modelos;
using Xunit;

namespace Datos.Pruebas.Hechos.Extensiones
{
  /// <summary>
  /// Pruebas positivas de condiciones
  /// </summary>
  public class Condiciones
  {
    /// <summary>
    /// Comprueba que el metodo de extension No Es Valida evalua correctamente
    /// una lista de condiciones que no puede ser usada
    /// </summary>
    [Fact]
    public void NoEsValida()
    {
      List<Condicion> condiciones = new List<Condicion>(0);
      Assert.True(condiciones.NoEsValida());
    }

    /// <summary>
    /// Comprueba que el metodo de extension Sql crea las instrucciones
    /// de condicion dadas correctamente
    /// </summary>
    [Fact]
    public void Sql()
    {
      List<Condicion> condiciones = new List<Condicion>(2)
      {
        new Condicion(@"Id",0,Operador.Mayor),
        new Condicion(@"Nombre",@"israel", Operador.Parecido)
      };
      Tuple<string, SqlParameter[]> valores = condiciones.Sql();
      Assert.True(!valores.Item1.NoEsValida() && valores.Item2.Length.Equals(2));
    }
  }
}
