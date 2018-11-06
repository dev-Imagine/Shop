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
    
    public partial class Venta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Venta()
        {
            this.DetalleVenta = new HashSet<DetalleVenta>();
        }
    
        public int idVenta { get; set; }
        public System.DateTime fecha { get; set; }
        public string estadoOrder { get; set; }
        public string estadoShipments { get; set; }
        public string estadoPayments { get; set; }
        public string orderId { get; set; }
        public string collectorId { get; set; }
        public int idLocalidad { get; set; }
        public decimal montoTotal { get; set; }
        public string emailComprador { get; set; }
        public string direccionShipments { get; set; }
    
        public virtual Localidad Localidad { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleVenta> DetalleVenta { get; set; }
    }
}
