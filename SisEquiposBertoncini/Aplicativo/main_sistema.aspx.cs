using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisEquiposBertoncini.Aplicativo.Controles;

namespace SisEquiposBertoncini.Aplicativo
{
    public partial class main_sistema : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_actualiza_horas_Click(object sender, EventArgs e)
        {
            Procesos_Globales.Actualizar_horas_todos_los_resumenes_mensuales();
            MessageBox.Show(this, "Listo! revisa algun mes de soldador", MessageBox.Tipo_MessageBox.Success);
        }
    }
}