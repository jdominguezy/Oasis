
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
    
public partial class Mano_Obra
{

    public string empresa { get; set; }

    public string planta { get; set; }

    public string orden_fa { get; set; }

    public System.DateTime fecha { get; set; }

    public string codigo_pro { get; set; }

    public string producto { get; set; }

    public string lote { get; set; }

    public Nullable<decimal> cantidad_fabricada { get; set; }

    public string cod_tarea { get; set; }

    public string tarea { get; set; }

    public string maquina { get; set; }

    public decimal numero_maquinas { get; set; }

    public decimal numero_persona { get; set; }

    public decimal horas_hombre_es { get; set; }

    public decimal horas_maquina { get; set; }

    public decimal horas_hombre { get; set; }

    public string responsable { get; set; }

}

}
