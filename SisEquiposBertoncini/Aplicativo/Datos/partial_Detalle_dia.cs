using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SisEquiposBertoncini.Aplicativo.Controles;

namespace SisEquiposBertoncini.Aplicativo.Datos
{
    public partial class Detalle_dia
    {
        /// <summary>
        /// Devuelve el total en horas del movimiento
        /// </summary>
        public string total_movimiento
        {
            get {
                return Horas_string.RestarHoras(hora_hasta, hora_desde);
            }
        }
    }
}