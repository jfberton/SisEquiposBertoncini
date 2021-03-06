/*
Run this script on:

        (local)\sqlexpress.equipos_berton_01    -  This database will be modified

to synchronize it with:

        (local)\sqlexpress.equipos_berton

You are recommended to back up your database before running this script

Script created by SQL Compare version 11.1.3 from Red Gate Software Ltd at 27/04/2016 21:20:47

*/
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[obtener_costo_amortizacion]'
GO
-- =================================================================
-- Author:		Bertoncini, José Federico
-- Create date: 18/04/2016
-- Description:	Obtiene el costo mensual por amortizar del equipo
-- =================================================================
CREATE PROCEDURE [dbo].[obtener_costo_amortizacion]
	@id_equipo int,
	@mes int,
	@anio int
AS
BEGIN
	
	--obtengo el valor del dolar del mes buscado
	declare @valor_dolar_mes as money = isnull((select cast(valor as money) from Valores_dolar where mes = @mes and anio = @anio), 0)

	--obtengo todos los items por amortizar del equipo y le agrego el anio y mes de hasta cuando deberia de tomarse en cuenta dicho item
	declare @table as table(id_equipo int, id_item int, nombre varchar(100), costo_cero_km_uss money, porcentaje_usado money, porcentaje_valor_recidual money,meses_por_amortizar int, periodo_alta_mse int, periodo_alta_anio int, anio_hasta int, mes_hasta int)
	insert into @table
	select 
			id_equipo
			, id_item
			, nombre
			, costo_cero_km_uss
			, CONVERT(decimal(4,2), porcentaje_usado)/100
			, CONVERT(decimal(4,2), porcentaje_valor_recidual)/100
			, meses_por_amortizar
			, periodo_alta_mes
			, periodo_alta_anio
			, 
			case
			 when ((periodo_alta_mes /*mes inicial*/ + (meses_por_amortizar%12) /*meses que sobran de los meses por amortizar luego de convertirlo a años*/)% 12) = 0 then
				(periodo_alta_anio + 
				(meses_por_amortizar/12) /*anios de mas*/ + 
				(periodo_alta_mes /*mes inicial*/ + (meses_por_amortizar%12) /*meses que sobran de los meses por amortizar luego de convertirlo a años*/)/ 12) - 1
			 else	
				periodo_alta_anio + 
				(meses_por_amortizar/12) /*anios de mas*/ + 
				(periodo_alta_mes /*mes inicial*/ + (meses_por_amortizar%12) /*meses que sobran de los meses por amortizar luego de convertirlo a años*/)/ 12 
			end as año_final
			, 
			case
			 when (periodo_alta_mes /*mes inicial*/ + (meses_por_amortizar%12) /*meses que sobran de los meses por amortizar luego de convertirlo a años*/)% 12 = 0 then 12
			 else (periodo_alta_mes /*mes inicial*/ + (meses_por_amortizar%12) /*meses que sobran de los meses por amortizar luego de convertirlo a años*/)% 12
			end as mes_final
	from Items_por_amortizar
	where id_equipo = @id_equipo


	--obtengo los items que unicamente se van a tomar en cuenta y agrego los valores por amortizar y el costo mensual
	declare @table1 as table(id_equipo int, id_item int, nombre varchar(100), costo_cero_km_uss money, porcentaje_usado money, porcentaje_valor_recidual money,meses_por_amortizar int, periodo_alta_mse int, periodo_alta_anio int, anio_hasta int, mes_hasta int, valor_por_amortizar money, valor_amortizacion_mensual money)
	insert into @table1
	select 
		*,
		cast(costo_cero_km_uss - 
			(costo_cero_km_uss * (1 - (porcentaje_usado))) - 
			((costo_cero_km_uss - (costo_cero_km_uss * (1 - (porcentaje_usado)))) * (porcentaje_valor_recidual)) as money) as Valor_por_amortizar,
		case
			when t.meses_por_amortizar > 0 then
				cast(costo_cero_km_uss - 
				(costo_cero_km_uss * (1 - (porcentaje_usado))) - 
				((costo_cero_km_uss - (costo_cero_km_uss * (1 - (porcentaje_usado)))) * (porcentaje_valor_recidual))/t.meses_por_amortizar as money)
			else
				cast(0 as money)
		end as valor_amortizacion_mensual		
	from 
		@table t 
	where 
		(@anio*100 + @mes)<= (t.anio_hasta*100 + t.mes_hasta)
		
	--Sumo los valores de amortizacion mensual de cada una de las partes que componen el equipo y la multimplico por el valor del dolar de ese mes
	select 
	SUM(t.valor_amortizacion_mensual)*@valor_dolar_mes
	from @table1 t

END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[obtener_valor]'
GO
-- =================================================================
-- Author:		Bertoncini, José Federico
-- Create date: 18/04/2016
-- Description:	Obtiene el costo mensual por amortizar del equipo
-- =================================================================
CREATE FUNCTION [dbo].[obtener_valor]
(
	@id_ingreso_egreso_mensual_equipo int,
	@id_item_ingreso_egreso int
)
RETURNS MONEY
AS
BEGIN
	--valor que retorno al final
	DECLARE @VALOR AS MONEY = 0
	
	--OBTENGO EL VALOR MES CON EL ID_ITEM Y EL ID_INGRESO_EGRESO_MENSUAL
	DECLARE @ID_VALOR_MES INT = (SELECT id FROM Valores_meses WHERE id_ingreso_egreso_mensual = @id_ingreso_egreso_mensual_equipo AND id_item = @id_item_ingreso_egreso)
	
	--obtengo los hijos del item que pase por parametro
	DECLARE @HIJOS AS TABLE(ID_ITEM INT, TIPO VARCHAR(10), NOMBRE VARCHAR(200), DESCRIPCION VARCHAR(200), ID_ITEM_PADRE INT, MOSTRAR_EN_EQUIPO BIT, MOSTRAR_EN_TRABAJO BIT)
	INSERT INTO @HIJOS
	SELECT * FROM dbo.Items_ingresos_egresos IIE WHERE IIE.ID_ITEM_PADRE = @id_item_ingreso_egreso
	
	IF((SELECT COUNT(*) FROM @HIJOS)>0)
	BEGIN
		DECLARE @ID_HIJO AS INT
		DECLARE Recorrer_hijos CURSOR 
		FOR
		SELECT ID_ITEM FROM @HIJOS
		-- apertura del cursor
		OPEN Recorrer_hijos 
		-- Lectura de la primera fila del cursor
		FETCH NEXT FROM Recorrer_hijos INTO @ID_HIJO
		WHILE (@@FETCH_STATUS = 0)
		BEGIN	
			
			SET @VALOR = @VALOR + (SELECT dbo.obtener_valor(@id_ingreso_egreso_mensual_equipo,@ID_HIJO))
			
			FETCH NEXT FROM Recorrer_hijos INTO @ID_HIJO
		END -- Fin del bucle WHILE
		CLOSE Recorrer_hijos
		DEALLOCATE Recorrer_hijos
	END
	ELSE
	BEGIN
		IF(NOT @ID_VALOR_MES  IS NULL)
		BEGIN
			SET @VALOR = ISNULL((SELECT CAST(SUM(MONTO) AS MONEY) FROM Detalle_valores_items_mes WHERE id_valor_mes = @ID_VALOR_MES),0)
		END
		ELSE
		BEGIN
			SET @VALOR = 0
		END
		
	END
	
	RETURN @VALOR

END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[temp_table_filas_items_mes_equipo]'
GO
CREATE TABLE [dbo].[temp_table_filas_items_mes_equipo]
(
[id] [int] NOT NULL IDENTITY(1, 1),
[id_valor_mes] [int] NOT NULL,
[concepto] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NOT NULL,
[valor] [decimal] (11, 2) NOT NULL,
[row_class] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NOT NULL,
[visible] [bit] NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_temp_table_filas_items_mes_equipo] on [dbo].[temp_table_filas_items_mes_equipo]'
GO
ALTER TABLE [dbo].[temp_table_filas_items_mes_equipo] ADD CONSTRAINT [PK_temp_table_filas_items_mes_equipo] PRIMARY KEY CLUSTERED  ([id])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[obtener_fila]'
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[obtener_fila]
	-- Add the parameters for the stored procedure here
	@id_item as int,
	@id_ingreso_egreso_mensual_equipo as int
AS
BEGIN
	
	INSERT INTO temp_table_filas_items_mes_equipo(id_valor_mes, concepto, valor, row_class, visible)
	SELECT
		(SELECT TOP 1 ID FROM Valores_meses VM WHERE VM.id_ingreso_egreso_mensual = @id_ingreso_egreso_mensual_equipo AND VM.id_item = ITEM.id_item) AS id_valor_mes,
		ITEM.nombre as concepto,
		(SELECT TOP 1 valor FROM Valores_meses VM WHERE VM.id_ingreso_egreso_mensual = @id_ingreso_egreso_mensual_equipo AND VM.id_item = ITEM.id_item) AS valor,
		
		'treegrid-' + CAST(ITEM.id_item AS VARCHAR(6)) + 
		(CASE
			WHEN not ITEM.id_item_padre is null THEN ' treegrid-parent-' + cast(item.id_item_padre AS VARCHAR(6))
			ELSE ''
		END) + 
		(CASE
			WHEN ITEM.id_item_padre IS NULL THEN ' h4'
			ELSE ''
		END) +
		 (CASE
			WHEN ITEM.tipo = 'Ingreso' THEN ' alert-success'
			ELSE ' alert-danger'
		END) AS row_class,
		CASE
			WHEN 
				(SELECT COUNT(*) FROM Items_ingresos_egresos WHERE id_item_padre = ITEM.id_item) = 0																					--item.Hijos.Count == 0 
				AND NOT (ITEM.nombre = 'Impuestos' AND (SELECT nombre FROM Items_ingresos_egresos WHERE id_item = ITEM.id_item_padre) = 'INGRESOS')										--!(item.nombre == "Impuestos" && item.Padre.nombre == "INGRESOS") && 
				AND NOT (ITEM.nombre = 'Accesorios Hs Extra 24% (1/12x2)' AND (SELECT nombre FROM Items_ingresos_egresos WHERE id_item = ITEM.id_item_padre) = 'Otros costos variables')--!(item.nombre == "Accesorios Hs Extra 24% (1/12x2)" && item.Padre.nombre == "Otros costos variables") && 
				AND NOT (ITEM.nombre = 'Amortización' AND (SELECT nombre FROM Items_ingresos_egresos WHERE id_item = ITEM.id_item_padre) = 'Costos Fijos No Erogables')					--!(item.nombre == "Amortización" && item.Padre.nombre == "Costos Fijos No Erogables");
				THEN 1
			ELSE 0
		END AS visible
	FROM
		Items_ingresos_egresos ITEM 
	where ITEM.id_item = @id_item
	
	DECLARE @ID_ITEM_A_RECORRER AS INT
	DECLARE RECORRER CURSOR 
	FOR
	SELECT ID_ITEM FROM Items_ingresos_egresos where id_item_padre = @id_item
	-- apertura del cursor
	OPEN RECORRER 
	-- Lectura de la primera fila del cursor
	FETCH NEXT FROM RECORRER INTO @ID_ITEM_A_RECORRER
	WHILE (@@FETCH_STATUS = 0)
	BEGIN	
		EXEC [dbo].obtener_fila
				@id_item = @ID_ITEM_A_RECORRER,
				@id_ingreso_egreso_mensual_equipo = @id_ingreso_egreso_mensual_equipo
		
		FETCH NEXT FROM RECORRER INTO @ID_ITEM_A_RECORRER
	END -- Fin del bucle WHILE
	CLOSE RECORRER
	DEALLOCATE RECORRER
END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Obtener_listado_items_ingreso_egreso_mensual]'
GO
-- =================================================================
-- Author:		Bertoncini, José Federico
-- Create date: 18/04/2016
-- Description:	Crea actualiza y lista los items ingresos-egresos
--				del equipo, mes y año pasados por parametro para 
--				mostrarlos en el listado con estructura de arbol.
-- =================================================================
CREATE PROCEDURE [dbo].[Obtener_listado_items_ingreso_egreso_mensual]
	@mes AS INT,
	@anio AS INT,
	@id_equipo AS INT
AS
BEGIN
		
	--///////////////////////////////////////////////
	--VERIFICO QUE EXISTA EL REGISTRO DE INGRESO_EGRESO_MENSUAL DE ESE EQUIPO
	DECLARE @id_ingreso_egreso_mensual_equipo AS INT = (SELECT TOP 1 id_ingreso_egreso_mensual FROM Ingresos_egresos_mensuales_equipos WHERE id_equipo = @id_equipo AND mes = @mes AND anio = @anio)

	IF(@id_ingreso_egreso_mensual_equipo IS NULL)
	BEGIN
		INSERT INTO Ingresos_egresos_mensuales_equipos(id_equipo, mes, anio) VALUES(@id_equipo, @mes, @anio)
		SET @id_ingreso_egreso_mensual_equipo = (SELECT id_ingreso_egreso_mensual FROM Ingresos_egresos_mensuales_equipos WHERE id_equipo = @id_equipo AND mes = @mes AND anio = @anio)
	END

	--OBTENGO EL VALOR MENSUAL DE AMORTIZACION PARA EL MES BUSCADO
	DECLARE @monto_amortizacion_mes AS MONEY
		DECLARE @amort TABLE ( resultado MONEY )
		INSERT INTO @amort 
		EXEC [dbo].[obtener_costo_amortizacion]
				@id_equipo,
				@mes,
				@anio
				
		set @monto_amortizacion_mes = (SELECT * FROM @amort)

	--OBTENGO EL ITEM INGRESO EGRESO DE AMORTIZACION Y LO ASIGNO AL VALOR MES CORRESPONDIENTE
	DECLARE @id_item_amortizacion AS INT = (SELECT ITEM.id_item FROM Items_ingresos_egresos ITEM JOIN Items_ingresos_egresos PADRE ON ITEM.id_item_padre = PADRE.id_item WHERE ITEM.nombre = 'Amortización' AND PADRE.nombre ='Costos Fijos No Erogables')
	DECLARE @id_valor_mes_amortizacion AS INT = (SELECT id FROM Valores_meses WHERE id_ingreso_egreso_mensual = @id_ingreso_egreso_mensual_equipo AND id_item = @id_item_amortizacion)
	IF( @id_valor_mes_amortizacion IS NULL)
	BEGIN 
	INSERT INTO Valores_meses (id_ingreso_egreso_mensual, id_item, valor) VALUES(@id_ingreso_egreso_mensual_equipo, @id_item_amortizacion, @monto_amortizacion_mes)	
	SET @id_valor_mes_amortizacion = (SELECT id FROM Valores_meses WHERE id_ingreso_egreso_mensual = @id_ingreso_egreso_mensual_equipo AND id_item = @id_item_amortizacion)
	INSERT INTO Detalle_valores_items_mes(id_valor_mes, descripcion, fecha, monto) VALUES(@id_valor_mes_amortizacion, 'AMORTIZACION', DateAdd(day, 1 - 1, DateAdd(month, @mes - 1, DateAdd(Year, @anio-1900, 0))), @monto_amortizacion_mes)
	END

	--RECORRO LOS ITEMS INGRESO Y EGRESO Y VERIFICO QUE EXISTAN SUS VALORES MES PARA ESTE INGRESO_EGRESO_MENSUAL_EQUIPO SINO LOS CREO EN CERO
	DECLARE @ID_ITEM_A_RECORRER AS INT
	DECLARE RECORRER CURSOR 
	FOR
	SELECT ID_ITEM FROM Items_ingresos_egresos
	-- apertura del cursor
	OPEN RECORRER 
	-- Lectura de la primera fila del cursor
	FETCH NEXT FROM RECORRER INTO @ID_ITEM_A_RECORRER
	WHILE (@@FETCH_STATUS = 0)
	BEGIN	
		
		DECLARE @ID_VALOR_MES AS INT = (SELECT TOP 1 ID FROM Valores_meses WHERE id_item = @ID_ITEM_A_RECORRER AND id_ingreso_egreso_mensual = @id_ingreso_egreso_mensual_equipo)
		IF(@ID_VALOR_MES IS NULL)
		BEGIN
			INSERT INTO Valores_meses(id_ingreso_egreso_mensual, id_item, valor) VALUES(@id_ingreso_egreso_mensual_equipo, @ID_ITEM_A_RECORRER, 0)
		END
		ELSE
		BEGIN
			UPDATE Valores_meses SET valor = (SELECT dbo.obtener_valor(@id_ingreso_egreso_mensual_equipo, @ID_ITEM_A_RECORRER)) 
			WHERE id_item = @ID_ITEM_A_RECORRER AND id_ingreso_egreso_mensual = @id_ingreso_egreso_mensual_equipo
		END
		
		
		
		FETCH NEXT FROM RECORRER INTO @ID_ITEM_A_RECORRER
	END -- Fin del bucle WHILE
	CLOSE RECORRER
	DEALLOCATE RECORRER

	delete temp_table_filas_items_mes_equipo
	
	
	DBCC CHECKIDENT ('temp_table_filas_items_mes_equipo', RESEED, 0);
	
	--cargo los valores de ingresos
	EXEC	[dbo].[obtener_fila]
			@id_item = 1,
			@id_ingreso_egreso_mensual_equipo = @id_ingreso_egreso_mensual_equipo
	
	--cargo los egresos
	EXEC	[dbo].[obtener_fila]
			@id_item = 2,
			@id_ingreso_egreso_mensual_equipo =  @id_ingreso_egreso_mensual_equipo

	SELECT	* from temp_table_filas_items_mes_equipo
	
END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	PRINT 'The database update failed'
END
GO
