
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
    
public partial class Cobros_Vendedor
{

    public int id_consolidado_cierre { get; set; }

    public string cobrador { get; set; }

    public string nom_cobrador { get; set; }

    public string fecha_cobro { get; set; }

    public string num_cobro { get; set; }

    public string fecha_factura { get; set; }

    public string factura { get; set; }

    public Nullable<decimal> valor_factura { get; set; }

    public decimal valor_cobro { get; set; }

    public string identificacion { get; set; }

    public string cliente { get; set; }

    public string cod_forma_pago { get; set; }

    public string forma_pago { get; set; }

    public string fecha_pago { get; set; }

    public int id_detalle_forma_cobro { get; set; }

    public int id_detalle_cobro { get; set; }

}

}
