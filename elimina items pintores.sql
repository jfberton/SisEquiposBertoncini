declare @items_pintor as table(id_item int)
insert into @items_pintor
select id_item from Items_ingresos_egresos where id_item = 10 or id_item_padre = 10
--elimino por equipo
delete from Detalle_valores_items_mes where id_detalle_valor_item_mes in 
(
select DD.id_detalle_valor_item_mes from
Valores_meses VM join
Detalle_valores_items_mes DD on VM.id = DD.id_valor_mes
where VM.id_item in (select id_item from @items_pintor)
)
delete Valores_meses where id_item in (select id_item from @items_pintor)

--elimino por categoria
delete from Detalle_valor_item_meses_categoria where id_detalle_valor_item_mes_categoria in 
(
select DD.id_detalle_valor_item_mes_categoria from
Valor_mes_categorias VM join
Detalle_valor_item_meses_categoria DD on VM.id_valor_mes_categoria_item = DD.id_valor_mes_categoria_item
where VM.id_item in (select id_item from @items_pintor)
)
delete Valor_mes_categorias where id_item in (select id_item from @items_pintor)


--elimino los items
delete Items_ingresos_egresos where id_item in (select id_item from @items_pintor)


