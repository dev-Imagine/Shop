using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
    public class ProductController : Controller
    {
        // VISTAS
        [HttpPost]
        public ActionResult ProductList(string stSearch)
        {
            srvProduct sPro = new srvProduct();
            ViewBag.cantProductos = sPro.TotalProductosBusqueda(stSearch);
            ViewBag.search = stSearch;
            srvCategories sCategoria = new srvCategories();
            ViewBag.lstCategorias = sCategoria.ObtenerCategorias();
            return View();
        }
        public ActionResult ProductDetail(int idProducto)
        {
            srvProduct sProd = new srvProduct();
            ViewBag.oProducto = sProd.obtenerProducto(idProducto);
            return View();
        }
        
        // VISTAS PARCIALES

        public ActionResult _ProductList(int pageIndex, int pageSize, string stSearch)
        {
            srvProduct sPro = new srvProduct();
            int cantProductos = sPro.TotalProductosBusqueda(stSearch);
            if (pageIndex * pageSize >= cantProductos)
            {
                ViewBag.complete = true;
                ViewBag.lstProductos = new List<Producto>();
            }
            else
            {
                ViewBag.complete = false;
                ViewBag.lstProductos = sPro.obtenerProductos(pageIndex, pageSize, stSearch);
            }
            return PartialView();
        }
        [HttpPost]
        public ActionResult _PopUpGuardarModificarCategoria(int idCategoria = 0)
        {
            try
            {
                srvCategories sCategoria = new srvCategories();
                
                return PartialView(sCategoria.ObtenerCategoria(idCategoria));
            }
            catch (Exception ex)
            {

                string exx = ex.ToString();
                return PartialView();
            }
            
        }

        //Metodos
        public JsonResult ObtenerSubcategoriaDeCategoria(int idCategoria)
        {
            try
            {
                srvCategories sCategoria = new srvCategories();
                string stListaOption = "<option selected value=\"0\">Subcategoria</option>";
                foreach (SubCategoria oSubcategoria in sCategoria.ObtenerSubCategorias(idCategoria))
                {
                    stListaOption = stListaOption + "<option value=\"" + oSubcategoria.idSubCategoria + "\">" + oSubcategoria.nombre + "</option>";
                }
                return Json(stListaOption);
            }
            catch (Exception)
            {
                return Json("<option selected value=\"0\">Subcategoria</option>");
            }

        }
        public JsonResult GuardarModificarCategoria(Categoria oCategoria, string[] Subcategorias, HttpPostedFileBase imagen)
        {
            try
            {
                Categoria oCatImagen = new Categoria();
                //usuario oUsuario = (usuario)Session["Usuario"];
                //if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                //{
                //    throw new Exception();
                //}
                oCategoria.nombre = oCategoria.nombre.ToUpper();
                SubCategoria oSubcategoria;
                foreach (string stCategoria in Subcategorias)
                {
                    string[] stCat = stCategoria.Split(';');
                    oSubcategoria = new SubCategoria();
                    oSubcategoria.idCategoria = oCategoria.idCategoria;
                    oSubcategoria.idSubCategoria = Convert.ToInt32(stCat[0]);
                    oSubcategoria.nombre = stCat[1].ToUpper();
                    oCategoria.SubCategoria.Add(oSubcategoria);
                }
                srvCategories sCategoria = new srvCategories();
                //Guardar imagen de categoria
                string stNombreArchivo = imagen.FileName.Substring(imagen.FileName.LastIndexOf("\\") + 1).ToString();
                string stRuta = "~/Images/Categories/";
                oCatImagen = sCategoria.ObtenerCategoria(oCategoria.idCategoria);
                if (oCategoria.nombreImagen != oCatImagen.nombreImagen || stNombreArchivo == "404_not_found.jpg" || oCategoria.idCategoria == 0)
                {
                    imagen.SaveAs(Server.MapPath(stRuta + stNombreArchivo));
                    oCategoria.nombreImagen = stNombreArchivo;
                }
                oCategoria = sCategoria.GuardarModificarCategoria(oCategoria);

                return Json(oCategoria.idCategoria + ";" + oCategoria.nombre);
            }
            catch (Exception)
            {
                return Json("");
            }
        }
        public JsonResult EliminarSubCategoria(int id_Subcategoria)
        {
            try
            {
                //usuario oUsuario = (usuario)Session["Usuario"];
                //if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                //{
                //    throw new Exception();
                //}
                srvCategories sCategoria = new srvCategories();
                sCategoria.EliminarSubcategoria(id_Subcategoria);
                return Json("True");
            }
            catch (Exception)
            {
                return Json("No se ha podido eliminar la subcategoría. Verifique que no tenga productos asignados.");
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GuardarModificarProducto(Producto oProducto,  string precio, HttpPostedFileBase imagen)
        {
            try
            {
                //usuario oUsuario = (usuario)Session["Usuario"];
                //if (oUsuario == null || oUsuario.idTipoUsuario != 2)
                //{
                //    Session.Clear();
                //    return RedirectToAction("Index", "Home");
                //}
                srvProduct sProducto = new srvProduct();
                oProducto.precio = Convert.ToDecimal(precio.Replace(".", ","));               
                sProducto.GuardarModificarProducto(oProducto);
                string stNombreArchivo = imagen.FileName.Substring(imagen.FileName.LastIndexOf("\\") + 1).ToString();
                string stRuta = "~/Images/Product/";                
                imagen.SaveAs(Server.MapPath(stRuta + stNombreArchivo));
                sProducto.guardarImagen(oProducto.idProducto, stNombreArchivo);
                return RedirectToAction("ViewProduct","Admin");
            }
            catch (Exception)
            {
                //return RedirectToAction("Error", "Error", new { stError = "Se produjo un error al intentar guardar o modificar el producto." });
                return RedirectToAction("Index", "Home");
            }

            
        }
        
    }
}