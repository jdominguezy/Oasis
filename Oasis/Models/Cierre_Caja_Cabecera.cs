
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
    
public partial class Cierre_Caja_Cabecera
{

    public int id_organizacion { get; set; }

    public string empresa { get; set; }

    public int id_cierre_caja { get; set; }

    public Nullable<System.DateTime> fecha_creacion { get; set; }

    public string numero { get; set; }

    public System.DateTime fecha_hasta { get; set; }

    public string usuario { get; set; }

    public int id_sucursal { get; set; }

    public string sucursal { get; set; }

    public decimal valor_usuario { get; set; }

    public string estado { get; set; }

    public string codigo_movil { get; set; }

    public Nullable<int> id_caja { get; set; }

    public string caja { get; set; }

    public Nullable<int> id_usuario { get; set; }

}

}
