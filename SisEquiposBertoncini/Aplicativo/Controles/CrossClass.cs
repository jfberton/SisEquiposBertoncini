using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace SisEquiposBertoncini.Aplicativo.Controles
{
    public static class CrossClass
    {
       
        public static string ObtenerEquipoDB(string equipo_excel)
        {
            string directorioRaiz = System.Web.HttpRuntime.AppDomainAppPath;
            XDocument miXML = XDocument.Load(directorioRaiz + "\\Aplicativo\\Controles\\cross.xml");
            string ret = "Sin clasificar";

            var equipoBuscado = miXML.Descendants("Equipos").FirstOrDefault(x => (string)x.Attribute("valor_excel") == equipo_excel);

            if (equipoBuscado != null)
            {
                ret = equipoBuscado.Attribute("valor_database").Value;
            }

            return ret;
        }

        public static string ObtenerItemDB(string equipo_excel)
        {
            string directorioRaiz = System.Web.HttpRuntime.AppDomainAppPath;
            XDocument miXML = XDocument.Load(directorioRaiz + "\\Aplicativo\\Controles\\cross.xml");
            string ret = "Sin clasificar";

            var itemBuscado = miXML.Descendants("Items").FirstOrDefault(x => (string)x.Attribute("valor_excel") == equipo_excel);

            if (itemBuscado != null)
            {
                ret = itemBuscado.Attribute("valor_database").Value;
            }

            return ret;
        }

    }
}