-- =============================================
-- Author:		Israel Ch.
-- Create date: 18/03/2020
-- Description:	Permite marcar como eliminados los registros de bitacora de un rango de tiempo especificado
-- =============================================
CREATE PROCEDURE [dbo].[EliminarEntradasDeBitacora]
	@Inicio Date,
	@Fin Date
AS
BEGIN	
	--Prevenir la seleccion multiple de resultados
	SET NOCOUNT ON;

    --Momento en el que se ha registrado la entrada y deberan eliminarse
	DECLARE @FechaInicio Date = @Inicio;
	
	--Ultimo momento en el que se ha registrado la entrada y debera eliminarse
	DECLARE @FechaFin Date = @Fin;	
	
	--Registro de resultados
	DECLARE @Resultados TABLE(
		--Indica si el resultado de la operacion en curso es incorrecto o no (0,1)
		Correcto bit,
		--Proporciona el contexto del resultado de la operacion
		Mensaje varchar(512) null
	);
	
	--Iniciar transaccion de operaciones
	BEGIN TRANSACTION T0
	BEGIN TRY
		--Marcar como eliminado todos los registros de bitacora
		UPDATE B
			SET B.Eliminado = GETDATE()
		FROM 
			Bitacora B
		WHERE
			 B.Creado >= @FechaInicio
			 AND B.Creado <= @FechaFin
			 AND B.Eliminado IS NULL;

		--Marcar como eliminado todos los detalles de bitacora
		UPDATE D
			SET D.Eliminado = GETDATE()
		FROM
			 BitacoraDetalle D
		WHERE
			 D.Creado >= @FechaInicio
			 AND D.Creado <= @FechaFin
			 AND D.Eliminado IS NULL;
			 		
		--Terminar la transaccion
		COMMIT TRANSACTION T0;	
		
		--Insertar dentro de los resultados todo correcto
		INSERT INTO @Resultados (			
			Correcto,			
			Mensaje
		)
		VALUES (
			--Correcto
			1,
			--Mensaje
			N'Se han marcado todos los registros dentro del rango especificado como eliminados'
		)
	END TRY
	BEGIN CATCH
		--Revertir todos los cambios
		ROLLBACK TRANSACTION T0;
		
		--Insertar dentro de los resultados los errores
		INSERT INTO @Resultados (
			Correcto,
			Mensaje
		)
		VALUES (
			--Correcto
			0,			
			--Mensaje
			ERROR_MESSAGE()
		)
	END CATCH		

	--Seleccionar todos los resultados
	SELECT
		Correcto,
		Mensaje
	FROM 
		@Resultados;
END
