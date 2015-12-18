using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SisEquiposBertoncini.Aplicativo.Datos;

namespace SisEquiposBertoncini.Aplicativo.Controles
{
    public static class Procesos_Globales
    {
        /// <summary>
        /// Actualiza unicamente las horas 
        /// </summary>
        public static void Actualizar_horas_todos_los_resumenes_mensuales()
        {
            using (var cxt = new Model1Container())
            {
                var empleados = cxt.Empleados.Where(x=>x.Categoria.nombre == "Soldador").ToList();

                foreach (Empleado empleado in empleados)
                {
                    var resumenes_mensuales = empleado.Resumenes_meses_empleado.ToList();
                    foreach (Resumen_mes_empleado resumen_mensual in resumenes_mensuales)
                    {
                        var dias_mes = empleado.Dias.Where(x => x.fecha.Month == resumen_mensual.mes && x.fecha.Year == resumen_mensual.anio);

                        string total_horas_normales = "00:00";
                        string total_horas_extra_50 = "00:00";
                        string total_horas_extra_100 = "00:00";
                        string total_horas_ausente = "00:00";
                        string total_horas_guardia = "00:00";
                        string total_horas_varios_taller = "00:00";

                        foreach (Dia dia in dias_mes)
                        {
                            string horasTotales = "00:00";
                            string horasAusente = "00:00";
                            string horasGuardia = "00:00";
                            string horasVariosTaller = "00:00";

                            foreach (Detalle_dia item_detalle in dia.Detalles)
                            {
                                if(!item_detalle.Equipo.OUT || dia.Empleado.Categoria.nombre == "Soldador")
                                {
                                    if (item_detalle.Equipo.nombre != "Ausencia")
                                    {
                                        horasTotales = Horas_string.SumarHoras(new string[] { horasTotales, Horas_string.RestarHoras(item_detalle.hora_hasta, item_detalle.hora_desde) });

                                        if (item_detalle.Equipo.nombre == "Guardia")
                                        {
                                            if(dia.Empleado.Categoria.nombre != "Soldador")
                                            {
                                                horasGuardia = Horas_string.SumarHoras(new string[] { horasGuardia, Horas_string.RestarHoras(item_detalle.hora_hasta, item_detalle.hora_desde) });
                                            }
                                            else
                                            {
                                                horasGuardia ="00:00";
                                                horasVariosTaller = Horas_string.SumarHoras(new string[] { horasVariosTaller, Horas_string.RestarHoras(item_detalle.hora_hasta, item_detalle.hora_desde) });
                                            }
                                        }

                                        if (item_detalle.Equipo.nombre == "Varios Taller")
                                        {
                                            horasVariosTaller = Horas_string.SumarHoras(new string[] { horasVariosTaller, Horas_string.RestarHoras(item_detalle.hora_hasta, item_detalle.hora_desde) });
                                        }
                                    }
                                    else
                                    {
                                        horasAusente = Horas_string.SumarHoras(new string[] { horasAusente, Horas_string.RestarHoras(item_detalle.hora_hasta, item_detalle.hora_desde) });
                                    }
                                }
                            }

                            dia.horas_normales = ObtenerHoras(horasTotales, dia.fecha, TipoHora.Normales);
                            dia.horas_extra_50 = ObtenerHoras(horasTotales, dia.fecha, TipoHora.Extra50);
                            dia.horas_extra_100 = ObtenerHoras(horasTotales, dia.fecha, TipoHora.Extra100);
                            dia.ausente = horasAusente;
                            dia.guardia = horasGuardia;
                            dia.varios_taller = horasVariosTaller;

                            total_horas_normales = Horas_string.SumarHoras(new string[]{dia.horas_normales, total_horas_normales});
                            total_horas_extra_50 = Horas_string.SumarHoras(new string[]{dia.horas_extra_50, total_horas_extra_50});
                            total_horas_extra_100 = Horas_string.SumarHoras(new string[]{dia.horas_extra_100, total_horas_extra_100});
                            total_horas_ausente = Horas_string.SumarHoras(new string[]{dia.ausente, total_horas_ausente});
                            total_horas_guardia = Horas_string.SumarHoras(new string[]{dia.guardia, total_horas_guardia});
                            total_horas_varios_taller = Horas_string.SumarHoras(new string[]{dia.varios_taller, total_horas_varios_taller});
                        }

                        if(empleado.Categoria.nombre =="Soldador")
                        {
                            resumen_mensual.dias_out = 0;
                        }
                        resumen_mensual.total_horas_ausente = total_horas_ausente;
                        resumen_mensual.total_horas_extra_100 = total_horas_extra_100;
                        resumen_mensual.total_horas_extra_50 = total_horas_extra_50;
                        resumen_mensual.total_horas_guardia = total_horas_guardia;
                        resumen_mensual.total_horas_normales = total_horas_normales;
                        resumen_mensual.total_horas_varios_taller = total_horas_varios_taller;

                    }

                    cxt.SaveChanges();
                }
            }
        }

        private enum TipoHora
        {
            Normales,
            Extra50,
            Extra100
        }

        private static string ObtenerHoras(string horas_totales, DateTime dia, TipoHora tipo_buscado)
        {
            string horas_normales = "00:00";
            string horas_extra_50 = "00:00";
            string horas_extra_100 = "00:00";
            Feriado feriado = null;
            using (var cxt = new Model1Container())
            {
                feriado = cxt.Feriados.FirstOrDefault(ff => ff.fecha == dia);
            }

            if (feriado == null)
            {
                switch (dia.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        if (Horas_string.AMayorQueB("04:00", horas_totales))
                        {
                            horas_normales = horas_totales;
                            horas_totales = "00:00";
                        }
                        else
                        {
                            horas_normales = "04:00";
                            horas_totales = Horas_string.RestarHoras(horas_totales, "04:00");
                        }

                        horas_extra_50 = horas_totales;
                        horas_extra_100 = "00:00";
                        break;
                    case DayOfWeek.Sunday:
                        horas_normales = "00:00";
                        horas_extra_50 = "00:00";
                        horas_extra_100 = horas_totales;
                        break;
                    default:
                        if (Horas_string.AMayorQueB("08:00", horas_totales))
                        {
                            horas_normales = horas_totales;
                            horas_totales = "00:00";
                        }
                        else
                        {
                            horas_normales = "08:00";
                            horas_totales = Horas_string.RestarHoras(horas_totales, "08:00");
                        }
                        horas_extra_50 = horas_totales;
                        horas_extra_100 = "00:00";
                        break;
                }
            }
            else
            {
                horas_normales = "00:00";
                horas_extra_50 = "00:00";
                horas_extra_100 = horas_totales;
            }

            switch (tipo_buscado)
            {
                case TipoHora.Normales:
                    return horas_normales;
                case TipoHora.Extra50:
                    return horas_extra_50;
                case TipoHora.Extra100:
                    return horas_extra_100;
                default:
                    return "00:00";
            }

        }
    }

}