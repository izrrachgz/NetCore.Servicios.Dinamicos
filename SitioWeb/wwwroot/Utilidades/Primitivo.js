//Dentro de esta definicion solo deberan ir aquellas funciones
//que tratan la informacion como datos sin contexto de negocio
//Ej: Ordenar un arreglo de manera ascendente, descendente, revertirlo.

//#region Funciones de Verificacion

/**
 * @description Verifica que el objeto dado es invalido para su uso
 * @param {any} o Objeto a evaluar
 * @return {boolean} Verdadero o falso
 */
function NoEsValido(o) {
  //Si no esta definido es invalido para su uso
  if (typeof o === "undefined") return true;
  //Si no es una cadena y esta vacia es invalido para su uso
  if (typeof o === "string" && o.trim().length === 0) return true;
  //Si es nulo es invalido para su uso
  if (o === null) return true;
  return false;
}

/**
 * @description Verifica si el objeto dado es un numero
 * @param {any} o Objeto a evaluar
 * @return {boolean} Verdadero o falso
 */
function EsNumero(o) {
  if (typeof o === "undefined") return false;
  if (o === null) return false;
  if (typeof o === "number") return true;
  return false;
}

/**
 * @description Verifica si el objeto dado es una cadena alfanumerica
 * @param {any} o Objeto a evaluar
 * @return {boolean} Verdadero o falso
 */
function EsCadena(o) {
  if (typeof o === "undefined") return false;
  if (o === null) return false;
  if (typeof o === "string") return true;
  return false;
}

/**
 * @description Verifica si el objeto dado es un valor booleano
 * @param {any} o Objeto a evaluar
 * @return {boolean} Verdadero o falso
 */
function EsBooleano(o) {
  if (typeof o === "undefined") return false;
  if (o === null) return false;
  if (typeof o === "boolean") return true;
  return false;
}

/**
 * @description Verifica si el objeto dado es un valor nulo
 * @param {any} o Objeto a evaluar
 * @return {boolean} Verdadero o falso
 */
function EsNulo(o) {
  if (typeof o === "undefined") return false;
  if (o === null) return true;
  return false;
}

//#endregion

//#region Funciones de Conversion

//#endregion

//#region Funciones de Ordenamiento

/**
 * @description Revierte el arreglo de valores
 * @param {any} arreglo Coleccion de valores
 * @return {Array<number>} Arreglo invertido
 */
function RevertirArreglo(arreglo) {
  //Si el arreglo no es valido para su uso se termina la tarea
  if (NoEsValido(arreglo) || arreglo.length === 0) {
    return [];
  }
  //Por si a alguien se hace el gracioso 😒
  if (arreglo.length === 1) {
    return arreglo;
  }
  //Establecer indice de arreglo envuelto
  var indices = [];
  for (var i = 0; i < arreglo.length; i++) {
    indices.push({
      "valor": arreglo[i]
    });
  }
  var total = indices.length - 1;
  //Navegar solo la mitad y revertir mediante intercambio
  for (var x = 0; x <= total / 2; x++) {
    var actual = indices[x].valor;
    arreglo[x] = indices[total - x].valor;
    arreglo[total - x] = actual;
  }
  //Limpiar indice utilizado
  indices = [];
  return arreglo;
}

/**
 * @description Revierte el listado de elementos 
 * @param {any} elementos Elementos html
 * @return {Array<Object<number>>} Coleccion invertida
 */
function RevertirElementos(elementos) {
  return RevertirArreglo(elementos);
}

//#endregion

//#region Funciones de Busqueda

/**
 * @description Realiza una busqueda dentro de una tabla y oculta las filas que no coinciden con la palabra buscada
 * @param {any} coleccion Elemento tabla
 * @param {any} busqueda Palabras de busqueda
 * @return {Array<Object<number>>} Coleccion de elementos que coinciden con la busqueda
 */
function EncontrarFilaEnColeccion(coleccion, busqueda) {
  //Si el arreglo no es valido para su uso se termina la tarea
  if (NoEsValido(coleccion) || NoEsValido(busqueda)) {
    return [];
  }
  var indices = [];
  busqueda = busqueda.toLowerCase();
  for (var x = 0; x < coleccion.length; x++) {
    var actual = coleccion[x];
    var text = actual.innerText;
    text = text.toLowerCase();
    if (text.indexOf(busqueda) >= 0) {
      indices.push(actual);
    }
  }
  return indices;
}

//#endregion

//#region Funciones de Notificacion

/**
 * @description Muestra una notificacion de navegador
 * @param {any} titulo Titulo
 * @param {any} mensaje Mensaje
 */
function NotificarViaAlerta(titulo, mensaje) {
  if (NoEsValido(titulo)) return;
  if (!NoEsValido(mensaje)) mensaje = "\n" + mensaje;
  alert(titulo + mensaje);
}

/**
 * @description Solicita al usuario la confirmacion de una accion
 * @param {any} titulo Titulo
 * @param {any} mensaje Mensaje
 * @param {any} resolver Funcion a resolver tras la confirmacion
 */
function SolicitarConfirmacion(titulo, mensaje, resolver) {
  if (NoEsValido(titulo)) return;
  if (!NoEsValido(mensaje)) mensaje = "\n" + mensaje;
  if (confirm(titulo + mensaje)) resolver();
}

//#endregion