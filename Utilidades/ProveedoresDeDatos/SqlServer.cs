using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Utilidades.Modelos;
using Utilidades.Configuraciones;
using Utilidades.Contratos;
using Utilidades.Extensiones;
using Utilidades.Mensajes;

namespace Utilidades.ProveedoresDeDatos
{
  /// <summary>
  /// Servicio de acciones genéricas con interacción a repositorio de datos
  /// </summary>
  /// <typeparam name="T">Entidad</typeparam>
  public class ProveedorDeDatos<T> : IProveedorDeDatos<T> where T : class, IEntidad, new()
  {
    #region Propiedades

    /// <summary>
    /// Cadena de conexion al repositorio de datos
    /// </summary>
    private string CadenaDeConexion { get; }

    /// <summary>
    /// Nombre de la tabla asociada a la entidad
    /// </summary>
    public string Tabla { get; }

    /// <summary>
    /// Tipo asociado a la entidad
    /// </summary>
    public Type Tipo { get; }

    /// <summary>
    /// Columnas asociadas a la entidad
    /// </summary>
    private List<string> Columnas { get; }

    /// <summary>
    /// Columnas de búsqueda asociadas a la entidad
    /// </summary>
    private List<string> ColumnasBusqueda { get; }

    /// <summary>
    /// Cuerpo de instrucción SQL para seleccionar entidades
    /// </summary>
    private const string SqlSeleccionar = @"SELECT {columnas} FROM [dbo].[{tabla}] WHERE {condicion};";

    /// <summary>
    /// Cuerpo de instrucción SQL para insertar una entidad nueva
    /// </summary>
    private const string SqlInsertar = @"INSERT INTO [dbo].[{tabla}] ({columnas}) output INSERTED.ID VALUES ({valores});";

    /// <summary>
    /// Cuerpo de instrucción SQL para actualizar una entidad existente
    /// </summary>
    private const string SqlActualizar = @"UPDATE [dbo].[{tabla}] SET {columnas=>valores} WHERE {condicion} AND ELIMINADO IS NULL;";

    #endregion

    #region Constructores

    /// <summary>
    /// Construye un servicio de entidad utilizando una instancia nueva de repositorio
    /// </summary>
    public ProveedorDeDatos()
    {
      CadenaDeConexion = Configuracion<ConfiguracionUtilidades>.Instancia.CadenaDeConexion;
      Tipo = typeof(T);
      Tabla = Tipo.Name;
      PropertyInfo[] info = Tipo.GetProperties();
      Columnas = info
        .Where(p => p.CustomAttributes
          .Select(a => a.AttributeType)
          .All(a => a != typeof(NotMappedAttribute) && a != typeof(JsonIgnoreAttribute))
        )
        .Select(p =>
        {
          string n = p.Name;
          if (p.CustomAttributes.Any(a => a.AttributeType == typeof(ColumnAttribute)))
          {
            string nombre = p.CustomAttributes
                              .Where(a => a.AttributeType == typeof(ColumnAttribute))
                              .Where(a => a.ConstructorArguments.Any(t => t.ArgumentType == typeof(string)))
                              .SelectMany(a => a.ConstructorArguments)
                              .Select(a => a.Value)
                              .FirstOrDefault()?.ToString() ?? @"";
            n = nombre.NoEsValida() ? n : nombre;
          }
          return n;
        })
        .ToList();
      ColumnasBusqueda = info
        .Where(p => p.PropertyType.Name.Equals(@"String"))
        .Where(p => p.CustomAttributes
          .Select(a => a.AttributeType)
          .All(a => a != typeof(NotMappedAttribute) && a != typeof(JsonIgnoreAttribute))
        )
        .Select(p =>
        {
          string n = p.Name;
          if (p.CustomAttributes.Any(a => a.AttributeType == typeof(ColumnAttribute)))
          {
            string nombre = p.CustomAttributes
                              .Where(a => a.AttributeType == typeof(ColumnAttribute))
                              .Where(a => a.ConstructorArguments.Any(t => t.ArgumentType == typeof(string)))
                              .SelectMany(a => a.ConstructorArguments)
                              .Select(a => a.Value)
                              .FirstOrDefault()?.ToString() ?? @"";
            n = nombre.NoEsValida() ? n : nombre;
          }
          return n;
        })
        .ToList();
    }

    #endregion

    #region Metodos Públicos

    /// <summary>
    /// Crea la instrucción SQL de inserción
    /// </summary>
    /// <param name="parametros">Lista de parametros</param>
    /// <returns>Cadena SQL para insertar con parametros iniciales</returns>
    public string CrearSqlInsertar(out List<SqlParameter> parametros)
    {
      //Agregar la tabla y columnas asociadas
      string sql = SqlInsertar
        .Replace(@"{tabla}", Tabla)
        .Replace(@"{columnas}", $"[{string.Join(@"], [", Columnas.Where(c => !c.Equals(@"Id")))}]");
      parametros = new List<SqlParameter>(Columnas.Count - 4);
      //Agregar valores para las columnas
      StringBuilder consulta = new StringBuilder();
      for (int i = 0; i < Columnas.Count; i++)
      {
        //Nombre de la propiedad (columna)
        string prop = Columnas.ElementAt(i);
        //El id se omite
        if (prop.Equals(@"Id")) continue;
        //Los campos de fecha se agregan acorde a la configuracion del servidor
        if (prop.Equals(@"Creado") || prop.Equals(@"Modificado")) consulta.Append(@"GETDATE()");
        else if (prop.Equals(@"Eliminado")) consulta.Append(@"NULL");
        else
        {
          string columna = $"@{prop}";
          //Agregar parametro vacio
          parametros.Add(new SqlParameter(columna, DBNull.Value));
          consulta.Append(columna);
        }
        consulta.Append((i + 1).Equals(Columnas.Count) ? @"" : @",");
      }
      sql = sql.Replace(@"{valores}", consulta.ToString());
      consulta.Clear();
      return sql;
    }

    /// <summary>
    /// Crea la instrucción SQL de actualización para la entidad dada
    /// </summary>
    /// <param name="parametros">Lista de parametros</param>
    /// <returns>Cadena SQL para actualizar con parametros iniciales</returns>
    public string CrearSqlActualizar(out List<SqlParameter> parametros)
    {
      //Agregar la tabla asociada
      string sql = SqlActualizar.Replace(@"{tabla}", Tabla);
      StringBuilder consulta = new StringBuilder();
      parametros = new List<SqlParameter>(Columnas.Count - 3);
      //Agregar columnas y valores para la consulta
      for (int i = 0; i < Columnas.Count; i++)
      {
        //Nombre de la propiedad (columna)
        string prop = Columnas.ElementAt(i);
        //El Id y la fecha de creación se omite
        if (prop.Equals(@"Id") || prop.Equals(@"Creado")) continue;
        consulta.Append(@"[").Append(prop).Append(@"]=");
        if (prop.Equals(@"Modificado")) consulta.Append(@"GETDATE()");
        else if (prop.Equals(@"Eliminado")) consulta.Append(@"NULL");
        else
        {
          string columna = $"@{prop}";
          //Agregar parametro vacio
          parametros.Add(new SqlParameter(columna, DBNull.Value));
          consulta.Append(columna);
        }
        consulta.Append((i + 1).Equals(Columnas.Count) ? @"" : @",");
      }
      sql = sql.Replace(@"{columnas=>valores}", consulta.ToString());
      consulta.Clear();
      sql = sql.Replace(@"{condicion}", @"Id=@Id");
      parametros.Add(new SqlParameter("@Id", ""));
      return sql;
    }

    /// <summary>
    /// Elimina de manera logica un modelo de entidad
    /// </summary>
    /// <param name="id">Identificador primario del modelo</param>
    /// <returns>Verdadero o Falso</returns>
    public async Task<RespuestaBasica> Eliminar(long id)
    {
      //Verificar identificador primario de entidad
      if (id <= 0)
        return new RespuestaBasica(false, Error.IdentificadorInvalido);
      return await Eliminar(new List<long>(1) { id });
    }

    /// <summary>
    /// Elimina de manera logica una serie de modelos de entidad
    /// </summary>
    /// <param name="ids">Lista de Identificadores primarios de Entidades</param>
    /// <returns>Verdadero o Falso</returns>
    public async Task<RespuestaBasica> Eliminar(List<long> ids)
    {
      //Verificar identificador primario de entidades
      if (ids.NoEsValida() || ids.Any(id => id <= 0))
        return new RespuestaBasica(false, Error.ListaInvalida);
      RespuestaBasica respuesta;
      using (SqlConnection conexion = new SqlConnection(CadenaDeConexion))
      {
        try
        {
          await conexion.OpenAsync();
          using (SqlTransaction transaccion = conexion.BeginTransaction())
          {
            try
            {
              //Actualizar la columna de eliminado al momento actual en las entidades
              using (SqlCommand comando = new SqlCommand($@"UPDATE [dbo].[{Tabla}] SET [Eliminado] = GETDATE() WHERE ELIMINADO IS NULL AND [Id] IN ({ string.Join(",", ids) });", transaccion.Connection, transaccion))
              {
                comando.CommandType = CommandType.Text;
                try
                {
                  long eliminados = await comando.ExecuteNonQueryAsync();
                  bool correcto = eliminados.Equals(ids.Count);
                  respuesta = new RespuestaBasica(correcto)
                  {
                    Mensaje = correcto ? Correcto.SolicitudCompletada : Error.DiferenciaDeElementosAfectados
                  };
                }
                catch (Exception ex)
                {
                  respuesta = new RespuestaBasica(ex);
                }
                comando.Dispose();
              }
              if (respuesta.Correcto)
                transaccion.Commit();
              else
                transaccion.Rollback();
            }
            catch (Exception ex)
            {
              respuesta = new RespuestaBasica(ex);
              transaccion.Rollback();
            }
            transaccion.Dispose();
            if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
          }
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaBasica(ex);
        }
        conexion.Dispose();
      }
      return respuesta;
    }

    #endregion

    #region Metodos Públicos Genéricos

    /// <summary>
    /// Devuelve un modelo de entidad asociado al servicio
    /// </summary>
    /// <param name="id">Identificador primario del modelo</param>
    /// <returns>Entidad</returns>
    public virtual async Task<RespuestaModelo<T>> Obtener(long id)
    {
      //Verificar el identificador primario de la entidad
      if (id <= 0)
        return new RespuestaModelo<T> { Mensaje = Error.IdentificadorInvalido };
      RespuestaModelo<T> respuesta;
      string sql = SqlSeleccionar
        .Replace("{tabla}", Tabla)
        .Replace("{columnas}", $"[{string.Join("], [", Columnas)}]")
        .Replace("{condicion}", "[Eliminado] IS NULL AND [Id] = @Id");
      using (SqlConnection conexion = new SqlConnection(CadenaDeConexion))
      {
        try
        {
          await conexion.OpenAsync();
          using (SqlCommand comando = new SqlCommand(sql, conexion))
          {
            try
            {
              using (SqlDataReader resultado = await comando.ExecuteReaderAsync())
              {
                try
                {
                  //Obtener la entidad solo con las columnas seleccionadas
                  T e = new T();
                  while (await resultado.ReadAsync())
                    foreach (string columna in Columnas)
                      Tipo.GetProperty(columna)?.SetValue(e, resultado[columna].Equals(DBNull.Value) ? null : resultado[columna]);
                  resultado.Close();
                  respuesta = new RespuestaModelo<T>(e);
                }
                catch (Exception ex)
                {
                  respuesta = new RespuestaModelo<T>(ex);
                }
                resultado.Dispose();
              }
            }
            catch (Exception ex)
            {
              respuesta = new RespuestaModelo<T>(ex);
            }
            comando.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<T>(ex);
        }
        conexion.Dispose();
      }
      return respuesta;
    }

    /// <summary>
    /// Devuelve una lista de entidades asociadas al servicio dentro de un modelo de datos paginado
    /// </summary>
    /// <param name="paginado">Solicitud de página</param>
    /// <param name="condicion">Condiciones para obtener las entidades</param>
    /// <returns>Lista de Entidades</returns>
    public virtual async Task<RespuestaColeccion<T>> Obtener(Paginado paginado, List<Condicion> condicion = null)
    {
      //Verificar páginado
      if (paginado == null)
        return new RespuestaColeccion<T> { Correcto = false, Mensaje = Error.PaginadoNoEsValido };
      //Verificar que las columnas de ordenamiento pertenezcan a la entidad
      if (!paginado.Orden.Columnas.TrueForAll(c => Columnas.Contains(c)))
        return new RespuestaColeccion<T> { Correcto = false, Mensaje = Error.ColumnasDeBusquedaNoCoinciden };
      //Verificar las condiciones si se encuentran definidas
      if (condicion != null && condicion.NoEsValida())
        return new RespuestaColeccion<T> { Correcto = false, Mensaje = Error.IdentificadorInvalido };
      //Verificar que las columnas de condicion pertenezcan a la entidad
      if (condicion != null && !condicion.Select(c => c.Columna).ToList().TrueForAll(c => Columnas.Contains(c)))
        return new RespuestaColeccion<T> { Correcto = false, Mensaje = Error.ColumnasDeBusquedaNoCoinciden };
      RespuestaColeccion<T> respuesta;
      using (SqlConnection conexion = new SqlConnection(CadenaDeConexion))
      {
        try
        {
          await conexion.OpenAsync();
          using (SqlCommand comando = new SqlCommand("", conexion))
          {
            comando.CommandType = CommandType.Text;
            //Consulta para agregar las condiciones de búsqueda
            StringBuilder condiciones = new StringBuilder();
            //Agregar las condiciones
            condiciones.Append(@"[Eliminado] is ").Append(paginado.Eliminados ? @"not NULL" : @"NULL");
            //Agregar filtro de rango de fecha
            if (!paginado.RangoFechaInicio.NoEsValida())
              condiciones.Append(@" AND [Modificado] >= @RangoFechaInicio");
            //Agregar filtro de rango de fecha
            if (!paginado.RangoFechaFin.NoEsValida())
              condiciones.Append(@" AND [Modificado] <= @RangoFechaFin");
            //Agregar filtro de búsqueda
            if (!paginado.Busqueda.NoEsValida() && !ColumnasBusqueda.NoEsValida())
              condiciones.Append($" AND ([{string.Join("] LIKE '%'+ @Busqueda +'%' OR [", ColumnasBusqueda)}] LIKE '%'+ @Busqueda +'%' )");
            //Obtener condiciones extra de búsqueda
            Tuple<string, SqlParameter[]> condicionesExtra = condicion?.Sql();
            //Agregar condiciones extra
            if (condicionesExtra != null)
              condiciones.Append(" AND ").Append(condicionesExtra.Item1);
            //Consulta de conteo de registros acorde a las condiciones
            StringBuilder conteo = new StringBuilder($@"SELECT COUNT(*) FROM [dbo].[{Tabla}] WHERE ");
            conteo.Append(condiciones);
            comando.CommandText = conteo.ToString();
            //limpia consulta de conteo
            conteo.Clear();
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
            //Agregar parametros extra
            if (condicionesExtra != null) comando.Parameters.AddRange(condicionesExtra.Item2);
            //Contar la cantidad de registros acorde a las condiciones
            long total;
            using (SqlDataReader lector = await comando.ExecuteReaderAsync())
            {
              try
              {
                bool leido = await lector.ReadAsync();
                total = leido ? lector.GetInt64(0) : 0;
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
              respuesta = new RespuestaColeccion<T>()
              {
                Correcto = false,
                Coleccion = null,
                Paginado = paginado
              };
            }
            else
            {
              condiciones.Append($@" ORDER BY [{string.Join("], [", paginado.Orden.Columnas)}] {(paginado.Orden.Ascendente ? "ASC" : "DESC")} OFFSET {paginado.PaginaIndice * paginado.Elementos} ROWS FETCH NEXT {paginado.Elementos} ROWS ONLY");
              comando.ResetCommandTimeout();
              comando.CommandText = SqlSeleccionar
                .Replace("{tabla}", Tabla)
                .Replace("{columnas}", $"[{string.Join("], [", Columnas)}]")
                .Replace("{condicion}", condiciones.ToString());
              //limpia consulta de registros
              using (SqlDataReader resultado = await comando.ExecuteReaderAsync())
              {
                try
                {
                  //Obtener la pagína solo con las columnas seleccionadas
                  List<T> lista = new List<T>();
                  while (await resultado.ReadAsync())
                  {
                    T e = new T();
                    foreach (string columna in Columnas) Tipo.GetProperty(columna)?.SetValue(e, resultado[columna].Equals(DBNull.Value) ? null : resultado[columna]);
                    lista.Add(e);
                  }
                  resultado.Close();
                  respuesta = new RespuestaColeccion<T>(paginado, lista);
                }
                catch (Exception ex)
                {
                  respuesta = new RespuestaColeccion<T>(ex);
                }
                resultado.Dispose();
              }
              condiciones.Clear();
            }
            comando.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaColeccion<T>(ex);
        }
        conexion.Dispose();
      }
      return respuesta;
    }

    /// <summary>
    /// Devuelve una lista de clave/valor asociadas a la entidad
    /// </summary>
    /// <param name="columna">Nombre de la columna para seleccionar</param>
    /// <returns>Lista de clave/valor asociados a la Entidad</returns>
    public virtual async Task<RespuestaColeccion<IndiceValor>> Obtener(string columna)
    {
      //Verificar la columna definida
      if (columna.NoEsValida())
        return new RespuestaColeccion<IndiceValor> { Correcto = false, Mensaje = Error.IdentificadorInvalido };
      //Verificar que la columna pertenezca a la entidad
      if (!Columnas.Contains(columna))
        return new RespuestaColeccion<IndiceValor> { Correcto = false, Mensaje = Error.ColumnasDeBusquedaNoCoinciden };
      RespuestaColeccion<IndiceValor> respuesta;
      string sql = SqlSeleccionar
        .Replace("{tabla}", Tabla)
        .Replace("{columnas}", $"[Id] as Indice, [{columna}] as Valor")
        .Replace("{condicion}", "[Eliminado] IS NULL");
      using (SqlConnection conexion = new SqlConnection(CadenaDeConexion))
      {
        try
        {
          await conexion.OpenAsync();
          using (SqlCommand comando = new SqlCommand(sql, conexion))
          {
            try
            {
              using (SqlDataReader resultado = await comando.ExecuteReaderAsync())
              {
                try
                {
                  //Obtener la pagína solo con las columnas seleccionadas
                  List<IndiceValor> lista = new List<IndiceValor>();
                  while (await resultado.ReadAsync())
                  {
                    lista.Add(new IndiceValor
                    {
                      Indice = resultado.GetInt64(0),
                      Valor = resultado.GetString(1)
                    });
                  }
                  resultado.Close();
                  respuesta = new RespuestaColeccion<IndiceValor>(lista);
                }
                catch (Exception ex)
                {
                  respuesta = new RespuestaColeccion<IndiceValor>(ex);
                }
                resultado.Dispose();
              }
            }
            catch (Exception ex)
            {
              respuesta = new RespuestaColeccion<IndiceValor>(ex);
            }
            comando.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaColeccion<IndiceValor>(ex);
        }
        conexion.Dispose();
      }
      return respuesta;
    }

    /// <summary>
    /// Devuelve una lista de entidades asociadas al servicio conteniendo solo las columnas indicadas
    /// </summary>
    /// <param name="columnas">Nombre de las columnas para seleccionar</param>
    /// <param name="paginado">Solicitud de página</param>
    /// <param name="condicion">Condiciones para obtener las entidades</param>
    /// <returns>Lista de entidades asociadas al servicio</returns>
    public virtual async Task<RespuestaColeccion<T>> Obtener(string[] columnas, Paginado paginado, List<Condicion> condicion = null)
    {
      //Verificar paginado y columnas
      if (paginado == null)
        return new RespuestaColeccion<T> { Correcto = false, Mensaje = Error.PaginadoNoEsValido };
      //Verificar columnas de seleccion existentes dentro de las de la entidad
      if (columnas.NoEsValida() || !columnas.ToList().TrueForAll(Columnas.Contains))
        return new RespuestaColeccion<T> { Correcto = false, Mensaje = Error.ColumnasDeBusquedaNoCoinciden };
      //Verificar que las columnas de ordenamiento pertenezcan a la entidad
      if (!paginado.Orden.Columnas.TrueForAll(columnas.Contains))
        return new RespuestaColeccion<T> { Correcto = false, Mensaje = Error.IdentificadorInvalido };
      //Verificar las condiciones si se encuentran definidas
      if (condicion != null && condicion.NoEsValida())
        return new RespuestaColeccion<T> { Correcto = false, Mensaje = Error.IdentificadorInvalido };
      //Verificar que las columnas de condicion pertenezcan a la entidad
      if (condicion != null && !condicion.Select(c => c.Columna).ToList().TrueForAll(columnas.Contains))
        return new RespuestaColeccion<T> { Correcto = false, Mensaje = Error.ColumnasDeBusquedaNoCoinciden };
      RespuestaColeccion<T> respuesta;
      using (SqlConnection conexion = new SqlConnection(CadenaDeConexion))
      {
        try
        {
          await conexion.OpenAsync();
          using (SqlCommand comando = new SqlCommand("", conexion))
          {
            comando.CommandType = CommandType.Text;
            //Consulta para agregar las condiciones de búsqueda
            StringBuilder condiciones = new StringBuilder();
            //Agregar las condiciones
            condiciones.Append(@"[Eliminado] is ").Append(paginado.Eliminados ? @"not NULL" : @"NULL");
            //Agregar filtro de rango de fecha
            if (!paginado.RangoFechaInicio.NoEsValida())
              condiciones.Append(@" AND [Modificado] >= @RangoFechaInicio");
            //Agregar filtro de rango de fecha
            if (!paginado.RangoFechaFin.NoEsValida())
              condiciones.Append(@" AND [Modificado] <= @RangoFechaFin");
            //Agregar filtro de búsqueda
            if (!paginado.Busqueda.NoEsValida() && !ColumnasBusqueda.NoEsValida())
              condiciones.Append($" AND ([{string.Join("] LIKE '%'+ @Busqueda +'%' OR [", ColumnasBusqueda)}] LIKE '%'+ @Busqueda +'%')");
            //Obtener condiciones extra de búsqueda
            Tuple<string, SqlParameter[]> condicionesExtra = condicion?.Sql();
            //Agregar condiciones extra
            if (condicionesExtra != null)
              condiciones.Append(" AND ").Append(condicionesExtra.Item1);
            //Consulta de conteo de registros acorde a las condiciones
            StringBuilder conteo = new StringBuilder($@"SELECT COUNT(*) FROM [dbo].[{Tabla}] WHERE ");
            conteo.Append(condiciones);
            comando.CommandText = conteo.ToString();
            //limpia consulta de conteo
            conteo.Clear();
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
            //Agregar parametros extra
            if (condicionesExtra != null) comando.Parameters.AddRange(condicionesExtra.Item2);
            //Contar la cantidad de registros acorde a las condiciones
            long total;
            using (SqlDataReader lector = await comando.ExecuteReaderAsync())
            {
              try
              {
                bool leido = await lector.ReadAsync();
                total = leido ? lector.GetInt64(0) : 0;
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
              respuesta = new RespuestaColeccion<T>()
              {
                Correcto = false,
                Coleccion = null,
                Paginado = paginado
              };
            }
            else
            {
              condiciones.Append($@" ORDER BY [{string.Join("], [", paginado.Orden.Columnas)}] {(paginado.Orden.Ascendente ? "ASC" : "DESC")} OFFSET {paginado.PaginaIndice * paginado.Elementos} ROWS FETCH NEXT {paginado.Elementos} ROWS ONLY");
              comando.ResetCommandTimeout();
              comando.CommandText = SqlSeleccionar
                .Replace("{tabla}", Tabla)
                .Replace("{columnas}", $"[{string.Join("], [", columnas)}]")
                .Replace("{condicion}", condiciones.ToString());
              //limpia consulta de registros
              using (SqlDataReader resultado = await comando.ExecuteReaderAsync())
              {
                try
                {
                  //Obtener la pagína solo con las columnas seleccionadas
                  List<T> lista = new List<T>();
                  while (await resultado.ReadAsync())
                  {
                    T e = new T();
                    foreach (string columna in columnas) Tipo.GetProperty(columna)?.SetValue(e, resultado[columna].Equals(DBNull.Value) ? null : resultado[columna]);
                    lista.Add(e);
                  }
                  resultado.Close();
                  respuesta = new RespuestaColeccion<T>(paginado, lista);
                }
                catch (Exception ex)
                {
                  respuesta = new RespuestaColeccion<T>(ex);
                }
                resultado.Dispose();
              }
              condiciones.Clear();
            }
            comando.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaColeccion<T>(ex);
        }
        conexion.Dispose();
      }
      return respuesta;
    }

    /// <summary>
    /// Guarda cambios efectuados al modelo de entidad
    /// </summary>
    /// <param name="modelo">Entidad</param>
    /// <returns>Verdadero o Falso</returns>
    public virtual async Task<RespuestaBasica> Guardar(T modelo)
    {
      //Verificar el modelo de datos de la entidad
      if (modelo == null)
        return new RespuestaBasica(false, Error.ModeloInvalido);
      return modelo.Id.Equals(0) ? await Insertar(modelo) : await Actualizar(modelo);
    }

    /// <summary>
    /// Guarda cambios efectuados a una serie de modelos de entidad
    /// </summary>
    /// <param name="entidades">Lista de Entidades</param>
    /// <returns>Arreglo de identificadores primarios guardados</returns>
    public virtual async Task<RespuestaColeccion<long>> Guardar(List<T> entidades)
    {
      //Verificar la lista de modelos de datos asociados a la entidad
      if (entidades.NoEsValida())
        return new RespuestaColeccion<long> { Correcto = false, Mensaje = Error.ListaInvalida };
      RespuestaColeccion<long> respuesta;
      using (SqlConnection conexion = new SqlConnection(CadenaDeConexion))
      {
        try
        {
          await conexion.OpenAsync();
          using (SqlTransaction transaccion = conexion.BeginTransaction())
          {
            //Coleccion de ids insertados/actualizados
            List<long> ids = new List<long>(entidades.Count);
            //Insertar las entidades
            if (entidades.Any(e => e.Id.Equals(0)))
            {
              RespuestaColeccion<long> insertados = await Insertar(entidades.Where(e => e.Id.Equals(0)).ToList(), transaccion);
              if (insertados.Correcto) ids.AddRange(insertados.Coleccion);
            }
            //Actualizar las entidades
            if (entidades.Any(e => !e.Id.Equals(0)))
            {
              RespuestaModelo<long> actualizados = await Actualizar(entidades.Where(e => !e.Id.Equals(0)).ToList(), transaccion);
              if (actualizados.Correcto) ids.AddRange(entidades.Where(e => !e.Id.Equals(0)).Select(e => e.Id).ToList());
            }
            respuesta = new RespuestaColeccion<long>(ids) { Correcto = !ids.NoEsValida() && ids.All(id => id > 0) };
            try
            {
              if (respuesta.Correcto)
                transaccion.Commit();
              else
                transaccion.Rollback();
            }
            catch (Exception ex)
            {
              respuesta = new RespuestaColeccion<long>(ex);
              transaccion.Rollback();
            }
            transaccion.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaColeccion<long>(ex);
        }
        conexion.Dispose();
      }
      return respuesta;
    }

    /// <summary>
    /// Inserta una Entidad de manera óptima
    /// </summary>
    /// <param name="modelo">Entidad</param>
    /// <returns>Identificador primario insertado</returns>
    public virtual async Task<RespuestaModelo<long>> Insertar(T modelo)
    {
      //Verificar el modelo de datos y el identificador primario del modelo asociado a la entidad
      if (modelo == null || !modelo.Id.Equals(0))
        return new RespuestaModelo<long> { Correcto = false, Mensaje = Error.ModeloInvalido };
      RespuestaModelo<long> respuesta;
      using (SqlConnection conexion = new SqlConnection(CadenaDeConexion))
      {
        try
        {
          await conexion.OpenAsync();
          using (SqlTransaction transaccion = conexion.BeginTransaction())
          {
            try
            {
              respuesta = await Insertar(modelo, transaccion);
              if (respuesta.Correcto)
                transaccion.Commit();
              else
                transaccion.Rollback();
            }
            catch (Exception ex)
            {
              respuesta = new RespuestaModelo<long>(ex);
              transaccion.Rollback();
            }
            transaccion.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<long>(ex);
        }
        conexion.Dispose();
      }
      return respuesta;
    }

    /// <summary>
    /// Inserta una Entidad de manera óptima
    /// </summary>
    /// <param name="modelo">Entidad</param>
    /// <param name="transaccion">Transacción abierta asociada a la conexión vigente</param>
    /// <returns>Identificador primario insertado</returns>
    public virtual async Task<RespuestaModelo<long>> Insertar(T modelo, SqlTransaction transaccion)
    {
      //Verificar el modelo de datos y el identificador primario del modelo asociado a la entidad
      if (modelo == null || !modelo.Id.Equals(0))
        return new RespuestaModelo<long> { Correcto = false, Mensaje = Error.ModeloInvalido };
      RespuestaModelo<long> respuesta;
      using (SqlCommand comando = new SqlCommand(CrearSqlInsertar(out List<SqlParameter> parametros), transaccion.Connection, transaccion))
      {
        try
        {
          //Agregar valores a los parametros acorde a la propiedad de la entidad
          parametros.ForEach(p =>
          {
            p.Value = Tipo.GetProperty(p.ParameterName.TrimStart('@'))?.GetValue(modelo);
            p.Value = p.Value ?? DBNull.Value;
            comando.Parameters.Add(p);
          });
          //Id de la entidad insertada
          long id = Convert.ToInt32(await comando.ExecuteScalarAsync());
          //Limpiar objetos utilizados
          parametros.Clear();
          parametros.TrimExcess();
          comando.Parameters.Clear();
          respuesta = new RespuestaModelo<long>(id) { Correcto = id > 0 };
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<long>(ex);
        }
        comando.Dispose();
      }
      return respuesta;
    }

    /// <summary>
    /// Inserta una serie de Entidades de manera óptima
    /// </summary>
    /// <param name="entidades">Lista de Entidades</param>
    /// <returns>Arreglo de identificadores primarios insertados</returns>
    public virtual async Task<RespuestaColeccion<long>> Insertar(List<T> entidades)
    {
      //Verificar la lista de modelo de datos y el identificador primario del modelo asociado a la entidad
      if (entidades.NoEsValida() || entidades.Any(m => !m.Id.Equals(0)))
        return new RespuestaColeccion<long> { Correcto = false, Mensaje = Error.ListaInvalida };
      RespuestaColeccion<long> respuesta;
      using (SqlConnection conexion = new SqlConnection(CadenaDeConexion))
      {
        try
        {
          await conexion.OpenAsync();
          using (SqlTransaction transaccion = conexion.BeginTransaction())
          {
            try
            {
              respuesta = await Insertar(entidades, transaccion);
              if (respuesta.Correcto)
                transaccion.Commit();
              else
                transaccion.Rollback();
            }
            catch (Exception ex)
            {
              respuesta = new RespuestaColeccion<long>(ex);
              transaccion.Rollback();
            }
            transaccion.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaColeccion<long>(ex);
        }
        conexion.Dispose();
      }
      return respuesta;
    }

    /// <summary>
    /// Inserta una serie de Entidades de manera óptima utilizando una conexión con transacción latente
    /// </summary>
    /// <param name="entidades">Lista de Entidades</param>
    /// <param name="transaccion">Transacción abierta asociada a la conexión vigente</param>
    /// <returns>Arreglo de identificadores primarios insertados</returns>
    public virtual async Task<RespuestaColeccion<long>> Insertar(List<T> entidades, SqlTransaction transaccion)
    {
      //Verificar el modelo de datos y el identificador primario del modelo asociado a la entidad
      //y que la transacción proporcionada se encuentra activa
      if (entidades.NoEsValida() || entidades.Any(m => !m.Id.Equals(0)) || transaccion.Connection.NoEsValida())
        return new RespuestaColeccion<long> { Correcto = false, Mensaje = Error.ListaInvalida };
      RespuestaColeccion<long> respuesta;
      List<long> ids = new List<long>(entidades.Count);
      using (SqlCommand comando = new SqlCommand(CrearSqlInsertar(out List<SqlParameter> parametros), transaccion.Connection, transaccion))
      {
        try
        {
          comando.Parameters.AddRange(parametros.ToArray());
          foreach (T e in entidades)
          {
            parametros.ForEach(p =>
            {
              object valor = Tipo.GetProperty(p.ParameterName.TrimStart('@'))?.GetValue(e);
              valor = valor ?? DBNull.Value;
              comando.Parameters[p.ParameterName].Value = valor;
            });
            long id = Convert.ToInt32(await comando.ExecuteScalarAsync());
            if (id <= 0) break;
            ids.Add(id);
          }
          //Limpiar objetos utilizados
          parametros.Clear();
          parametros.TrimExcess();
          comando.Parameters.Clear();
          //Todos los ids insertados deben ser mayor a 0
          bool correcto = !ids.NoEsValida() && ids.All(id => id > 0);
          respuesta = new RespuestaColeccion<long>(ids)
          {
            Correcto = correcto,
            Mensaje = correcto ? Correcto.SolicitudCompletada : Error.DiferenciaDeElementosAfectados
          };
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaColeccion<long>(ex);
        }
        comando.Dispose();
      }
      return respuesta;
    }

    /// <summary>
    /// Actualiza una Entidad de manera óptima
    /// </summary>
    /// <param name="modelo">Entidad</param>
    /// <returns>Cantidad de filas afectadas</returns>
    public virtual async Task<RespuestaModelo<long>> Actualizar(T modelo)
    {
      //Verificar el modelo de datos y el identificador primario del modelo asociado a la entidad
      if (modelo == null || modelo.Id.Equals(0))
        return new RespuestaModelo<long> { Correcto = false, Mensaje = Error.ModeloInvalido };
      RespuestaModelo<long> respuesta;
      using (SqlConnection conexion = new SqlConnection(CadenaDeConexion))
      {
        try
        {
          await conexion.OpenAsync();
          using (SqlTransaction transaccion = conexion.BeginTransaction())
          {
            try
            {
              respuesta = await Actualizar(modelo, transaccion);
              if (respuesta.Correcto)
                transaccion.Commit();
              else
                transaccion.Rollback();
            }
            catch (Exception ex)
            {
              respuesta = new RespuestaModelo<long>(ex);
              transaccion.Rollback();
            }
            transaccion.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<long>(ex);
        }
        conexion.Dispose();
      }
      return respuesta;
    }

    /// <summary>
    /// Actualiza una Entidad de manera óptima
    /// </summary>
    /// <param name="modelo">Entidad</param>
    /// <param name="transaccion">Transacción abierta asociada a la conexión vigente</param>
    /// <returns>Cantidad de filas afectadas</returns>
    public virtual async Task<RespuestaModelo<long>> Actualizar(T modelo, SqlTransaction transaccion)
    {
      //Verificar el modelo de datos y el identificador primario del modelo asociado a la entidad
      if (modelo == null || modelo.Id.Equals(0))
        return new RespuestaModelo<long> { Correcto = false, Mensaje = Error.ModeloInvalido };
      RespuestaModelo<long> respuesta;
      using (SqlCommand comando = new SqlCommand(CrearSqlActualizar(out List<SqlParameter> parametros), transaccion.Connection, transaccion))
      {
        try
        {
          parametros.ForEach(p =>
          {
            p.Value = Tipo.GetProperty(p.ParameterName.TrimStart('@'))?.GetValue(modelo);
            p.Value = p.Value ?? DBNull.Value;
            comando.Parameters.Add(p);
          });
          //El número de afectados debe ser al menos 1
          long afectados = await comando.ExecuteNonQueryAsync();
          //Limpiar objetos utilizados
          parametros.Clear();
          parametros.TrimExcess();
          comando.Parameters.Clear();
          respuesta = new RespuestaModelo<long>(afectados) { Correcto = afectados > 0 };
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<long>(ex);
        }
        comando.Dispose();
      }
      return respuesta;
    }

    /// <summary>
    /// Actualiza una serie de Entidades de manera óptima
    /// </summary>
    /// <param name="entidades">Lista de Entidades</param>
    /// <returns>Cantidad de filas afectadas</returns>
    public virtual async Task<RespuestaModelo<long>> Actualizar(List<T> entidades)
    {
      //Verificar el modelo de datos y el identificador primario del modelo asociado a la entidad
      if (entidades.NoEsValida() || entidades.Any(m => m.Id.Equals(0)))
        return new RespuestaModelo<long> { Correcto = false, Mensaje = Error.ListaInvalida };
      RespuestaModelo<long> respuesta;
      using (SqlConnection conexion = new SqlConnection(CadenaDeConexion))
      {
        try
        {
          await conexion.OpenAsync();
          using (SqlTransaction transaccion = conexion.BeginTransaction())
          {
            try
            {
              respuesta = await Actualizar(entidades, transaccion);
              if (respuesta.Correcto)
                transaccion.Commit();
              else
                transaccion.Rollback();
            }
            catch (Exception ex)
            {
              respuesta = new RespuestaModelo<long>(ex);
              transaccion.Rollback();
            }
            transaccion.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<long>(ex);
        }
        conexion.Dispose();
      }
      return respuesta;
    }

    /// <summary>
    /// Actualiza una serie de Entidades de manera óptima utilizando una conexión con transacción latente
    /// </summary>
    /// <param name="entidades">Lista de Entidades</param>
    /// <param name="transaccion">Transacción abierta asociada a la conexión vigente</param>
    /// <returns>Cantidad de filas afectadas</returns>
    public virtual async Task<RespuestaModelo<long>> Actualizar(List<T> entidades, SqlTransaction transaccion)
    {
      //Verificar el modelo de datos y el identificador primario del modelo asociado a la entidad
      //y que la transacción proporcionada se encuentra activa
      if (entidades.NoEsValida() || entidades.Any(m => m.Id.Equals(0)) || transaccion.Connection.NoEsValida())
        return new RespuestaModelo<long> { Correcto = false, Mensaje = Error.ListaInvalida };
      RespuestaModelo<long> respuesta;
      using (SqlCommand comando = new SqlCommand(CrearSqlActualizar(out List<SqlParameter> parametros), transaccion.Connection, transaccion))
      {
        try
        {
          long afectadas = 0;
          comando.Parameters.AddRange(parametros.ToArray());
          foreach (T e in entidades)
          {
            parametros.ForEach(p =>
            {
              object valor = Tipo.GetProperty(p.ParameterName.TrimStart('@'))?.GetValue(e);
              valor = valor ?? DBNull.Value;
              comando.Parameters[p.ParameterName].Value = valor;
            });
            afectadas += await comando.ExecuteNonQueryAsync();
          }
          //La cantidad de filas afectadas debe ser igual a la cantidad total de elementos contenidos en la lista de entidades
          respuesta = new RespuestaModelo<long>(afectadas) { Correcto = afectadas.Equals(entidades.Count) };
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<long>(ex);
        }
        comando.Dispose();
      }
      return respuesta;
    }

    #endregion
  }
}
