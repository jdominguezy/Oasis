
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
    
public partial class NC_Diario
{

    public string empresa { get; set; }

    public string sucursal { get; set; }

    public string nombre_fiscal { get; set; }

    public Nullable<int> id_categoria_empresa { get; set; }

    public string categoria { get; set; }

    public string identificacion { get; set; }

    public string numero_factura { get; set; }

    public Nullable<System.DateTime> fecha_factura { get; set; }

    public string motivo_nc { get; set; }

    public string secuencial_nc { get; set; }

    public string descripcion_documento { get; set; }

    public Nullable<System.DateTime> fecha_documento { get; set; }

    public string numero_documento { get; set; }

    public Nullable<decimal> valor_nc { get; set; }

    public Nullable<int> id_vendedor { get; set; }

    public string vendedor { get; set; }

    public string nombre_agente_comercial { get; set; }

    public string nota { get; set; }

    public Nullable<decimal> valor_factura { get; set; }

}

}
