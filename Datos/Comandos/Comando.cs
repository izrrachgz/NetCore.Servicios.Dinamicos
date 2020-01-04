using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos.Extensiones;
using Datos.Modelos;
using Datos.Utilidades;

namespace Datos.Comandos
{
  /// <summary>
  /// Provee el mecanismo para ejecutar
  /// comandos sql
  /// </summary>
  public class Comando
  {
    /// <summary>
    /// Cadena de conexion al repositorio de datos
    /// </summary>
    private string CadenaDeConexion { get; }

    public Comando()
    {
      CadenaDeConexion = Configuracion.Instancia.CadenaDeConexion;
    }

    /// <summary>
    /// Ejecuta una instruccion Sql
    /// </summary>
    /// <param name="sql">Instruccion de consulta : sql plano, nombre de procedimiento almacenado</param>
    /// <param name="parametros">Parametros para agregar al comando</param>
    /// <param name="tipo">Tipo de instruccion</param>
    /// <returns>Listado de resultado de filas</returns>
    private async Task<RespuestaColeccion<FilaDeTabla>> Sql(string sql, SqlParameter[] parametros = null, CommandType tipo = CommandType.Text)
    {
      //Verificar consulta
      if (sql.NoEsValida())
        return new RespuestaColeccion<FilaDeTabla>() { Correcto = false, Mensaje = @"La instruccion sql no es valida." };
      RespuestaColeccion<FilaDeTabla> respuesta;
      using (SqlConnection conexion = new SqlConnection(CadenaDeConexion))
      {
        try
        {
          //Establecer la conexion con el repositorio de datos
          await conexion.OpenAsync();
          using (SqlCommand comando = new SqlCommand(sql, conexion))
          {
            //Establecer el tipo de comando que ejecutaremos
            comando.CommandType = tipo;
            //Agregar los parametros proporcionados
            if (parametros != null && parametros.Length > 0)
              comando.Parameters.AddRange(parametros);
            //Leer los resultados
            using (SqlDataReader lector = await comando.ExecuteReaderAsync())
            {
              //Si hay al menos 1 resultado agregarlo a la lista de respuesta
              if (lector.HasRows)
              {
                List<FilaDeTabla> lista = new List<FilaDeTabla>();
                int indice = 0;
                while (await lector.ReadAsync())
                {
                  List<ColumnaDeTabla> columnas = new List<ColumnaDeTabla>(lector.FieldCount);
                  //Agregar cada valor de columna a la fila
                  for (int c = 0; c < lector.FieldCount; c++)
                    columnas.Add(new ColumnaDeTabla(c, lector.GetName(c), lector.GetValue(c)));
                  lista.Add(new FilaDeTabla(indice, columnas));
                  indice++;
                }
                respuesta = new RespuestaColeccion<FilaDeTabla>(lista);
              }
              else
              {
                //Falso positivo, el servidor ha completado la tarea correctamente pero no ha devuelto ningun resultado
                respuesta = new RespuestaColeccion<FilaDeTabla>()
                {
                  Correcto = false,
                  Mensaje = @"La ejecucion del comando no ha devuelto ningun resultado."
                };
              }
              lector.Dispose();
            }
            comando.Dispose();
          }
          //Cerrar la conexion establecida
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaColeccion<FilaDeTabla>(ex);
        }
        conexion.Dispose();
      }
      return respuesta;
    }

    /// <summary>
    /// Ejecuta un comando sql de tipo texto plano
    /// </summary>
    /// <param name="sql">Texto plano en formato sql para ejecutar</param>
    /// <param name="parametros">Parametros para agregar al comando</param>
    /// <returns>Listado de resultado de filas</returns>
    public async Task<RespuestaColeccion<FilaDeTabla>> Consulta(string sql, SqlParameter[] parametros = null)
      => await Sql(sql, parametros);

    /// <summary>
    /// Ejecuta un procedimiento almacenado
    /// </summary>
    /// <param name="procedimiento">Nombre del procedimiento</param>
    /// <param name="parametros">Parametros para agregar al comando</param>
    /// <returns>Listado de resultado de filas</returns>
    public async Task<RespuestaColeccion<FilaDeTabla>> Procedimiento(string procedimiento, SqlParameter[] parametros = null)
      => await Sql(procedimiento, parametros, CommandType.StoredProcedure);

    /// <summary>
    /// Consulta la unión de dos entidades
    /// </summary>
    /// <param name="union">Solicitud de unión</param>
    /// <param name="paginado">Página solicitada</param>
    /// <returns>Colección de filas resultado de la unión</returns>
    public async Task<RespuestaColeccion<FilaDeTabla>> Union(Union union, Paginado paginado)
    {
      //Verificar valor de tablas
      if (union.Tablas.Item1.NoEsValida() || union.Tablas.Item2.NoEsValida())
        return new RespuestaColeccion<FilaDeTabla>() { Correcto = false, Mensaje = @"La estructura de unión no es válida." };
      //Verificar que no sea la misma
      if (union.Tablas.Item1.Equals(union.Tablas.Item2))
        return new RespuestaColeccion<FilaDeTabla>() { Correcto = false, Mensaje = @"No debes unir la tabla asi misma." };
      //Verificar valor de uniones
      if (union.Uniones.Item1.NoEsValida() || union.Uniones.Item2.NoEsValida() || !union.Uniones.Item1.Count.Equals(union.Uniones.Item2.Count))
        return new RespuestaColeccion<FilaDeTabla>() { Correcto = false, Mensaje = @"Las columnas de unión no son válidas." };
      //Consulta para seleccionar registros unidos
      StringBuilder consulta = new StringBuilder("SELECT ");
      if (union.Seleccion == null)
      {
        //Seleccionar todas las columnas de ambas tablas
        consulta.Append($"[dbo].[{union.Tablas.Item1}].*, [dbo].[{union.Tablas.Item2}].* ");
      }
      else
      {
        //Seleccionar columnas de la tabla izquierda
        if (!union.Seleccion.Item1.NoEsValida())
        {
          consulta.Append($"[dbo].[{union.Tablas.Item1}].[")
            .Append(string.Join($"], [dbo].[{union.Tablas.Item1}].[", union.Seleccion.Item1))
            .Append("]");
        }
        //Seleccionar columnas de la tabla derecha
        if (!union.Seleccion.Item2.NoEsValida())
        {
          if (!union.Seleccion.Item1.NoEsValida()) consulta.Append(", ");
          consulta.Append($"[dbo].[{union.Tablas.Item2}].[")
            .Append(string.Join($"], [dbo].[{union.Tablas.Item2}].[", union.Seleccion.Item2))
            .Append("]");
        }
      }
      //Indicar el tipo de unión entre las tablas
      consulta.Append($" FROM [dbo].[{union.Tablas.Item1}] as t1, [dbo].[{union.Tablas.Item2}] ");
      switch (union.Tipo)
      {
        case TipoUnion.Interna:
          consulta.Append("INNER JOIN ");
          break;
        case TipoUnion.Izquierda:
          consulta.Append("LEFT JOIN ");
          break;
        case TipoUnion.Derecha:
          consulta.Append("RIGHT JOIN ");
          break;
        default:
          consulta.Append("INNER JOIN ");
          break;
      }
      consulta.Append($"[dbo].[{union.Tablas.Item1}] ON ");
      //Agregar columnas de unión
      for (int x = 0; x < union.Uniones.Item1.Count; x++)
      {
        consulta.Append($@"[dbo].[{union.Tablas.Item1}].[{union.Uniones.Item1.ElementAt(x)}] = [dbo].[{union.Tablas.Item2}].[{union.Uniones.Item2.ElementAt(x)}]")
          .Append(x + 1 == union.Uniones.Item1.Count ? "" : " AND ");
      }
      //Consulta para agregar las condiciones de búsqueda
      StringBuilder condiciones = new StringBuilder(" WHERE ");
      condiciones.Append($@"[dbo].[{union.Tablas.Item1}].[Eliminado] is ").Append(paginado.Eliminados ? @"not NULL" : @"NULL");
      condiciones.Append($@" AND [dbo].[{union.Tablas.Item2}].[Eliminado] is ").Append(paginado.Eliminados ? @"not NULL" : @"NULL");
      //Agregar filtro de rango de fecha
      if (!paginado.RangoFechaInicio.NoEsValida())
      {
        condiciones.Append($@" AND [dbo].[{union.Tablas.Item1}].[Modificado] >= @RangoFechaInicio");
        condiciones.Append($@" AND [dbo].[{union.Tablas.Item2}].[Modificado] >= @RangoFechaInicio");
      }
      //Agregar filtro de rango de fecha
      if (!paginado.RangoFechaFin.NoEsValida())
      {
        condiciones.Append($@" AND [dbo].[{union.Tablas.Item1}].[Modificado] <= @RangoFechaFin");
        condiciones.Append($@" AND [dbo].[{union.Tablas.Item2}].[Modificado] <= @RangoFechaFin");
      }
      //Agregar filtro de busqueda
      if (!paginado.Busqueda.NoEsValida() && union.Seleccion != null)
      {
        //Agregar filtro a las columnas de la tabla a la izquierda
        if (!union.Seleccion.Item1.NoEsValida())
          condiciones.Append($" AND ([{string.Join($"] LIKE N'%@Busqueda%' OR [dbo].[{union.Tablas.Item1}].[", union.Seleccion.Item1)}] LIKE N'%@Busqueda%')");
        //Agregar filtro a las columnas de la tabla a la derecha
        if (!union.Seleccion.Item2.NoEsValida())
          condiciones.Append($" AND ([{string.Join($"] LIKE N'%@Busqueda%' OR [dbo].[{union.Tablas.Item2}].[", union.Seleccion.Item2)}] LIKE N'%@Busqueda%')");
      }
      //Consultar registros acorde a las condiciones dadas
      RespuestaColeccion<FilaDeTabla> respuesta;
      using (SqlConnection conexion = new SqlConnection(CadenaDeConexion))
      {
        try
        {
          await conexion.OpenAsync();
          using (SqlCommand comando = new SqlCommand("", conexion))
          {
            StringBuilder conteo = new StringBuilder($"SELECT COUNT(*) FROM [dbo].[{union.Tablas.Item1}], [dbo].[{union.Tablas.Item2}]");
            conteo.Append(condiciones);
            comando.CommandText = conteo.ToString();
            //limpia consulta de conteo
            conteo.Clear();
            comando.CommandType = CommandType.Text;
            //Agregar parámetros
            object[] parametros = new object[3];
            if (!paginado.RangoFechaInicio.NoEsValida())
              parametros[0] = new SqlParameter(@"@RangoFechaInicio", paginado.RangoFechaInicio);
            if (!paginado.RangoFechaFin.NoEsValida())
              parametros[1] = new SqlParameter(@"@RangoFechaFin", paginado.RangoFechaFin);
            if (!paginado.Busqueda.NoEsValida())
              parametros[2] = new SqlParameter(@"@Busqueda", paginado.Busqueda);
            //Si hay al menos un parámetros, se agregan a la consulta.
            if (parametros.Any(p => p != null)) comando.Parameters.AddRange(parametros.Where(p => p != null).ToArray());
            //Contar la cantida de registros acorde a las condiciones
            int total;
            using (SqlDataReader lector = await comando.ExecuteReaderAsync())
            {
              try
              {
                total = await lector.ReadAsync() ? lector.GetInt32(0) : 0;
                lector.Close();
              }
              catch (Exception)
              {
                total = 0;
              }
              lector.Dispose();
            }
            //Inicializar la configuración de paginado
            paginado.CalcularPaginado(total);
            //No hay registros
            if (total.Equals(0))
            {
              respuesta = new RespuestaColeccion<FilaDeTabla>()
              {
                Correcto = false,
                Coleccion = null,
                Paginado = paginado
              };
            }
            else
            {
              consulta.Append(condiciones)
                .Append($@" ORDER BY [dbo].[{union.Tablas.Item1}].[Id] OFFSET {paginado.PaginaIndice * paginado.Elementos} ROWS FETCH NEXT {paginado.Elementos} ROWS ONLY");
              comando.CommandText = consulta.ToString();
              //limpia consulta de registros y condiciones
              consulta.Clear();
              condiciones.Clear();
              comando.ResetCommandTimeout();
              using (SqlDataReader lector = await comando.ExecuteReaderAsync())
              {
                try
                {
                  List<FilaDeTabla> lista = new List<FilaDeTabla>();
                  if (lector.HasRows)
                  {
                    int indice = 0;
                    while (await lector.ReadAsync())
                    {
                      List<ColumnaDeTabla> columnas = new List<ColumnaDeTabla>(lector.FieldCount);
                      //Agregar cada valor de columna a la fila
                      for (int c = 0; c < lector.FieldCount; c++)
                        columnas.Add(new ColumnaDeTabla(c, lector.GetName(c), lector.GetValue(c)));
                      lista.Add(new FilaDeTabla(indice, columnas));
                      indice++;
                    }
                  }
                  respuesta = new RespuestaColeccion<FilaDeTabla>(lista);
                  lector.Close();
                }
                catch (Exception ex)
                {
                  respuesta = new RespuestaColeccion<FilaDeTabla>(ex);
                }
                lector.Dispose();
              }
            }
            comando.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaColeccion<FilaDeTabla>(ex);
        }
      }
      return respuesta;
    }
  }
}
