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
    
    public partial class facturas_no_aplicable
    {
        public int id_factura { get; set; }
        public string secuencial_factura { get; set; }
        public int id_organizacion { get; set; }
        public Nullable<bool> indicador_no_afecta_mesa { get; set; }
        public Nullable<bool> indicador_no_afecta_vendedor { get; set; }
        public Nullable<bool> indicador_no_afecta_empresa { get; set; }
    }
}
