
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Oasis.Models
{

using System;
    using System.Collections.Generic;
    
public partial class productos
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public productos()
    {

        this.prov_oc_detalle = new HashSet<prov_oc_detalle>();

    }


    public int id_producto { get; set; }

    public string descripcion { get; set; }

    public decimal valor_unitario { get; set; }

    public string um { get; set; }

    public string categoria { get; set; }

    public bool iva { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<prov_oc_detalle> prov_oc_detalle { get; set; }

}

}
