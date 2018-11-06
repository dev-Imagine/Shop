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
        [HttpPost]
        public JsonResult AddProduct(int idProducto, int cantidad)
        {
            try
            {
                Venta oVenta = (Venta)Session["Cart"];
                if (oVenta == null) {
                    oVenta = new Venta();
                }
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
                oDetalle.subTotal = oProducto.precio * cantidad;
                oDetalle.cantidad = cantidad;
                oVenta.DetalleVenta.Add(oDetalle);
                oVenta.montoTotal += oDetalle.subTotal;
                Session["Cart"] = oVenta;
                return Json("true");
            }
            catch (Exception)
            {
                return Json("Ocurrió un erro al agreegar el producto.");
            }
        }
    }
}