
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
    
public partial class Cartera_Proveedor
{

    public string empresa { get; set; }

    public string identificacion { get; set; }

    public string nombre_comercial { get; set; }

    public string categoria { get; set; }

    public string secuencial_factura { get; set; }

    public System.DateTime fecha_factura { get; set; }

    public Nullable<System.DateTime> fecha_vencimiento { get; set; }

    public Nullable<decimal> valor_factura { get; set; }

    public Nullable<int> dias_emitida { get; set; }

    public Nullable<int> dias_diferencia { get; set; }

    public decimal saldo { get; set; }

    public string condicion_pago { get; set; }

    public string nota { get; set; }

}

}
