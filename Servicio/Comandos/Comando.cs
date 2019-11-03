using System;
using System.Data;
using System.Data.SqlClient;
using Servicio.Extensiones;
using Servicio.Modelos;
using Servicio.Procedimientos;

namespace Servicio.Comandos
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
    /// Ejecuta un comando sql de tipo texto plano
    /// </summary>
    /// <param name="sql">Texto plano en formato sql para ejecutar</param>
    /// <param name="parametros">Parametros para agregar al comando</param>
    /// <returns>Objeto devuelto tras ejecutar la consulta</returns>
    public RespuestaModelo<object> Consulta(string sql, SqlParameter[] parametros = null)
    {
      if (sql.NoEsValida())
        return new RespuestaModelo<object>() { Correcto = false, Mensaje = @"la consulta no es valida." };
      RespuestaModelo<object> respuesta;
      using (SqlConnection conexion = new SqlConnection(CadenaDeConexion))
      {
        try
        {
          conexion.Open();
          using (SqlCommand comando = new SqlCommand(sql, conexion))
          {
            comando.CommandType = CommandType.Text;
            if (parametros != null && parametros.Length > 0)
              comando.Parameters.AddRange(parametros);
            try
            {
              respuesta = new RespuestaModelo<object>(comando.ExecuteScalar());
            }
            catch (Exception ex)
            {
              respuesta = new RespuestaModelo<object>(ex);
            }
            comando.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<object>(ex);
        }
        conexion.Dispose();
      }
      return respuesta;
    }

    /// <summary>
    /// Ejecuta un procedimiento almacenado
    /// </summary>
    /// <param name="procedimiento">Nombre del procedimiento</param>
    /// <param name="parametros">Parametros para agregar al comando</param>
    /// <returns>Objeto devuelto tras ejecutar la consulta</returns>
    public RespuestaModelo<object> Procedimiento(string procedimiento, SqlParameter[] parametros = null)
    {
      RespuestaModelo<object> respuesta;
      using (SqlConnection conexion = new SqlConnection(CadenaDeConexion))
      {
        try
        {
          conexion.Open();
          using (SqlCommand comando = new SqlCommand(procedimiento, conexion))
          {
            comando.CommandType = CommandType.StoredProcedure;
            if (parametros != null && parametros.Length > 0)
              comando.Parameters.AddRange(parametros);
            try
            {
              respuesta = new RespuestaModelo<object>(comando.ExecuteScalar());
            }
            catch (Exception ex)
            {
              respuesta = new RespuestaModelo<object>(ex);
            }
            comando.Dispose();
          }
          if (conexion.State.Equals(ConnectionState.Open)) conexion.Close();
        }
        catch (Exception ex)
        {
          respuesta = new RespuestaModelo<object>(ex);
        }
        conexion.Dispose();
      }
      return respuesta;
    }
  }
}
