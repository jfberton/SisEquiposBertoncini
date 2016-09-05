using System;
using SisEquiposBertoncini.Aplicativo.Controles;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SisEquiposBertoncini.Aplicativo.Datos;
using SisEquiposBertoncini.Aplicativo.Seguridad;
using System.Web.Security;

namespace SisEquiposBertoncini
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();

                switch (Request.QueryString["mode"])
                {
                    case "session_end":
                        MessageBox.Show(this, "Su sesión ha caducado", MessageBox.Tipo_MessageBox.Default, "Ups!");
                        break;
                    case "trucho":
                        Response.Write("<script>" +
                          "alert('Su perfil tiene definidos los permisos para ingresar en ese directorio.');" +
                          "location.href='Default.aspx';" + "</script>");
                        break;
                    default:
                        Session["CXT"] = new Model1Container();
                        Session["UsuarioLogueado"] = null;
                        inputUsr.Focus();
                        break;
                }
            }
        }

        protected void btn_ingresar_Click(object sender, EventArgs e)
        {
            using (var cxt = new Model1Container())
            {
                if (cxt.Database.CreateIfNotExists())
                {
                    InicializarDB();
                }

                string clave = Cripto.Encriptar(inputPassword.Value);
                Usuario usr = cxt.Usuarios.FirstOrDefault(uu => uu.user == inputUsr.Value && uu.pass == clave);
                if (usr != null)
                {
                    Session["UsuarioLogueado"] = usr;
                    FormsAuthentication.RedirectFromLoginPage(usr.nombre, false);
                }
                else
                {
                    MessageBox.Show(this, "Usuario o contraseña incorrecta!", MessageBox.Tipo_MessageBox.Danger, "Datos de acceso incorrectos");
                }

            }
        }

        protected void btn_ver_demo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AdminLTE/index.html");
        }

        private void InicializarDB()
        {
            using (var cxt = new Model1Container())
            {
                string clave = Cripto.Encriptar("123456");

                Usuario usr = new Usuario()
                {
                    nombre = "José Federico Bertoncini",
                    user = "berton",
                    pass = clave,
                    perfil = perfil_usuario.Admin
                };

                cxt.Usuarios.Add(usr);

                cxt.Areas.Add(new Area() { nombre = "Taller" });

                

                cxt.Categorias_empleados.Add(new Categoria_empleado() { nombre = "Administrativo", descripcion = "" });
                cxt.Categorias_empleados.Add(new Categoria_empleado() { nombre = "Mecánico", descripcion = "" });
                cxt.Categorias_empleados.Add(new Categoria_empleado() { nombre = "Soldador", descripcion = "" });
                cxt.Categorias_empleados.Add(new Categoria_empleado() { nombre = "Pintor", descripcion = "" });

                
                cxt.Categorias_equipos.Add(new Categoria_equipo() { nombre = "Gruas", descripcion = "" });
                cxt.Categorias_equipos.Add(new Categoria_equipo() { nombre = "Camiones y carretones", descripcion = "" });
                cxt.Categorias_equipos.Add(new Categoria_equipo() { nombre = "Vehículos menores", descripcion = "" });
                cxt.Categorias_equipos.Add(new Categoria_equipo() { nombre = "Ventas", descripcion = "Equipos dedicados a la venta" });
                cxt.Categorias_equipos.Add(new Categoria_equipo() { nombre = "Trabajos particulares", descripcion = "Agrupa trabajos particulares" });
                Categoria_equipo ce = new Categoria_equipo() { nombre = "Otros", descripcion = "Agrupa otros equipos - conceptos" };
                cxt.Categorias_equipos.Add(ce);
                cxt.SaveChanges();

                cxt.Equipos.Add(new Equipo() { Categoria = ce, Generico = true, nombre = "Ausencia", fecha_baja = null, OUT = false, notas = "" });
                cxt.Equipos.Add(new Equipo() { Categoria = ce, Generico = true, nombre = "Guardia", fecha_baja = null, OUT = false, notas = "" });
                cxt.Equipos.Add(new Equipo() { Categoria = ce, Generico = true, nombre = "Varios Taller", fecha_baja = null, OUT = false, notas = "" });
                cxt.SaveChanges();

                Item_ingreso_egreso ingreso = new Item_ingreso_egreso() { tipo = "Ingreso", nombre = "INGRESOS", descripcion = "Representa el total de los ingresos que produce el equipo" };
                    Item_ingreso_egreso Trabajado = new Item_ingreso_egreso() { tipo = "Ingreso", nombre = "Trabajado", descripcion = "", Padre = ingreso };
                    Item_ingreso_egreso Interno = new Item_ingreso_egreso() { tipo = "Ingreso", nombre = "Interno", descripcion = "", Padre = ingreso };
                    Item_ingreso_egreso Presupuestado = new Item_ingreso_egreso() { tipo = "Ingreso", nombre = "Presupuestado", descripcion = "", Padre = ingreso };
                    Item_ingreso_egreso Impuestos = new Item_ingreso_egreso() { tipo = "Ingreso", nombre = "Impuestos", descripcion = "", Padre = ingreso };


                Item_ingreso_egreso egreso = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "EGRESOS", descripcion = "Representa el total de los egresos que produce el equipo" };
                    Item_ingreso_egreso costos_fijos = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Costos Fijos", descripcion = "", Padre = egreso };
                        Item_ingreso_egreso costos_fijos_erogables = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Costos Fijos Erogables", descripcion = "", Padre = costos_fijos };
                            Item_ingreso_egreso sueldo_chofer_ponderado = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Sueldo y CS Chofer (Ponderado)", descripcion = "", Padre = costos_fijos_erogables };
                            Item_ingreso_egreso seguro_terceros = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Seguro Terceros", descripcion = "", Padre = costos_fijos_erogables };
                            Item_ingreso_egreso seguro_tecnico = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Seguro Técnico ", descripcion = "", Padre = costos_fijos_erogables };
                            Item_ingreso_egreso seguro_compartido = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Seguro Compartido", descripcion = "", Padre = costos_fijos_erogables };
                            Item_ingreso_egreso control_satelitar = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Control Satelital", descripcion = "", Padre = costos_fijos_erogables };            
                            Item_ingreso_egreso bureou_veritas = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Bureau Veritas", descripcion = "", Padre = costos_fijos_erogables };
                            Item_ingreso_egreso patente = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Patente (bimestral)", descripcion = "", Padre = costos_fijos_erogables };            
                        Item_ingreso_egreso costos_fijos_no_erogables = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Costos Fijos No Erogables", descripcion = "", Padre = costos_fijos };
                            Item_ingreso_egreso amortizacion = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Amortización", descripcion = "", Padre = costos_fijos_no_erogables };            
                
                    Item_ingreso_egreso costos_variables = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Costos Variables", descripcion = "", Padre = egreso };
                        Item_ingreso_egreso repuestos = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Repuestos y Reparac. FC y PP", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso combustible = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Combustible", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso combustible_playa = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Combustible Playa", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso lubricantes = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Lubricantes", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso filtros = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Filtros", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso neumaticos = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Neumáticos", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso pintura = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Pintura", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso izaje = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Elem. de Izaje Individual", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso camioneta = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Gastos Camioneta Individ.", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso alquiler_auto = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Alquiler Auto", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso multas = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Multas y comisiones", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso multas_pp = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Multas y comisiones PP", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso viaticos = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Viaticos FC", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso viaticos_pp = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Viaticos Presupuestado", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso horas_extra = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Horas Extra Chofer", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso accesorios = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Accesorios Hs Extra 24% (1/12x2)", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso mano_obra = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Mano Obra Ayudantes", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso otros = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Otros", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso otros_insumos = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Otros Insumos CF PP", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso varios = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Varios ", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso indumentaria = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Indumentaria p personal(incluye elem de protección personal)", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso telefono_celular = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Teléfono celular (abono)", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso herramientas = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Herramienta ", descripcion = "", Padre = costos_variables };
                        Item_ingreso_egreso fletes = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Fletes", descripcion = "", Padre = costos_variables };
                        
                        Item_ingreso_egreso costos_variables_en_funcion = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Costos Variab. En Función FC", descripcion = "", Padre = costos_variables };
                            Item_ingreso_egreso elementos_de_izaje = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Elem. De Izaje General", descripcion = "", Padre = costos_variables_en_funcion };
                            Item_ingreso_egreso gastos_camioneta_general = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Gastos Camioneta General", descripcion = "", Padre = costos_variables_en_funcion };
                            Item_ingreso_egreso gastos_administracion = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Gastos Administración", descripcion = "", Padre = costos_variables_en_funcion };
                        
                        Item_ingreso_egreso soldadores_segun_horas_hombre = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Soldadores Según Hs Hbre.", descripcion = "", Padre = costos_variables };
                            Item_ingreso_egreso mano_obra_soldadores = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Mano Obra Soldadores", descripcion = "", Padre = soldadores_segun_horas_hombre };
                            Item_ingreso_egreso insumos_soldadores = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Insumos Soldadores", descripcion = "", Padre = soldadores_segun_horas_hombre };
                            Item_ingreso_egreso herramientas_soldadores = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Herramientas Soldadores", descripcion = "", Padre = soldadores_segun_horas_hombre };
                            Item_ingreso_egreso viaticos_soldadores = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Viaticos Soldadores", descripcion = "", Padre = soldadores_segun_horas_hombre };
                            Item_ingreso_egreso viaticos_pp_soldadores = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Viaticos Presup Soldadores", descripcion = "", Padre = soldadores_segun_horas_hombre };

                        Item_ingreso_egreso mecanicos_segun_horas_hombre = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Mecánicos Según Hs Hbre.", descripcion = "", Padre = costos_variables };
                            Item_ingreso_egreso mano_obra_mecanicos = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Mano Obra Taller", descripcion = "", Padre = mecanicos_segun_horas_hombre };
                            Item_ingreso_egreso insumos_mecanicos = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Insumos Taller", descripcion = "", Padre = mecanicos_segun_horas_hombre };
                            Item_ingreso_egreso herramientas_mecanicos = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Herramientas Taller", descripcion = "", Padre = mecanicos_segun_horas_hombre };
                            Item_ingreso_egreso viaticos_mecanicos = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Viaticos Taller", descripcion = "", Padre = mecanicos_segun_horas_hombre };
                            Item_ingreso_egreso viaticos_pp_mecanicos = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Viaticos Presup Taller", descripcion = "", Padre = mecanicos_segun_horas_hombre };
                
                        Item_ingreso_egreso pintores_segun_horas_hombre = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Pintor Según Hs Hbre.", descripcion = "", Padre = costos_variables };
                            Item_ingreso_egreso mano_obra_pintores = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Mano Obra Pintor", descripcion = "", Padre = pintores_segun_horas_hombre };
                            Item_ingreso_egreso insumos_pintores = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Insumos Pintor", descripcion = "", Padre = pintores_segun_horas_hombre };
                            Item_ingreso_egreso herramientas_pintores = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Herramientas Pintor", descripcion = "", Padre = pintores_segun_horas_hombre };
                            Item_ingreso_egreso viaticos_pintores = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Viaticos Pintor", descripcion = "", Padre = pintores_segun_horas_hombre };
                            Item_ingreso_egreso viaticos_pp_pintores = new Item_ingreso_egreso() { tipo = "Egreso", nombre = "Viaticos Presup Pintor", descripcion = "", Padre = pintores_segun_horas_hombre };


                cxt.Items_ingresos_egresos.Add(ingreso);
                cxt.Items_ingresos_egresos.Add(Trabajado);
                cxt.Items_ingresos_egresos.Add(Interno);
                cxt.Items_ingresos_egresos.Add(Presupuestado);
                cxt.Items_ingresos_egresos.Add(Impuestos);
                cxt.Items_ingresos_egresos.Add(egreso);
                cxt.Items_ingresos_egresos.Add(costos_fijos);
                cxt.Items_ingresos_egresos.Add(costos_fijos_erogables);
                cxt.Items_ingresos_egresos.Add(sueldo_chofer_ponderado);
                cxt.Items_ingresos_egresos.Add(seguro_terceros);
                cxt.Items_ingresos_egresos.Add(seguro_tecnico);
                cxt.Items_ingresos_egresos.Add(seguro_compartido);
                cxt.Items_ingresos_egresos.Add(control_satelitar);
                cxt.Items_ingresos_egresos.Add(bureou_veritas);
                cxt.Items_ingresos_egresos.Add(patente);
                cxt.Items_ingresos_egresos.Add(costos_fijos_no_erogables);
                cxt.Items_ingresos_egresos.Add(amortizacion);
                cxt.Items_ingresos_egresos.Add(costos_variables);
                cxt.Items_ingresos_egresos.Add(repuestos);
                cxt.Items_ingresos_egresos.Add(combustible);
                cxt.Items_ingresos_egresos.Add(combustible_playa);
                cxt.Items_ingresos_egresos.Add(lubricantes);
                cxt.Items_ingresos_egresos.Add(filtros);
                cxt.Items_ingresos_egresos.Add(neumaticos);
                cxt.Items_ingresos_egresos.Add(pintura);
                cxt.Items_ingresos_egresos.Add(izaje);
                cxt.Items_ingresos_egresos.Add(camioneta);
                cxt.Items_ingresos_egresos.Add(alquiler_auto);
                cxt.Items_ingresos_egresos.Add(multas);
                cxt.Items_ingresos_egresos.Add(multas_pp);
                cxt.Items_ingresos_egresos.Add(viaticos);
                cxt.Items_ingresos_egresos.Add(viaticos_pp);
                cxt.Items_ingresos_egresos.Add(horas_extra);
                cxt.Items_ingresos_egresos.Add(accesorios);
                cxt.Items_ingresos_egresos.Add(mano_obra);
                cxt.Items_ingresos_egresos.Add(otros);
                cxt.Items_ingresos_egresos.Add(otros_insumos);
                cxt.Items_ingresos_egresos.Add(varios);
                cxt.Items_ingresos_egresos.Add(indumentaria);
                cxt.Items_ingresos_egresos.Add(telefono_celular);
                cxt.Items_ingresos_egresos.Add(herramientas);
                cxt.Items_ingresos_egresos.Add(fletes);
                cxt.Items_ingresos_egresos.Add(costos_variables_en_funcion);
                cxt.Items_ingresos_egresos.Add(elementos_de_izaje);
                cxt.Items_ingresos_egresos.Add(gastos_camioneta_general);
                cxt.Items_ingresos_egresos.Add(gastos_administracion);
                cxt.Items_ingresos_egresos.Add(soldadores_segun_horas_hombre);
                cxt.Items_ingresos_egresos.Add(mano_obra_soldadores);
                cxt.Items_ingresos_egresos.Add(insumos_soldadores);
                cxt.Items_ingresos_egresos.Add(herramientas_soldadores);
                cxt.Items_ingresos_egresos.Add(viaticos_soldadores);
                cxt.Items_ingresos_egresos.Add(viaticos_pp_soldadores);
                cxt.Items_ingresos_egresos.Add(mecanicos_segun_horas_hombre);
                cxt.Items_ingresos_egresos.Add(mano_obra_mecanicos);
                cxt.Items_ingresos_egresos.Add(insumos_mecanicos);
                cxt.Items_ingresos_egresos.Add(herramientas_mecanicos);
                cxt.Items_ingresos_egresos.Add(viaticos_mecanicos);
                cxt.Items_ingresos_egresos.Add(viaticos_pp_mecanicos);
                cxt.Items_ingresos_egresos.Add(pintores_segun_horas_hombre);
                cxt.Items_ingresos_egresos.Add(mano_obra_pintores);
                cxt.Items_ingresos_egresos.Add(insumos_pintores);
                cxt.Items_ingresos_egresos.Add(herramientas_pintores);
                cxt.Items_ingresos_egresos.Add(viaticos_pintores);
                cxt.Items_ingresos_egresos.Add(viaticos_pp_pintores);

                cxt.SaveChanges();
            }
        }
    }
}