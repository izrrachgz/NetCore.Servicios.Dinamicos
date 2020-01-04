using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Datos.Enumerados;
using Datos.Modelos;

namespace Datos.Extensiones
{
  public static class ExtensionesDeCondiciones
  {
    /// <summary>
    /// Indica si la lista es nula o no contiene ningun elemento
    /// o sobrepasa el rango de parametros admitidos por el motor
    /// de búsqueda
    /// </summary>
    /// <param name="condiciones">Condiciones a evaluar</param>
    /// <returns>Verdadero o falso</returns>
    public static bool NoEsValida(this List<Condicion> condiciones)
    {
      return condiciones == null || !condiciones.Any() || condiciones.Count > 2100 || condiciones.Any(c => c.Columna.NoEsValida());
    }

    /// <summary>
    /// Proporciona la consulta sql acorde a las condiciones
    /// especificadas y los parametros que deben asociarse
    /// </summary>
    /// <param name="condiciones">Condiciones a evaluar</param>
    /// <returns>Tupla con consulta sql y parametros</returns>
    public static Tuple<string, SqlParameter[]> Sql(this List<Condicion> condiciones)
    {
      //Verificar las condiciones proporcionadas
      if (condiciones.NoEsValida())
        return new Tuple<string, SqlParameter[]>("", new SqlParameter[0]);
      condiciones = condiciones
        .Where(c => !c.Columna.Equals("Eliminado"))
        .ToList();
      //Verificar las condiciones proporcionadas
      if (condiciones.NoEsValida())
        return new Tuple<string, SqlParameter[]>("", new SqlParameter[0]);
      //Construir la serie de condiciones
      StringBuilder sb = new StringBuilder(" ");
      SqlParameter[] parametros = new SqlParameter[condiciones.Count];
      for (int i = 0; i < condiciones.Count; i++)
      {
        Condicion c = condiciones.ElementAt(i);
        string parametro = $@"{c.Columna}{i}";
        parametros[i] = new SqlParameter(parametro, c.Valor ?? DBNull.Value);
        switch (c.Operador)
        {
          case Operador.Igual:
            sb.Append($@"([{c.Columna}] = @{parametro})");
            break;
          case Operador.Menor:
            sb.Append($@"([{c.Columna}] < @{parametro})");
            break;
          case Operador.MenorIgual:
            sb.Append($@"([{c.Columna}] <= @{parametro})");
            break;
          case Operador.Distinto:
            sb.Append($@"([{c.Columna}] <> @{parametro})");
            break;
          case Operador.Mayor:
            sb.Append($@"([{c.Columna}] > @{parametro})");
            break;
          case Operador.MayorIgual:
            sb.Append($@"([{c.Columna}] >= @{parametro})");
            break;
          case Operador.Diferente:
            sb.Append($@"([{c.Columna}] != @{parametro})");
            break;
          case Operador.Es:
            sb.Append($@"([{c.Columna}] is @{parametro})");
            break;
          case Operador.NoEs:
            sb.Append($@"([{c.Columna}] is not @{parametro})");
            break;
          case Operador.Parecido:
            sb.Append($@"([{c.Columna}] LIKE '%'+ @{parametro} +'%')");
            break;
          default:
            sb.Append($@"([{c.Columna}] = @{parametro})");
            break;
        }
        if (i + 1 != condiciones.Count) sb.Append(@" AND");
      }
      return new Tuple<string, SqlParameter[]>(sb.ToString(), parametros);
    }
  }
}
