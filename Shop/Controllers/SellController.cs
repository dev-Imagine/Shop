using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
    public class SellController : Controller
    {
        public ActionResult Cart()
        {
            Venta oVenta = (Venta)Session["Cart"];
            if (oVenta != null && oVenta.DetalleVenta.Count() >= 1)
            {
                ViewBag.Venta = oVenta;
            }
            return View();
        }
        // METODOS
        [HttpPost]
        public JsonResult AddProduct(int idProducto, int cantidad)
        {
            try
            {
                Venta oVenta = (Venta)Session["Cart"];
                if (oVenta == null) {
                    oVenta = new Venta();
                }

                if (oVenta.DetalleVenta.Where(x=> x.idProducto == idProducto).Count() == 1)
                {
                    if (oVenta.DetalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().Producto.stockActual < (cantidad + oVenta.DetalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().cantidad))
                    {
                        return Json("Stock insuficiente.");
                    }

                    oVenta.DetalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().cantidad += cantidad;
                    oVenta.DetalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().subTotal += (oVenta.DetalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().Producto.precio * cantidad) - Convert.ToDecimal(((oVenta.DetalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().Producto.precio * cantidad) * oVenta.DetalleVenta.Where(x => x.idProducto == idProducto).FirstOrDefault().Producto.descuento) / 100);
                   
                    oVenta.montoTotal = oVenta.DetalleVenta.Sum(x => x.subTotal);
                }
                else
                {
                    srvProduct sProduct = new srvProduct();
                    Producto oProducto = sProduct.obtenerProducto(idProducto);

                    if (oProducto.stockActual < cantidad)
                    {
                        return Json("Stock insuficiente.");
                    }
                    DetalleVenta oDetalle = new DetalleVenta();
                    oDetalle.Producto = oProducto; // <---------------- Solo para mostrar
                    oDetalle.idProducto = oProducto.idProducto;
                    oDetalle.precioUnitario = oProducto.precio;
                    oDetalle.descuento = oProducto.descuento;
                    oDetalle.subTotal = (oProducto.precio * cantidad) - Convert.ToDecimal(((oProducto.precio * cantidad) * oDetalle.descuento) / 100);
                    oDetalle.cantidad = cantidad;
                    oVenta.DetalleVenta.Add(oDetalle);
                    oVenta.montoTotal += oDetalle.subTotal;
                    Session["Cart"] = oVenta;
                }


                return Json("true");
            }
            catch (Exception)
            {
                return Json("Ocurrió un erro al agregar el producto.");
            }
        }
        public JsonResult RemoveProduct (int idProducto)
        {
            try
            {
                Venta oVenta = (Venta)Session["Cart"];
                oVenta.DetalleVenta.Remove(oVenta.DetalleVenta.Where(x=> x.idProducto== idProducto).FirstOrDefault());
                Session["Cart"] = oVenta;
                return Json(oVenta.DetalleVenta.Sum(x=>x.cantidad));
            }
            catch (Exception)
            {
                return Json("Ocurrió un erro al quitar el producto.");
            }
        }
    }
}