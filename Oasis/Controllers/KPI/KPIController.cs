using Oasis.Models;
using Oasis.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oasis.Controllers.KPI
{
    [CustomAuthorize(Roles = "KPI")]
    public class KPIController : Controller
    {
        // GET: KPI
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SalidaMaterial()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SalidaMaterial(DateTime fecha_desde, DateTime fecha_hasta)
        {
            using(var db = new as2oasis()){
                var _fecha_desde = Convert.ToDateTime(fecha_desde);
                var _fecha_hasta = Convert.ToDateTime(fecha_hasta);
                return Json(db.KPI_Salida_Material.Where(x =>
                    x.FECHA_FABRICACION >= fecha_desde && 
                    x.FECHA_FABRICACION <= fecha_hasta)
                    .ToList()
                    .Select(x => new
                    {
                        fecha_fabricacion = x.FECHA_FABRICACION.ToShortDateString(),
                        x.ORDEN_FABRICACION,
                        x.PLANTA,
                        x.LOTE,
                        x.CODIGO_PT,
                        x.PT,
                        x.CANTIDAD,
                        fecha_salida_material = x.FECHA_SALIDA_MATERIAL.ToShortDateString(),
                        x.PEDIDO_SALIDA
                    }));
            }
        }
    }
}