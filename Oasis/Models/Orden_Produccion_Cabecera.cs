
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
    
public partial class Orden_Produccion_Cabecera
{

    public string OP { get; set; }

    public string lote { get; set; }

    public System.DateTime fecha { get; set; }

    public Nullable<System.DateTime> fecha_cierre { get; set; }

    public string codigo { get; set; }

    public Nullable<int> cantidad { get; set; }

    public Nullable<int> cantidad_fabricada { get; set; }

    public Nullable<decimal> rendimiento { get; set; }

    public string nombre { get; set; }

    public string planta { get; set; }

    public int id_orden_fabricacion { get; set; }

    public string nombre_ruta { get; set; }

    public string codigo_mba3 { get; set; }

    public Nullable<decimal> presentacion_producto { get; set; }

}

}
