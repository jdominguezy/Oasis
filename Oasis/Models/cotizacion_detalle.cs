
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
    
public partial class cotizacion_detalle
{

    public int id_cotizacion_detalle { get; set; }

    public int id_producto_instituciones { get; set; }

    public int id_cotizacion_cabecera { get; set; }

    public decimal cantidad_producto { get; set; }

    public decimal valor_linea { get; set; }

    public decimal porcentaje_diferencia { get; set; }

    public string codigo_alterno { get; set; }



    public virtual producto_instituciones producto_instituciones { get; set; }

    public virtual cotizacion_cabecera cotizacion_cabecera { get; set; }

}

}
