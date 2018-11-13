using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Services
{
    public class srvMetodosGenericos
    {
        public static string GetDimensionsProducts(List<DetalleVenta> lstDetalles)
        {
            int alto = 0;
            int ancho = 0;
            int largo = 0;
            int peso = 0;
            foreach (DetalleVenta oDetalleVenta in lstDetalles)
            {
                alto += oDetalleVenta.Producto.ME_alto * oDetalleVenta.cantidad;
                ancho += oDetalleVenta.Producto.ME_ancho * oDetalleVenta.cantidad;
                largo += oDetalleVenta.Producto.ME_largo * oDetalleVenta.cantidad;
                peso += oDetalleVenta.Producto.ME_peso * oDetalleVenta.cantidad;
            }
            if (alto > 70)
            {
                alto = 70;
            }
            if (ancho > 70)
            {
                ancho = 70;
            }
            if (largo > 70)
            {
                largo = 70;
            }
            if (peso > 25000)
            {
                peso = 25000;
            }
            return alto + "x" + ancho + "x" + largo + "," + peso;
        }
        public static string GetDateSale(DateTime fecha)
        {
            TimeSpan tsDiferenciaFecha = new TimeSpan();
            DateTime dtHoy = DateTime.Now;
            DateTime dtFechaVenta = new DateTime();
            string stTiempoInicio = "";
            tsDiferenciaFecha = fecha - dtHoy;
            if (tsDiferenciaFecha.Ticks <= 0)
            {
                    stTiempoInicio = "Hace ";
                    tsDiferenciaFecha = dtHoy - fecha;
                    if (Convert.ToInt32(tsDiferenciaFecha.TotalDays) < 1)
                    {
                    dtFechaVenta = new DateTime(tsDiferenciaFecha.Ticks);
                        stTiempoInicio = stTiempoInicio + " y " + dtFechaVenta.Hour.ToString().PadLeft(2, '0') + ":" + dtFechaVenta.Minute.ToString().PadLeft(2, '0') + " horas";

                    }
                    else
                    {
                        if (Convert.ToInt32(tsDiferenciaFecha.TotalDays) >= 1 && Convert.ToInt32(tsDiferenciaFecha.TotalDays) <= 7)
                        {
                            stTiempoInicio = stTiempoInicio + Convert.ToInt32(tsDiferenciaFecha.TotalDays).ToString() + " días ";
                        }
                        else
                        {
                            if (Convert.ToInt32(tsDiferenciaFecha.TotalDays) >= 8 && Convert.ToInt32(tsDiferenciaFecha.TotalDays) <= 30)
                            {
                                int i_semanas = Convert.ToInt32(Math.Round(Convert.ToDecimal(tsDiferenciaFecha.TotalDays) / 7, 1, MidpointRounding.AwayFromZero));
                                stTiempoInicio = stTiempoInicio + i_semanas + " semana";
                                if (i_semanas != 1)
                                {
                                    stTiempoInicio = stTiempoInicio + "s";
                                }
                                stTiempoInicio = stTiempoInicio + " ";
                            }
                            else
                            {
                                if (Convert.ToInt32(tsDiferenciaFecha.TotalDays) >= 31 && Convert.ToInt32(tsDiferenciaFecha.TotalDays) <= 365)
                                {
                                    int i_meses = Convert.ToInt32(Math.Round(Convert.ToDecimal(tsDiferenciaFecha.TotalDays) / 30, 1, MidpointRounding.AwayFromZero));
                                    stTiempoInicio = stTiempoInicio + i_meses + " mes";
                                    if (i_meses != 1)
                                    {
                                        stTiempoInicio = stTiempoInicio + "es";
                                    }
                                    stTiempoInicio = stTiempoInicio + " ";
                                }
                                else
                                {
                                    int i_años = Convert.ToInt32(Math.Round(Convert.ToDecimal(tsDiferenciaFecha.TotalDays) / 365, 1, MidpointRounding.AwayFromZero));
                                    stTiempoInicio = stTiempoInicio + i_años + " año";
                                    if (i_años != 1)
                                    {
                                        stTiempoInicio = stTiempoInicio + "s";
                                    }
                                    stTiempoInicio = stTiempoInicio + " ";
                                }
                            }
                        }
                    }
            }
            return  stTiempoInicio;
        }
    }
}