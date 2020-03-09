var SolicitudHttp = function (encabezados) {
  
  /**
   * @description Efectua una solicitud Http via Fetch de tipo POST
   * @param {any} urlBase Url base del recurso
   * @param {any} metodo Metodo para acceder al recurso
   * @param {any} parametros Parametros que se deben incluir en la solicitud codificados como Json
   * @return {any} Promesa de solicitud
   */
  this.Post = function (urlBase, metodo, parametros) {
    //Si los parametros no son validos se omite la solicitud.
    if (NoEsValido(urlBase) || NoEsValido(metodo) || NoEsValido(parametros)) {
      return undefined;
    }
    return fetch(urlBase + metodo,
      {
        method: "POST",
        body: JSON.stringify(parametros),
        headers: {
          "Content-Type": "application/json"
        }
      });
  };

  /**
   * @description Efectua una solicitud Http via Fetch de tipo GET
   * @param {any} urlBase Url base del recurso
   * @param {any} metodo Metodo para acceder al recurso
   * @param {any} parametros Parametros que se deben incluir en la solicitud codificados como Url
   * @return {any} Promesa de solicitud
   */
  this.Get = function (urlBase, metodo, parametros) {
    //Si los parametros no son validos se omite la solicitud
    if (NoEsValido(urlBase) || NoEsValido(metodo)) {
      return undefined;
    }
    if (!NoEsValido(parametros)) {
      parametros = parametros.substr(0, 1) === "?" ? parametros : "?" + parametros;
    }
    return fetch(urlBase + metodo + parametros,
      {
        method: "GET"
      });
  };

  /**
   * @description Efectua una solicitud Http via Fetch de tipo DELETE
   * @param {any} urlBase Url base del recurso
   * @param {any} metodo Metodo para acceder al recurso
   * @param {any} parametros Parametros que se deben incluir en la solicitud codificados como Url
   * @return {any} Promesa de solicitud
   */
  this.Delete = function (urlBase, metodo, parametros) {
    //Si los parametros no son validos se omite la solicitud
    if (NoEsValido(urlBase) || NoEsValido(metodo)) {
      return undefined;
    }
    if (!NoEsValido(parametros)) {
      parametros = parametros.substr(0, 1) === "?" ? parametros : "?" + parametros;
    }
    return fetch(urlBase + metodo + parametros,
      {
        method: "delete"
      });
  };

};