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
    
    public partial class guia_urbano_troq
    {
        public int id_guia_urbano_troq { get; set; }
        public string identificacion { get; set; }
        public Nullable<int> id_factura { get; set; }
        public Nullable<int> id_organizacion { get; set; }
        public string empresa { get; set; }
        public string secuencial_factura { get; set; }
        public string provincia { get; set; }
        public string ciudad { get; set; }
        public string parroquia { get; set; }
        public Nullable<System.DateTime> fecha_factura { get; set; }
        public Nullable<decimal> valor_factura { get; set; }
        public Nullable<System.DateTime> fecha_guia_troquelada { get; set; }
        public string direccion { get; set; }
        public Nullable<decimal> peso { get; set; }
        public string serial_urbano { get; set; }
        public Nullable<int> bultos { get; set; }
    }
}
