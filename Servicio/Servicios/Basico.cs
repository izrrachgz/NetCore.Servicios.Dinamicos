using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Servicio.Contexto;
using Servicio.Contratos;
using Servicio.Extensiones;
using Servicio.Mensajes;
using Servicio.Modelos;

namespace Servicio.Servicios
{
  /// <summary>
  /// Servicio de acciones genéricas con interacción a repositorio de datos
  /// </summary>
  /// <typeparam name="T">Entidad</typeparam>
  public class Servicio<T> : IServicio<T> where T : class, IEntidad, new()
  {
    #region Propiedades

    /// <summary>
    /// Nombre de la tabla asociada a la entidad
    /// </summary>
    public string Tabla => Tipo.Name;

    /// <summary>
    /// Entidad asociada al servicio
    /// </summary>
    public T Entidad { get; }

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

    /// <summary>
    /// Repositorio de datos
    /// </summary>
    internal Repositorio Repositorio { get; set; }

    #endregion

    #region Constructores

    /// <summary>
    /// Construye un servicio de entidad utilizando una instancia nueva de repositorio
    /// </summary>
    public Servicio()
    {
      Repositorio = new Repositorio();
      Entidad = new T();
      Tipo = Entidad.GetType();
      Columnas = Entidad.NombreColumnas;
      ColumnasBusqueda = Entidad.ColumnasParaBuscar;
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
        string prop = Columnas.ElementAt(i);
        if (prop.Equals(@"Id")) continue;
        if (prop.Equals(@"Creado") || prop.Equals(@"Modificado")) consulta.Append(@"GETDATE()");
        else if (prop.Equals(@"Eliminado")) consulta.Append(@"NULL");
        else
        {
          string columna = $"@{Columnas.ElementAt(i)}";
          parametros.Add(new SqlParameter(columna, ""));
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
        string prop = Columnas.ElementAt(i);
        if (prop.Equals(@"Id") || prop.Equals(@"Creado")) continue;
        consulta.Append(@"[").Append(prop).Append(@"]=");
        if (prop.Equals(@"Modificado")) consulta.Append(@"GETDATE()");
        else if (prop.Equals(@"Eliminado")) consulta.Append(@"NULL");
        else
        {
          string columna = $"@{Columnas.ElementAt(i)}";
          parametros.Add(new SqlParameter(columna, ""));
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
    public RespuestaBasica Eliminar(int id)
    {
      if (id <= 0) return new RespuestaBasica(false, Error.IdentificadorInvalido);
      return Eliminar(new List<int>(1) { id });
    }

    /// <summary>
    /// Elimina de manera logica una serie de modelos de entidad
    /// </summary>
    /// <param name="ids">Lista de Identificadores primarios de Entidades</param>
    /// <returns>Verdadero o Falso</returns>
    public RespuestaBasica Eliminar(List<int> ids)
    {
      if (ids.NoEsValida() || ids.Any(id => id <= 0)) return new RespuestaBasica(false, Error.ListaInvalida);
      RespuestaBasica respuesta;

      using (SqlConnection conexion = new SqlConnection(Repositorio.CadenaDeConexion))
      {
        try
        {
          conexion.Open();
          using (SqlTransaction transaccion = conexion.BeginTransaction())
          {
            try
            {
              //Actualizar la columna de eliminado al momento actual en las entidades
              using (SqlCommand comando = new SqlCommand($@"UPDATE {Tabla} SET [Eliminado] = GETDATE() WHERE ELIMINADO IS NULL AND [Id] IN ({ string.Join(",", ids) });", transaccion.Connection, transaccion))
              {
                try
                {
                  bool eliminados = comando.ExecuteNonQuery().Equals(ids.Count);
                  respuesta = new RespuestaBasica(eliminados) { Mensaje = eliminados ? Correcto.SolicitudCompletada : Error.DiferenciaDeElementosAfectados };
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
    public virtual RespuestaModelo<T> Obtener(int id)
    {
      if (id <= 0) return new RespuestaModelo<T> { Mensaje = Error.IdentificadorInvalido };
      RespuestaModelo<T> respuesta;
      try
      {
        respuesta = new RespuestaModelo<T>(
          Repositorio.Set<T>()
            .FirstOrDefault(e => e.Id.Equals(id))
        );
      }
      catch (Exception ex)
      {
        respuesta = new RespuestaModelo<T>(ex);
      }
      return respuesta;
    }

    /// <summary>
    /// Devuelve una lista de entidades asociadas al servicio dentro de un modelo de datos paginado
    /// </summary>
    /// <param name="paginado">Solicitud de página</param>
    /// <returns>Lista de Entidades</returns>
    public virtual RespuestaColeccion<T> Obtener(Paginado paginado)
    {
      if (paginado == null) return new RespuestaColeccion<T> { Correcto = false, Mensaje = Error.IdentificadorInvalido };
      RespuestaColeccion<T> respuesta;
      try
      {
        StringBuilder consulta = new StringBuilder();
        //Agregar la tabla asociada
        string sql = SqlSeleccionar
          .Replace("{tabla}", Tabla)
          .Replace("{columnas}", $"[{string.Join("], [", Columnas)}]");
        //Agregar las condiciones
        consulta.Append(@"[Eliminado] is ").Append(paginado.Eliminados ? @"not NULL" : @"NULL");
        //Agregar filtro de rango de fecha
        if (!paginado.RangoFechaInicio.NoEsValida())
          consulta.Append(@" AND [FechaModificado]>='@RangoFechaInicio'");
        //Agregar filtro de rango de fecha
        if (!paginado.RangoFechaFin.NoEsValida())
          consulta.Append(@" AND [FechaModificado]<='@RangoFechaFin'");
        //Agregar filtro de busqueda
        if (!paginado.Busqueda.NoEsValida() && !ColumnasBusqueda.NoEsValida())
          consulta.Append($" AND ([{string.Join("] LIKE '%@Busqueda%' OR [", ColumnasBusqueda)}] LIKE '%@Busqueda%')");
        //Contar el total de elementos con la condición dada
        int total;
        using (SqlConnection conexion = new SqlConnection(Repositorio.CadenaDeConexion))
        {
          try
          {
            conexion.Open();
            using (SqlCommand comando = new SqlCommand($@"SELECT COUNT(*) FROM [dbo].[{Tabla}] WHERE {consulta};", conexion))
            {
              try
              {
                total = Convert.ToInt32(comando.ExecuteScalar());
              }
              catch (Exception)
              {
                total = 0;
              }
              comando.Dispose();
            }
            if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
          }
          catch (Exception)
          {
            total = 0;
          }
          conexion.Dispose();
        }
        //Actualizar estado del indice de pagína
        paginado.CalcularPaginado(total);
        //Verificar que al menos contenga 1 elemento
        if (total <= 0)
          respuesta = new RespuestaColeccion<T>(paginado, new List<T>(0));
        else
        {
          consulta.Append($@" ORDER BY [Id] OFFSET {paginado.PaginaIndice * paginado.Elementos} ROWS FETCH NEXT {paginado.Elementos} ROWS ONLY");
          sql = sql.Replace(@"{condicion}", consulta.ToString());
          consulta.Clear();
          //Agregar parámetros
          object[] parametros = new object[3];
          if (!paginado.RangoFechaInicio.NoEsValida())
            parametros[0] = new SqlParameter(@"@RangoFechaInicio", paginado.RangoFechaInicio.ToString("s"));
          if (!paginado.RangoFechaFin.NoEsValida())
            parametros[1] = new SqlParameter(@"@RangoFechaFin", paginado.RangoFechaFin.ToString("s"));
          if (!paginado.Busqueda.NoEsValida())
            parametros[2] = new SqlParameter(@"@Busqueda", paginado.Busqueda);
          //Si hay al menos un parámetros, se agregan a la consulta.
          using (SqlConnection conexion = new SqlConnection(Repositorio.CadenaDeConexion))
          {
            try
            {
              conexion.Open();
              using (SqlCommand comando = new SqlCommand(sql, conexion))
              {
                try
                {
                  if (parametros.Any(p => p != null)) comando.Parameters.AddRange(parametros.Where(p => p != null).ToArray());
                  using (SqlDataReader resultado = comando.ExecuteReader())
                  {
                    try
                    {
                      //Obtener la pagína solo con las columnas seleccionadas
                      List<T> lista = new List<T>();
                      while (resultado.Read())
                      {
                        T e = new T();
                        foreach (string columna in Columnas) Tipo.GetProperty(columna)?.SetValue(e, resultado[columna]);
                        lista.Add(e);
                      }
                      resultado.Close();
                      respuesta = new RespuestaColeccion<T>(paginado, lista);
                    }
                    catch (Exception e)
                    {
                      respuesta = new RespuestaColeccion<T>(e);
                    }
                    resultado.Dispose();
                  }
                }
                catch (Exception ex)
                {
                  respuesta = new RespuestaColeccion<T>(ex);
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
        }
      }
      catch (Exception ex)
      {
        respuesta = new RespuestaColeccion<T>(ex);
      }
      return respuesta;
    }

    /// <summary>
    /// Devuelve una lista de clave/valor asociadas a la entidad
    /// </summary>
    /// <param name="columna">Nombre de la columna para seleccionar</param>
    /// <returns>Lista de clave/valor asociados a la Entidad</returns>
    public virtual RespuestaColeccion<ClaveValor> Obtener(string columna)
    {
      if (columna.NoEsValida()) return new RespuestaColeccion<ClaveValor> { Correcto = false, Mensaje = Error.IdentificadorInvalido };
      if (!Columnas.Contains(columna)) return new RespuestaColeccion<ClaveValor> { Correcto = false, Mensaje = @"La columna proporcionada no se encuentra en la entidad." };
      RespuestaColeccion<ClaveValor> respuesta;
      string sql = SqlSeleccionar
        .Replace("{tabla}", Tabla)
        .Replace("{columnas}", $"[Id] as Clave, [{columna}] as Valor")
        .Replace("{condicion}", "[Eliminado] IS NULL");
      using (SqlConnection conexion = new SqlConnection(Repositorio.CadenaDeConexion))
      {
        try
        {
          conexion.Open();
          using (SqlCommand comando = new SqlCommand(sql, conexion))
          {
            try
            {
              using (SqlDataReader resultado = comando.ExecuteReader())
              {
                try
                {
                  //Obtener la pagína solo con las columnas seleccionadas
                  List<ClaveValor> lista = new List<ClaveValor>();
                  while (resultado.Read())
                  {
                    lista.Add(new ClaveValor
                    {
                      Clave = resultado.GetInt32(0),
                      Valor = resultado.GetString(1)
                    });
                  }
                  resultado.Close();
                  respuesta = new RespuestaColeccion<ClaveValor>(lista);
                }
                catch (Exception e)
                {
                  respuesta = new RespuestaColeccion<ClaveValor>(e);
                }
                resultado.Dispose();
              }
            }
            catch (Exception e)
            {
              respuesta = new RespuestaColeccion<ClaveValor>(e);
            }
            comando.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception e)
        {
          respuesta = new RespuestaColeccion<ClaveValor>(e);
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
    /// <returns>Lista de entidades asociadas al servicio</returns>
    public virtual RespuestaColeccion<T> Obtener(string[] columnas, Paginado paginado)
    {
      if (columnas.NoEsValida() || paginado == null) return new RespuestaColeccion<T> { Correcto = false, Mensaje = Error.IdentificadorInvalido };
      if (!columnas.ToList().TrueForAll(Columnas.Contains)) return new RespuestaColeccion<T> { Correcto = false, Mensaje = @"La columna proporcionada no se encuentra en la entidad." };
      RespuestaColeccion<T> respuesta;
      using (SqlConnection conexion = new SqlConnection(Repositorio.CadenaDeConexion))
      {
        try
        {
          conexion.Open();
          using (SqlCommand comando = new SqlCommand(@"", conexion))
          {
            try
            {
              StringBuilder consulta = new StringBuilder();
              string sql = SqlSeleccionar
                .Replace("{tabla}", Tabla)
                .Replace("{columnas}", $"[{string.Join("], [", columnas)}]");
              //Agregar las condiciones
              consulta.Append(@"[Eliminado] is ").Append(paginado.Eliminados ? @"not NULL" : @"NULL");
              //Agregar filtro de rango de fecha
              if (!paginado.RangoFechaInicio.NoEsValida())
                consulta.Append(@" AND [FechaModificado]>='@RangoFechaInicio'");
              //Agregar filtro de rango de fecha
              if (!paginado.RangoFechaFin.NoEsValida())
                consulta.Append(@" AND [FechaModificado]<='@RangoFechaFin'");
              //Agregar filtro de busqueda
              if (!paginado.Busqueda.NoEsValida() && !ColumnasBusqueda.NoEsValida())
                consulta.Append($" AND ([{string.Join("] LIKE '%@Busqueda%' OR [", ColumnasBusqueda)}] LIKE '%@Busqueda%')");
              //Contar el total de elementos con la condición dada
              comando.CommandText = $@"SELECT COUNT(*) FROM [dbo].[{Tabla}] WHERE {consulta};";
              int total = Convert.ToInt32(comando.ExecuteScalar());
              //Actualizar estado del indice de pagína
              paginado.CalcularPaginado(total);
              //Verificar que al menos contenga 1 elemento
              if (total <= 0)
                respuesta = new RespuestaColeccion<T>(paginado, new List<T>(0));
              else
              {
                consulta.Append($@" ORDER BY [Id] OFFSET {paginado.PaginaIndice * paginado.Elementos} ROWS FETCH NEXT {paginado.Elementos} ROWS ONLY");
                sql = sql.Replace(@"{condicion}", consulta.ToString());
                consulta.Clear();
                //Agregar parámetros
                object[] parametros = new object[3];
                if (!paginado.RangoFechaInicio.NoEsValida())
                  parametros[0] = new SqlParameter(@"@RangoFechaInicio", paginado.RangoFechaInicio.ToString("s"));
                if (!paginado.RangoFechaFin.NoEsValida())
                  parametros[1] = new SqlParameter(@"@RangoFechaFin", paginado.RangoFechaFin.ToString("s"));
                if (!paginado.Busqueda.NoEsValida())
                  parametros[2] = new SqlParameter(@"@Busqueda", paginado.Busqueda);
                //Si hay al menos un parámetros, se agregan a la consulta.
                if (parametros.Any(p => p != null)) comando.Parameters.AddRange(parametros.Where(p => p != null).ToArray());
                comando.CommandText = sql;
                using (SqlDataReader resultado = comando.ExecuteReader())
                {
                  try
                  {
                    //Obtener la pagína solo con las columnas seleccionadas
                    List<T> lista = new List<T>();
                    while (resultado.Read())
                    {
                      T e = new T();
                      foreach (string columna in columnas) Tipo.GetProperty(columna)?.SetValue(e, resultado[columna]);
                      lista.Add(e);
                    }
                    resultado.Close();
                    respuesta = new RespuestaColeccion<T>(lista);
                  }
                  catch (Exception e)
                  {
                    respuesta = new RespuestaColeccion<T>(e);
                  }
                  resultado.Dispose();
                }
              }
            }
            catch (Exception e)
            {
              respuesta = new RespuestaColeccion<T>(e);
            }
            comando.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception e)
        {
          respuesta = new RespuestaColeccion<T>(e);
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
    public virtual RespuestaBasica Guardar(T modelo)
    {
      if (modelo == null) return new RespuestaBasica(false, Error.ModeloInvalido);
      RespuestaBasica respuesta;
      try
      {
        DateTime ahora = DateTime.Now;
        modelo.Modificado = ahora;
        //Agregar como nuevo
        if (modelo.Id.Equals(0))
        {
          modelo.Creado = ahora;
          Repositorio.Set<T>().Add(modelo);
          Repositorio.Entry(modelo).State = EntityState.Added;
        }
        //Agregar como modificado
        else if (modelo.Id > 0)
        {
          modelo.Creado = Repositorio.Set<T>()
           .AsNoTracking()
           .Where(e => e.Id.Equals(modelo.Id))
           .Select(e => e.Creado)
           .FirstOrDefault();
          Repositorio.Set<T>().Attach(modelo);
          Repositorio.Entry(modelo).State = EntityState.Modified;
        }
        respuesta = new RespuestaBasica(Repositorio.SaveChanges() >= 1);
      }
      catch (Exception ex)
      {
        respuesta = new RespuestaBasica(ex);
      }
      return respuesta;
    }

    /// <summary>
    /// Guarda cambios efectuados a una serie de modelos de entidad
    /// </summary>
    /// <param name="entidades">Lista de Entidades</param>
    /// <returns>Verdadero o Falso</returns>
    public virtual RespuestaColeccion<int> Guardar(List<T> entidades)
    {
      if (entidades.NoEsValida()) return new RespuestaColeccion<int> { Correcto = false, Mensaje = Error.ListaInvalida };
      RespuestaColeccion<int> respuesta;
      try
      {
        DateTime ahora = DateTime.Now;
        //Iniciar transacción
        Repositorio.Database.BeginTransaction();
        //Elementos para agregar
        int totalAgregar = entidades.Select(m => m.Id).Count(id => id.Equals(0));
        bool agregar = totalAgregar > 0;
        if (agregar)
          Repositorio.Set<T>()
            .AddRange(
              entidades
                .Where(m => m.Id.Equals(0))
                .Select(m =>
                {
                  m.Creado = ahora;
                  m.Modificado = ahora;
                  return m;
                })
                .ToList()
            );
        //Elementos para actualizar
        int totalActualizar = entidades.Select(m => m.Id).Count(id => id > 0);
        bool actualizar = totalActualizar > 0;
        if (actualizar)
          entidades
            .Where(m => m.Id > 0)
            .Select(m =>
            {
              m.Modificado = ahora;
              return m;
            }).ToList().ForEach(m =>
            {
              m.Creado = Repositorio.Set<T>()
                .AsNoTracking()
                .Where(t => t.Id.Equals(m.Id))
                .Select(t => t.Creado)
                .FirstOrDefault();
              Repositorio.Set<T>().Attach(m);
              Repositorio.Entry(m).State = EntityState.Modified;
            });
        //El total de elementos debe ser la suma de los agregados y actualizados
        bool guardados = Repositorio.SaveChanges() >= totalAgregar + totalActualizar;
        List<int> ids = entidades.Where(e => e.Id > 0)
          .Select(e => e.Id)
          .ToList();
        entidades.Clear();
        respuesta = new RespuestaColeccion<int>(ids) { Correcto = guardados && !ids.NoEsValida() && ids.All(id => id > 0) };
        if (respuesta.Correcto)
          Repositorio.Database.CurrentTransaction.Commit();
        else
          Repositorio.Database.CurrentTransaction.Rollback();
      }
      catch (Exception ex)
      {
        respuesta = new RespuestaColeccion<int>(ex);
        Repositorio.Database.CurrentTransaction.Rollback();
      }
      return respuesta;
    }

    /// <summary>
    /// Inserta una Entidad de manera óptima
    /// </summary>
    /// <param name="modelo">Entidad</param>
    /// <returns>Identificador primario insertado</returns>
    public virtual RespuestaModelo<int> Insertar(T modelo)
    {
      if (modelo == null || !modelo.Id.Equals(0)) return new RespuestaModelo<int> { Correcto = false, Mensaje = Error.ModeloInvalido };
      RespuestaModelo<int> respuesta;
      using (SqlConnection conexion = new SqlConnection(Repositorio.CadenaDeConexion))
      {
        try
        {
          conexion.Open();
          using (SqlTransaction transaccion = conexion.BeginTransaction())
          {
            try
            {
              using (SqlCommand comando = new SqlCommand(@"", transaccion.Connection, transaccion))
              {
                try
                {
                  comando.CommandText = CrearSqlInsertar(out List<SqlParameter> parametros);
                  parametros.ForEach(p =>
                  {
                    p.Value = Tipo.GetProperty(p.ParameterName.TrimStart('@'))?.GetValue(modelo);
                    comando.Parameters.Add(p);
                  });
                  //Id de la entidad insertada
                  int id = Convert.ToInt32(comando.ExecuteScalar());
                  respuesta = new RespuestaModelo<int>(id) { Correcto = id > 0 };
                }
                catch (Exception ex)
                {
                  respuesta = new RespuestaModelo<int>(ex);
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
              respuesta = new RespuestaModelo<int>(ex);
              transaccion.Rollback();
            }
            transaccion.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<int>(ex);
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
    public virtual RespuestaModelo<int> Insertar(T modelo, SqlTransaction transaccion)
    {
      if (modelo == null || !modelo.Id.Equals(0)) return new RespuestaModelo<int> { Correcto = false, Mensaje = Error.ModeloInvalido };
      RespuestaModelo<int> respuesta;
      using (SqlCommand comando = new SqlCommand(@"", transaccion.Connection, transaccion))
      {
        try
        {
          comando.CommandText = CrearSqlInsertar(out List<SqlParameter> parametros);
          parametros.ForEach(p =>
          {
            p.Value = Tipo.GetProperty(p.ParameterName.TrimStart('@'))?.GetValue(modelo);
            comando.Parameters.Add(p);
          });
          //Id de la entidad insertada
          int id = Convert.ToInt32(comando.ExecuteScalar());
          parametros.Clear();
          parametros.TrimExcess();
          comando.Parameters.Clear();
          respuesta = new RespuestaModelo<int>(id) { Correcto = id > 0 };
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<int>(ex);
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
    public virtual RespuestaColeccion<int> Insertar(List<T> entidades)
    {
      if (entidades.NoEsValida() || entidades.Any(m => !m.Id.Equals(0))) return new RespuestaColeccion<int> { Correcto = false, Mensaje = Error.ListaInvalida };
      RespuestaColeccion<int> respuesta;
      using (SqlConnection conexion = new SqlConnection(Repositorio.CadenaDeConexion))
      {
        try
        {
          conexion.Open();
          using (SqlTransaction transaccion = conexion.BeginTransaction())
          {
            try
            {
              List<int> ids = new List<int>(entidades.Count);
              using (SqlCommand comando = new SqlCommand(@"", transaccion.Connection, transaccion))
              {
                try
                {
                  comando.CommandText = CrearSqlInsertar(out List<SqlParameter> parametros);
                  comando.Parameters.AddRange(parametros.ToArray());
                  foreach (T e in entidades)
                  {
                    parametros.ForEach(p =>
                    {
                      p.Value = Tipo.GetProperty(p.ParameterName.TrimStart('@'))?.GetValue(e);
                      comando.Parameters[p.ParameterName].Value = p.Value;
                    });
                    int id = Convert.ToInt32(comando.ExecuteScalar());
                    if (id <= 0) break;
                    ids.Add(id);
                  }
                  parametros.Clear();
                  parametros.TrimExcess();
                  comando.Parameters.Clear();
                  bool correcto = !ids.NoEsValida() && ids.All(id => id > 0);
                  respuesta = new RespuestaColeccion<int>(ids)
                  {
                    Correcto = correcto,
                    Mensaje = correcto ? Correcto.SolicitudCompletada : Error.DiferenciaDeElementosAfectados
                  };
                }
                catch (Exception e)
                {
                  respuesta = new RespuestaColeccion<int>(e);
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
              respuesta = new RespuestaColeccion<int>(ex);
              transaccion.Rollback();
            }
            transaccion.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaColeccion<int>(ex);
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
    public virtual RespuestaColeccion<int> Insertar(List<T> entidades, SqlTransaction transaccion)
    {
      if (entidades.NoEsValida() || entidades.Any(m => !m.Id.Equals(0)) || transaccion.Connection.NoEsValida())
        return new RespuestaColeccion<int> { Correcto = false, Mensaje = Error.ListaInvalida };
      RespuestaColeccion<int> respuesta;
      List<int> ids = new List<int>(entidades.Count);
      using (SqlCommand comando = new SqlCommand(@"", transaccion.Connection, transaccion))
      {
        try
        {
          comando.CommandText = CrearSqlInsertar(out List<SqlParameter> parametros);
          comando.Parameters.AddRange(parametros.ToArray());
          foreach (T e in entidades)
          {
            parametros.ForEach(p =>
            {
              p.Value = Tipo.GetProperty(p.ParameterName.TrimStart('@'))?.GetValue(e);
              comando.Parameters[p.ParameterName].Value = p.Value;
            });
            int id = Convert.ToInt32(comando.ExecuteScalar());
            if (id <= 0) break;
            ids.Add(id);
          }
          parametros.Clear();
          parametros.TrimExcess();
          comando.Parameters.Clear();
          //Todos los ids insertados deben ser mayor a 0
          bool correcto = !ids.NoEsValida() && ids.All(id => id > 0);
          respuesta = new RespuestaColeccion<int>(ids)
          {
            Correcto = correcto,
            Mensaje = correcto ? Correcto.SolicitudCompletada : Error.DiferenciaDeElementosAfectados
          };
        }
        catch (Exception e)
        {
          respuesta = new RespuestaColeccion<int>(e);
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
    public virtual RespuestaModelo<int> Actualizar(T modelo)
    {
      if (modelo == null || modelo.Id.Equals(0)) return new RespuestaModelo<int> { Correcto = false, Mensaje = Error.ModeloInvalido };
      RespuestaModelo<int> respuesta;
      using (SqlConnection conexion = new SqlConnection(Repositorio.CadenaDeConexion))
      {
        try
        {
          conexion.Open();
          using (SqlTransaction transaccion = conexion.BeginTransaction())
          {
            try
            {
              using (SqlCommand comando = new SqlCommand(@"", transaccion.Connection, transaccion))
              {
                try
                {
                  comando.CommandText = CrearSqlActualizar(out List<SqlParameter> parametros);
                  parametros.ForEach(p =>
                  {
                    p.Value = Tipo.GetProperty(p.ParameterName.TrimStart('@'))?.GetValue(modelo);
                    comando.Parameters.Add(p);
                  });
                  //El número de afectados debe ser al menos 1
                  int afectados = comando.ExecuteNonQuery();
                  parametros.Clear();
                  parametros.TrimExcess();
                  comando.Parameters.Clear();
                  respuesta = new RespuestaModelo<int>(afectados) { Correcto = afectados > 0 };
                }
                catch (Exception ex)
                {
                  respuesta = new RespuestaModelo<int>(ex);
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
              respuesta = new RespuestaModelo<int>(ex);
              transaccion.Rollback();
            }
            transaccion.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<int>(ex);
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
    public virtual RespuestaModelo<int> Actualizar(T modelo, SqlTransaction transaccion)
    {
      if (modelo == null || modelo.Id.Equals(0)) return new RespuestaModelo<int> { Correcto = false, Mensaje = Error.ModeloInvalido };
      RespuestaModelo<int> respuesta;
      using (SqlCommand comando = new SqlCommand(@"", transaccion.Connection, transaccion))
      {
        try
        {
          comando.CommandText = CrearSqlActualizar(out List<SqlParameter> parametros);
          parametros.ForEach(p =>
          {
            p.Value = Tipo.GetProperty(p.ParameterName.TrimStart('@'))?.GetValue(modelo);
            comando.Parameters.Add(p);
          });
          //El número de afectados debe ser al menos 1
          int afectados = comando.ExecuteNonQuery();
          parametros.Clear();
          parametros.TrimExcess();
          comando.Parameters.Clear();
          respuesta = new RespuestaModelo<int>(afectados) { Correcto = afectados > 0 };
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<int>(ex);
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
    public virtual RespuestaModelo<int> Actualizar(List<T> entidades)
    {
      if (entidades.NoEsValida() || entidades.Any(m => m.Id.Equals(0))) return new RespuestaModelo<int> { Correcto = false, Mensaje = Error.ListaInvalida };
      RespuestaModelo<int> respuesta;
      using (SqlConnection conexion = new SqlConnection(Repositorio.CadenaDeConexion))
      {
        try
        {
          conexion.Open();
          using (SqlTransaction transaccion = conexion.BeginTransaction())
          {
            try
            {
              using (SqlCommand comando = new SqlCommand(@"", transaccion.Connection, transaccion))
              {
                try
                {
                  int afectadas = 0;
                  comando.CommandText = CrearSqlActualizar(out List<SqlParameter> parametros);
                  comando.Parameters.AddRange(parametros.ToArray());
                  foreach (T e in entidades)
                  {
                    parametros.ForEach(p =>
                    {
                      p.Value = Tipo.GetProperty(p.ParameterName.TrimStart('@'))?.GetValue(e);
                      comando.Parameters[p.ParameterName].Value = p.Value;
                    });
                    afectadas += comando.ExecuteNonQuery();
                  }
                  parametros.Clear();
                  parametros.TrimExcess();
                  comando.Parameters.Clear();
                  //La cantidad de filas afectadas debe ser igual a la cantidad total de elementos contenidos en la lista de entidades
                  respuesta = new RespuestaModelo<int>(afectadas) { Correcto = afectadas.Equals(entidades.Count) };
                }
                catch (Exception ex)
                {
                  respuesta = new RespuestaModelo<int>(ex);
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
              respuesta = new RespuestaModelo<int>(ex);
              transaccion.Rollback();
            }
            transaccion.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<int>(ex);
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
    public virtual RespuestaModelo<int> Actualizar(List<T> entidades, SqlTransaction transaccion)
    {
      if (entidades.NoEsValida() || entidades.Any(m => m.Id.Equals(0)) || transaccion.Connection.NoEsValida())
        return new RespuestaModelo<int> { Correcto = false, Mensaje = Error.ListaInvalida };
      RespuestaModelo<int> respuesta;
      using (SqlCommand comando = new SqlCommand(@"", transaccion.Connection, transaccion))
      {
        try
        {
          int afectadas = 0;
          comando.CommandText = CrearSqlActualizar(out List<SqlParameter> parametros);
          comando.Parameters.AddRange(parametros.ToArray());
          foreach (T e in entidades)
          {
            parametros.ForEach(p =>
            {
              p.Value = Tipo.GetProperty(p.ParameterName.TrimStart('@'))?.GetValue(e);
              comando.Parameters[p.ParameterName].Value = p.Value;
            });
            afectadas += comando.ExecuteNonQuery();
          }
          //La cantidad de filas afectadas debe ser igual a la cantidad total de elementos contenidos en la lista de entidades
          respuesta = new RespuestaModelo<int>(afectadas) { Correcto = afectadas.Equals(entidades.Count) };
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<int>(ex);
        }
        comando.Dispose();
      }
      return respuesta;
    }

    #endregion
  }
}
