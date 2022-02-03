using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oasis.Models
{
    public class Presupuesto_Consolidado_Result
    {
        public int id_vendedor { get; set; }

        public string nombre_vendedor { get; set; }

        public Nullable<double> valor_venta { get; set; }

        public Nullable<decimal> ventas_brutas { get; set; }

        public Nullable<decimal> total_nc { get; set; }

        public Nullable<decimal> ventas_neta { get; set; }

        public Nullable<double> alcance_venta { get; set; }

        public Nullable<double> valor_cobro { get; set; }

        public Nullable<decimal> total_cobros { get; set; }

        public Nullable<double> alcance_cobro { get; set; }

        public string empresa { get; set; }

        public string sucursal { get; set; }

    }
}