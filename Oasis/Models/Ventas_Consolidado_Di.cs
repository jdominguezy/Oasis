
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
    
public partial class Ventas_Consolidado_Di
{

    public string empresa { get; set; }

    public string sucursal { get; set; }

    public string identificacion { get; set; }

    public int id_organizacion { get; set; }

    public string secuencial_factura { get; set; }

    public string ciudad { get; set; }

    public System.DateTime fecha_factura { get; set; }

    public Nullable<decimal> valor_factura { get; set; }

    public System.DateTime fecha_guia { get; set; }

    public string descripcion { get; set; }

    public string email { get; set; }

    public string nombre_comercial { get; set; }

    public string categoria { get; set; }

    public Nullable<int> id_vendedor { get; set; }

    public string vendedor { get; set; }

    public string telefono1 { get; set; }

    public string telefono2 { get; set; }

    public string estado { get; set; }

    public Nullable<System.DateTime> fecha_creacion_pedido { get; set; }

    public Nullable<System.DateTime> fecha_autorizacion_pedido { get; set; }

    public string numero { get; set; }

}

}
