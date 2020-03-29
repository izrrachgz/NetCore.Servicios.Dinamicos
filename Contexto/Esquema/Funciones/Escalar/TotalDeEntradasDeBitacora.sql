-- =============================================
-- Author:		Israel Ch
-- Create date: 18/03/2020
-- Description:	Permite obtener el total de entradas de bitacora registradas dentro del rango de tiempo especificado
-- =============================================
CREATE FUNCTION [dbo].[TotalDeEntradasDeBitacora]
(
	--Rango de inicio del momento de registro para incluir en la busqueda
	@Inicio Date null,
	--Rango de fin del momento de registro para incluir en la busqueda
	@Fin Date null
)
RETURNS bigint
AS
BEGIN
	--Cantidad total de registros de bitacora
	DECLARE @Total bigint = 0;
	
	--Momento de inicio en el que se ha registrado la entrada de bitacora
	DECLARE @FechaInicio Date = @Inicio;
	
	--Momento de fin en el que se ha registrado la entrada de bitacora
	DECLARE @FechaFin Date = @Fin;

	--Tomar en cuenta las 2 fechas
	IF @FechaInicio IS NOT NULL AND @FechaFin IS NOT NULL
	BEGIN
		SET @Total = (
			SELECT 
				COUNT(*) AS Total
			FROM
				Bitacora B
			WHERE
				B.Creado >= @FechaInicio 
				AND B.Creado <= @FechaFin
				AND B.Eliminado IS NULL
		);
	END

	--Tomar en cuenta solo la fecha de inicio
	IF @FechaInicio IS NOT NULL AND @FechaFin IS NULL
	BEGIN
		SET @Total = (
			SELECT 
				COUNT(*) AS Total
			FROM
				Bitacora B
			WHERE
				B.Creado >= @FechaInicio 					
				AND B.Eliminado IS NULL
		);
	END

	--Tomar en cuenta solo la fecha de fin
	IF @FechaInicio IS NULL AND @FechaFin IS NOT NULL
	BEGIN
		SET @Total = (
			SELECT 
				COUNT(*) AS Total
			FROM
				Bitacora B
			WHERE
				B.Creado <= @FechaFin 					
				AND B.Eliminado IS NULL
		);
	END

	RETURN @Total;
END
