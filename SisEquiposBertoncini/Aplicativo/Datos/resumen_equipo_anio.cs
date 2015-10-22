using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisEquiposBertoncini.Aplicativo.Datos
{
    public class resumen_equipo_anio
    {
        public enum agrupaciones
        {
            enero = 1,
            febrero = 2,
            marzo = 3,
            abril = 4,
            mayo = 5,
            junio = 6,
            julio = 7,
            agosto = 8,
            septiembre = 9,
            octubre = 10,
            noviembre = 11,
            diciembre = 12,
            total_anio = 13,
            promedio_mensual = 14
        }

        public struct valor_item_mes
        {
            public int id_concepto { get; set; }
            public agrupaciones agrupacion { get; set; }
            public decimal valor { get; set; }
        }

        private List<valor_item_mes> Valores_anio = new List<valor_item_mes>();

        public resumen_equipo_anio(int anio, int id_equipo)
        {
            using (var cxt = new Model1Container())
            {
                foreach (Item_ingreso_egreso concepto in cxt.Items_ingresos_egresos)
                {
                    decimal total_concepto_anio = 0;
                    decimal meses_cargados_anio = 0;
                    for (int i = 0; i < 12; i++)
                    {
                        Ingreso_egreso_mensual_equipo valor_equipo_mes = cxt.Ingresos_egresos_mensuales_equipos.FirstOrDefault(x => x.id_equipo == id_equipo && x.mes == i + 1 && x.anio == anio);
                        if (valor_equipo_mes != null)
                        {
                            Valor_mes vm = valor_equipo_mes.Valores_meses.FirstOrDefault(ii => ii.id_item == concepto.id_item && ii.id_ingreso_egreso_mensual == valor_equipo_mes.id_ingreso_egreso_mensual);
                            if (vm != null)
                            {
                                Valores_anio.Add(new valor_item_mes() { id_concepto = concepto.id_item, agrupacion = ((agrupaciones)i + 1), valor = vm.valor });
                                total_concepto_anio += vm.valor;
                            }
                            else
                            {
                                Valores_anio.Add(new valor_item_mes() { id_concepto = concepto.id_item, agrupacion = ((agrupaciones)i + 1), valor = 0 });
                            }
                            meses_cargados_anio++;
                        }
                        else
                        {
                            Valores_anio.Add(new valor_item_mes() { id_concepto = concepto.id_item, agrupacion = ((agrupaciones)i + 1), valor = 0 });
                        }
                    }

                    decimal promedio = meses_cargados_anio > 0 ? total_concepto_anio / meses_cargados_anio : 0;
                    Valores_anio.Add(new valor_item_mes() { id_concepto = concepto.id_item, agrupacion = agrupaciones.total_anio, valor = total_concepto_anio });
                    Valores_anio.Add(new valor_item_mes() { id_concepto = concepto.id_item, agrupacion = agrupaciones.promedio_mensual, valor = promedio });
                }
            }
        }

        public List<valor_item_mes> Obtener_agrupacion_por_concepto(int id_concepto)
        {
            return Valores_anio.Where(x => x.id_concepto == id_concepto).ToList();
        }


        public enum conceptos_analisis_economico_financiero
        {
            econo_resultado,
            econo_impuesto,
            econo_resultado_despues_impuestos,
            finan_resultado,
            finan_impuesto,
            finan_resultado_despues_impuestos
        }

        public struct resultados_economicos_financieros
        {
            public agrupaciones agrupacion { get; set; }
            public decimal valor { get; set; }
            public conceptos_analisis_economico_financiero tipo { get; set; }
        }

        /// <summary>
        /// devuelve los resultados ecoómicos de todas las agrupaciones
        /// </summary>
        public List<resultados_economicos_financieros> analisis_economico_financiero()
        {
            List<resultados_economicos_financieros> todos = new List<resultados_economicos_financieros>();
            List<resultados_economicos_financieros> resultado_economico = new List<resultados_economicos_financieros>();
            List<resultados_economicos_financieros> impuesto_economico = new List<resultados_economicos_financieros>();
            List<resultados_economicos_financieros> resultado_menos_impuesto_economico = new List<resultados_economicos_financieros>();
            List<resultados_economicos_financieros> resultado_financiero = new List<resultados_economicos_financieros>();
            List<resultados_economicos_financieros> impuesto_financiero = new List<resultados_economicos_financieros>();
            List<resultados_economicos_financieros> resultado_menos_impuesto_financiero = new List<resultados_economicos_financieros>();

            using (var cxt = new Model1Container())
            {
                Item_ingreso_egreso ingreso = cxt.Items_ingresos_egresos.FirstOrDefault(x => x.nombre == "INGRESOS" && x.Padre == null);
                Item_ingreso_egreso egreso = cxt.Items_ingresos_egresos.FirstOrDefault(x => x.nombre == "EGRESOS" && x.Padre == null);
                Item_ingreso_egreso costos_fijos_erogables = cxt.Items_ingresos_egresos.FirstOrDefault(x => x.nombre == "Costos Fijos Erogables" && x.Padre.nombre == "Costos Fijos");
                Item_ingreso_egreso costos_variables = cxt.Items_ingresos_egresos.FirstOrDefault(x => x.nombre == "Costos Variables" && x.Padre.nombre == "EGRESOS");
                
                if (ingreso != null && egreso != null)
                {
                    #region Economico
                    for (int i = 0; i < 14; i++)
                    {
                        agrupaciones agrupacion = ((agrupaciones)i + 1);
                        decimal valor = Valores_anio.First(x => x.id_concepto == ingreso.id_item && x.agrupacion == agrupacion).valor - Valores_anio.First(x => x.id_concepto == egreso.id_item && x.agrupacion == agrupacion).valor;
                        resultado_economico.Add(new resultados_economicos_financieros()
                        {
                            agrupacion = agrupacion,
                            tipo = conceptos_analisis_economico_financiero.econo_resultado,
                            valor = valor
                        });

                        todos.Add(new resultados_economicos_financieros()
                        {
                            agrupacion = agrupacion,
                            tipo = conceptos_analisis_economico_financiero.econo_resultado,
                            valor = valor
                        });
                    }

                    for (int i = 0; i < 14; i++)
                    {
                        agrupaciones agrupacion = ((agrupaciones)i + 1);
                        decimal valor = resultado_economico.First(x => x.agrupacion == agrupacion).valor * Convert.ToDecimal(Convert.ToDecimal(35) / Convert.ToDecimal(100));
                        impuesto_economico.Add(
                            new resultados_economicos_financieros()
                            {
                                agrupacion = agrupacion,
                                tipo = conceptos_analisis_economico_financiero.econo_impuesto,
                                valor = valor
                            });

                        todos.Add(
                            new resultados_economicos_financieros()
                            {
                                agrupacion = agrupacion,
                                tipo = conceptos_analisis_economico_financiero.econo_impuesto,
                                valor = valor
                            });
                    }

                    for (int i = 0; i < 14; i++)
                    {
                        agrupaciones agrupacion = ((agrupaciones)i + 1);
                        decimal valor = resultado_economico.First(x => x.agrupacion == agrupacion).valor - impuesto_economico.First(x => x.agrupacion == agrupacion).valor;
                        impuesto_economico.Add(
                            new resultados_economicos_financieros()
                            {
                                agrupacion = agrupacion,
                                tipo = conceptos_analisis_economico_financiero.econo_resultado_despues_impuestos,
                                valor = valor
                            });

                        todos.Add(
                            new resultados_economicos_financieros()
                            {
                                agrupacion = agrupacion,
                                tipo = conceptos_analisis_economico_financiero.econo_resultado_despues_impuestos,
                                valor = valor
                            });
                    }
                    #endregion

                    #region Financiero

                    for (int i = 0; i < 14; i++)
                    {
                        agrupaciones agrupacion = ((agrupaciones)i + 1);
                        decimal valor = Valores_anio.First(x => x.id_concepto == ingreso.id_item && x.agrupacion == agrupacion).valor - 
                                    Valores_anio.First(x => x.id_concepto == costos_fijos_erogables.id_item && x.agrupacion == agrupacion).valor - 
                                    Valores_anio.First(x => x.id_concepto == costos_variables.id_item && x.agrupacion == agrupacion).valor;

                        resultado_financiero.Add(new resultados_economicos_financieros()
                        {
                            agrupacion = agrupacion,
                            tipo = conceptos_analisis_economico_financiero.finan_resultado,
                            valor = valor
                        });

                        todos.Add(new resultados_economicos_financieros()
                        {
                            agrupacion = agrupacion,
                            tipo = conceptos_analisis_economico_financiero.finan_resultado,
                            valor = valor
                        });
                    }

                    for (int i = 0; i < 14; i++)
                    {
                        agrupaciones agrupacion = ((agrupaciones)i + 1);
                        decimal valor = resultado_financiero.First(x => x.agrupacion == agrupacion).valor * Convert.ToDecimal(Convert.ToDecimal(35) / Convert.ToDecimal(100));
                        impuesto_financiero.Add(
                            new resultados_economicos_financieros()
                            {
                                agrupacion = agrupacion,
                                tipo = conceptos_analisis_economico_financiero.finan_impuesto,
                                valor = valor
                            });

                        todos.Add(
                            new resultados_economicos_financieros()
                            {
                                agrupacion = agrupacion,
                                tipo = conceptos_analisis_economico_financiero.finan_impuesto,
                                valor = valor
                            });
                    }

                    for (int i = 0; i < 14; i++)
                    {
                        agrupaciones agrupacion = ((agrupaciones)i + 1);
                        decimal valor =  resultado_financiero.First(x => x.agrupacion == agrupacion).valor - impuesto_financiero.First(x => x.agrupacion == agrupacion).valor;
                        impuesto_economico.Add(
                            new resultados_economicos_financieros()
                            {
                                agrupacion = agrupacion,
                                tipo = conceptos_analisis_economico_financiero.finan_resultado_despues_impuestos,
                                valor =valor
                            });

                        todos.Add(
                            new resultados_economicos_financieros()
                            {
                                agrupacion = agrupacion,
                                tipo = conceptos_analisis_economico_financiero.finan_resultado_despues_impuestos,
                                valor = valor
                            });
                    }

                    #endregion
                }
            }
            
            return todos;
        }
    }
}