//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Imagen
    {
        public int idImagen { get; set; }
        public int idProducto { get; set; }
        public string archivo { get; set; }
        public bool principal { get; set; }
    
        public virtual Producto Producto { get; set; }
    }
}
