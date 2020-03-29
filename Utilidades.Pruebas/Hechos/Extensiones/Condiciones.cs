using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Utilidades.Enumerados;
using Utilidades.Extensiones;
using Utilidades.Modelos;
using Xunit;

namespace Utilidades.Pruebas.Hechos.Extensiones
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
      List<Condicion> condiciones = new List<Condicion>()
      {
        new Condicion(@"Id",0,Operador.Mayor),
        new Condicion(@"Nombre",@"israel", Operador.Parecido),
        new Condicion(@"Id",new string []{@"1",@"2",@"3"}, Operador.DentroDe),
        new Condicion(@"Id",new int []{1,2,3}, Operador.DentroDe),
      };
      Tuple<string, SqlParameter[]> valores = condiciones.Sql();
      Assert.True(!valores.Item1.NoEsValida() && valores.Item2.Length.Equals(condiciones.Count));
    }
  }
}
