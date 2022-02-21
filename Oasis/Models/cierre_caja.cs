
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
    
public partial class cierre_caja
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public cierre_caja()
    {

        this.detalle_cierre_caja = new HashSet<detalle_cierre_caja>();

    }


    public int id_cierre_caja { get; set; }

    public Nullable<System.DateTime> fecha_creacion { get; set; }

    public Nullable<System.DateTime> fecha_modificacion { get; set; }

    public Nullable<int> id_entidad_origen { get; set; }

    public string usuario_creacion { get; set; }

    public string usuario_modificacion { get; set; }

    public decimal caja_chica { get; set; }

    public decimal canje { get; set; }

    public decimal diferencia { get; set; }

    public int estado { get; set; }

    public System.DateTime fecha_hasta { get; set; }

    public Nullable<int> id_dispositivo_sincronizacion { get; set; }

    public int id_organizacion { get; set; }

    public int id_sucursal { get; set; }

    public string nombre_sucursal { get; set; }

    public string numero { get; set; }

    public decimal total_usuario { get; set; }

    public decimal valor { get; set; }

    public Nullable<int> id_caja { get; set; }

    public int id_usuario { get; set; }

    public string codigo_movil { get; set; }



    public virtual caja caja { get; set; }

    public virtual usuario usuario { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<detalle_cierre_caja> detalle_cierre_caja { get; set; }

}

}
