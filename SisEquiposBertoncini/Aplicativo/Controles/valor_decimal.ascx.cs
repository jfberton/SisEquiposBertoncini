using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SisEquiposBertoncini.Aplicativo.Controles
{
    public partial class valor_decimal : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HabilitaEditar();
        }

        public enum Formato
        {
            ninguno,
            pesos,
            porcentaje
        }

        public Formato Formato_valor { get; set; }

        public string Valor_str
        {
            get
            {
                switch (Formato_valor)
                {
                    case Formato.ninguno: return Valor.ToString();
                    case Formato.pesos: return Valor.ToString("$ #,##0.00");
                    case Formato.porcentaje: return Valor.ToString("P2");
                    default: return "";
                }
            }

            set
            {
                decimal valor = 0;
                decimal.TryParse(value, out valor);
                switch (Formato_valor)
                {
                    case Formato.ninguno:
                        tb_valor.Text = valor.ToString("#,##0.00");
                        break;
                    case Formato.pesos:
                        tb_valor.Text = valor.ToString("$ #,##0.00");
                        break;
                    case Formato.porcentaje:
                        tb_valor.Text = valor.ToString("P2");
                        break;
                    default:
                        break;
                }

            }
        }

        public decimal Valor
        {
            get
            {
                decimal valor = 0;
                switch (Formato_valor)
                {
                    case Formato.ninguno:
                        decimal.TryParse(tb_valor.Text, out valor);
                        break;
                    case Formato.pesos:
                        decimal.TryParse(tb_valor.Text.Replace("$", ""), out valor);
                        break;
                    case Formato.porcentaje:
                        decimal.TryParse(tb_valor.Text.Replace("%", ""), out valor);
                        break;
                    default:
                        break;
                }

                return valor;
            }
            set
            {
                switch (Formato_valor)
                {
                    case Formato.ninguno:
                        tb_valor.Text = value.ToString("#,##0.00");
                        break;
                    case Formato.pesos:
                        tb_valor.Text = value.ToString("$ #,##0.00");
                        break;
                    case Formato.porcentaje:
                        tb_valor.Text = value.ToString("P2");
                        break;
                    default:
                        break;
                }
            }
        }

        private bool Editando { get; set; }

        public event System.EventHandler Modifico_valor;

        protected void btn_edit_ServerClick(object sender, EventArgs e)
        {
            Editando = true;
            HabilitaEditar();
        }

        protected void btn_ok_ServerClick(object sender, EventArgs e)
        {
            Editando = false;
            HabilitaEditar();

            if (Formato_valor == Formato.porcentaje)
            {
                decimal valor = 0;
                decimal.TryParse(tb_valor.Text, out valor);
                valor = valor / Convert.ToDecimal(100);
                Valor = valor;
            }
            else
            {
                Valor_str = tb_valor.Text;
            }

            if (this.Modifico_valor != null)
                this.Modifico_valor(this, new EventArgs());
        }

        protected void btn_cancel_ServerClick(object sender, EventArgs e)
        {
            Editando = false;
            HabilitaEditar();
        }

        private void HabilitaEditar()
        {
            btn_edit.Visible = !Editando;
            btn_cancel.Visible = Editando;
            btn_ok.Visible = Editando;
            tb_valor.Enabled = Editando;
        }


    }
}