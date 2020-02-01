using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Datos.Extensiones;
using Datos.Modelos;
using Negocio.Modelos;
using Newtonsoft.Json;

namespace Negocio.Utilidades
{
  /// <summary>
  /// Provee el mecanismo para efectuar solicitudes http
  /// </summary>
  public class SolicitudHttp
  {
    private List<EncabezadoHttp> Encabezados { get; }

    public SolicitudHttp(List<EncabezadoHttp> encabezados = null)
    {
      Encabezados = new List<EncabezadoHttp>();
    }

    #region Solicitudes tipo POST

    /// <summary>
    /// Permite realizar una solicitud POST utilizando como parametros
    /// un objeto serializado en notacion JSON
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Post(string urlBase, string metodo, string parametros)
    {
      if (urlBase.NoEsValida() || metodo.NoEsValida() || parametros.NoEsValida())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage respuesta;
      try
      {
        using (HttpClient cliente = new HttpClient())
        {
          cliente.BaseAddress = new Uri(urlBase);
          if (Encabezados != null)
          {
            cliente.DefaultRequestHeaders.Clear();
            Encabezados.ForEach(h => cliente.DefaultRequestHeaders.Add(h.Nombre, h.Valor));
          }
          using (HttpContent contenido = new StringContent(parametros, Encoding.UTF8, @"application/json"))
          {
            respuesta = await cliente.PostAsync(metodo, contenido);
            contenido.Dispose();
          }
          cliente.Dispose();
        }
      }
      catch (Exception ex)
      {
        respuesta = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return respuesta;
    }

    /// <summary>
    /// Permite realizar una solicitud POST utilizando como parametros
    /// un arreglo de bytes
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Post(string urlBase, string metodo, byte[] parametros)
    {
      if (urlBase.NoEsValida() || metodo.NoEsValida() || parametros.NoEsValido())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage respuesta;
      try
      {
        using (HttpClient cliente = new HttpClient())
        {
          cliente.BaseAddress = new Uri(urlBase);
          if (Encabezados != null)
          {
            cliente.DefaultRequestHeaders.Clear();
            Encabezados.ForEach(h => cliente.DefaultRequestHeaders.Add(h.Nombre, h.Valor));
          }
          using (HttpContent contenido = new ByteArrayContent(parametros))
          {
            respuesta = await cliente.PostAsync(metodo, contenido);
            contenido.Dispose();
          }
          cliente.Dispose();
        }
      }
      catch (Exception ex)
      {
        respuesta = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return respuesta;
    }

    /// <summary>
    /// Permite realizar una solicitud POST utilizando como parametros
    /// un diccionario de claves
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Post(string urlBase, string metodo, Dictionary<string, string> parametros)
    {
      if (urlBase.NoEsValida() || metodo.NoEsValida())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage respuesta;
      try
      {
        using (HttpClient cliente = new HttpClient())
        {
          cliente.BaseAddress = new Uri(urlBase);
          if (Encabezados != null)
          {
            cliente.DefaultRequestHeaders.Clear();
            Encabezados.ForEach(h => cliente.DefaultRequestHeaders.Add(h.Nombre, h.Valor));
          }
          using (HttpContent contenido = new FormUrlEncodedContent(parametros))
          {
            respuesta = await cliente.PostAsync(metodo, contenido);
            contenido.Dispose();
          }
          cliente.Dispose();
        }
      }
      catch (Exception ex)
      {
        respuesta = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return respuesta;
    }

    /// <summary>
    /// Permite realizar una solicitud POST utilizando como parametros
    /// un flujo de datos
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Post(string urlBase, string metodo, Stream parametros)
    {

      if (urlBase.NoEsValida() || metodo.NoEsValida())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage respuesta;
      try
      {
        using (HttpClient cliente = new HttpClient())
        {
          cliente.BaseAddress = new Uri(urlBase);
          if (Encabezados != null)
          {
            cliente.DefaultRequestHeaders.Clear();
            Encabezados.ForEach(h => cliente.DefaultRequestHeaders.Add(h.Nombre, h.Valor));
          }
          using (HttpContent contenido = new StreamContent(parametros))
          {
            respuesta = await cliente.PostAsync(metodo, contenido);
            contenido.Dispose();
          }
          cliente.Dispose();
        }
      }
      catch (Exception ex)
      {
        respuesta = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return respuesta;
    }

    #endregion

    #region Solicitudes Tipo GET

    /// <summary>
    /// Permite realizar una solicitud GET utilizando como parametros
    /// una cadena alfanumerica codificada como url
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Get(string urlBase, string metodo, string parametros)
    {
      if (urlBase.NoEsValida() || metodo.NoEsValida() || parametros.NoEsValida())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage respuesta;
      try
      {
        using (HttpClient cliente = new HttpClient())
        {
          cliente.BaseAddress = new Uri(urlBase);
          if (Encabezados != null)
          {
            cliente.DefaultRequestHeaders.Clear();
            Encabezados.ForEach(h => cliente.DefaultRequestHeaders.Add(h.Nombre, h.Valor));
          }
          parametros = parametros.StartsWith(@"?") ? parametros : @"?" + parametros;
          respuesta = await cliente.GetAsync(metodo + parametros);
          cliente.Dispose();
        }
      }
      catch (Exception ex)
      {
        respuesta = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return respuesta;
    }

    /// <summary>
    /// Permite realizar una solicitud GET utilizando como parametros
    /// un modelo de clave/valor
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Get(string urlBase, string metodo, KeyValuePair<string, string> parametros)
      => await Get(urlBase, metodo, $@"{parametros.Key}={parametros.Value}");

    /// <summary>
    /// Permite realizar una solicitud GET utilizando como parametros
    /// un diccionario asociativo de claves y valores
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Get(string urlBase, string metodo, Dictionary<string, dynamic> parametros)
      => await Get(urlBase, metodo, string.Join(@"&", parametros.Select(p => $@"{p.Key}={p.Value}")));

    /// <summary>
    /// Permite realizar una solicitud GET utilizando como parametros
    /// una lista de modelos de clave/valor
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Get(string urlBase, string metodo, List<KeyValuePair<string, string>> parametros)
      => await Get(urlBase, metodo, string.Join(@"&", parametros.Select(p => $@"{p.Key}={p.Value}")));

    #endregion

    #region Solicitudes Tipo PUT

    /// <summary>
    /// Permite realizar una solicitud PUT utilizando como parametros
    /// un objeto serializado en notacion JSON
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Put(string urlBase, string metodo, string parametros)
    {
      if (urlBase.NoEsValida() || metodo.NoEsValida() || parametros.NoEsValida())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage respuesta;
      try
      {
        using (HttpClient cliente = new HttpClient())
        {
          cliente.BaseAddress = new Uri(urlBase);
          if (Encabezados != null)
          {
            cliente.DefaultRequestHeaders.Clear();
            Encabezados.ForEach(h => cliente.DefaultRequestHeaders.Add(h.Nombre, h.Valor));
          }
          using (HttpContent contenido = new StringContent(parametros, Encoding.UTF8, @"application/json"))
          {
            respuesta = await cliente.PutAsync(metodo, contenido);
            contenido.Dispose();
          }
          cliente.Dispose();
        }
      }
      catch (Exception ex)
      {
        respuesta = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return respuesta;
    }

    /// <summary>
    /// Permite realizar una solicitud PUT utilizando como parametros
    /// un arreglo de bytes
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Put(string urlBase, string metodo, byte[] parametros)
    {
      if (urlBase.NoEsValida() || metodo.NoEsValida() || parametros.NoEsValido())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage respuesta;
      try
      {
        using (HttpClient cliente = new HttpClient())
        {
          cliente.BaseAddress = new Uri(urlBase);
          if (Encabezados != null)
          {
            cliente.DefaultRequestHeaders.Clear();
            Encabezados.ForEach(h => cliente.DefaultRequestHeaders.Add(h.Nombre, h.Valor));
          }
          using (HttpContent contenido = new ByteArrayContent(parametros))
          {
            respuesta = await cliente.PutAsync(metodo, contenido);
            contenido.Dispose();
          }
          cliente.Dispose();
        }
      }
      catch (Exception ex)
      {
        respuesta = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return respuesta;
    }

    /// <summary>
    /// Permite realizar una solicitud PUT utilizando como parametros
    /// un diccionario de claves
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Put(string urlBase, string metodo, Dictionary<string, string> parametros)
    {
      if (urlBase.NoEsValida() || metodo.NoEsValida())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage respuesta;
      try
      {
        using (HttpClient cliente = new HttpClient())
        {
          cliente.BaseAddress = new Uri(urlBase);
          if (Encabezados != null)
          {
            cliente.DefaultRequestHeaders.Clear();
            Encabezados.ForEach(h => cliente.DefaultRequestHeaders.Add(h.Nombre, h.Valor));
          }
          using (HttpContent contenido = new FormUrlEncodedContent(parametros))
          {
            respuesta = await cliente.PutAsync(metodo, contenido);
            contenido.Dispose();
          }
          cliente.Dispose();
        }
      }
      catch (Exception ex)
      {
        respuesta = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return respuesta;
    }

    /// <summary>
    /// Permite realizar una solicitud PUT utilizando como parametros
    /// un flujo de datos
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Put(string urlBase, string metodo, Stream parametros)
    {
      if (urlBase.NoEsValida() || metodo.NoEsValida())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage respuesta;
      try
      {
        using (HttpClient cliente = new HttpClient())
        {
          cliente.BaseAddress = new Uri(urlBase);
          if (Encabezados != null)
          {
            cliente.DefaultRequestHeaders.Clear();
            Encabezados.ForEach(h => cliente.DefaultRequestHeaders.Add(h.Nombre, h.Valor));
          }
          using (HttpContent contenido = new StreamContent(parametros))
          {
            respuesta = await cliente.PutAsync(metodo, contenido);
            contenido.Dispose();
          }
          cliente.Dispose();
        }
      }
      catch (Exception ex)
      {
        respuesta = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return respuesta;
    }

    #endregion

    #region Solicitudes Tipo Delete

    /// <summary>
    /// Permite realizar una solicitud DELETE utilizando como parametros
    /// una cadena alfanumerica codificada como url
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Delete(string urlBase, string metodo, string parametros)
    {
      if (urlBase.NoEsValida() || metodo.NoEsValida() || parametros.NoEsValida())
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      HttpResponseMessage respuesta;
      try
      {
        using (HttpClient cliente = new HttpClient())
        {
          cliente.BaseAddress = new Uri(urlBase);
          if (Encabezados != null)
          {
            cliente.DefaultRequestHeaders.Clear();
            Encabezados.ForEach(h => cliente.DefaultRequestHeaders.Add(h.Nombre, h.Valor));
          }
          parametros = parametros.StartsWith(@"?") ? parametros : @"?" + parametros;
          respuesta = await cliente.DeleteAsync(metodo + parametros);
          cliente.Dispose();
        }
      }
      catch (Exception ex)
      {
        respuesta = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
          Content = new StringContent(JsonConvert.SerializeObject(ex))
        };
      }
      return respuesta;
    }

    /// <summary>
    /// Permite realizar una solicitud DELETE utilizando como parametros
    /// un modelo de clave/valor
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Delete(string urlBase, string metodo, KeyValuePair<string, string> parametros)
      => await Delete(urlBase, metodo, $@"{parametros.Key}={parametros.Value}");

    /// <summary>
    /// Permite realizar una solicitud DELETE utilizando como parametros
    /// un diccionario asociativo de claves y valores
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Delete(string urlBase, string metodo, Dictionary<string, dynamic> parametros)
      => await Delete(urlBase, metodo, string.Join(@"&", parametros.Select(p => $@"{p.Key}={p.Value}")));

    /// <summary>
    /// Permite realizar una solicitud DELETE utilizando como parametros
    /// una lista de modelos de clave/valor
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="parametros">Objeto a incluir en la solicitud</param>
        /// <returns>Mensaje de la solicitud</returns>
    public async Task<HttpResponseMessage> Delete(string urlBase, string metodo, List<KeyValuePair<string, string>> parametros)
      => await Delete(urlBase, metodo, string.Join(@"&", parametros.Select(p => $@"{p.Key}={p.Value}")));

    #endregion

    #region Solicitudes Tipo Descarga

    /// <summary>
    /// Permite realizar una solicitud http para descargar
    /// el recurso identificado
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <returns>Modelo de datos en bytes</returns>
    public async Task<RespuestaModelo<byte[]>> Descargar(string urlBase, string metodo)
    {
      if (urlBase.NoEsValida() || metodo.NoEsValida())
        return new RespuestaModelo<byte[]>() { Correcto = false, Mensaje = @"Los datos para realizar la solicitud no son validos." };
      RespuestaModelo<byte[]> respuesta;
      try
      {
        using (WebClient cliente = new WebClient())
        {
          Uri url = new Uri(urlBase + metodo);
          respuesta = new RespuestaModelo<byte[]>(await cliente.DownloadDataTaskAsync(url));
          cliente.Dispose();
        }
      }
      catch (Exception ex)
      {
        respuesta = new RespuestaModelo<byte[]>(ex);
      }
      return respuesta;
    }

    /// <summary>
    /// Permite realizar una solicitud http para descargar
    /// el recurso identificado y guardarlo en el directorio especificado
    /// </summary>
    /// <param name="urlBase">Direccion principal del recurso</param>
    /// <param name="metodo">Direccion del metodo para acceder al recurso</param>
    /// <param name="directorio">Direccion destino del archivo</param>
    /// <returns>Respuesta basica que indica el estado de la tarea</returns>
    public async Task<RespuestaBasica> DescargarEnDirectorio(string urlBase, string metodo, string directorio)
    {
      if (urlBase.NoEsValida() || metodo.NoEsValida())
        return new RespuestaModelo<byte[]>() { Correcto = false, Mensaje = @"Los datos para realizar la solicitud no son validos." };
      RespuestaBasica respuesta;
      try
      {
        using (WebClient cliente = new WebClient())
        {
          Uri url = new Uri(urlBase + metodo);
          await cliente.DownloadFileTaskAsync(url, directorio);
          respuesta = new RespuestaBasica(new FileInfo(directorio).Exists);
          cliente.Dispose();
        }
      }
      catch (Exception ex)
      {
        respuesta = new RespuestaBasica(ex);
      }
      return respuesta;
    }

    #endregion
  }
}
