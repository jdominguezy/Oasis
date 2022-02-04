using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Oasis.Models;
using Oasis.Models.Login;
using OpenXmlPowerTools;

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
                var facturas =
                    context.Ventas_Consolidado
                    .ToList()
                    .Where(x => x.empresa == empresa &&
                                x.sucursal == sucursal &&
                                categoriaCliente.Contains(x.categoria) &&
                                x.fecha_factura >= DateTime.Parse(fecha_desde) &&
                                x.fecha_factura <= DateTime.Parse(fecha_hasta)
                            );

                if (!String.IsNullOrEmpty(visitador))
                {
                    var codigo_visitador = Int16.Parse(visitador);
                    facturas = facturas.Where(x => x.id_vendedor == codigo_visitador);
                }

                var listaFacturas = facturas
                    .ToList()
                    .Select(x => new
                    {
                        x.empresa,
                        x.sucursal,
                        x.categoria,
                        x.identificacion,
                        x.nombre_comercial,
                        x.secuencial_factura,
                        fecha_factura = x.fecha_factura.ToShortDateString(),
                        valor = x.valor_factura,
                        x.vendedor,
                        x.descripcion,
                        x.estado
                    });


                var listaFacturas_json = JsonConvert.SerializeObject(listaFacturas, Formatting.Indented);

                return Json(listaFacturas_json, JsonRequestBehavior.AllowGet);

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
                var presupuesto = context.Presupuesto_Consolidado(empresa, sucursal, fecha_desde_, fecha_hasta_,tipoCliente_);
                
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
            string tipoCliente)
        {
            var tipoC = JsonConvert.DeserializeObject(tipoCliente);
            var tipoCliente_ = tipoCliente.Replace(@"[", string.Empty).Replace(@"]", string.Empty).Replace("\"", string.Empty);
            var lista_tipoCliente = tipoCliente_.Split(',');
            using (var context = new as2oasis())
            {
                //context.Database.CommandTimeout = 320;
                var cartera =
                    context.Cartera.Where(
                        x => 
                        x.empresa == empresa &&
                        x.sucursal == sucursal && 
                        lista_tipoCliente.Contains(x.categoria)
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
                        secuencial_factura = (x.secuencial_factura.Replace("-", string.Empty)),
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
                         
                return Json(cartera, JsonRequestBehavior.AllowGet);
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
        public JsonResult ObtenerVentasProducto(string fecha_desde, string fecha_hasta, string empresa)
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
                        Fecha_factura = x.Fecha_factura.Value.ToShortDateString(),
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
                        x.Código_producto
                    }), JsonRequestBehavior.AllowGet
                    );

                data_json.MaxJsonLength = 500000000;

                return data_json;
            }
        }
    }
}
