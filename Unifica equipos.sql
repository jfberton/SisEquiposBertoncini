
declare @id_equipo_baja as int = 115
declare @id_equipo_unificado as int = 118
declare @nuevo_nombre_equipo_unificado as varchar(100)= 'SPRINTER'

--doy de baja y actualizo el nombre del equipo segun corresponda
update equipos set nombre = @nuevo_nombre_equipo_unificado where id_equipo = @id_equipo_unificado
update equipos set fecha_baja = GETDATE() where id_equipo = @id_equipo_baja

--cambio las horas afectadas del equipo a bar de baja por el otro
update Detalles_dias set id_equipo = @id_equipo_unificado where id_equipo = @id_equipo_baja

/*valores mensuales*/

select
'UPDATE Detalle_valores_items_mes SET  id_valor_mes = ' + 
CAST((select top 1 VM_1.id
	from
		Ingresos_egresos_mensuales_equipos IEE_1 join
		Valores_meses VM_1 on IEE_1.id_ingreso_egreso_mensual = VM_1.id_ingreso_egreso_mensual
	where
		IEE_1.id_equipo = @id_equipo_unificado and
		IEE_1.mes = IEE.mes and
		IEE_1.anio = IEE.anio and
		VM_1.id_item = VM.id_item) AS VARCHAR(10))
 + ' WHERE id_detalle_valor_item_mes = ' + CAST(DD.id_detalle_valor_item_mes AS VARCHAR(10)) + char(13)
from 
	Ingresos_egresos_mensuales_equipos IEE join
	Valores_meses VM on IEE.id_ingreso_egreso_mensual = VM.id_ingreso_egreso_mensual join
	Detalle_valores_items_mes DD on VM.id = DD.id_valor_mes
where 
	IEE.id_equipo = @id_equipo_baja

/*

*/