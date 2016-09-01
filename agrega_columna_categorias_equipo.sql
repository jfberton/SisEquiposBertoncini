
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
PRINT N'Dropping foreign keys from [dbo].[Items_por_amortizar]'
GO
ALTER TABLE [dbo].[Items_por_amortizar] DROP CONSTRAINT [FK_EquipoItem_por_amortizar]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[Categorias_equipos]'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[Categorias_equipos] ADD
[toma_en_cuenta_planilla_costos_horas_hombre] [bit] NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Items_por_amortizar]'
GO
ALTER TABLE [dbo].[Items_por_amortizar] ADD CONSTRAINT [FK_EquipoItem_por_amortizar] FOREIGN KEY ([id_equipo]) REFERENCES [dbo].[Equipos] ([id_equipo])
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
