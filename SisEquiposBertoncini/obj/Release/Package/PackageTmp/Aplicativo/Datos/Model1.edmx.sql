
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/14/2016 17:21:30
-- Generated from EDMX file: D:\Desarrollo\Tio\SisEquiposBertoncini\SisEquiposBertoncini\Aplicativo\Datos\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [equipos_berton_01];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CategoriaEquipo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Equipos] DROP CONSTRAINT [FK_CategoriaEquipo];
GO
IF OBJECT_ID(N'[dbo].[FK_Categoria_empleadoEmpleado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Empleados] DROP CONSTRAINT [FK_Categoria_empleadoEmpleado];
GO
IF OBJECT_ID(N'[dbo].[FK_EmpleadoDia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Dias] DROP CONSTRAINT [FK_EmpleadoDia];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipoDetalle_dia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Detalles_dias] DROP CONSTRAINT [FK_EquipoDetalle_dia];
GO
IF OBJECT_ID(N'[dbo].[FK_DiaDetalle_dia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Detalles_dias] DROP CONSTRAINT [FK_DiaDetalle_dia];
GO
IF OBJECT_ID(N'[dbo].[FK_AreaEmpleado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Empleados] DROP CONSTRAINT [FK_AreaEmpleado];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipoItem_por_amortizar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Items_por_amortizar] DROP CONSTRAINT [FK_EquipoItem_por_amortizar];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipoIngreso_egreso_mensual_equipo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ingresos_egresos_mensuales_equipos] DROP CONSTRAINT [FK_EquipoIngreso_egreso_mensual_equipo];
GO
IF OBJECT_ID(N'[dbo].[FK_Ingreso_egreso_mensual_equipoValor_mes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Valores_meses] DROP CONSTRAINT [FK_Ingreso_egreso_mensual_equipoValor_mes];
GO
IF OBJECT_ID(N'[dbo].[FK_Item_ingreso_egresoValor_mes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Valores_meses] DROP CONSTRAINT [FK_Item_ingreso_egresoValor_mes];
GO
IF OBJECT_ID(N'[dbo].[FK_Item_ingreso_egresoItem_ingreso_egreso]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Items_ingresos_egresos] DROP CONSTRAINT [FK_Item_ingreso_egresoItem_ingreso_egreso];
GO
IF OBJECT_ID(N'[dbo].[FK_EmpleadoResumen_mes_empleado]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Resumenes_meses_empleados] DROP CONSTRAINT [FK_EmpleadoResumen_mes_empleado];
GO
IF OBJECT_ID(N'[dbo].[FK_Valor_mesDetalle_valor_item_mes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Detalle_valores_items_mes] DROP CONSTRAINT [FK_Valor_mesDetalle_valor_item_mes];
GO
IF OBJECT_ID(N'[dbo].[FK_Planilla_gasto_administrativoAux_planilla_gasto_administracion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Aux_planilla_gastos_administracion] DROP CONSTRAINT [FK_Planilla_gasto_administrativoAux_planilla_gasto_administracion];
GO
IF OBJECT_ID(N'[dbo].[FK_Aux_total_categoria_mesValor_mes_categoria]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Valor_mes_categorias] DROP CONSTRAINT [FK_Aux_total_categoria_mesValor_mes_categoria];
GO
IF OBJECT_ID(N'[dbo].[FK_Item_ingreso_egresoValor_mes_categoria]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Valor_mes_categorias] DROP CONSTRAINT [FK_Item_ingreso_egresoValor_mes_categoria];
GO
IF OBJECT_ID(N'[dbo].[FK_Valor_mes_categoriaDetalle_valor_item_mes_categoria]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Detalle_valor_item_meses_categoria] DROP CONSTRAINT [FK_Valor_mes_categoriaDetalle_valor_item_mes_categoria];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Equipos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Equipos];
GO
IF OBJECT_ID(N'[dbo].[Categorias_equipos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categorias_equipos];
GO
IF OBJECT_ID(N'[dbo].[Empleados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Empleados];
GO
IF OBJECT_ID(N'[dbo].[Categorias_empleados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categorias_empleados];
GO
IF OBJECT_ID(N'[dbo].[Usuarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Usuarios];
GO
IF OBJECT_ID(N'[dbo].[Dias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Dias];
GO
IF OBJECT_ID(N'[dbo].[Detalles_dias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Detalles_dias];
GO
IF OBJECT_ID(N'[dbo].[Areas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Areas];
GO
IF OBJECT_ID(N'[dbo].[Items_por_amortizar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Items_por_amortizar];
GO
IF OBJECT_ID(N'[dbo].[Valores_dolar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Valores_dolar];
GO
IF OBJECT_ID(N'[dbo].[Ingresos_egresos_mensuales_equipos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ingresos_egresos_mensuales_equipos];
GO
IF OBJECT_ID(N'[dbo].[Items_ingresos_egresos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Items_ingresos_egresos];
GO
IF OBJECT_ID(N'[dbo].[Valores_meses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Valores_meses];
GO
IF OBJECT_ID(N'[dbo].[Feriados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Feriados];
GO
IF OBJECT_ID(N'[dbo].[Resumenes_meses_empleados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Resumenes_meses_empleados];
GO
IF OBJECT_ID(N'[dbo].[Detalle_valores_items_mes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Detalle_valores_items_mes];
GO
IF OBJECT_ID(N'[dbo].[Aux_planilla_calculos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Aux_planilla_calculos];
GO
IF OBJECT_ID(N'[dbo].[Aux_planilla_gastos_horas_hombres]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Aux_planilla_gastos_horas_hombres];
GO
IF OBJECT_ID(N'[dbo].[Aux_planilla_gastos_administracion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Aux_planilla_gastos_administracion];
GO
IF OBJECT_ID(N'[dbo].[Planilla_gastos_administrativo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Planilla_gastos_administrativo];
GO
IF OBJECT_ID(N'[dbo].[Aux_total_categoria_meses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Aux_total_categoria_meses];
GO
IF OBJECT_ID(N'[dbo].[Valor_mes_categorias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Valor_mes_categorias];
GO
IF OBJECT_ID(N'[dbo].[Detalle_valor_item_meses_categoria]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Detalle_valor_item_meses_categoria];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Equipos'
CREATE TABLE [dbo].[Equipos] (
    [id_equipo] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [id_categoria] int  NOT NULL,
    [notas] nvarchar(max)  NOT NULL,
    [fecha_baja] datetime  NULL,
    [OUT] bit  NOT NULL,
    [Generico] bit  NOT NULL,
    [EsTrabajo] bit  NULL
);
GO

-- Creating table 'Categorias_equipos'
CREATE TABLE [dbo].[Categorias_equipos] (
    [id_categoria] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [descripcion] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Empleados'
CREATE TABLE [dbo].[Empleados] (
    [id_empleado] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [id_categoria] int  NOT NULL,
    [id_area] int  NOT NULL,
    [dni] nvarchar(max)  NOT NULL,
    [fecha_nacimiento] datetime  NOT NULL,
    [fecha_baja] datetime  NULL,
    [fecha_alta] datetime  NULL
);
GO

-- Creating table 'Categorias_empleados'
CREATE TABLE [dbo].[Categorias_empleados] (
    [id_categoria] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [descripcion] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Usuarios'
CREATE TABLE [dbo].[Usuarios] (
    [id] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [user] nvarchar(max)  NOT NULL,
    [pass] nvarchar(max)  NOT NULL,
    [perfil] int  NOT NULL
);
GO

-- Creating table 'Dias'
CREATE TABLE [dbo].[Dias] (
    [id_dia] int IDENTITY(1,1) NOT NULL,
    [fecha] datetime  NOT NULL,
    [id_empleado] int  NOT NULL,
    [horas_normales] nvarchar(max)  NOT NULL,
    [horas_extra_50] nvarchar(max)  NOT NULL,
    [horas_extra_100] nvarchar(max)  NOT NULL,
    [estado_tm] int  NOT NULL,
    [estado_tt] int  NOT NULL,
    [ausente] nvarchar(max)  NOT NULL,
    [guardia] nvarchar(max)  NOT NULL,
    [varios_taller] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Detalles_dias'
CREATE TABLE [dbo].[Detalles_dias] (
    [id_detalle_dia] int IDENTITY(1,1) NOT NULL,
    [id_equipo] int  NOT NULL,
    [hora_desde] nvarchar(max)  NOT NULL,
    [hora_hasta] nvarchar(max)  NOT NULL,
    [id_dia] int  NOT NULL
);
GO

-- Creating table 'Areas'
CREATE TABLE [dbo].[Areas] (
    [id_area] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Items_por_amortizar'
CREATE TABLE [dbo].[Items_por_amortizar] (
    [id_item] int IDENTITY(1,1) NOT NULL,
    [id_equipo] int  NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [costo_cero_km_uss] decimal(11,2)  NOT NULL,
    [porcentaje_usado] decimal(5,2)  NOT NULL,
    [porcentaje_valor_recidual] decimal(5,2)  NOT NULL,
    [meses_por_amortizar] int  NOT NULL,
    [periodo_alta_mes] int  NOT NULL,
    [periodo_alta_anio] int  NOT NULL
);
GO

-- Creating table 'Valores_dolar'
CREATE TABLE [dbo].[Valores_dolar] (
    [id] int IDENTITY(1,1) NOT NULL,
    [mes] int  NOT NULL,
    [anio] int  NOT NULL,
    [valor] decimal(11,2)  NOT NULL
);
GO

-- Creating table 'Ingresos_egresos_mensuales_equipos'
CREATE TABLE [dbo].[Ingresos_egresos_mensuales_equipos] (
    [id_ingreso_egreso_mensual] int IDENTITY(1,1) NOT NULL,
    [id_equipo] int  NOT NULL,
    [mes] int  NOT NULL,
    [anio] int  NOT NULL
);
GO

-- Creating table 'Items_ingresos_egresos'
CREATE TABLE [dbo].[Items_ingresos_egresos] (
    [id_item] int IDENTITY(1,1) NOT NULL,
    [tipo] nvarchar(max)  NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [descripcion] nvarchar(max)  NOT NULL,
    [id_item_padre] int  NULL,
    [mostrar_en_equipo] bit  NULL,
    [mostrar_en_trabajo] bit  NULL
);
GO

-- Creating table 'Valores_meses'
CREATE TABLE [dbo].[Valores_meses] (
    [id] int IDENTITY(1,1) NOT NULL,
    [id_ingreso_egreso_mensual] int  NOT NULL,
    [id_item] int  NOT NULL,
    [valor] decimal(11,2)  NOT NULL
);
GO

-- Creating table 'Feriados'
CREATE TABLE [dbo].[Feriados] (
    [id_feriado] int IDENTITY(1,1) NOT NULL,
    [fecha] datetime  NOT NULL,
    [descripcion] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Resumenes_meses_empleados'
CREATE TABLE [dbo].[Resumenes_meses_empleados] (
    [id_resumen_mes] int IDENTITY(1,1) NOT NULL,
    [id_empleado] int  NOT NULL,
    [mes] int  NOT NULL,
    [anio] int  NOT NULL,
    [dias_laborables] decimal(5,2)  NOT NULL,
    [dias_ausente] decimal(5,2)  NOT NULL,
    [dias_presente] decimal(5,2)  NOT NULL,
    [dias_por_cargar] decimal(5,2)  NOT NULL,
    [dias_out] decimal(5,2)  NOT NULL,
    [dias_presentes_en_dias_no_laborables] decimal(5,2)  NOT NULL,
    [total_horas_normales] nvarchar(max)  NOT NULL,
    [total_horas_extra_50] nvarchar(max)  NOT NULL,
    [total_horas_extra_100] nvarchar(max)  NOT NULL,
    [Sueldo] decimal(11,2)  NOT NULL,
    [total_horas_ausente] nvarchar(max)  NOT NULL,
    [total_horas_guardia] nvarchar(max)  NOT NULL,
    [total_horas_varios_taller] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Detalle_valores_items_mes'
CREATE TABLE [dbo].[Detalle_valores_items_mes] (
    [id_detalle_valor_item_mes] int IDENTITY(1,1) NOT NULL,
    [id_valor_mes] int  NOT NULL,
    [fecha] datetime  NOT NULL,
    [monto] decimal(11,2)  NOT NULL,
    [descripcion] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Aux_planilla_calculos'
CREATE TABLE [dbo].[Aux_planilla_calculos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [id_equipo] int  NOT NULL,
    [considera_guardia] bit  NOT NULL,
    [mes] int  NOT NULL,
    [anio] int  NOT NULL
);
GO

-- Creating table 'Aux_planilla_gastos_horas_hombres'
CREATE TABLE [dbo].[Aux_planilla_gastos_horas_hombres] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [insumos_taller] decimal(11,2)  NOT NULL,
    [herramientas] decimal(11,2)  NOT NULL,
    [viaticos] decimal(11,2)  NOT NULL,
    [viaticos_presupuestados] decimal(11,2)  NOT NULL,
    [mes] int  NOT NULL,
    [anio] int  NOT NULL,
    [tipo_empleado] nvarchar(max)  NOT NULL,
    [indumentaria] decimal(11,2)  NOT NULL,
    [repuestos] decimal(11,2)  NOT NULL,
    [gastos_varios] decimal(11,2)  NOT NULL,
    [otros] decimal(11,2)  NOT NULL,
    [repuestos_presupuestados] decimal(11,2)  NOT NULL
);
GO

-- Creating table 'Aux_planilla_gastos_administracion'
CREATE TABLE [dbo].[Aux_planilla_gastos_administracion] (
    [id_detalle_gastos_administrativos] int IDENTITY(1,1) NOT NULL,
    [id_equipo] int  NOT NULL,
    [porcentaje] decimal(11,2)  NOT NULL,
    [id_planilla_gastos_administartivos] int  NOT NULL
);
GO

-- Creating table 'Planilla_gastos_administrativo'
CREATE TABLE [dbo].[Planilla_gastos_administrativo] (
    [id_planilla_gastos_administartivos] int IDENTITY(1,1) NOT NULL,
    [mes] int  NOT NULL,
    [anio] int  NOT NULL,
    [monto_telefonia_celular] decimal(11,2)  NOT NULL,
    [monto_sueldos] decimal(11,2)  NOT NULL,
    [monto_honorarios_sistema] decimal(11,2)  NOT NULL,
    [monto_honorarios_contables] decimal(11,2)  NOT NULL,
    [monto_papeleria] decimal(11,2)  NOT NULL,
    [monto_otros] decimal(11,2)  NOT NULL
);
GO

-- Creating table 'Aux_total_categoria_meses'
CREATE TABLE [dbo].[Aux_total_categoria_meses] (
    [id_aux_total_categoria_mes] int IDENTITY(1,1) NOT NULL,
    [id_categoria_equipo] int  NOT NULL,
    [mes] int  NOT NULL,
    [anio] int  NOT NULL
);
GO

-- Creating table 'Valor_mes_categorias'
CREATE TABLE [dbo].[Valor_mes_categorias] (
    [id_valor_mes_categoria_item] int IDENTITY(1,1) NOT NULL,
    [id_aux_total_categoria_mes] int  NOT NULL,
    [id_item] int  NOT NULL,
    [valor] decimal(11,2)  NOT NULL
);
GO

-- Creating table 'Detalle_valor_item_meses_categoria'
CREATE TABLE [dbo].[Detalle_valor_item_meses_categoria] (
    [id_detalle_valor_item_mes_categoria] int IDENTITY(1,1) NOT NULL,
    [fecha] datetime  NOT NULL,
    [monto] decimal(11,2)  NOT NULL,
    [descripcion] nvarchar(max)  NOT NULL,
    [id_valor_mes_categoria_item] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id_equipo] in table 'Equipos'
ALTER TABLE [dbo].[Equipos]
ADD CONSTRAINT [PK_Equipos]
    PRIMARY KEY CLUSTERED ([id_equipo] ASC);
GO

-- Creating primary key on [id_categoria] in table 'Categorias_equipos'
ALTER TABLE [dbo].[Categorias_equipos]
ADD CONSTRAINT [PK_Categorias_equipos]
    PRIMARY KEY CLUSTERED ([id_categoria] ASC);
GO

-- Creating primary key on [id_empleado] in table 'Empleados'
ALTER TABLE [dbo].[Empleados]
ADD CONSTRAINT [PK_Empleados]
    PRIMARY KEY CLUSTERED ([id_empleado] ASC);
GO

-- Creating primary key on [id_categoria] in table 'Categorias_empleados'
ALTER TABLE [dbo].[Categorias_empleados]
ADD CONSTRAINT [PK_Categorias_empleados]
    PRIMARY KEY CLUSTERED ([id_categoria] ASC);
GO

-- Creating primary key on [id] in table 'Usuarios'
ALTER TABLE [dbo].[Usuarios]
ADD CONSTRAINT [PK_Usuarios]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id_dia] in table 'Dias'
ALTER TABLE [dbo].[Dias]
ADD CONSTRAINT [PK_Dias]
    PRIMARY KEY CLUSTERED ([id_dia] ASC);
GO

-- Creating primary key on [id_detalle_dia] in table 'Detalles_dias'
ALTER TABLE [dbo].[Detalles_dias]
ADD CONSTRAINT [PK_Detalles_dias]
    PRIMARY KEY CLUSTERED ([id_detalle_dia] ASC);
GO

-- Creating primary key on [id_area] in table 'Areas'
ALTER TABLE [dbo].[Areas]
ADD CONSTRAINT [PK_Areas]
    PRIMARY KEY CLUSTERED ([id_area] ASC);
GO

-- Creating primary key on [id_item] in table 'Items_por_amortizar'
ALTER TABLE [dbo].[Items_por_amortizar]
ADD CONSTRAINT [PK_Items_por_amortizar]
    PRIMARY KEY CLUSTERED ([id_item] ASC);
GO

-- Creating primary key on [id] in table 'Valores_dolar'
ALTER TABLE [dbo].[Valores_dolar]
ADD CONSTRAINT [PK_Valores_dolar]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id_ingreso_egreso_mensual] in table 'Ingresos_egresos_mensuales_equipos'
ALTER TABLE [dbo].[Ingresos_egresos_mensuales_equipos]
ADD CONSTRAINT [PK_Ingresos_egresos_mensuales_equipos]
    PRIMARY KEY CLUSTERED ([id_ingreso_egreso_mensual] ASC);
GO

-- Creating primary key on [id_item] in table 'Items_ingresos_egresos'
ALTER TABLE [dbo].[Items_ingresos_egresos]
ADD CONSTRAINT [PK_Items_ingresos_egresos]
    PRIMARY KEY CLUSTERED ([id_item] ASC);
GO

-- Creating primary key on [id] in table 'Valores_meses'
ALTER TABLE [dbo].[Valores_meses]
ADD CONSTRAINT [PK_Valores_meses]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id_feriado] in table 'Feriados'
ALTER TABLE [dbo].[Feriados]
ADD CONSTRAINT [PK_Feriados]
    PRIMARY KEY CLUSTERED ([id_feriado] ASC);
GO

-- Creating primary key on [id_resumen_mes] in table 'Resumenes_meses_empleados'
ALTER TABLE [dbo].[Resumenes_meses_empleados]
ADD CONSTRAINT [PK_Resumenes_meses_empleados]
    PRIMARY KEY CLUSTERED ([id_resumen_mes] ASC);
GO

-- Creating primary key on [id_detalle_valor_item_mes] in table 'Detalle_valores_items_mes'
ALTER TABLE [dbo].[Detalle_valores_items_mes]
ADD CONSTRAINT [PK_Detalle_valores_items_mes]
    PRIMARY KEY CLUSTERED ([id_detalle_valor_item_mes] ASC);
GO

-- Creating primary key on [Id] in table 'Aux_planilla_calculos'
ALTER TABLE [dbo].[Aux_planilla_calculos]
ADD CONSTRAINT [PK_Aux_planilla_calculos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Aux_planilla_gastos_horas_hombres'
ALTER TABLE [dbo].[Aux_planilla_gastos_horas_hombres]
ADD CONSTRAINT [PK_Aux_planilla_gastos_horas_hombres]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id_detalle_gastos_administrativos] in table 'Aux_planilla_gastos_administracion'
ALTER TABLE [dbo].[Aux_planilla_gastos_administracion]
ADD CONSTRAINT [PK_Aux_planilla_gastos_administracion]
    PRIMARY KEY CLUSTERED ([id_detalle_gastos_administrativos] ASC);
GO

-- Creating primary key on [id_planilla_gastos_administartivos] in table 'Planilla_gastos_administrativo'
ALTER TABLE [dbo].[Planilla_gastos_administrativo]
ADD CONSTRAINT [PK_Planilla_gastos_administrativo]
    PRIMARY KEY CLUSTERED ([id_planilla_gastos_administartivos] ASC);
GO

-- Creating primary key on [id_aux_total_categoria_mes] in table 'Aux_total_categoria_meses'
ALTER TABLE [dbo].[Aux_total_categoria_meses]
ADD CONSTRAINT [PK_Aux_total_categoria_meses]
    PRIMARY KEY CLUSTERED ([id_aux_total_categoria_mes] ASC);
GO

-- Creating primary key on [id_valor_mes_categoria_item] in table 'Valor_mes_categorias'
ALTER TABLE [dbo].[Valor_mes_categorias]
ADD CONSTRAINT [PK_Valor_mes_categorias]
    PRIMARY KEY CLUSTERED ([id_valor_mes_categoria_item] ASC);
GO

-- Creating primary key on [id_detalle_valor_item_mes_categoria] in table 'Detalle_valor_item_meses_categoria'
ALTER TABLE [dbo].[Detalle_valor_item_meses_categoria]
ADD CONSTRAINT [PK_Detalle_valor_item_meses_categoria]
    PRIMARY KEY CLUSTERED ([id_detalle_valor_item_mes_categoria] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [id_categoria] in table 'Equipos'
ALTER TABLE [dbo].[Equipos]
ADD CONSTRAINT [FK_CategoriaEquipo]
    FOREIGN KEY ([id_categoria])
    REFERENCES [dbo].[Categorias_equipos]
        ([id_categoria])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoriaEquipo'
CREATE INDEX [IX_FK_CategoriaEquipo]
ON [dbo].[Equipos]
    ([id_categoria]);
GO

-- Creating foreign key on [id_categoria] in table 'Empleados'
ALTER TABLE [dbo].[Empleados]
ADD CONSTRAINT [FK_Categoria_empleadoEmpleado]
    FOREIGN KEY ([id_categoria])
    REFERENCES [dbo].[Categorias_empleados]
        ([id_categoria])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Categoria_empleadoEmpleado'
CREATE INDEX [IX_FK_Categoria_empleadoEmpleado]
ON [dbo].[Empleados]
    ([id_categoria]);
GO

-- Creating foreign key on [id_empleado] in table 'Dias'
ALTER TABLE [dbo].[Dias]
ADD CONSTRAINT [FK_EmpleadoDia]
    FOREIGN KEY ([id_empleado])
    REFERENCES [dbo].[Empleados]
        ([id_empleado])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmpleadoDia'
CREATE INDEX [IX_FK_EmpleadoDia]
ON [dbo].[Dias]
    ([id_empleado]);
GO

-- Creating foreign key on [id_equipo] in table 'Detalles_dias'
ALTER TABLE [dbo].[Detalles_dias]
ADD CONSTRAINT [FK_EquipoDetalle_dia]
    FOREIGN KEY ([id_equipo])
    REFERENCES [dbo].[Equipos]
        ([id_equipo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipoDetalle_dia'
CREATE INDEX [IX_FK_EquipoDetalle_dia]
ON [dbo].[Detalles_dias]
    ([id_equipo]);
GO

-- Creating foreign key on [id_dia] in table 'Detalles_dias'
ALTER TABLE [dbo].[Detalles_dias]
ADD CONSTRAINT [FK_DiaDetalle_dia]
    FOREIGN KEY ([id_dia])
    REFERENCES [dbo].[Dias]
        ([id_dia])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DiaDetalle_dia'
CREATE INDEX [IX_FK_DiaDetalle_dia]
ON [dbo].[Detalles_dias]
    ([id_dia]);
GO

-- Creating foreign key on [id_area] in table 'Empleados'
ALTER TABLE [dbo].[Empleados]
ADD CONSTRAINT [FK_AreaEmpleado]
    FOREIGN KEY ([id_area])
    REFERENCES [dbo].[Areas]
        ([id_area])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AreaEmpleado'
CREATE INDEX [IX_FK_AreaEmpleado]
ON [dbo].[Empleados]
    ([id_area]);
GO

-- Creating foreign key on [id_equipo] in table 'Items_por_amortizar'
ALTER TABLE [dbo].[Items_por_amortizar]
ADD CONSTRAINT [FK_EquipoItem_por_amortizar]
    FOREIGN KEY ([id_equipo])
    REFERENCES [dbo].[Equipos]
        ([id_equipo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipoItem_por_amortizar'
CREATE INDEX [IX_FK_EquipoItem_por_amortizar]
ON [dbo].[Items_por_amortizar]
    ([id_equipo]);
GO

-- Creating foreign key on [id_equipo] in table 'Ingresos_egresos_mensuales_equipos'
ALTER TABLE [dbo].[Ingresos_egresos_mensuales_equipos]
ADD CONSTRAINT [FK_EquipoIngreso_egreso_mensual_equipo]
    FOREIGN KEY ([id_equipo])
    REFERENCES [dbo].[Equipos]
        ([id_equipo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipoIngreso_egreso_mensual_equipo'
CREATE INDEX [IX_FK_EquipoIngreso_egreso_mensual_equipo]
ON [dbo].[Ingresos_egresos_mensuales_equipos]
    ([id_equipo]);
GO

-- Creating foreign key on [id_ingreso_egreso_mensual] in table 'Valores_meses'
ALTER TABLE [dbo].[Valores_meses]
ADD CONSTRAINT [FK_Ingreso_egreso_mensual_equipoValor_mes]
    FOREIGN KEY ([id_ingreso_egreso_mensual])
    REFERENCES [dbo].[Ingresos_egresos_mensuales_equipos]
        ([id_ingreso_egreso_mensual])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ingreso_egreso_mensual_equipoValor_mes'
CREATE INDEX [IX_FK_Ingreso_egreso_mensual_equipoValor_mes]
ON [dbo].[Valores_meses]
    ([id_ingreso_egreso_mensual]);
GO

-- Creating foreign key on [id_item] in table 'Valores_meses'
ALTER TABLE [dbo].[Valores_meses]
ADD CONSTRAINT [FK_Item_ingreso_egresoValor_mes]
    FOREIGN KEY ([id_item])
    REFERENCES [dbo].[Items_ingresos_egresos]
        ([id_item])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Item_ingreso_egresoValor_mes'
CREATE INDEX [IX_FK_Item_ingreso_egresoValor_mes]
ON [dbo].[Valores_meses]
    ([id_item]);
GO

-- Creating foreign key on [id_item_padre] in table 'Items_ingresos_egresos'
ALTER TABLE [dbo].[Items_ingresos_egresos]
ADD CONSTRAINT [FK_Item_ingreso_egresoItem_ingreso_egreso]
    FOREIGN KEY ([id_item_padre])
    REFERENCES [dbo].[Items_ingresos_egresos]
        ([id_item])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Item_ingreso_egresoItem_ingreso_egreso'
CREATE INDEX [IX_FK_Item_ingreso_egresoItem_ingreso_egreso]
ON [dbo].[Items_ingresos_egresos]
    ([id_item_padre]);
GO

-- Creating foreign key on [id_empleado] in table 'Resumenes_meses_empleados'
ALTER TABLE [dbo].[Resumenes_meses_empleados]
ADD CONSTRAINT [FK_EmpleadoResumen_mes_empleado]
    FOREIGN KEY ([id_empleado])
    REFERENCES [dbo].[Empleados]
        ([id_empleado])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmpleadoResumen_mes_empleado'
CREATE INDEX [IX_FK_EmpleadoResumen_mes_empleado]
ON [dbo].[Resumenes_meses_empleados]
    ([id_empleado]);
GO

-- Creating foreign key on [id_valor_mes] in table 'Detalle_valores_items_mes'
ALTER TABLE [dbo].[Detalle_valores_items_mes]
ADD CONSTRAINT [FK_Valor_mesDetalle_valor_item_mes]
    FOREIGN KEY ([id_valor_mes])
    REFERENCES [dbo].[Valores_meses]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Valor_mesDetalle_valor_item_mes'
CREATE INDEX [IX_FK_Valor_mesDetalle_valor_item_mes]
ON [dbo].[Detalle_valores_items_mes]
    ([id_valor_mes]);
GO

-- Creating foreign key on [id_planilla_gastos_administartivos] in table 'Aux_planilla_gastos_administracion'
ALTER TABLE [dbo].[Aux_planilla_gastos_administracion]
ADD CONSTRAINT [FK_Planilla_gasto_administrativoAux_planilla_gasto_administracion]
    FOREIGN KEY ([id_planilla_gastos_administartivos])
    REFERENCES [dbo].[Planilla_gastos_administrativo]
        ([id_planilla_gastos_administartivos])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Planilla_gasto_administrativoAux_planilla_gasto_administracion'
CREATE INDEX [IX_FK_Planilla_gasto_administrativoAux_planilla_gasto_administracion]
ON [dbo].[Aux_planilla_gastos_administracion]
    ([id_planilla_gastos_administartivos]);
GO

-- Creating foreign key on [id_aux_total_categoria_mes] in table 'Valor_mes_categorias'
ALTER TABLE [dbo].[Valor_mes_categorias]
ADD CONSTRAINT [FK_Aux_total_categoria_mesValor_mes_categoria]
    FOREIGN KEY ([id_aux_total_categoria_mes])
    REFERENCES [dbo].[Aux_total_categoria_meses]
        ([id_aux_total_categoria_mes])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Aux_total_categoria_mesValor_mes_categoria'
CREATE INDEX [IX_FK_Aux_total_categoria_mesValor_mes_categoria]
ON [dbo].[Valor_mes_categorias]
    ([id_aux_total_categoria_mes]);
GO

-- Creating foreign key on [id_item] in table 'Valor_mes_categorias'
ALTER TABLE [dbo].[Valor_mes_categorias]
ADD CONSTRAINT [FK_Item_ingreso_egresoValor_mes_categoria]
    FOREIGN KEY ([id_item])
    REFERENCES [dbo].[Items_ingresos_egresos]
        ([id_item])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Item_ingreso_egresoValor_mes_categoria'
CREATE INDEX [IX_FK_Item_ingreso_egresoValor_mes_categoria]
ON [dbo].[Valor_mes_categorias]
    ([id_item]);
GO

-- Creating foreign key on [id_valor_mes_categoria_item] in table 'Detalle_valor_item_meses_categoria'
ALTER TABLE [dbo].[Detalle_valor_item_meses_categoria]
ADD CONSTRAINT [FK_Valor_mes_categoriaDetalle_valor_item_mes_categoria]
    FOREIGN KEY ([id_valor_mes_categoria_item])
    REFERENCES [dbo].[Valor_mes_categorias]
        ([id_valor_mes_categoria_item])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Valor_mes_categoriaDetalle_valor_item_mes_categoria'
CREATE INDEX [IX_FK_Valor_mes_categoriaDetalle_valor_item_mes_categoria]
ON [dbo].[Detalle_valor_item_meses_categoria]
    ([id_valor_mes_categoria_item]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------