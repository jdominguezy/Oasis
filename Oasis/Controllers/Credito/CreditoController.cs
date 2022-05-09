using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Oasis.Models;
using Oasis.Models.Login;
using OpenXmlPowerTools;
using static Oasis.Reporte;

namespace Oasis.Controllers.Credito
{
    [CustomAuthorize(Roles = "Credito,Instituciones,Consolidado")]
    public class CreditoController : Controller
    {

        //cambios en credito
       

        // GET: Credito
        public ActionResult Index()
        {
            return View();
        }

        public class VentasXVendedor{
            public string fecha_creacion;
            public string secuencial_factura;
            public string cliente;
            public string ciudad;
            //public string categoria;
            public string valor_factura;
        }

        public class CobrosXVendedor
        {
            public string fecha_creacion;
            public string fecha_aplicacion;
            public string secuencial_factura;
            public string codigo_cobro;
            public string cliente;
            public string categoria;
            public string valor_cobro;
        }

        public class NCXVendedor
        {
            public string fecha_creacion;
            public string secuencial_factura;
            public string secuencial_nc;
            public string cliente;
            public string motivo;
            public string valor_nc;
        }

        public class SP_Reporte_Cartera
        {
            public string empresa;
            public string sucursal;
            public string identificacion;

            public string nombre_comercial;

            public string categoria;

            public Nullable<int> id_vendedor_cliente;

            public string vendedor_cliente;

            public Nullable<int> id_vendedor_factura;

            public string vendedor_factura;

            public string secuencial_factura;

            public string fecha_factura;

            public string fecha_vencimiento;

            public string provincia;

            public string ciudad;

            public string parroquia;

            public string direccion;

            public Nullable<decimal> valor_factura;

            public decimal totalChequePost;

            public Nullable<decimal> saldo_pendiente;

            public Nullable<int> dias_emitida;

            public Nullable<int> dias_diferencia;

            public string descripcion;

            public string contacto;
        }

        public JsonResult ObtenerVentasPorVendedor(
            string empresa,
            string sucursal,
            string fecha_desde,
            string fecha_hasta,
            string tipocliente,
            string vendedor,
            Boolean indicadorDI
            )
        {
            DateTime fecha_desde_ = DateTime.Parse(fecha_desde);
            DateTime fecha_hasta_ = DateTime.Parse(fecha_hasta);
            string tipoCliente_ = "";
            if (tipocliente == null)
            {
                tipoCliente_ = "FARMACIA,DISTRIBUIDORES";
            } else
            {
                tipoCliente_ = tipocliente.Replace(@"[", string.Empty).Replace(@"]", string.Empty).Replace("\"", string.Empty);
            }

            using (var context = new as2oasis())
            {
                if (!indicadorDI)
                {
                    var ventas = context.VentasPorVendedor(empresa, sucursal, fecha_desde_, fecha_hasta_, tipoCliente_,vendedor);
                    List<VentasXVendedor> Ventas = new List<VentasXVendedor>();
                    foreach (var item in ventas)
                    {
                        var vta = new VentasXVendedor();
                        vta.fecha_creacion = ((DateTime)item.fecha_factura).ToShortDateString();
                        vta.secuencial_factura = item.secuencial_factura;
                        vta.cliente = item.nombre_comercial;
                        vta.ciudad = item.ciudad;
                        vta.valor_factura = ((float)item.valor_factura).ToString("N2", System.Globalization.CultureInfo.InvariantCulture);
                        Ventas.Add(vta);
                    }
                    var ventas_json = JsonConvert.SerializeObject(Ventas, Formatting.Indented);

                    return Json(ventas_json, JsonRequestBehavior.AllowGet);
                } else
                {
                    var ventas = context.VentasPorVendedorDi(empresa, sucursal, fecha_desde_, fecha_hasta_, tipoCliente_, vendedor);
                    List<VentasXVendedor> Ventas = new List<VentasXVendedor>();
                    foreach (var item in ventas)
                    {
                        var vta = new VentasXVendedor();
                        vta.fecha_creacion = ((DateTime)item.fecha_factura).ToShortDateString();
                        vta.secuencial_factura = item.secuencial_factura;
                        vta.cliente = item.nombre_comercial;
                        vta.ciudad = item.ciudad;
                        vta.valor_factura = ((float)item.valor_factura).ToString("N2", System.Globalization.CultureInfo.InvariantCulture);
                        Ventas.Add(vta);
                    }
                    var ventas_json = JsonConvert.SerializeObject(Ventas, Formatting.Indented);

                    return Json(ventas_json, JsonRequestBehavior.AllowGet);

                }
            }
        }

        public JsonResult ObtenerCobrosPorVendedor(
            string empresa,
            string sucursal,
            string fecha_desde,
            string fecha_hasta,
            string tipocliente,
            string vendedor,
            Boolean indicadorDI
            )
        {

            DateTime fecha_desde_ = DateTime.Parse(fecha_desde);
            DateTime fecha_hasta_ = DateTime.Parse(fecha_hasta);


            string tipoCliente_ = "";
            if (tipocliente == null)
            {
                tipocliente = "DISTRIBUIDORES,FARMACIA";
            }
            else
            {
                tipoCliente_ = tipocliente.Replace(@"[", string.Empty).Replace(@"]", string.Empty).Replace("\"", string.Empty);
            }

            using (var context = new as2oasis())
            {
                if (!indicadorDI)
                {
                    var cobros = context.CobrosPorVendedor(empresa, sucursal, fecha_desde_, fecha_hasta_, tipoCliente_, vendedor);
                    List<CobrosXVendedor> Cobros = new List<CobrosXVendedor>();
                    foreach (var item in cobros)
                    {
                        var cbr = new CobrosXVendedor();
                        cbr.fecha_aplicacion = ((DateTime)item.fecha_aplicacion).ToShortDateString();
                        cbr.fecha_creacion = ((DateTime)item.fecha_creacion).ToShortDateString();
                        cbr.codigo_cobro = item.codigo;
                        cbr.secuencial_factura = item.secuencial_factura;
                        cbr.cliente = item.nombre_comercial;
                        cbr.categoria = item.categoria;
                        cbr.valor_cobro = ((float)item.valor).ToString("N2", System.Globalization.CultureInfo.InvariantCulture);
                        Cobros.Add(cbr);
                    }
                    var cobros_json = JsonConvert.SerializeObject(Cobros, Formatting.Indented);

                    return Json(cobros_json, JsonRequestBehavior.AllowGet);
                }
                else
                {
                var cobros = context.CobrosPorVendedorDi(empresa, sucursal, fecha_desde_, fecha_hasta_, tipoCliente_, vendedor);
                List<CobrosXVendedor> Cobros = new List<CobrosXVendedor>();
                foreach (var item in cobros)
                {
                    var cbr = new CobrosXVendedor();
                    cbr.fecha_aplicacion = ((DateTime)item.fecha_aplicacion).ToShortDateString();
                    cbr.fecha_creacion = ((DateTime)item.fecha_creacion).ToShortDateString();
                    cbr.codigo_cobro = item.codigo;
                    cbr.secuencial_factura = item.secuencial_factura;
                    cbr.cliente = item.nombre_comercial;
                    cbr.categoria = item.categoria;
                    cbr.valor_cobro = ((float)item.valor).ToString("N2", System.Globalization.CultureInfo.InvariantCulture);
                    Cobros.Add(cbr);
                }

                var cobros_json = JsonConvert.SerializeObject(Cobros, Formatting.Indented);

                return Json(cobros_json, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public JsonResult ReporteCobros(
            string empresa,
            string sucursal,
            string fecha_desde,
            string fecha_hasta,
            string tipoCliente,
            string visitador)
        {
            var tipoC = JsonConvert.DeserializeObject(tipoCliente);
            var tipoCliente_ = tipoCliente.Replace(@"[", string.Empty).Replace(@"]", string.Empty).Replace("\"", string.Empty);
            string[] categoriaCliente = tipoCliente_.Split(',');

            using (var context = new as2oasis())
            {
                var cobros =
                    context.Cobros_Consolidado
                    .ToList()
                    .Where(x => x.empresa == empresa &&
                                x.sucursal == sucursal &&
                                categoriaCliente.Contains(x.categoria) &&
                                x.fecha_aplicacion >= DateTime.Parse(fecha_desde) &&
                                x.fecha_aplicacion <= DateTime.Parse(fecha_hasta)
                            );

                if (!String.IsNullOrEmpty(visitador))
                {
                    var codigo_visitador = Int16.Parse(visitador);
                    cobros = cobros.Where(x => x.id_vendedor == codigo_visitador);
                }

                var listaCobros = cobros
                    .ToList()
                    .Select(x => new
                    {
                        x.empresa,
                        x.sucursal,
                        x.categoria,
                        x.codigo,
                        x.nombre_comercial,
                        x.codigo_cobro,
                        fecha_creacion = x.fecha_creacion.Value.ToShortDateString(),
                        fecha_aplicacion = x.fecha_aplicacion.Value.ToShortDateString(),
                        valor = x.valor,
                        secuencial_factura=x.numero,
                        fecha_factura = x.fecha_factura.Value.ToShortDateString(),
                        fecha_vencimiento = x.fecha_vencimiento.Value.ToShortDateString(),
                        x.vendedor,
                        descripcion = x.descripcion,
                        descripcion2 = x.descripcion2,
                        valor_factura = x.total,
                        x.forma_pago,
                        x.documento_referencia
                        
                    });


                var listaCobros_json = JsonConvert.SerializeObject(listaCobros, Formatting.Indented);
                
                var json_data =  Json(listaCobros_json, JsonRequestBehavior.AllowGet);
                json_data.MaxJsonLength = 5000000;
                return json_data;
            }
        }

        [HttpGet]
        public JsonResult ReporteFacturas(
            string empresa,
            string sucursal,
            string fecha_desde,
            string fecha_hasta,
            string tipoCliente,
            string visitador)
        {
            var tipoC = JsonConvert.DeserializeObject(tipoCliente);
            var tipoCliente_ = tipoCliente.Replace(@"[", string.Empty).Replace(@"]", string.Empty).Replace("\"", string.Empty);
            string[] categoriaCliente = tipoCliente_.Split(',');

            using (var context = new as2oasis())
            {
                //var facturas =
                //    context.Ventas_Consolidado
                //    .ToList()
                //    .Where(x => x.empresa == empresa &&
                //                x.sucursal == sucursal &&
                //                categoriaCliente.Contains(x.categoria) &&
                //                x.fecha_factura >= DateTime.Parse(fecha_desde) &&
                //                x.fecha_factura <= DateTime.Parse(fecha_hasta)
                //            );

                var facturas =
                   context.Ventas_Consolidado
                   .ToList()
                   .Where(x => categoriaCliente.Contains(x.categoria) &&
                               x.fecha_factura >= DateTime.Parse(fecha_desde) &&
                               x.fecha_factura <= DateTime.Parse(fecha_hasta)
                           );

                if (empresa != "0")
                {
                    facturas = facturas.Where(x => x.empresa == empresa);
                }

                if (sucursal != "0")
                {
                    facturas = facturas.Where(x => x.sucursal == sucursal);
                }

                if (!String.IsNullOrEmpty(visitador))
                {
                    var codigo_visitador = Int16.Parse(visitador);
                    facturas = facturas.Where(x => x.id_vendedor == codigo_visitador);
                }

                //var listaFacturas = facturas
                //    .ToList()
                //    .Select(x => new
                //    {
                //        x.empresa,
                //        x.sucursal,
                //        x.categoria,
                //        x.identificacion,
                //        x.nombre_comercial,
                //        x.secuencial_factura,
                //        fecha_factura = (x.fecha_factura == null? null: x.fecha_factura.ToShortDateString()),
                //        valor = x.valor_factura,
                //        x.vendedor,
                //        x.descripcion,
                //        x.estado
                //    });

                var listaFacturas = Json(facturas.ToList()
                           .Select(x => new
                           {
                               x.empresa,
                               x.sucursal,
                               x.categoria,
                               x.identificacion,
                               x.nombre_comercial,
                               x.secuencial_factura,
                               fecha_factura = (x.fecha_factura == null ? null : x.fecha_factura.ToShortDateString()),
                               valor = x.valor_factura,
                               x.vendedor,
                               x.descripcion,
                               x.estado
                           }),
                           JsonRequestBehavior.AllowGet);

                listaFacturas.MaxJsonLength = 500000000;
                return listaFacturas;

            }
        }

        public JsonResult ObtenerNCPorVendedor(
            string empresa,
            string sucursal,
            string fecha_desde,
            string fecha_hasta,
            string tipocliente,
            string vendedor,
            Boolean indicadorDI
            )
        {
            DateTime fecha_desde_ = DateTime.Parse(fecha_desde);
            DateTime fecha_hasta_ = DateTime.Parse(fecha_hasta);

            string tipoCliente_ = "";
            if (tipocliente == null)
            {
                tipocliente = "DISTRIBUIDORES,FARMACIA";
            }
            else
            {
                tipoCliente_ = tipocliente.Replace(@"[", string.Empty).Replace(@"]", string.Empty).Replace("\"", string.Empty);
            } 

            using (var context = new as2oasis())
            {
                if (!indicadorDI)
                {
                var nc = context.NCPorVendedor(empresa, sucursal, fecha_desde_, fecha_hasta_, tipoCliente_, vendedor);
                List<NCXVendedor> NC = new List<NCXVendedor>();
                foreach (var item in nc)
                {
                    var nc_ = new NCXVendedor();
                    nc_.fecha_creacion = ((DateTime)item.fecha_documento).ToShortDateString();
                    nc_.secuencial_factura = item.numero_factura;
                    nc_.secuencial_nc = item.secuencial_nc;
                    nc_.cliente = item.nombre_fiscal;
                    nc_.motivo = item.motivo_nc;
                    nc_.valor_nc =item.valor_nc.ToString();
                    NC.Add(nc_);
                }
                var nc_json = JsonConvert.SerializeObject(NC, Formatting.Indented);
                return Json(nc_json, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var nc = context.NCPorVendedorDi(empresa, sucursal, fecha_desde_, fecha_hasta_, tipoCliente_, vendedor);
                    List<NCXVendedor> NC = new List<NCXVendedor>();
                    foreach (var item in nc)
                    {
                        var nc_ = new NCXVendedor();
                        nc_.fecha_creacion = ((DateTime)item.fecha_documento).ToShortDateString();
                        nc_.secuencial_factura = item.numero_factura;
                        nc_.secuencial_nc = item.secuencial_nc;
                        nc_.cliente = item.nombre_fiscal;
                        nc_.motivo = item.motivo_nc;
                        nc_.valor_nc = item.valor_nc.ToString();
                        NC.Add(nc_);
                    }
                    var nc_json = JsonConvert.SerializeObject(NC, Formatting.Indented);
                    return Json(nc_json, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult VerFactura()
        {
            ViewBag.Opciones = ListaEmpresas();
            return View();
        }

        public ActionResult VerDVP()
        {
            return View();
        }

        public ActionResult VentasProducto()
        {
            return View();
        }

        public ActionResult MovimientoClientes()
        {
            return View();
        }

        public ActionResult PresupuestoCobrosCons()
        {
            return View();
        }

        public ActionResult GestionCarteraVcda()
        {
            return View();
        }

        public ActionResult DetalleCobros()
        {
            return View();
        }

        public ActionResult DevolucionDetalleCobro()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ObtenerDVP(string fecha_desde, string fecha_hasta, string empresa)
        {

            using (var db = new as2oasis())
            {
                var _fecha_inicio = Convert.ToDateTime(fecha_desde);
                var _fecha_fin = Convert.ToDateTime(fecha_hasta);

                IQueryable<DVP> data = db.DVP.Where(
                    x => x.Fecha_factura >= _fecha_inicio &&
                    x.Fecha_factura <= _fecha_fin 
                    );

                if (empresa != "0")
                {
                    data = data.Where(x => x.Empresa == empresa);
                }


                var data_json = Json(
                    data
                    .ToList()
                    .Select(x => new
                    { 
                        x.Empresa,
                        x.Tipo_documento,
                        Fecha_factura =  x.Fecha_factura.Value.ToShortDateString(),
                        x.Ciudad,
                        x.Provincia,
                        x.Parroquia,
                        x.Tipo_cliente,
                        x.Canal,
                        x.RUC,
                        x.Cliente,
                        x.id_motivo_nota_credito_cliente,
                        x.Secuencial_documento,
                        x.indicador_afecta_devolucion,
                        x.Código_producto, 
                        x.Código_MBA,
                        x.Producto,
                        x.Categoría,
                        x.Subcategoría,
                        x.UM,
                        x.Cantidad,
                        x.Valor_total,
                        x.Tipo_venta,
                        x.codigo,
                        x.Cod__Vendedor,
                        x.Vendedor,
                        x.NC,
                        x.Descripción_NC,
                        Fecha_NC  =  x.Fecha_NC.HasValue?
                        x.Fecha_NC.Value.ToShortDateString():null,
                        x.clave_acceso,
                        x.id_factura_cliente,
                        x.memo
                    }), JsonRequestBehavior.AllowGet
                    );

                data_json.MaxJsonLength= 500000000;
                
                return data_json;
            }
        }

        public ActionResult VerCobro()
        {
            ViewBag.Opciones = ListaEmpresas();
            return View();
        }

        [HttpGet]
        public JsonResult DetalleFactura(string ClaveAcceso)
        {
            ClaveAcceso = ClaveAcceso.Substring(1, ClaveAcceso.Length-1);
            using (var context = new as2oasis())
            {
                var detalle_factura =
                    context.DVP 
                    .Where(x => x.clave_acceso.Contains(ClaveAcceso))
                    .Select(x => new { x.Cliente, x.Código_producto, x.Producto, x.Cantidad, x.UM });
                var presupuesto_json = JsonConvert.SerializeObject(detalle_factura, Formatting.Indented);

                return Json(presupuesto_json, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ObtenerPresupuestoDistribuidor(
            string empresa,
            string sucursal,
            string fecha_desde,
            string fecha_hasta,
            int codigo_vendedor)
        {

            DateTime fecha_desde_ = DateTime.Parse(fecha_desde);
            DateTime fecha_hasta_ = DateTime.Parse(fecha_hasta);

            using (var context = new as2oasis())
            {
                var presupuesto = context.Presupuesto(empresa, sucursal, fecha_desde_, fecha_hasta_, "DISTRIBUIDORES");
                var presupuesto_filtrado = presupuesto.Where(x => x.id_vendedor == codigo_vendedor).ToList();
                var presupuesto_json = JsonConvert.SerializeObject(presupuesto_filtrado, Formatting.Indented);
                return Json(presupuesto_json, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ObtenerPresupuesto(
            string empresa, 
            string sucursal,
            string fecha_desde,
            string fecha_hasta, 
            string tipoCliente) {

            DateTime fecha_desde_ = DateTime.Parse(fecha_desde);
            DateTime fecha_hasta_ = DateTime.Parse(fecha_hasta);
            
            //var tipoC = JsonConvert.DeserializeObject(tipoCliente);
            //string csv = jsonToCSV(tipoCliente, ",");

            //var lista = tipoC.Split(',');
            var tipoCliente_ = tipoCliente.Replace(@"[", string.Empty).Replace(@"]", string.Empty).Replace("\"",string.Empty);

            if (empresa == "0")
            {
                empresa = "";
            }

            if (sucursal == "0")
            {
                sucursal = "";
            }

            using (var context = new as2oasis())
            {
                var presupuesto = context.Presupuesto(empresa, sucursal, fecha_desde_, fecha_hasta_,tipoCliente_);

                var presupuesto_json = JsonConvert.SerializeObject(presupuesto, Formatting.Indented);

                return Json(presupuesto_json,JsonRequestBehavior.AllowGet);

                //foreach (Course cs in courses)
                //    Console.WriteLine(cs.CourseName);

            }
            //return View();
        }

        public JsonResult ObtenerVendedoresDistribuidores(string empresa, string sucursal, string textoBusqueda)
        {
            as2oasis oasis = new as2oasis();
            var vendedores = oasis.Vendedores_Distribuidores
                .AsEnumerable()
                .Where(
                x =>
                (x.username.ToLower().Contains(textoBusqueda.ToLower()) == true
                || x.nombre.ToLower().Contains(textoBusqueda.ToLower()) == true))
                .GroupBy(x => new { x.id_vendedor, x.username })
                .Select(x => new
                {
                    id_vendedor = x.Key.id_vendedor,
                    username = x.Key.username,
                })
                .Take(5)
                ;
            return Json(vendedores, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerConsolidadoDI(
            string empresa,
            string sucursal,
            string fecha_desde,
            string fecha_hasta)
        {

            DateTime fecha_desde_ = DateTime.Parse(fecha_desde);
            DateTime fecha_hasta_ = DateTime.Parse(fecha_hasta);

            using (var context = new as2oasis())
            {
                var presupuesto = context.PresupuestoDI(empresa, sucursal, fecha_desde_, fecha_hasta_);
                var presupuesto_json = JsonConvert.SerializeObject(presupuesto, Formatting.Indented);

                return Json(presupuesto_json, JsonRequestBehavior.AllowGet);
                //foreach (Course cs in courses)
                //    Console.WriteLine(cs.CourseName);
            }
            //return View();
        }

        public JsonResult ObtenerCartera(
            string empresa,
            string sucursal,
            string tipoCliente,
            string fecha_desde,
            string fecha_hasta)
        {
            try
            {
                var tipoC = JsonConvert.DeserializeObject(tipoCliente);
                var tipoCliente_ = tipoCliente.Replace(@"[", string.Empty).Replace(@"]", string.Empty).Replace("\"", string.Empty);
                var lista_tipoCliente = tipoCliente_.Split(',');                

                using (var context = new as2oasis())
                {
                    //context.Database.CommandTimeout = 320;
                    IQueryable<Reporte_Cartera_2> cartera;

                    var _fecha_inicio = Convert.ToDateTime(fecha_desde);
                    var _fecha_fin = Convert.ToDateTime(fecha_hasta);

                    cartera = context.Reporte_Cartera_2.Where(
                        x => x.fecha_factura >= _fecha_inicio &&
                        x.fecha_factura <= _fecha_fin &&
                        lista_tipoCliente.Contains(x.categoria)
                        );

                    var cuenta = cartera.Count();

                    if (empresa != "0" && sucursal != "0")
                    {
                        cartera = cartera.Where(x => x.empresa == empresa);
                        cartera = cartera.Where(x => x.sucursal == sucursal);

                        var data_json = cartera.ToList()
                        .Select(x => new
                        {
                            x.empresa,
                            x.sucursal,
                            x.identificacion,
                            x.nombre_comercial,
                            x.categoria,
                            x.vendedor_cliente,
                            x.vendedor_factura,
                            secuencial_factura = (x.secuencial_factura.Replace("-", string.Empty)),
                            x.descripcion,
                            //fecha_factura = x.fecha_factura == null ? "" : x.fecha_factura.ToShortDateString(),
                            fecha_factura = x.fecha_factura == null ? "" : x.fecha_factura.ToString("yyyy-MM-dd"),
                            fecha_vencimiento = x.fecha_vencimiento == null ? "" : x.fecha_vencimiento,
                            x.provincia,
                            x.ciudad,
                            x.parroquia,
                            x.direccion,
                            valor_factura = x.valor_factura,
                            totalChequePost = x.totalChequePost,
                            saldo_pendiente = x.saldo_pendiente,
                            x.dias_emitida,
                            x.dias_diferencia
                        });

                        var resultado = JsonConvert.SerializeObject(data_json, Formatting.Indented);

                        return Json(resultado, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        if (empresa != "0")
                        {
                            cartera = cartera.Where(x => x.empresa == empresa);
                        }

                        if (sucursal != "0")
                        {
                            cartera = cartera.Where(x => x.sucursal == sucursal);
                        }

                        //cartera = cartera.OrderBy(x => new {x.empresa, x.sucursal});

                        var data_json = Json(cartera.ToList()
                        .Select(x => new
                        {
                            x.empresa,
                            x.sucursal,
                            x.identificacion,
                            x.nombre_comercial,
                            x.categoria,
                            x.vendedor_cliente,
                            x.vendedor_factura,
                            secuencial_factura = (x.secuencial_factura.Replace("-", string.Empty)),
                            x.descripcion,
                            //fecha_factura = x.fecha_factura == null ? "" : x.fecha_factura.ToShortDateString(),
                            //fecha_vencimiento = x.fecha_vencimiento == null ? "" : x.fecha_vencimiento.Value.ToShortDateString(),
                            fecha_factura = x.fecha_factura == null ? "" : x.fecha_factura.ToString("yyyy-MM-dd"),
                            fecha_vencimiento = x.fecha_vencimiento == null ? "" : x.fecha_vencimiento,
                            x.provincia,
                            x.ciudad,
                            x.parroquia,
                            x.direccion,
                            valor_factura = x.valor_factura,
                            totalChequePost = x.totalChequePost,
                            saldo_pendiente = x.saldo_pendiente,
                            x.dias_emitida,
                            x.dias_diferencia
                        }), JsonRequestBehavior.AllowGet
                        );

                        data_json.MaxJsonLength = 500000000;

                        return data_json;
                    }
                }
            }
            catch (Exception e)
            {
                var resultado = 0;
                e.InnerException.ToString();
                return Json(resultado, JsonRequestBehavior.AllowGet);

            }
            
        }

        [HttpGet]
        public JsonResult ObtenerFacturas(string textoBusqueda, string empresa)
        {
            AS2Context as2 = new AS2Context();
            var facturas = from factura in as2.factura_cliente
                           .Where(x => x.id_factura_cliente_padre == null
                           && x.numero.Contains(textoBusqueda))
                           join vendedor in as2.usuario
                           on factura.id_agente_comercial equals vendedor.id_usuario into VendedorFactura
                           from vfc in VendedorFactura.DefaultIfEmpty()
                           join emp in as2.organizacion
                           on factura.id_organizacion equals emp.id_organizacion
                           where emp.nombre_comercial == empresa
                           select new
                           {
                               codigo_factura = factura.id_factura_cliente,
                               secuencial = factura.numero,
                               id_vendedor = factura.id_agente_comercial,
                               vendedor = vfc.nombre_usuario.ToUpper()
                           };
            return Json(facturas, JsonRequestBehavior.AllowGet);

        }


        public JsonResult ObtenerCarteraVisitador(
            string empresa,
            string sucursal,
            string tipoCliente,
            string visitador)
        {
            var tipoC = JsonConvert.DeserializeObject(tipoCliente);
            var tipoCliente_ = tipoCliente.Replace(@"[", string.Empty).Replace(@"]", string.Empty).Replace("\"", string.Empty);

            using (var context = new as2oasis())
            {

                var id_visitador = Int16.Parse(visitador);

                var anticipo =
                    context.Anticipos
                    .AsEnumerable()
                    .Where(x => x.empresa == empresa &&
                        x.sucursal == sucursal &&
                        x.id_vistador == id_visitador)
                    .ToList()
                    .Select(x => new
                    {
                        empresa = x.empresa,
                        sucursal = x.sucursal,
                        identificacion =x.ruc_cliente,
                        nombre_comercial =x.nombre_cliente,
                        categoria = x.categoria,
                        vendedor_cliente = x.visitador,
                        vendedor_factura = x.visitador,
                        tipo_documento = "ANT.",
                        secuencial_factura =x.sencuencia_documento,
                        descripcion="",
                        fecha_factura =x.fecha_documento.Value.ToShortDateString(),
                        fecha_vencimiento="",
                        provincia="",
                        ciudad="",
                        parroquia="",
                        direccion="",
                        valor_factura  = (Decimal?)0.00,
                        totalChequePost = (Decimal)0.00,
                        saldo_pendiente = (Decimal?)(x.valor_anticipo*-1),
                        dias_emitida = (int?)0,
                        dias_diferencia = (int?)0
                    });

                
                var cartera =
                      context.Cartera.Where(
                        x =>
                        x.id_vendedor_cliente == id_visitador ||
                        x.id_vendedor_factura == id_visitador
                        ) 
                      .ToList()
                    .Select(x => new
                    {
                        x.empresa,
                        x.sucursal,
                        x.identificacion,
                        x.nombre_comercial,
                        x.categoria,
                        x.vendedor_cliente,
                        x.vendedor_factura,
                        tipo_documento = "FACT.",
                        secuencial_factura =(x.secuencial_factura.Replace("-", string.Empty)),
                        x.descripcion,
                        fecha_factura = x.fecha_factura.ToShortDateString(),
                        fecha_vencimiento = x.fecha_vencimiento.Value.ToShortDateString(),
                        x.provincia,
                        x.ciudad,
                        x.parroquia,
                        x.direccion,
                        valor_factura = x.valor_factura,
                        totalChequePost = x.totalChequePost,
                        saldo_pendiente = x.saldo_pendiente,
                        x.dias_emitida,
                        x.dias_diferencia
                    });
                var cartera_json = JsonConvert.SerializeObject(cartera.Concat(anticipo), Formatting.Indented);

                return Json(cartera_json, JsonRequestBehavior.AllowGet);
                //foreach (Course cs in courses)
                //    Console.WriteLine(cs.CourseName);
            }
            //return View();
        }

        [CustomAuthorize(Roles = "Credito,Instituciones,Consolidado")]
        public ActionResult Consolidado()
        {
            ViewBag.Opciones = ListaEmpresas();
            return View();
        }

        [CustomAuthorize(Roles = "Credito,Consolidado")]
        public ActionResult ConsolidadoDI()
        {
            return View();
        }

        public ActionResult EditarFactura()
        {
            ViewBag.Opciones = ListaEmpresas();
            return View();
        }


        // GET: Cartera
        public ActionResult Cartera()
        {
            ViewBag.Opciones = ListaEmpresas();
            return View();
        }

        public ActionResult CarteraVendedor()
        {
            return View();
        }

        public JsonResult ObtenerNC(
            string empresa,
            string sucursal,
            string fecha_desde,
            string fecha_hasta,
            string tipocliente,
            string visitador = null
            )
        {
            DateTime fecha_desde_ = DateTime.Parse(fecha_desde);
            DateTime fecha_hasta_ = DateTime.Parse(fecha_hasta);


            var tipoCliente_ = tipocliente.Replace(@"[", string.Empty).Replace(@"]", string.Empty).Replace("\"", string.Empty);
            string[] categoriaCliente = tipoCliente_.Split(',');

            using (var context = new as2oasis())
            {
                var nc =
                    context.NC_Diario
                    .Where(x => x.empresa == empresa &&
                                x.sucursal == sucursal &&
                                categoriaCliente.Contains(x.categoria) &&
                                x.fecha_documento>=fecha_desde_ &&
                                x.fecha_documento<=fecha_hasta_
                            );

                if (!String.IsNullOrEmpty(visitador))
                {
                    var codigo_visitador = Int16.Parse(visitador);
                    nc = nc.Where(x => x.id_vendedor == codigo_visitador);
                }

                var listaNC = nc
                    .ToList()
                    .Select(x => new {
                        x.empresa,
                        x.sucursal,
                        x.identificacion,
                        x.nombre_fiscal,
                        fecha_documento = x.fecha_documento.Value.ToShortDateString(),
                        x.secuencial_nc,
                        x.motivo_nc,
                        x.valor_nc,
                        fecha_factura = x.fecha_factura.Value.ToShortDateString(),
                        factura = x.numero_factura,
                        x.vendedor,
                        x.nota,
                        x.descripcion_documento,
                        x.valor_factura

                    });

                var nc_json = JsonConvert.SerializeObject(listaNC, Formatting.Indented);
                
                return Json(nc_json, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ObtenerNCAfectacion(
            string empresa,
            string sucursal,
            string fecha_desde,
            string fecha_hasta,
            string tipocliente,
            string visitador = null
            )
        {
            DateTime fecha_desde_ = DateTime.Parse(fecha_desde);
            DateTime fecha_hasta_ = DateTime.Parse(fecha_hasta);


            var tipoCliente_ = tipocliente.Replace(@"[", string.Empty).Replace(@"]", string.Empty).Replace("\"", string.Empty);
            string[] categoriaCliente = tipoCliente_.Split(',');

            using (var context = new as2oasis())
            {
                var nc =
                    context.NC_Consolidado
                    .Where(x => x.empresa == empresa &&
                                x.sucursal == sucursal &&
                                categoriaCliente.Contains(x.categoria)
                            );

                if (!String.IsNullOrEmpty(visitador))
                {
                    var codigo_visitador = Int16.Parse(visitador);
                    nc = nc.Where(x => x.id_vendedor == codigo_visitador);
                }

                var listaNC = nc
                    .ToList()
                    .Select(x => new {
                        x.empresa,
                        x.sucursal,
                        x.identificacion,
                        x.nombre_fiscal,
                        fecha_documento = x.fecha_documento.Value.ToShortDateString(),
                        x.secuencial_nc,
                        x.motivo_nc,
                        valor = x.valor_nc,
                        fecha_factura = x.fecha_factura.Value.ToShortDateString(),
                        factura = x.numero_factura,
                        x.vendedor,
                    });

                var nc_json = JsonConvert.SerializeObject(listaNC, Formatting.Indented);
                return Json(nc_json, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ObtenerChequesPost(
            string empresa,
            string sucursal,
            string tipoCliente,
            string visitador = null)
        {
            var tipoC = JsonConvert.DeserializeObject(tipoCliente);
            var tipoCliente_ = tipoCliente.Replace(@"[", string.Empty).Replace(@"]", string.Empty).Replace("\"", string.Empty);
            string[] categoriaCliente = tipoCliente_.Split(',');

            using (var context = new as2oasis())
            {
                var chequesPost =
                    context.Cheques_Postfechados
                    .Where(x => x.empresa == empresa &&
                                x.sucursal == sucursal &&
                                categoriaCliente.Contains(x.categoria)
                            );

                if (!String.IsNullOrEmpty(visitador))
                {
                    var codigo_visitador = Int16.Parse(visitador);
                    chequesPost = chequesPost.Where(x => x.id_usuario == codigo_visitador);
                }

                var listaCHQPost = chequesPost
                    .ToList()
                    .Select(x => new {
                        x.empresa,
                        x.sucursal,
                        x.codigo,
                        x.nombre_cliente,
                        x.codigo_cobro,
                        x.valor,
                        fecha_creacion = x.fecha_creacion.Value.ToShortDateString(),
                        fecha_aplicacion = x.fecha_cobro.Value.ToShortDateString(),
                        x.secuencial_factura,
                        x.dias_credito_otorgado,
                        x.vendedor,
                        descripcion = x.descripcion +" " +x.descripcion2
                    });


                var chequesPost_json = JsonConvert.SerializeObject(listaCHQPost, Formatting.Indented);

                return Json(chequesPost_json, JsonRequestBehavior.AllowGet);
             }
        }

        public ActionResult ChequesPost()
        {
            ViewBag.Opciones = ListaEmpresas();
            ViewBag.Title = "Cheques postfechados";
            return View();
        }

        public ActionResult NotasCredito()
        {
            ViewBag.Opciones = ListaEmpresas();
            ViewBag.Title = "Notas de crédito";
            return View();
        }

        public List<SelectListItem> ListaEmpresas()
        {
            var id_usuario = (User as CustomPrincipal).id_usuario;
            List<SelectListItem> lst = new List<SelectListItem>();
            using (var context = new as2oasis())
            {
                var listaEmpresas = context.detalle_usuario_empresa_sucursal
                    .Where(x => x.id_usuario == id_usuario)
                    .GroupBy(x => x.empresaOasis)
                    .Select(x => new SelectListItem
                    {
                        Text = x.Key.nombre,
                        Value = x.Key.nombre
                    }).ToList();

                
                //listaEmpresas.Insert(0, new detalle_usuario_empresa_sucursal() { id_empresa = 0});
                return listaEmpresas;
            }
        }

        public ActionResult ListaSucursales(int id_empresa)
        {
            var id_usuario = (User as CustomPrincipal).id_usuario;
            List<SelectListItem> lst = new List<SelectListItem>();
            using (var context = new as2oasis())
            {
                var listaSucursales = context.detalle_usuario_empresa_sucursal
                    .Where(x => x.id_usuario == id_usuario && x.id_empresa == id_empresa) 
                    .Select(x => new SelectListItem
                    {
                        Text = x.sucursalOasis.nombre,
                        Value = x.sucursalOasis.nombre
                    }).ToList();
                return Json(listaSucursales,JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Credito/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Credito/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Credito/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Credito/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }


        [HttpPost]
        public ActionResult GuardarFactura (int id_factura, int id_visitador_nuevo)
        {
            try
            {
                AS2Context as2 = new AS2Context();
                factura_cliente fact = new factura_cliente();
                fact = as2.factura_cliente.Find(id_factura);
                fact.id_agente_comercial = id_visitador_nuevo;
                as2.Entry(fact).State = EntityState.Modified;
                as2.SaveChanges();
                return new HttpStatusCodeResult(200);
            } catch
            {
                return new HttpStatusCodeResult(400);
            }
        }

        // POST: Credito/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Credito/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Credito/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public JsonResult SugerirFechas(string empresa, string sucursal)
        {
            as2oasis oasis = new as2oasis();
            DateTime hoy = DateTime.Today;

            var resultadoFecha =
                oasis.presupuesto_cabecera.Where(x => x.empresa == empresa
                && x.sucursal == sucursal
                && (x.fecha_desde <= hoy
                && x.fecha_hasta >= hoy)).ToList()
                .Select(x=>new {
                        fecha_desde=x.fecha_desde.ToShortDateString(),
                        fecha_hasta=x.fecha_hasta.ToShortDateString()
                }).FirstOrDefault();

            if (resultadoFecha == null)
            {
                resultadoFecha =
                    oasis.presupuesto_cabecera.Where(
                    x => x.empresa == empresa
                    && x.sucursal == sucursal)
                    .OrderByDescending(x => x.fecha_hasta)
                    .ToList()
                    .Select(x => new
                    {
                        fecha_desde = x.fecha_desde.ToShortDateString(),
                        fecha_hasta = x.fecha_hasta.ToShortDateString()
                    })
                    .FirstOrDefault(); 
            }

            var msjError = new {MensajeError="No se puede obtener una fecha sugerida"};
            var resultadoFecha_json = JsonConvert.SerializeObject(resultadoFecha, Formatting.Indented,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

            if (resultadoFecha is null) { 
                return Json(msjError, JsonRequestBehavior.AllowGet);
            }

            return Json(resultadoFecha_json, JsonRequestBehavior.AllowGet);
           
        }

        [HttpPost]
        public JsonResult ObtenerVentasProducto(string fecha_desde, string fecha_hasta, string empresa, string sucursal, string producto, string cliente)
        {

            using (var db = new as2oasis())
            {
                var _fecha_inicio = Convert.ToDateTime(fecha_desde);
                var _fecha_fin = Convert.ToDateTime(fecha_hasta);

                IQueryable<Ventas_Producto_Precio> data = db.Ventas_Producto_Precio.Where(
                    x => x.fecha_factura >= _fecha_inicio &&
                    x.fecha_factura <= _fecha_fin
                    );

                if (empresa != "0")
                {
                    data = data.Where(x => x.Empresa == empresa);
                }

                if (sucursal != "0")
                {
                    data = data.Where(x => x.localidad == sucursal);
                }

                if (producto != "0")
                {
                    data = data.Where(x => x.producto.Contains(producto));
                }

                if (cliente != "0")
                {
                    data = data.Where(x => x.Cliente == cliente);
                }


                var data_json = Json(
                    data
                    .ToList()
                    .Select(x => new
                    {
                        x.Empresa,
                        x.localidad,
                        x.tipo_documento,
                        Fecha_factura = x.fecha_factura.Value.ToShortDateString(),
                        x.identificacion,
                        x.Cliente,
                        x.factura,
                        x.codigo_producto,
                        x.producto,
                        x.cantidad,
                        x.bonificacion,
                        x.porc_bonifica,
                        x.valores,
                        x.precio,
                        x.precio_especial
                    }), JsonRequestBehavior.AllowGet
                    );

                data_json.MaxJsonLength = 500000000;

                return data_json;
            }
        }

        [HttpPost]
        public JsonResult ObtenerClientes(string fecha_desde, string fecha_hasta, string empresa, string sucursal, string cliente)
        {

            using (var db = new as2oasis())
            {
                var _fecha_inicio = Convert.ToDateTime(fecha_desde);
                var _fecha_fin = Convert.ToDateTime(fecha_hasta);

                IQueryable<Ventas_Producto_Precio> data = db.Ventas_Producto_Precio.Where(
                    x => x.fecha_factura >= _fecha_inicio &&
                    x.fecha_factura <= _fecha_fin
                    );

                if (empresa != "0")
                {
                    data = data.Where(x => x.Empresa == empresa);
                }

                if (sucursal != "0")
                {
                    data = data.Where(x => x.localidad == sucursal);
                }

                if (cliente != "0")
                {
                    data = data.Where(x => x.Cliente == cliente);
                }

                var data_json = Json(
                    data
                    .ToList()
                    .Select(x => new
                    {
                        x.Empresa,
                        x.localidad,
                        x.tipo_documento,
                        Fecha_factura = x.fecha_factura.Value.ToShortDateString(),
                        x.identificacion,
                        x.Cliente,
                        x.factura,
                        x.codigo_producto,
                        x.producto,
                        x.cantidad,
                        x.bonificacion,
                        x.porc_bonifica,
                        x.valores,
                        x.precio,
                        x.precio_especial
                    }), JsonRequestBehavior.AllowGet
                    );

                data_json.MaxJsonLength = 500000000;

                return data_json;
            }
        }

        [HttpPost]
        public JsonResult ObtenerCobrosConsolidado(string fecha_desde, string fecha_hasta, string empresa)
        {

            using (var db = new as2oasis())
            {
                var _fecha_inicio = Convert.ToDateTime(fecha_desde);
                var _fecha_fin = Convert.ToDateTime(fecha_hasta);

                IQueryable<Ventas_Producto_Precio> data = db.Ventas_Producto_Precio.Where(
                    x => x.fecha_factura >= _fecha_inicio &&
                    x.fecha_factura <= _fecha_fin
                    );

                if (empresa != "0")
                {
                    data = data.Where(x => x.Empresa == empresa);
                }


                var data_json = Json(
                    data
                    .ToList()
                    .Select(x => new
                    {
                        x.Empresa,
                        x.localidad,
                        x.tipo_documento,
                        Fecha_factura = x.fecha_factura.Value.ToShortDateString(),
                        x.identificacion,
                        x.Cliente,
                        x.factura,
                        x.codigo_producto,
                        x.producto,
                        x.cantidad,
                        x.bonificacion,
                        x.porc_bonifica,
                        x.valores,
                        x.precio,
                        x.precio_especial
                    }), JsonRequestBehavior.AllowGet
                    );

                data_json.MaxJsonLength = 500000000;

                return data_json;
            }
        }

        [HttpPost]
        public JsonResult ObtenerDetalleCobros(string sucursal, string usuario, string estado, string numero, string empresa, string fecha_desde, string fecha_hasta)
        {
            int codigo_usuario = 10;

            using (var db = new as2oasis())
            {
                var _fecha_inicio = Convert.ToDateTime(fecha_desde);
                var _fecha_fin = Convert.ToDateTime(fecha_hasta);

                //IQueryable<Detalle_Cobros> data = db.Detalle_Cobros.Where(
                //    x => x.fecha_cobro >= _fecha_inicio &&
                //    x.fecha_cobro <= _fecha_fin &&
                //    x.estado_consolidado != 5
                //    );

                if (empresa == "0")
                {
                    empresa = "";
                }

                if (sucursal == "0")
                {
                    sucursal = "";
                }

                if (usuario != null)
                {
                    codigo_usuario = Convert.ToInt32(usuario);
                    //data = data.Where(x => x.id_usuario == codigo_usuario);
                }

                if (estado == "10")
                {
                    estado = "";
                }

                var data = db.SP_Detalle_Cobros(1,empresa, sucursal, _fecha_inicio, _fecha_fin, codigo_usuario, estado, 0,null);

                var data_json = JsonConvert.SerializeObject(data, Formatting.Indented);
                var json_data = Json(data_json, JsonRequestBehavior.AllowGet);
                json_data.MaxJsonLength = 5000000;
                return json_data;
                 
            }
        }

        [HttpPost]
        public JsonResult ObtenerCierres(string textoBusqueda)
        {
            using (var db = new AS2Context())
            {
                return Json(db.cierre_caja.Where(x => x.numero.Contains(textoBusqueda) 
               ).Take(5).Select(x => new
               {
                   x.numero
               }).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult VerDepositoCaja()
        {
            int precio = 1;
            as2oasis oasis = new as2oasis();
            var dato_a_editar = oasis.detalle_lista_precio
                .Where(x => x.id_detalle_lista_precio == precio)
                .FirstOrDefault();
            return View(dato_a_editar);
        }

        [HttpGet]
        public ActionResult Seleccionar()
        {
            int precio = 1;
            as2oasis oasis = new as2oasis();
            var dato_a_editar = oasis.detalle_lista_precio
                .Where(x => x.id_detalle_lista_precio == precio)
                .FirstOrDefault();
            return View(dato_a_editar);
        }

        public JsonResult ObtenerPresupuestoEmpresa(
           string empresa,
           string sucursal,
           string fecha_desde,
           string fecha_hasta,
           string tipoCliente)
        {

            DateTime fecha_desde_ = DateTime.Parse(fecha_desde);
            DateTime fecha_hasta_ = DateTime.Parse(fecha_hasta);

            //var tipoC = JsonConvert.DeserializeObject(tipoCliente);
            //string csv = jsonToCSV(tipoCliente, ",");

            //var lista = tipoC.Split(',');
            var tipoCliente_ = tipoCliente.Replace(@"[", string.Empty).Replace(@"]", string.Empty).Replace("\"", string.Empty);

            if (empresa == "0")
            {
                empresa = "";
            }

            if (sucursal == "0")
            {
                sucursal = "";
            }

            using (var context = new as2oasis())
            {
                //var presupuesto = context.Presupuesto_Consolidado(empresa, sucursal, fecha_desde_, fecha_hasta_,tipoCliente_);

                var presupuesto = context.Presupuesto_Consolidado(empresa, sucursal, fecha_desde_, fecha_hasta_, tipoCliente_).ToList();

                var presupuesto_json = JsonConvert.SerializeObject(presupuesto, Formatting.Indented);

                return Json(presupuesto_json, JsonRequestBehavior.AllowGet);

            }
            //return View();
        }

        public JsonResult ObtenerConsolidadoCobros(int id_consolidado)
        {
          
            using (var context = new as2oasis())
            {
                
                if (id_consolidado > 0)
                {
                    IQueryable<Reporte_Consolidado_Cierre> data = context.Reporte_Consolidado_Cierre.Where(
                    x => x.id_consolidado_cierre == id_consolidado
                    );

                    var data_json = Json(
                    data
                    .ToList()
                    .Select(x => new
                    {
                        x.empresa,
                        fecha_creacion = x.fecha_creacion==null?null:x.fecha_creacion,
                        x.numero_consolidado,
                        x.numero_cobro,
                        x.recaudador,
                        x.valor_cobro,
                        x.valor_total
                       
                    }), JsonRequestBehavior.AllowGet
                    );

                    data_json.MaxJsonLength = 500000000;

                    return data_json;
                }
                else
                {
                    var resultado = 0;
                    return Json(resultado, JsonRequestBehavior.AllowGet);
                }
                
            }
        }

        [HttpPost]
        public ActionResult GuardarConsolidado(string empresa, string sucursal, string fecha_desde, string fecha_hasta)
        {
            try
            {
                int codigo_consolidado = 0;
                string secuencia = "";
                int cod_cobro = 0;
                int cod_secuencia = 0;
                int numero_sec = 0;
                var db = new as2oasis();
                var _fecha_inicio = Convert.ToDateTime(fecha_desde);
                var _fecha_fin = Convert.ToDateTime(fecha_hasta);
                int id_organizacion = 0;
                int id_sucursal = 0;
                int id_cobro = 0;
                int c_cobros = 0;
                string _usuario = User.Identity.GetUserName();


                if (empresa == "0")
                {
                    return new HttpStatusCodeResult(400);
                }

                if (sucursal == "0")
                {
                    return new HttpStatusCodeResult(400);
                }

                var lista_numero = db.V_cobros_tmp.Where(x=> x.estado == 1 && x.usuario_creacion == _usuario 
                                                        && x.empresa == empresa).ToList();
                c_cobros = lista_numero.Count();

                if (c_cobros <= 0)
                {
                    //return new HttpStatusCodeResult(400);
                    var errorModel = new { error = "No hay datos para Procesar" };
                    return new JsonHttpStatusResult(errorModel, HttpStatusCode.InternalServerError);

                    //Response.StatusCode = Convert.ToInt32(System.Net.HttpStatusCode.InternalServerError);
                    //return Json(new { Data = "There was an error" }, JsonRequestBehavior.AllowGet);

                }

                foreach (var lis in lista_numero)
                {
                    cod_cobro = lis.id_cobro;
                    
                    //detalle = db.Detalle_Cobros.Where(x => x.cod_tabla == cod_cobro).FirstOrDefault();
                    var detalle = db.SP_Guarda_Cobros(2, empresa, sucursal, _fecha_inicio, _fecha_fin, 10, null, cod_cobro, null).ToList();

                    if (detalle == null)
                    {
                        return new HttpStatusCodeResult(400);
                    }
                    else
                    {
                        foreach (var re in detalle)
                        {
                            id_organizacion = Convert.ToInt32(re.id_organizacion);
                            id_sucursal = Convert.ToInt32(re.id_sucursal);
                            id_cobro = Convert.ToInt32(re.id_cobro);
                            
                        }
                    }

                    using (var as2 = new AS2Context())
                    {
                        var cons_secuencia = as2.secuencia.Where(x => x.id_organizacion == id_organizacion &&
                                    x.prefijo == "CCDC-").FirstOrDefault();

                        numero_sec = cons_secuencia.numero + 1;
                        cod_secuencia = cons_secuencia.id_secuencia;

                        if (numero_sec >= 1 && numero_sec < 10)
                        {
                            secuencia = cons_secuencia.prefijo + "0000" + Convert.ToString(numero_sec);
                        }

                        if (numero_sec >= 10 && numero_sec < 99)
                        {
                            secuencia = cons_secuencia.prefijo + "000" + Convert.ToString(numero_sec);
                        }

                        if (numero_sec >= 99 && numero_sec < 999)
                        {
                            secuencia = cons_secuencia.prefijo + "00" + Convert.ToString(numero_sec);
                        }

                        if (numero_sec >= 999 && numero_sec < 9999)
                        {
                            secuencia = cons_secuencia.prefijo + "0" + Convert.ToString(numero_sec);
                        }

                    }

                    using (var oasis = new as2oasis())
                    {

                        var consolidado = oasis.consolidado_cierre.Where(x => x.id_organizacion == id_organizacion &&
                                        x.numero_consolidado == secuencia).FirstOrDefault();

                        if (consolidado == null && codigo_consolidado <= 0)
                        {
                            consolidado_cierre cons_cierre = new consolidado_cierre();
                            cons_cierre.id_organizacion = id_organizacion;
                            cons_cierre.id_sucursal = id_sucursal;
                            cons_cierre.numero_consolidado = secuencia;
                            cons_cierre.fecha = DateTime.Now;
                            cons_cierre.estado = 1;
                            cons_cierre.fecha = DateTime.Now;
                            cons_cierre.fecha_creacion = DateTime.Now;
                            cons_cierre.usuario_creacion = User.Identity.GetUserName();

                            oasis.consolidado_cierre.Add(cons_cierre);
                            oasis.SaveChanges();

                            codigo_consolidado = cons_cierre.id_consolidado_cierre;

                        }

                        //var detalle_cobro = db.Detalle_Cobros.Where(x => x.id_cobro == id_cobro).ToList();
                        var detalle_cobro = db.SP_Guarda_Cobros(3, empresa, sucursal, _fecha_inicio, _fecha_fin, 10, null, id_cobro, null).ToList();
                        
                        if (detalle_cobro == null)
                        {
                            return new HttpStatusCodeResult(400);
                        }

                        detalle_consolidado_cierre detalle_cons = new detalle_consolidado_cierre();

                        foreach (var item in detalle_cobro)
                        {

                            detalle_cons.id_consolidado_cierre = codigo_consolidado;
                            detalle_cons.id_organizacion = item.id_organizacion;
                            detalle_cons.id_sucursal = item.id_sucursal;
                            detalle_cons.id_cobro = item.id_cobro;
                            detalle_cons.id_detalle_forma_cobro = item.id_detalle_forma_cobro;
                            detalle_cons.id_detalle_cobro = item.id_detalle_cobro;
                            detalle_cons.numero_consolidado = secuencia;
                            detalle_cons.numero = item.numero;
                            detalle_cons.numero_ant = item.num_ant;
                            detalle_cons.valor = item.valor_cobro;
                            detalle_cons.cobrador = item.cobrador;
                            detalle_cons.id_forma_pago = item.id_forma_pago;
                            detalle_cons.fecha_pago = Convert.ToDateTime(item.fecha_pago);
                            detalle_cons.documento = item.num_documento;
                            detalle_cons.valor_total = item.valor_total;
                            detalle_cons.fecha_creacion = DateTime.Now;
                            detalle_cons.usuario_creacion = User.Identity.GetUserName();

                            oasis.detalle_consolidado_cierre.Add(detalle_cons);
                            oasis.SaveChanges();

                        }

                    }

                }

                ActualizaConsolidado(numero_sec, cod_secuencia, codigo_consolidado);
                //ImprimirConsolidado(codigo_consolidado);

                return new HttpStatusCodeResult(200);
               
            }
            catch (Exception err)            
            {
                err.InnerException.ToString();
                return new HttpStatusCodeResult(400);
            }

        }

        public ActionResult ActualizaConsolidado(int secuencia, int cod_secuencia, int cod_consolidado)
        {
            try
            {

                var consolidado = new consolidado_cierre();
                decimal total = 0;
                var db = new as2oasis();

                //using (var db = new as2oasis())
                //{
                    consolidado = db.consolidado_cierre.Where(x => x.id_consolidado_cierre == cod_consolidado).FirstOrDefault();
                    var detalle_consolidado = db.detalle_consolidado_cierre.Where(x => x.id_consolidado_cierre == cod_consolidado).ToList();
                //}
                                   
                using (var as2 = new AS2Context())
                {
                    var act_secuencia = as2.secuencia.Where(x => x.id_secuencia == cod_secuencia).FirstOrDefault();
                    
                    act_secuencia.numero = secuencia;
                    act_secuencia.usuario_modificacion = User.Identity.GetUserName();
                    act_secuencia.fecha_modificacion = DateTime.Now;
                    as2.SaveChanges();

                    if (consolidado != null && detalle_consolidado != null)
                    {

                        foreach (var item in detalle_consolidado)
                        {
                            total = total + item.valor;

                            var detalle_cobro = as2.detalle_forma_cobro.Where(x => x.id_detalle_forma_cobro == item.id_detalle_forma_cobro).FirstOrDefault();

                            if (detalle_cobro != null)
                            {
                                detalle_cobro.estado_consolidado = 5;
                                detalle_cobro.fecha_consolidado = DateTime.Now;
                                as2.SaveChanges();
                            }

                            var act_cobro = as2.cobro.Where(x => x.id_cobro == item.id_cobro).FirstOrDefault();
                            if (act_cobro != null)
                            {
                                act_cobro.estado_consolidado = 5;
                                act_cobro.usuario_modificacion = User.Identity.GetUserName();
                                act_cobro.fecha_modificacion = DateTime.Now;
                                as2.SaveChanges();
                            }
                            
                            var detalle = db.SP_Quita_cobros(item.id_cobro);

                        }                       

                    }

                }

                if (total > 0)
                {
                    consolidado.valor_total = total;
                    db.SaveChanges();
                }

                return new HttpStatusCodeResult(200);

            }
            catch (Exception err)
            {
                err.InnerException.ToString();
                return new HttpStatusCodeResult(400);
            }

        }

        [HttpPost]
        public JsonResult ObtenerDetalleCheques(string empresa, string sucursal, string fecha_desde, string fecha_hasta, string estado)
        {

            using (var db = new as2oasis())
            {
                var _fecha_inicio = Convert.ToDateTime(fecha_desde);
                var _fecha_fin = Convert.ToDateTime(fecha_hasta);

                IQueryable<Detalle_Cheques> data = db.Detalle_Cheques.Where(
                    x =>x.fecha_cheque >= _fecha_inicio &&
                    x.fecha_cheque <= _fecha_fin
                    );

                if (empresa != "0")
                {
                    data = data.Where(x => x.empresa == empresa);
                }

                if (sucursal != "0")
                {
                    data = data.Where(x => x.sucursal == sucursal);
                }

                if (estado != "10")
                {
                    if (estado == "Consolidado")
                    {
                        data = data.Where(x => x.nom_estado_cons == estado);
                    }
                    else
                    {
                        data = data.Where(x => x.nom_estado == estado);
                    }
                    
                }

                var data_json = Json(
                    data
                    .ToList()
                    .Select(x => new
                    {
                        x.empresa,
                        x.sucursal,
                        x.numero,
                        fecha_cobro = x.fecha_cobro == null ? null : x.fecha_cobro.ToString("yyyy-MM-dd"),
                        x.cobrador,
                        x.identificacion,
                        x.cliente,
                        num_documento = x.num_documento == null ? null : x.num_documento,
                        //x.factura,
                        nom_banco = x.nom_banco == null ? null : x.nom_banco,
                        x.cta_bancaria,
                        x.num_cheque,
                        fecha_cheque = x.fecha_cheque==null?null:Convert.ToString(x.fecha_cheque),
                        x.memo,
                        //x.valor_cobro,
                        x.valor_total,
                        x.nom_estado,
                        Fecha_Validacion = x.fecha_consolidado
                    }), JsonRequestBehavior.AllowGet
                    );

                data_json.MaxJsonLength = 500000000;

                return data_json;
            }

        }

        [HttpPost]
        public JsonResult ObtenerConsolidadoCierres(string empresa, string numero)
        {

            using (var db = new as2oasis())
            {
                
                IQueryable<Consulta_Consolidado_Cierres> data = db.Consulta_Consolidado_Cierres.Where(
                    x => x.estado != 10 
                    );

                if (empresa != "0")
                {
                    data = data.Where(x => x.empresa == empresa);
                }

                if (numero != "0")
                {
                    data = data.Where(x => x.numero_consolidado == numero);
                }

                var data_json = Json(
                    data
                    .ToList()
                    .Select(x => new
                    {
                        x.empresa,
                        x.sucursal,
                        fecha = x.fecha == null?null: Convert.ToString(x.fecha),
                        x.numero_consolidado,
                        x.valor_total,
                        x.nom_estado,
                        codigo_consolidado = x.id_consolidado_cierre
                    }), JsonRequestBehavior.AllowGet
                    );

                data_json.MaxJsonLength = 500000000;

                return data_json;
            }
        }

        public JsonResult ObtenerCarteraProceso(
          string empresa,
            string sucursal,
            string tipoCliente,
            string fecha_desde,
            string fecha_hasta)
        {
            DateTime fecha_desde_ = DateTime.Parse(fecha_desde);
            DateTime fecha_hasta_ = DateTime.Parse(fecha_hasta);

            var tipoC = JsonConvert.DeserializeObject(tipoCliente);
            var tipoCliente_ = tipoCliente.Replace(@"[", string.Empty).Replace(@"]", string.Empty).Replace("\"", string.Empty);
            var lista_tipoCliente = tipoCliente_.Split(',');

            if (empresa == "0")
            {
                empresa = "";
            }

            if (sucursal == "0")
            {
                sucursal = "";
            }

            using (var context = new as2oasis())
            {
            
                var nc = context.SP_Reporte_Cartera(empresa, sucursal, fecha_desde_, fecha_hasta_, tipoCliente_).ToList();

                var nc_json = JsonConvert.SerializeObject(nc, Formatting.Indented);
                var json_data = Json(nc_json, JsonRequestBehavior.AllowGet);
                json_data.MaxJsonLength = 50000000;
                return json_data;

            }


        }

        public ActionResult ImprimirConsolidado(int codigo_consolidado)
        {

            using (var db = new as2oasis())
            {
                var cabecera = db.Consulta_Consolidado_Cierres
                    .Where(x => x.id_consolidado_cierre == codigo_consolidado)
                    .FirstOrDefault();

                string empresa = cabecera.empresa;

                var vendedores = db.SP_Detalle_Vendedor(codigo_consolidado).ToList();
               
                using (MemoryStream myMemoryStream = new MemoryStream())
                {
                    Reporte R = new Reporte();
                    R.MemoryStream = myMemoryStream;
                    R.Empresa = empresa;
                    R.tipo_reporte = Reporte.TipoReporte.Reporte;
                    float[] margenes = new float[] { 0f, 0f, 20f, 10f };
                    //var doc = R.CrearDocA4(margenes);
                    var doc = R.CrearDoc(true);
                    //var doc = R.CrearDocA4(margenes);
                    var pdf = R.CrearPDF();
                    var hoy = DateTime.Now;

                    var total_efec = 0.00;
                    var total_cheq = 0.00;
                    var total_chep = 0.00;
                    var total_trans = 0.00;
                    var total_dep = 0.00;

                    var total_ant = 0.00;
                    var total_ret = 0.00;
                    var total_cruce = 0.00;
                    var total_otro = 0.00;

                    var fuente_cabecera = R.CrearFuente("georgia", 10, 1, BaseColor.BLACK);
                    var fuente_cabecera_regular = R.CrearFuente("georgia", 10, 0, BaseColor.BLACK);
                    var fuente_tabla_detalle = R.CrearFuente("georgia", 8, 0, BaseColor.BLACK);
                    var fuente_tabla_cabecera = R.CrearFuente("georgia", 8, 1, BaseColor.BLACK);

                    var _standardFont = FontFactory.GetFont("SEGOE UI", 15, Font.BOLD, BaseColor.BLACK);
                    var subtitulo = FontFactory.GetFont("SEGOE UI", 7, Font.BOLD, BaseColor.BLACK);
                    var encabezado_tabla = FontFactory.GetFont("SEGOE UI", 8, Font.BOLD, BaseColor.BLACK);
                    var detalle = FontFactory.GetFont("SEGOE UI", 7, Font.NORMAL, BaseColor.BLACK);

                    doc.AddTitle($"Consolidado Cobros#{cabecera.numero_consolidado}");
                    doc.Open();

                    //doc.Add(R.ImagenFondo(empresa));
                    //500 totw
                    var encabezado = new PdfPTable(3)
                    {
                        LockedWidth = true,
                        //TotalWidth = 500f,
                        TotalWidth = 800f,
                        SpacingBefore = 5f,
                        //SpacingBefore = 3f,
                        SpacingAfter = 20f
                    };

                    encabezado.SetWidths(new float[] { 66f, 100f, 66f});
                    //encabezado.SetWidths(new float[] { 66f, 100f, 66f, 150f, 66f, 52f });
                    //encabezado.SetWidths(new float[] { 66f, 70f, 66f, 100f, 66f, 52f, 66f, 50f });

                    var table1 = new PdfPTable(3)
                    {
                        LockedWidth = true,
                        //TotalWidth = 500f,
                        TotalWidth = 800f,
                        SpacingBefore = 5f,
                        //SpacingAfter = 20f
                    };

                    var _Titulo = new Chunk($"ANALISIS DE COBRO NO. {cabecera.numero_consolidado} \n  Fecha: {cabecera.fecha} \n {cabecera.empresa} ",
                        _standardFont);
                    Paragraph __Titulo = new Paragraph(_Titulo);
                    __Titulo.Alignment = Element.ALIGN_CENTER;

                    //var _Empresa = new Chunk($"EMPRESA {cabecera.empresa} ",
                    //    _standardFont);
                    //Paragraph __Empresa = new Paragraph(_Empresa);
                    //__Empresa.Alignment = Element.ALIGN_LEFT;

                    //table1.SetWidths(new float[] { 100f, 200f, 100f});
                    table1.SetWidths(new float[] { 100f, 200f, 100f});

                    PdfPCell cell1 = new PdfPCell();
                    cell1 = new PdfPCell();

                    cell1.Padding = 0;
                    cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell1.Border = PdfPCell.NO_BORDER;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(_Titulo));
                    cell1.Padding = 0;
                    cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell1.Border = PdfPCell.NO_BORDER;

                    //PdfPCell cell2 = new PdfPCell();
                    //cell2 = new PdfPCell();

                    //cell2.Padding = 0;
                    //cell2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    //cell2.Border = PdfPCell.NO_BORDER;
                    //table1.AddCell(cell2);

                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase("Usuario: " + User.Identity.GetUserName() + " \n " + "Fecha Impresion:" + hoy));
                    cell1.Padding = 0;
                    cell1.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    cell1.Border = PdfPCell.NO_BORDER;

                    table1.AddCell(cell1);

                    //cell1 = new PdfPCell(new Phrase(cabecera.empresa));
                    //cell1.Padding = 0;
                    //cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    //cell1.Border = PdfPCell.NO_BORDER;

                    //table1.AddCell(cell1);

                    doc.Add(table1);

                    doc.Add(Chunk.NEWLINE);

                    #region detalle
                    table1.FlushContent();
                    table1.SetWidths(new float[] { 150f, 150f, 100f});

                    var detalle_cierres = new PdfPTable(13);
                    detalle_cierres.LockedWidth = true;
                    //detalle_cierres.TotalWidth = 500f;
                    detalle_cierres.TotalWidth = 800f;
                    detalle_cierres.SpacingBefore = 3f;
                    //detalle_cierres.SetWidths(new float[] { 50f, 205f, 20f,45f,45f,45f,45f,45f});
                    detalle_cierres.SetWidths(new float[] { 30f, 35f, 30f, 45f, 35f, 40f, 80f, 60f, 30f, 40f, 32f, 32f, 32f });

                    PdfPCell cell_detalle = new PdfPCell();
                    cell_detalle.Padding = 0;
                    cell_detalle.Border = PdfPCell.NO_BORDER;

                    //var vendedores = db.Detalle_Vendedores.Where(x => x.id_consolidado_cierre == cabecera.id_consolidado_cierre).ToList();

                    foreach (var item in vendedores)
                    {

                        cell1 = new PdfPCell(new Phrase(item.cobrador));
                        cell1.Padding = 0;
                        cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        cell1.PaddingBottom = 4f;
                        cell1.CellEvent = new RoundedBorder();
                        cell1.Border = PdfPCell.NO_BORDER;
                        //cell1.BackgroundColor = new BaseColor(53, 101, 255);

                        table1.AddCell(cell1);

                        cell1 = new PdfPCell(new Phrase(""));
                        cell1.Padding = 0;
                        cell1.Border = PdfPCell.NO_BORDER;

                        table1.AddCell(cell1);

                        cell1 = new PdfPCell(new Phrase(""));
                        cell1.Padding = 0;
                        cell1.Border = PdfPCell.NO_BORDER;

                        table1.AddCell(cell1);

                        //if(MP.Count()>0)
                        if (item.total_cobro > 0)
                            doc.Add(table1);

                        detalle_cierres.AddCell(new Phrase("Fecha Cob.", subtitulo));
                        detalle_cierres.AddCell(new Phrase("Cobro", subtitulo));
                        detalle_cierres.AddCell(new Phrase("Fecha Fact.", subtitulo));
                        detalle_cierres.AddCell(new Phrase("Factura", subtitulo));
                        detalle_cierres.AddCell(new Phrase("Valor Fact.", subtitulo));
                        detalle_cierres.AddCell(new Phrase("Ruc", subtitulo));
                        detalle_cierres.AddCell(new Phrase("Cliente", subtitulo));
                        detalle_cierres.AddCell(new Phrase("Forma Pago", subtitulo));
                        detalle_cierres.AddCell(new Phrase("Fecha Doc.", subtitulo));
                        detalle_cierres.AddCell(new Phrase("# Documento", subtitulo));
                        detalle_cierres.AddCell(new Phrase("Monto", subtitulo));
                        detalle_cierres.AddCell(new Phrase("Valor Cob.", subtitulo));
                        detalle_cierres.AddCell(new Phrase("Nota", subtitulo));

                        //var detalle_cobro = db.Cobros_Vendedor.Where(x => x.id_consolidado_cierre == cabecera.id_consolidado_cierre &&
                        //                                             x.cobrador == item.cobrador); 
                        var detalle_cobro = db.SP_Cobros_Vendedor(cabecera.id_consolidado_cierre, item.cobrador);
                        var suma_efec = 0.00;
                        var suma_cheq = 0.00;
                        var suma_chep = 0.00;
                        var suma_trans = 0.00;
                        var suma_dep = 0.00;
                        var suma_ant = 0.00;
                        var suma_ret = 0.00;
                        var suma_cruce = 0.00;
                        var suma_otro = 0.00;

                        foreach (var cob in detalle_cobro)
                        {
                            detalle_cierres.AddCell(new Phrase(cob.fecha_cobro, detalle));
                            detalle_cierres.AddCell(new Phrase(cob.num_cobro, detalle));
                            detalle_cierres.AddCell(new Phrase(cob.fecha_factura, detalle));
                            detalle_cierres.AddCell(new Phrase(cob.factura, detalle));
                            detalle_cierres.AddCell(new Phrase(string.Format("{0:n2}", cob.valor_factura), detalle));
                            detalle_cierres.AddCell(new Phrase(cob.identificacion, detalle));
                            detalle_cierres.AddCell(new Phrase(cob.cliente, detalle));                            
                            detalle_cierres.AddCell(new Phrase(cob.forma_pago, detalle));
                            detalle_cierres.AddCell(new Phrase(cob.fecha_pago, detalle));
                            detalle_cierres.AddCell(new Phrase(cob.documento, detalle));
                            detalle_cierres.AddCell(new Phrase(string.Format("{0:n2}", cob.valor_total), detalle));
                            detalle_cierres.AddCell(new Phrase(string.Format("{0:n2}", cob.valor_cobro), detalle));
                            detalle_cierres.AddCell(new Phrase(cob.nota, detalle));

                            if (cob.estado_co != 0)
                            {
                                if (cob.cod_forma_pago == "EFEC")
                                {
                                    suma_efec = suma_efec + Convert.ToDouble(cob.valor_cobro);

                                    if (cob.cod_ant == "ANC")
                                    {
                                        suma_ant = suma_ant + Convert.ToDouble(cob.valor_cobro);

                                    }

                                }

                                else if (cob.cod_forma_pago == "CHEQ")
                                {
                                    suma_cheq = suma_cheq + Convert.ToDouble(cob.valor_cobro);
                                    if (cob.cod_ant == "ANC")
                                    {
                                        suma_ant = suma_ant + Convert.ToDouble(cob.valor_cobro);

                                    }

                                }

                                else if (cob.cod_forma_pago == "CHEQP")
                                {
                                    suma_chep = suma_chep + Convert.ToDouble(cob.valor_cobro);

                                    if (cob.cod_ant == "ANC")
                                    {
                                        suma_ant = suma_ant + Convert.ToDouble(cob.valor_cobro);

                                    }

                                }

                                else if (cob.cod_forma_pago == "DEPB" || cob.cod_forma_pago == "DEPBDA")
                                {
                                    suma_dep = suma_dep + Convert.ToDouble(cob.valor_cobro);

                                    if (cob.cod_ant == "ANC")
                                    {
                                        suma_ant = suma_ant + Convert.ToDouble(cob.valor_cobro);

                                    }

                                }

                                else if (cob.cod_forma_pago == "CTA" || cob.cod_forma_pago == "CTADA")
                                {
                                    suma_trans = suma_trans + Convert.ToDouble(cob.valor_cobro);

                                    if (cob.cod_ant == "ANC")
                                    {
                                        suma_ant = suma_ant + Convert.ToDouble(cob.valor_cobro);

                                    }

                                }


                                else if (cob.cod_forma_pago == "RETFUENTE" || cob.cod_forma_pago == "RTFTE1.75%" || cob.cod_forma_pago == "RTFTE2%"
                                    || cob.cod_forma_pago == "RTFTE2.75%" || cob.cod_forma_pago == "RTFTE8%")
                                {
                                    suma_ret = suma_ret + Convert.ToDouble(cob.valor_cobro);

                                    if (cob.cod_ant == "ANC")
                                    {
                                        suma_ant = suma_ant + Convert.ToDouble(cob.valor_cobro);

                                    }

                                }

                                else if (cob.cod_forma_pago == "CR")
                                {
                                    suma_cruce = suma_cruce + Convert.ToDouble(cob.valor_cobro);

                                    if (cob.cod_ant == "ANC")
                                    {
                                        suma_ant = suma_ant + Convert.ToDouble(cob.valor_cobro);

                                    }

                                }

                                else
                                {
                                    suma_otro = suma_otro + Convert.ToDouble(cob.valor_cobro);

                                    if (cob.cod_ant == "ANC")
                                    {
                                        suma_ant = suma_ant + Convert.ToDouble(cob.valor_cobro);

                                    }

                                }

                            }
                            

                        }

                        total_efec = total_efec + suma_efec;
                        total_cheq = total_cheq + suma_cheq;
                        total_chep = total_chep + suma_chep;
                        total_dep = total_dep + suma_dep;
                        total_trans = total_trans + suma_trans;
                        total_ant = total_ant + suma_ant;
                        total_ret = total_ret + suma_ret;
                        total_cruce = total_cruce + suma_cruce;
                        total_otro = total_otro + suma_otro;

                        //if (MP.Count() > 0)
                        if (item.total_cobro > 0)
                            doc.Add(detalle_cierres);
                        detalle_cierres.FlushContent();
                        table1.FlushContent();

                        cell1 = new PdfPCell(new Phrase("Subtotal EFEC:", subtitulo));
                        cell1.Border = PdfPCell.NO_BORDER;
                        detalle_cierres.AddCell(cell1);
                        cell1 = new PdfPCell(new Phrase(string.Format("{0:n2}", suma_efec), detalle));
                        cell1.Border = PdfPCell.NO_BORDER;
                        detalle_cierres.AddCell(cell1);
                        cell1 = new PdfPCell(new Phrase("Subtotal CHEQ:", subtitulo));
                        cell1.Border = PdfPCell.NO_BORDER;
                        detalle_cierres.AddCell(cell1);
                        cell1 = new PdfPCell(new Phrase(string.Format("{0:n2}", suma_cheq), detalle));
                        cell1.Border = PdfPCell.NO_BORDER;
                        detalle_cierres.AddCell(cell1);
                        cell1 = new PdfPCell(new Phrase("Subtotal CHEQP:", subtitulo));
                        cell1.Border = PdfPCell.NO_BORDER;
                        detalle_cierres.AddCell(cell1);
                        cell1 = new PdfPCell(new Phrase(string.Format("{0:n2}", suma_chep), detalle));
                        cell1.Border = PdfPCell.NO_BORDER;
                        detalle_cierres.AddCell(cell1);
                        cell1 = new PdfPCell(new Phrase("Subtotal Dept.: "+ string.Format("{0:n2}", suma_dep), subtitulo));
                        cell1.Border = PdfPCell.NO_BORDER;
                        detalle_cierres.AddCell(cell1);
                        cell1 = new PdfPCell(new Phrase("Subtotal Transferencia:", subtitulo));
                        cell1.Border = PdfPCell.NO_BORDER;
                        detalle_cierres.AddCell(cell1);
                        cell1 = new PdfPCell(new Phrase(string.Format("{0:n2}", suma_trans), detalle));
                        cell1.Border = PdfPCell.NO_BORDER;
                        detalle_cierres.AddCell(cell1);

                        cell1 = new PdfPCell(new Phrase("Subtotal Ant.: " + string.Format("{0:n2}", suma_ant), subtitulo));
                        cell1.Border = PdfPCell.NO_BORDER;
                        detalle_cierres.AddCell(cell1);
                        cell1 = new PdfPCell(new Phrase("Subtotal REt.: " + string.Format("{0:n2}", suma_ret), subtitulo));
                        cell1.Border = PdfPCell.NO_BORDER;
                        detalle_cierres.AddCell(cell1);
                        cell1 = new PdfPCell(new Phrase("Subtotal Cruce.: " + string.Format("{0:n2}", suma_cruce), subtitulo));
                        cell1.Border = PdfPCell.NO_BORDER;
                        detalle_cierres.AddCell(cell1);
                        cell1 = new PdfPCell(new Phrase("Subtotal Otros.: " + string.Format("{0:n2}", suma_otro), subtitulo));
                        cell1.Border = PdfPCell.NO_BORDER;
                        detalle_cierres.AddCell(cell1);


                        //if (MP.Count() > 0)
                        if (item.total_cobro > 0)
                            doc.Add(detalle_cierres);
                        detalle_cierres.FlushContent();

                    }

                    #endregion

                    #region Totales
                    cell1 = new PdfPCell(new Phrase("Total EFEC:", subtitulo));
                    cell1.Border = PdfPCell.NO_BORDER;
                    detalle_cierres.AddCell(cell1);
                    cell1 = new PdfPCell(new Phrase(string.Format("{0:n2}", total_efec), detalle));
                    cell1.Border = PdfPCell.NO_BORDER;
                    detalle_cierres.AddCell(cell1);
                    cell1 = new PdfPCell(new Phrase("Total CHEQ:", subtitulo));
                    cell1.Border = PdfPCell.NO_BORDER;
                    detalle_cierres.AddCell(cell1);
                    cell1 = new PdfPCell(new Phrase(string.Format("{0:n2}", total_cheq), detalle));
                    cell1.Border = PdfPCell.NO_BORDER;
                    detalle_cierres.AddCell(cell1);
                    cell1 = new PdfPCell(new Phrase("Total CHEQP:", subtitulo));
                    cell1.Border = PdfPCell.NO_BORDER;
                    detalle_cierres.AddCell(cell1);
                    cell1 = new PdfPCell(new Phrase(string.Format("{0:n2}", total_chep), detalle));
                    cell1.Border = PdfPCell.NO_BORDER;
                    detalle_cierres.AddCell(cell1);
                    cell1 = new PdfPCell(new Phrase("Total Dept.: "+ string.Format("{0:n2}", total_dep), subtitulo));
                    cell1.Border = PdfPCell.NO_BORDER;
                    detalle_cierres.AddCell(cell1);
                    cell1 = new PdfPCell(new Phrase("Total Transferencia:", subtitulo));
                    cell1.Border = PdfPCell.NO_BORDER;
                    detalle_cierres.AddCell(cell1);
                    cell1 = new PdfPCell(new Phrase(string.Format("{0:n2}", total_trans), detalle));
                    cell1.Border = PdfPCell.NO_BORDER;
                    detalle_cierres.AddCell(cell1);
                    cell1 = new PdfPCell(new Phrase("Total Ant.: " + string.Format("{0:n2}", total_ant), subtitulo));
                    cell1.Border = PdfPCell.NO_BORDER;
                    detalle_cierres.AddCell(cell1);
                    cell1 = new PdfPCell(new Phrase("Total REt.: " + string.Format("{0:n2}", total_ret), subtitulo));
                    cell1.Border = PdfPCell.NO_BORDER;
                    detalle_cierres.AddCell(cell1);
                    cell1 = new PdfPCell(new Phrase("Total Cruce.: " + string.Format("{0:n2}", total_cruce), subtitulo));
                    cell1.Border = PdfPCell.NO_BORDER;
                    detalle_cierres.AddCell(cell1);
                    cell1 = new PdfPCell(new Phrase("Total Otros: " + string.Format("{0:n2}", total_otro), subtitulo));
                    cell1.Border = PdfPCell.NO_BORDER;
                    detalle_cierres.AddCell(cell1);


                    doc.Add(detalle_cierres);
                    detalle_cierres.FlushContent();

                    #endregion

                    #region FIRMAS   

                    //var tabla_firma = new PdfPTable(2);
                    //tabla_firma.LockedWidth = true;
                    //tabla_firma.TotalWidth = 600f;
                    //tabla_firma.SpacingBefore = 25f;
                    //tabla_firma.SetWidths(new float[] { 250f, 250f });

                    //cell1 = new PdfPCell(new Phrase("________________"));
                    //cell1.Padding = 0;
                    //cell1.Border = PdfPCell.NO_BORDER;
                    //cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //tabla_firma.AddCell(cell1);

                    //cell1 = new PdfPCell(new Phrase("________________"));
                    //cell1.Padding = 0;
                    //cell1.Border = PdfPCell.NO_BORDER;
                    //cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //tabla_firma.AddCell(cell1);

                    //cell1 = new PdfPCell(new Phrase("REVISADO \n Departamento de Auditoría"));
                    //cell1.Padding = 5f;
                    //cell1.Border = PdfPCell.NO_BORDER;
                    //cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //tabla_firma.AddCell(cell1);

                    //cell1 = new PdfPCell(new Phrase("APROBADO"));
                    //cell1.Padding = 5f;
                    //cell1.Border = PdfPCell.NO_BORDER;
                    //cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //tabla_firma.AddCell(cell1);

                    //doc.Add(tabla_firma);
                    #endregion
                    doc.Close();

                    var pdf_generado = R.GenerarPDF();

                    byte[] file = pdf_generado;
                    MemoryStream output = new MemoryStream();
                    output.Write(file, 0, file.Length);
                    output.Position = 0;

                    return new FileStreamResult(output, "application/pdf");
                
                }
            }

        }

        public ActionResult GuardarDevolucion(int codigo_consolidado)
        {
            try
            {

                AS2Context as2 = new AS2Context();

                using (var db = new as2oasis())
                {

                    var cabecera = db.consolidado_cierre.Where(x => x.id_consolidado_cierre == codigo_consolidado).FirstOrDefault();
                    var consolidado = db.detalle_consolidado_cierre.Where(x => x.id_consolidado_cierre == codigo_consolidado);

                    var cuenta_reg = consolidado.Count();
                    var suma_reg = consolidado.Sum(x => x.valor <= 0 ? 0 : x.valor);

                    if (cabecera.estado == 6)
                    {
                        return new HttpStatusCodeResult(400);
                    }

                    if (suma_reg > 0)
                    {
                        devolucion_consolidado_cierre devolucion = new devolucion_consolidado_cierre();
                        devolucion.id_organizacion = cabecera.id_organizacion;
                        devolucion.id_sucursal = cabecera.id_sucursal;
                        devolucion.id_consolidado_cierre = cabecera.id_consolidado_cierre;
                        devolucion.numero_consolidado = cabecera.numero_consolidado;
                        devolucion.valor = suma_reg;
                        devolucion.cant_reg = cuenta_reg;
                        devolucion.fecha_creacion = DateTime.Now;
                        devolucion.usuario_creacion = User.Identity.GetUserName();
                        devolucion.fecha_modificacion = DateTime.Now;
                        devolucion.usuario_modificacion = User.Identity.GetUserName();

                        db.devolucion_consolidado_cierre.Add(devolucion);
                        db.SaveChanges();

                        cabecera.estado = 6;
                        cabecera.fecha_modificacion = DateTime.Now;
                        cabecera.usuario_modificacion = User.Identity.GetUserName();
                        db.SaveChanges();

                        foreach (var item in consolidado)
                        {
                            var forma_cobro = as2.detalle_forma_cobro.Where(x => x.id_detalle_forma_cobro == item.id_detalle_forma_cobro).FirstOrDefault();

                            if (forma_cobro != null)
                            {
                                forma_cobro.estado_consolidado = 0;
                                forma_cobro.fecha_modificacion = DateTime.Now;
                                forma_cobro.usuario_modificacion = User.Identity.GetUserName();
                                as2.SaveChanges();
                            }
                            
                            var act_cobro = as2.cobro.Where(x => x.id_cobro == item.id_cobro).FirstOrDefault();

                            if (act_cobro != null)
                            {
                                act_cobro.estado_consolidado = 0;
                                act_cobro.fecha_modificacion = DateTime.Now;
                                act_cobro.usuario_modificacion = User.Identity.GetUserName();
                                as2.SaveChanges();
                            }

                        }

                    }

                    return new HttpStatusCodeResult(200);

                }

            }
            catch (Exception err)
            {
                err.InnerException.ToString();
                return new HttpStatusCodeResult(400);
            }

        }

        public void GuardarCobroTmp(string empresa, string sucursal, string fecha_desde, string fecha_hasta, string numero)
        {
            try
            {
                var _fecha_inicio = Convert.ToDateTime(fecha_desde);
                var _fecha_fin = Convert.ToDateTime(fecha_hasta);
                int cod_cobro = Convert.ToInt32(numero);

                using (var db = new as2oasis())
                {

                    var cobro = db.V_cobros_tmp.Where(x => x.id_cobro == cod_cobro).FirstOrDefault();
                    if (cobro == null)
                    {
                        var empresaCobro = db.V_cobros_empresa.Where(x => x.id_cobro == cod_cobro).FirstOrDefault();

                        cobros_tmp cobro_tmp = new cobros_tmp();
                        cobro_tmp.id_cobro = cod_cobro;
                        cobro_tmp.empresa = empresaCobro.empresa;
                        cobro_tmp.sucursal = sucursal;
                        cobro_tmp.fecha_inicio = _fecha_inicio;
                        cobro_tmp.fecha_fin = _fecha_fin;
                        cobro_tmp.estado = 1;
                        cobro_tmp.fecha_creacion = DateTime.Now;
                        cobro_tmp.usuario_creacion = User.Identity.GetUserName();

                        db.cobros_tmp.Add(cobro_tmp);
                        db.SaveChanges();
                    }                    

                 }

            }
            catch (Exception err)
            {
                err.InnerException.ToString();               
            }

        }

        public void EliminarCobroTmp(string empresa, string numero)
        {
            try
            {
                int cod_cobro = Convert.ToInt32(numero);

                using (var db = new as2oasis())
                {

                    var detalle = db.SP_Quita_cobros(cod_cobro);

                }

            }
            catch (Exception err)
            {
                err.InnerException.ToString();

            }

        }

        public JsonResult CargaCobroTmp(string empresa, string sucursal)
        {
            using (var db = new as2oasis())
            {

                //var data = db.V_cobros_tmp.Where(x => x.empresa == empresa && x.sucursal == sucursal);
                //var data_json = JsonConvert.SerializeObject(data, Formatting.Indented);
                //var json_data = Json(data_json, JsonRequestBehavior.AllowGet);
                //json_data.MaxJsonLength = 5000000;
                //return json_data;

                return Json(db.V_cobros_tmp.Where(x => x.empresa == empresa && x.sucursal == sucursal)
               .Select(x => new
               {
                   x.id_cobro
               }).ToList(), JsonRequestBehavior.AllowGet);

            }
        }
    
    }

}
