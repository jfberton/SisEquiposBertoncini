using SisEquiposBertoncini.Aplicativo.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisEquiposBertoncini.Aplicativo.Controles
{
    public class Horas_string
    {
        /// <summary>
        /// Determina si una hora es distinto de cero
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool HoraNoNula(string p)
        {
            int iHora = Convert.ToInt16(p.Split(':')[0]);
            int iMin = Convert.ToInt16(p.Split(':')[1]);
            return iHora + iMin > 0;
        }


        /// <summary>
        /// Suma una coleccion de string formato hora (000:00)
        /// </summary>
        /// <param name="hora">Coleccion de strings</param>
        /// <returns>string con formato hora (000:00)</returns>
        public static string SumarHoras(string[] hora)
        {
            int horas = 0;
            int min = 0;
            foreach (string h in hora)
            {
                if (h != "")
                {
                    string[] hhmm = h.Split(':');
                    if (hhmm[1].Trim().Length > 0)
                    {
                        min += Convert.ToInt32(hhmm[1].Trim());
                        while (min >= 60)
                        {
                            horas += 1;
                            min -= 60;
                        }
                    }

                    if (hhmm[0].Trim().Length > 0)
                    {
                        horas += Convert.ToInt32(hhmm[0].Trim());
                    }
                }
            }
            string ret = FormatearHorasString(horas.ToString() + ":" + min.ToString());

            return ret;
        }

        /// <summary>
        /// Resta dos string con formato horas(000:00)
        /// </summary>
        /// <param name="minuendo">Minuendo de la operacion en formato hora (000:00)</param>
        /// <param name="sustraendo">Sustraendo de la operacion en formato hora (000:00)</param>
        /// <returns>string con el resultado de la resta en formato hora (000:00)</returns>
        public static string RestarHoras(string minuendo, string sustraendo)
        {
            string ret = string.Empty;

            if (AMayorQueB(minuendo, sustraendo))
            {
                ret = CalcularRestaAMenosB(minuendo, sustraendo);
            }
            else
            {
                ret = "-" + CalcularRestaAMenosB(sustraendo, minuendo);
            }

            return ret;
        }

        private static string CalcularRestaAMenosB(string a, string b)
        {
            int horasMinuendo = 0;
            int horasSustraendo = 0;
            int minutosMinuendo = 0;
            int minutosSustraendo = 0;
            int horasResultado = 0;
            int minutosResultado = 0;

            //Cargo Minuendo
            string[] hhmmMin = a.Split(':');
            horasMinuendo = Convert.ToInt32(hhmmMin[0]);
            minutosMinuendo = Convert.ToInt32(hhmmMin[1]);
            //Cargo Sustraendo
            string[] hhmmSus = b.Split(':');
            horasSustraendo = Convert.ToInt32(hhmmSus[0]);
            minutosSustraendo = Convert.ToInt32(hhmmSus[1]);

            //Cálculo
            horasResultado = horasMinuendo - horasSustraendo;
            minutosResultado = minutosMinuendo - minutosSustraendo;

            //Corrijo
            if (horasResultado > 0 && minutosResultado < 0)
            {
                horasResultado -= 1;
                minutosResultado += 60;
            }

            if (horasResultado < 0 && minutosResultado < 0)
            {
                minutosResultado = minutosResultado * -1;
            }

            bool ponerNegativaHoraCero = false;
            if (horasResultado == 0 && minutosResultado < 0)
            {
                minutosResultado = minutosResultado * -1;
                ponerNegativaHoraCero = true;
            }

            string ret = string.Empty;

            ret = FormatearHorasString(horasResultado.ToString() + ":" + minutosResultado.ToString());

            if (ponerNegativaHoraCero)
            {
                ret = "-" + ret;
            }

            return ret;
        }

        /// <summary>
        /// Formatea el string acomodando corractamente los valores
        /// </summary>
        /// <param name="hora">Hora sin formatear ej 0 5:00</param>
        /// <returns>String hora formateado correctamente</returns>
        public static string FormatearHorasString(string hora)
        {
            string ret = string.Empty;
            int horasResultado = Convert.ToInt32(hora.Split(':')[0]);
            int minutosResultado = Convert.ToInt32(hora.Split(':')[1]);

            if (horasResultado >= 0)
            {
                if (horasResultado.ToString().Length == 1) { ret = "00" + horasResultado.ToString(); }
                else
                    if (horasResultado.ToString().Length == 2) { ret = "0" + horasResultado.ToString(); }
                    else
                    { ret = horasResultado.ToString(); }
            }
            else
            {
                if (horasResultado.ToString().Length == 1) { ret = "-00" + Convert.ToInt32(Math.Abs(horasResultado)).ToString(); }
                else
                    if (horasResultado.ToString().Length == 2)
                    { ret = "-0" + Convert.ToInt32(Math.Abs(horasResultado)).ToString(); }
                    else
                    { ret = horasResultado.ToString(); }
            }
            ret += ":";
            if (minutosResultado.ToString().Length == 1) { ret += "0" + minutosResultado.ToString(); }
            else { ret += minutosResultado.ToString(); }
            return ret;
        }


        /// <summary>
        /// Determina si un string en formato 000:00 es mayor que otro, TENER EN CUENTA QUE SON VALORES ABSOLUTOS, SIN SIGNO
        /// </summary>
        /// <param name="a">Valor A (Sin signo)</param>
        /// <param name="b">Valor B (Sin signo)</param>
        public static bool AMayorQueB(string a, string b)
        {
            int horasA = Convert.ToInt32(a.Split(':')[0]);
            int minA = Convert.ToInt32(a.Split(':')[1]);

            int horasB = Convert.ToInt32(b.Split(':')[0]);
            int minB = Convert.ToInt32(b.Split(':')[1]);

            if (horasA > horasB)
            {
                return true;
            }
            else
            {
                if (horasA == horasB)
                {
                    if (minA > minB)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }


        public static decimal HorasADecimales(string strhora)
        { 
            decimal hora = Convert.ToDecimal(strhora.Split(':')[0]);
            decimal minutos =  Convert.ToDecimal(strhora.Split(':')[1]);
            decimal minutos_divisor = 60;

            return hora + (minutos / minutos_divisor);
        }
    }

}