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
    }
}