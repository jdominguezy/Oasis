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
    
    public partial class Reporte_Analisis_Costos
    {
        public string empresa { get; set; }
        public string planta { get; set; }
        public Nullable<int> mes { get; set; }
        public string mes_cierre { get; set; }
        public string orden_pro { get; set; }
        public string lote { get; set; }
        public string producto { get; set; }
        public Nullable<decimal> cant_elaborar { get; set; }
        public Nullable<decimal> cantidad_fabricada { get; set; }
        public decimal precio_ultima_venta { get; set; }
        public Nullable<decimal> costo_mod { get; set; }
        public int id_orden_fabricacion { get; set; }
        public Nullable<decimal> costo_me { get; set; }
        public Nullable<decimal> costo_mp { get; set; }
        public Nullable<decimal> costo_otros { get; set; }
        public string tipo_orden { get; set; }
        public Nullable<int> cajas_elaboradas { get; set; }
        public Nullable<int> costo_un { get; set; }
        public Nullable<int> costo_caja { get; set; }
        public Nullable<int> costo_total { get; set; }
        public Nullable<int> rendimiento { get; set; }
        public Nullable<System.DateTime> fecha_cierre { get; set; }
        public string codigo_pro { get; set; }
    }
}