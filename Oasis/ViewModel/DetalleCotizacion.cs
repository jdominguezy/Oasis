using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oasis.Models;

namespace Oasis.ViewModel
{
    public class Cotizacion
    {
        public int id_empresa { get; set; }
        public DateTime fecha_documento { get; set; }
        public int id_cliente { get; set; }
        public string forma_pago { get; set; }
        public int meses_tiempo_garantia { get; set; }
        public int dias_tiempo_entrega { get; set; }
        public string persona_contacto { get; set; }
        public decimal total { get; set; }
        public List<detalle_cotizacion> ListaDetalleCotizacion { get; set; } 
    }

    public class detalle_cotizacion
    {
        public int id_cpc { get; set; }
        public int id_producto { get; set; }
        public string codigo_alterno { get; set; }
        public decimal cantidad { get; set; }
        public decimal valor_unitario { get; set; }
        public decimal subtotal_linea { get; set; }
    }
}