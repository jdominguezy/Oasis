
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
    
public partial class prov_oc_detalle
{

    public int id_oc_detalle { get; set; }

    public int id_oc_principal { get; set; }

    public int id_producto { get; set; }

    public decimal cantidad_producto { get; set; }

    public decimal valor_linea { get; set; }

    public decimal iva { get; set; }



    public virtual productos productos { get; set; }

    public virtual prov_oc_principal prov_oc_principal { get; set; }

}

}
