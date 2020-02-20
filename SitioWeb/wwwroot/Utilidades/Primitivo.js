/**
 * @description Verifica que el objeto dado es invalido para su uso
 * @param {any} o Objeto a evaluar
 * @return {boolean} Verdadero o falso
 */
function ObjetoInvalido(o) {
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