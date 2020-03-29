using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Utilidades.Modelos;
using Utilidades.Enumerados;

namespace Utilidades.Extensiones
{
  /// <summary>
  /// Provee metodos de extension de listas de condiciones sql
  /// </summary>
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
      //Verificar las condiciones proporcionadas filtradas
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
          case Operador.DentroDe:
            //Es una lista de string
            if (c.Valor.EsColeccionDeCaracteres())
            {
              sb.Append($@"([{c.Columna}] IN ");
              if (c.Valor is IEnumerable<char>)
                sb.Append($@"(N'{string.Join(@"',N'", c.Valor as IEnumerable<char>)}'))");
              if (c.Valor is IEnumerable<string>)
                sb.Append($@"(N'{string.Join(@"',N'", c.Valor as IEnumerable<string>)}'))");
            }
            //Es una lista de numerica
            if (c.Valor.EsColeccionNumerica())
            {
              sb.Append($@"([{c.Columna}] IN ");
              if (c.Valor is IEnumerable<byte>)
                sb.Append($@"({string.Join(@", ", c.Valor as IEnumerable<byte>)}))");
              if (c.Valor is IEnumerable<short>)
                sb.Append($@"({string.Join(@", ", c.Valor as IEnumerable<short>)}))");
              if (c.Valor is IEnumerable<int>)
                sb.Append($@"({string.Join(@", ", c.Valor as IEnumerable<int>)}))");
              if (c.Valor is IEnumerable<long>)
                sb.Append($@"({string.Join(@", ", c.Valor as IEnumerable<long>)}))");
              if (c.Valor is IEnumerable<float>)
                sb.Append($@"({string.Join(@", ", c.Valor as IEnumerable<float>)}))");
              if (c.Valor is IEnumerable<decimal>)
                sb.Append($@"({string.Join(@", ", c.Valor as IEnumerable<decimal>)}))");
              if (c.Valor is IEnumerable<double>)
                sb.Append($@"({string.Join(@", ", c.Valor as IEnumerable<double>)}))");
            }
            break;
          case Operador.FueraDe:
            //Es una lista de string
            if (c.Valor.EsColeccionDeCaracteres())
            {
              sb.Append($@"([{c.Columna}] NOT IN ");
              if (c.Valor is IEnumerable<char>)
                sb.Append($@"(N'{string.Join(@"',N'", c.Valor as IEnumerable<char>)}'))");
              if (c.Valor is IEnumerable<string>)
                sb.Append($@"(N'{string.Join(@"',N'", c.Valor as IEnumerable<string>)}'))");
            }
            //Es una lista de numerica
            if (c.Valor.EsColeccionNumerica())
            {
              sb.Append($@"([{c.Columna}] NOT IN ");
              if (c.Valor is IEnumerable<byte>)
                sb.Append($@"({string.Join(@", ", c.Valor as IEnumerable<byte>)}))");
              if (c.Valor is IEnumerable<short>)
                sb.Append($@"({string.Join(@", ", c.Valor as IEnumerable<short>)}))");
              if (c.Valor is IEnumerable<int>)
                sb.Append($@"({string.Join(@", ", c.Valor as IEnumerable<int>)}))");
              if (c.Valor is IEnumerable<long>)
                sb.Append($@"({string.Join(@", ", c.Valor as IEnumerable<long>)}))");
              if (c.Valor is IEnumerable<float>)
                sb.Append($@"({string.Join(@", ", c.Valor as IEnumerable<float>)}))");
              if (c.Valor is IEnumerable<decimal>)
                sb.Append($@"({string.Join(@", ", c.Valor as IEnumerable<decimal>)}))");
              if (c.Valor is IEnumerable<double>)
                sb.Append($@"({string.Join(@", ", c.Valor as IEnumerable<double>)}))");
            }
            break;
          default:
            sb.Append($@"([{c.Columna}] = @{parametro})");
            break;
        }
        if (i + 1 != condiciones.Count) sb.Append(@" AND ");
      }
      return new Tuple<string, SqlParameter[]>(sb.ToString(), parametros);
    }
  }
}
