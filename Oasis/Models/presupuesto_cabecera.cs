
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
    
public partial class presupuesto_cabecera
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public presupuesto_cabecera()
    {

        this.presupuesto_detalle = new HashSet<presupuesto_detalle>();

    }


    public int id_presupuesto { get; set; }

    public string empresa { get; set; }

    public string sucursal { get; set; }

    public string descripcion { get; set; }

    public System.DateTime fecha_desde { get; set; }

    public System.DateTime fecha_hasta { get; set; }

    public bool activo { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<presupuesto_detalle> presupuesto_detalle { get; set; }

}

}
