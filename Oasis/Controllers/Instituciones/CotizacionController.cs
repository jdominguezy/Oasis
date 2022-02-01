using iTextSharp.text;
using iTextSharp.text.pdf;
using Oasis.Models;
using Oasis.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Oasis.Controllers.Instituciones
{
    public class CotizacionController : Controller
    {
        // GET: Cotizacion
        public ActionResult Index()
        {
            //var db = new as2oasis();
            return View();
        }

        public ActionResult CrearCotizacion()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CotizacionesGeneradas(int id_cotizacion)
        {
            using (var db = new as2oasis())
            {
                var id_empresa_cotizacion = db.cotizacion_cabecera
                    .Where(x => x.id_cotizacion == id_cotizacion)
                    .Select(x => x.empresaOasis.id_empresa)
                    .FirstOrDefault();

                var cotizaciones_generadas = db.relacion_cotizacion_ganadora_perdedora
                    .Where(x => x.id_cotizacion_ganadora == id_cotizacion)
                    .Select(x => x.cotizacion_cabecera1.empresaOasis.id_empresa)
                    .ToList()
                    ;

                cotizaciones_generadas.Add(id_empresa_cotizacion);

                var empresas_pendientes = db.empresaOasis
                    .Where(x => !cotizaciones_generadas.Any(y => y == x.id_empresa))
                    .Select(x => new { x.id_empresa, x.nombre })
                    .ToList();

                return Json(empresas_pendientes, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        public ActionResult ObtenerCotizacionesGeneradas(int id_cotizacion)
        {
            using (var db = new as2oasis())
            {
                var id_empresa_cotizacion = db.cotizacion_cabecera
                    .Where(x => x.id_cotizacion == id_cotizacion)
                    .Select(x => new
                    {
                        empresa = x.empresaOasis.nombre,
                        id_cotizacion = id_cotizacion
                    })
                    .FirstOrDefault();

                var cotizaciones_generadas = db.relacion_cotizacion_ganadora_perdedora
                    .Where(x => x.id_cotizacion_ganadora == id_cotizacion)
                    .Select(x => new
                    {
                        empresa = x.cotizacion_cabecera1.empresaOasis.nombre,
                        id_cotizacion = x.cotizacion_cabecera1.id_cotizacion
                    })
                    .ToList()
                    ;

                cotizaciones_generadas.Add(id_empresa_cotizacion);
                

                return Json(cotizaciones_generadas, JsonRequestBehavior.AllowGet);

            }
        }

        public class CotizacionesFaltantes
        {
            public List<int> empresas { get; set; }
            public int id_cotizacion_principal { get; set; }
        }

        [HttpGet]
        public void ImprimirCotizacion(int id_cotizacion)
        {
            Response.Clear();
            Response.ClearHeaders();
            Response.Write(modelarPDF(id_cotizacion));
            Response.Flush();
            Response.End();
        }

        public string ObtenerNombreMes(int mes)
        {
            switch (mes)
            {
                case 1:
                    return "enero";
                case 2:
                    return "febrero";
                case 3:
                    return "marzo";
                case 4:
                    return "abril";
                case 5:
                    return "mayo";
                case 6:
                    return "junio";
                case 7:
                    return "julio";
                case 8:
                    return "agosto";
                case 9:
                    return "septiembre";
                case 10:
                    return "octubre";
                case 11:
                    return "noviembre";
                case 12:
                    return "diciembre";
                default:
                    return null;

            }
        }

        public string modelarPDF(int id_cotizacion)
        {
            using (var db = new as2oasis())
            {
                var cabecera = db.cotizacion_cabecera
                    .Where(x => x.id_cotizacion == id_cotizacion)
                    .FirstOrDefault();
                var detalle = db.cotizacion_detalle
                    .Where(x => x.id_cotizacion_cabecera == id_cotizacion);
                var empresa = cabecera.empresaOasis.nombre;


                using (MemoryStream myMemoryStream = new MemoryStream())
                {
                    Reporte R = new Reporte();
                    R.MemoryStream = myMemoryStream;
                    R.Empresa = empresa;
                    R.tipo_reporte = Reporte.TipoReporte.Cotizacion;
                    string ruc_empresa = "";
                    float[] margenes = new float[] { 0f, 0f, 0f, 0f };

                    switch (cabecera.id_empresaOasis)
                    {
                        case 60://DISERMED
                            ruc_empresa = "0860004580001";
                            margenes = new float[] { 0f, 0f, 80f, 100f };
                            break;
                        case 61://DITROYA
                            ruc_empresa = "1360053400001";
                            margenes = new float[] { 0f, 0f, 100f, 100f };
                            break; 
                        default:
                            ruc_empresa = new AS2Context()
                            .organizacion.Where(x => x.id_organizacion == cabecera.id_empresaOasis)
                            .Select(x => x.identificacion).FirstOrDefault();
                            switch (cabecera.id_empresaOasis)
                            {
                                case 1: //LABOVIDA
                                    margenes = new float[] { 30f, 0f, 100f, 100f };
                                    break;
                                case 51://FARMALIGHT
                                    margenes = new float[] { 0f, 0f, 30f, 100f };
                                    break;
                                case 56://DANIVET
                                    margenes = new float[] { 0f, 0f, 40f, 100f };
                                    break;
                                case 57://LEBENFARMA
                                    margenes = new float[] { 0f, 0f, 30f, 100f };
                                    break;
                                case 58://ANYUPA
                                    margenes = new float[] { 0f, 0f, 30f, 100f };
                                    break;
                                case 59://MEDITOTAL
                                    margenes = new float[] { 0f, 0f, 30f, 100f };
                                    break;
                            }
                            break;
                    }
                    var doc = R.CrearDocA4(margenes);
                    var pdf = R.CrearPDF();
                    var hoy = DateTime.Now;
                    var fuente_cabecera = R.CrearFuente("georgia", 10, 1, BaseColor.BLACK);
                    var fuente_cabecera_regular = R.CrearFuente("georgia", 10, 0, BaseColor.BLACK);
                    var fuente_tabla_detalle = R.CrearFuente("georgia", 8, 0, BaseColor.BLACK);
                    var fuente_tabla_cabecera = R.CrearFuente("georgia", 8, 1, BaseColor.BLACK);

                    var cliente = new AS2Context()
                        .empresa
                        .Where(x => x.id_empresa == cabecera.id_cliente) 
                        .FirstOrDefault();

                    PdfPTable tabla_cabecera_1 = new PdfPTable(2)
                    {
                        LockedWidth = true,
                        TotalWidth = 550f,
                        SpacingBefore = 4f
                    };


                    tabla_cabecera_1.SetWidths(new float[] { 275f, 275f });
                    tabla_cabecera_1.AddCell(
                        R.CrearCelda($"Guayaquil, {hoy.Day+" de "+ ObtenerNombreMes(hoy.Month) + " del "+ hoy.Year}"
                        ,fuente_cabecera,0));
                    tabla_cabecera_1.AddCell(
                        R.CrearCelda($"RUC. {ruc_empresa}", fuente_cabecera,2));
                    tabla_cabecera_1.AddCell(
                        R.CrearCelda($"COTIZACION No. {cabecera.id_cotizacion}", fuente_cabecera,0));
                    tabla_cabecera_1.AddCell(
                        R.CrearCelda($"{empresa} ", fuente_cabecera,2));

                    doc.AddTitle($"Cotizacion {"#"+id_cotizacion+"-"+empresa}");
                    doc.Open();

                    doc.Add(R.ImagenFondo(empresa));

                    doc.Add(tabla_cabecera_1);

                    var tabla_cabecera_2 = new PdfPTable(1)
                    {
                        LockedWidth = true,
                        TotalWidth = 550f,
                        SpacingBefore = 10f
                    };

                    tabla_cabecera_2.AddCell(
                        R.CrearCelda(cliente.nombre_comercial, fuente_cabecera));
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda($"RUC: {cliente.identificacion}", fuente_cabecera));
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda($"DIRECCIÓN: {cliente.direccion_empresa.First().descripcion}", fuente_cabecera));

                    doc.Add(tabla_cabecera_2);

                    tabla_cabecera_2.DeleteBodyRows();
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda("Departamento de adquisiciones", fuente_cabecera_regular,0));
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda("Ciudad.-", fuente_cabecera_regular,0));
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda($"{empresa} tiene el agrado de extender " +
                        "la siguiente propuesta de los fármacos en mención", 
                        fuente_cabecera_regular,0));
                    doc.Add(tabla_cabecera_2);

                    var tabla_detalle = new PdfPTable(9)
                    {
                        LockedWidth = true,
                        TotalWidth = 550f,
                        SpacingBefore = 15f
                    };
                    tabla_detalle.SetWidths(new float[] { 
                        30f,40.11f,81.11f,
                        81.11f,101.11f,61.11f,
                        61.11f,41.11f,61.11f});


                    tabla_detalle.AddCell(
                       R.CrearCelda("ITEM", fuente_tabla_cabecera,0,Rectangle.BOX));
                    tabla_detalle.AddCell(
                       R.CrearCelda("CPC", fuente_tabla_cabecera, 0, Rectangle.BOX));
                    tabla_detalle.AddCell(
                       R.CrearCelda("Principio activo", fuente_tabla_cabecera, 0, Rectangle.BOX));
                    tabla_detalle.AddCell(
                       R.CrearCelda("Forma farmacéutica", fuente_tabla_cabecera, 0, Rectangle.BOX));
                    tabla_detalle.AddCell(
                       R.CrearCelda("Concentración", fuente_tabla_cabecera, 0, Rectangle.BOX));
                    tabla_detalle.AddCell(
                       R.CrearCelda("Presentación", fuente_tabla_cabecera, 0, Rectangle.BOX));
                    tabla_detalle.AddCell(
                       R.CrearCelda("Cantidad", fuente_tabla_cabecera, 0, Rectangle.BOX));
                    tabla_detalle.AddCell(
                       R.CrearCelda("P. Unitario", fuente_tabla_cabecera, 0, Rectangle.BOX));
                    tabla_detalle.AddCell(
                       R.CrearCelda("Total", fuente_tabla_cabecera, 0, Rectangle.BOX));
                    

                    int contador = 1;

                    foreach (var linea in detalle)
                    {
                        tabla_detalle.AddCell(
                        R.CrearCelda(contador.ToString(), fuente_tabla_detalle, 0, Rectangle.BOX));
                        tabla_detalle.AddCell(
                        R.CrearCelda(linea.producto_instituciones.cpc.nombre,
                        fuente_tabla_detalle, 0, Rectangle.BOX));
                        tabla_detalle.AddCell(
                        R.CrearCelda(linea.producto_instituciones.nombre_generico, fuente_tabla_detalle, 0, Rectangle.BOX));
                        tabla_detalle.AddCell(
                        R.CrearCelda(linea.producto_instituciones.forma_farmaceutica.nombre, fuente_tabla_detalle, 0, Rectangle.BOX));
                        tabla_detalle.AddCell(
                        R.CrearCelda(linea.producto_instituciones.concentracion.nombre, fuente_tabla_detalle, 0, Rectangle.BOX));
                        tabla_detalle.AddCell(
                        R.CrearCelda(linea.producto_instituciones.presentacion.nombre, fuente_tabla_detalle, 0, Rectangle.BOX));
                        tabla_detalle.AddCell(
                        R.CrearCelda(linea.cantidad_producto.ToString(), fuente_tabla_detalle, 0, Rectangle.BOX));
                        tabla_detalle.AddCell(
                        R.CrearCelda((linea.valor_linea/linea.cantidad_producto).ToString(), fuente_tabla_detalle, 0, Rectangle.BOX));
                        tabla_detalle.AddCell(
                        R.CrearCelda(linea.valor_linea.ToString(), fuente_tabla_detalle, 0, Rectangle.BOX));
                        contador++;
                    }
                    doc.Add(tabla_detalle);

                    tabla_cabecera_1.DeleteBodyRows();

                    tabla_cabecera_1.AddCell(
                        R.CrearCelda("SUBTOTAL", fuente_cabecera,0));
                    tabla_cabecera_1.AddCell(
                        R.CrearCelda($"$ {cabecera.valor_total}", fuente_cabecera,2));
                    tabla_cabecera_1.AddCell(
                        R.CrearCelda("DESCUENTO", fuente_cabecera,0));
                    tabla_cabecera_1.AddCell(
                        R.CrearCelda(" ", fuente_cabecera,2));
                    tabla_cabecera_1.AddCell(
                        R.CrearCelda("IVA 0%", fuente_cabecera,0));
                    tabla_cabecera_1.AddCell(
                        R.CrearCelda($"$ {cabecera.valor_total}", fuente_cabecera,2));
                    tabla_cabecera_1.AddCell(
                        R.CrearCelda("IVA 12%", fuente_cabecera,0));
                    tabla_cabecera_1.AddCell(
                        R.CrearCelda(" ", fuente_cabecera,2));
                    tabla_cabecera_1.AddCell(
                        R.CrearCelda("TOTAL", fuente_cabecera,0));
                    tabla_cabecera_1.AddCell(
                        R.CrearCelda($"$ {cabecera.valor_total}", fuente_cabecera,2));
                    doc.Add(tabla_cabecera_1);


                    tabla_cabecera_2.DeleteBodyRows();
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda($"La forma de pago es  {cabecera.forma_pago}", fuente_cabecera,0));
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda($"La oferta tiene un " +
                        $" periodo de validez de 90 días", fuente_cabecera,0));
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda($"Tiempo de garantía:  {cabecera.meses_tiempo_garantia} meses", fuente_cabecera,0));
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda($"Fecha mínima de caducidad:  12 meses", fuente_cabecera,0));
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda($"Tiempo de entrega: {cabecera.dias_tiempo_entrega} días laborables", fuente_cabecera,0));
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda($"Procedencia: Ecuador", fuente_cabecera,0));
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda($"Marca: LABOVIDA S.A.", fuente_cabecera,0));
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda($"Email: danivet_servicios@hotmail.com", fuente_cabecera,0));
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda($"Persona contacto: {cabecera.persona_contacto}", fuente_cabecera,0));
                    tabla_cabecera_2.AddCell(
                        R.CrearCelda($"Número de teléfono: 0979256749", fuente_cabecera,0));
                    doc.Add(tabla_cabecera_2);

                    doc.Close();
                    var pdf_generado = R.GenerarPDF();

                    return Convert.ToBase64String(pdf_generado);
                }

            }
        }


        [HttpPost]
        public ActionResult GenerarCotizacionesPerdedoras(CotizacionesFaltantes faltantes)
        {
            try
            {
                using (var db = new as2oasis())
                {
                    var detalle_productos =
                        db.cotizacion_detalle.Where(x => x.id_cotizacion_cabecera ==
                        faltantes.id_cotizacion_principal)
                        .Select(x => x.id_producto_instituciones);

                    foreach (var empresa in faltantes.empresas)
                    {
                        var cabecera_original =
                            db.cotizacion_cabecera
                            .Where(x => x.id_cotizacion == faltantes.id_cotizacion_principal)
                            .First();
                        var cabecera_nueva =
                            new cotizacion_cabecera();
                        cabecera_nueva.anulada = 0;
                        cabecera_nueva.dias_periodo_validez = cabecera_original.dias_periodo_validez;
                        cabecera_nueva.dias_tiempo_entrega = cabecera_original.dias_tiempo_entrega;
                        cabecera_nueva.id_empresaOasis = empresa;
                        cabecera_nueva.id_vendedor = cabecera_original.id_vendedor;
                        cabecera_nueva.indicador_perdedora = 1;
                        cabecera_nueva.indicador_ganadora = 0;
                        cabecera_nueva.meses_tiempo_garantia = cabecera_original.meses_tiempo_garantia;
                        cabecera_nueva.persona_contacto = cabecera_original.persona_contacto;
                        cabecera_nueva.forma_pago = cabecera_original.forma_pago;
                        cabecera_nueva.fecha_documento = DateTime.Now;
                        cabecera_nueva.id_cliente = cabecera_original.id_cliente;
                        var detalles = db.cotizacion_detalle
                            .Where(x => x.id_cotizacion_cabecera == faltantes.id_cotizacion_principal)
                            .ToList();
                        var lista_detalle_nuevo = new List<cotizacion_detalle>();

                        foreach (var producto in detalles)
                        {
                            var rnd = new Random();
                            var _cotizacion_detalle = new cotizacion_detalle();
                            _cotizacion_detalle.id_producto_instituciones = producto.id_producto_instituciones;
                            _cotizacion_detalle.codigo_alterno = producto.codigo_alterno;
                            _cotizacion_detalle.cantidad_producto = producto.cantidad_producto;
                            var _lista_precio = db
                                .detalle_lista_precio
                                .Where(x => x.id_producto_inst == producto.id_producto_instituciones)
                                .FirstOrDefault();
                            var porcentaje = _lista_precio.porcentaje;

                            var precio_mayor_porcentaje =
                                (producto.valor_linea / producto.cantidad_producto) +
                                (producto.valor_linea / producto.cantidad_producto) * _lista_precio.porcentaje / 100;

                            var precio_mayor =
                                precio_mayor_porcentaje > _lista_precio.precio_mayor ?
                                _lista_precio.precio_mayor : precio_mayor_porcentaje;

                            _cotizacion_detalle.valor_linea = producto.cantidad_producto * precio_mayor_porcentaje;
                            _cotizacion_detalle.porcentaje_diferencia = 0;
                            lista_detalle_nuevo.Add(_cotizacion_detalle);
                        }

                        cabecera_nueva.valor_total = lista_detalle_nuevo.Sum(x => x.valor_linea);
                        cabecera_nueva.cotizacion_detalle = lista_detalle_nuevo;
                        db.cotizacion_cabecera.Add(cabecera_nueva);
                        db.SaveChanges();

                        int id_nueva_cotizacion = cabecera_nueva.id_cotizacion;
                        var nueva_relacion = new relacion_cotizacion_ganadora_perdedora();
                        nueva_relacion.id_cotizacion_ganadora = cabecera_original.id_cotizacion;
                        nueva_relacion.id_cotizacion_perdedora = id_nueva_cotizacion;
                        nueva_relacion.estado = 1;
                        nueva_relacion.memo = $"COTIZACION PERDEDOR DE {cabecera_original.id_cotizacion + "-" + cabecera_original.empresaOasis.nombre}";

                        db.relacion_cotizacion_ganadora_perdedora.Add(nueva_relacion);
                        db.SaveChanges();

                    }

                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //throw;
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        [HttpPost]
        public ActionResult CrearCotizacion(FormCollection cotizacion)
        {
            using (var db = new as2oasis())
            {
                var _cotizacion_cabecera = new cotizacion_cabecera();
                var id_empresa = Convert.ToInt32(cotizacion["id_empresa"]);
                var _ruc_cliente = cotizacion["id_cliente"];
                _cotizacion_cabecera.id_empresaOasis = id_empresa;
                _cotizacion_cabecera.fecha_documento = Convert.ToDateTime(cotizacion["fecha_documento"]);
                var as2 = new AS2Context();
                var id_cliente = as2.empresa.Where(x => x.codigo == _ruc_cliente &&
                        x.id_organizacion == id_empresa)
                        .Select(x => x.id_empresa).FirstOrDefault();

                _cotizacion_cabecera.id_cliente = id_cliente;
                _cotizacion_cabecera.forma_pago = cotizacion["forma_pago"];
                _cotizacion_cabecera.meses_tiempo_garantia = Convert.ToInt32(cotizacion["meses_tiempo_garantia"]);
                _cotizacion_cabecera.dias_tiempo_entrega = Convert.ToInt32(cotizacion["dias_tiempo_entrega"]);
                _cotizacion_cabecera.persona_contacto = cotizacion["persona_contacto"];
                _cotizacion_cabecera.indicador_ganadora = 1;


                var productos = cotizacion["id_producto"].Split(',');
                var cod_alternos = cotizacion["codigo_alterno"].Split(',');
                var cantidades = cotizacion["cantidad"].Split(',');
                var valores_unitarios = cotizacion["valor_unitario"].Split(',');
                var lista_detalle_cotizacion = new List<cotizacion_detalle>();
                for (int i = 0; i < productos.Length; i++)
                {
                    var _cotizacion_detalle = new cotizacion_detalle();
                    _cotizacion_detalle.id_producto_instituciones = Convert.ToInt32(productos[i]);
                    _cotizacion_detalle.codigo_alterno = cod_alternos[i];
                    _cotizacion_detalle.cantidad_producto = Convert.ToDecimal(cantidades[i]);
                    _cotizacion_detalle.valor_linea = Convert.ToDecimal(cantidades[i]) * Convert.ToDecimal(valores_unitarios[i]);
                    _cotizacion_detalle.porcentaje_diferencia = 0;
                    lista_detalle_cotizacion.Add(_cotizacion_detalle);
                }

                _cotizacion_cabecera.valor_total = lista_detalle_cotizacion.Sum(x => x.valor_linea);
                _cotizacion_cabecera.cotizacion_detalle = lista_detalle_cotizacion;
                db.cotizacion_cabecera.Add(_cotizacion_cabecera);
                db.SaveChanges();
            }

            Console.WriteLine(cotizacion.ToString());
            return RedirectToAction("Index");
        }

        #region Clientes
        public ActionResult ObtenerClientes(string textoBusqueda)
        {
            return Json(new AS2Context().empresa
                .Where(x =>
                x.indicador_cliente == true
                && x.categoria_empresa.codigo == "INST")
                .GroupBy(x => new
                {
                    x.codigo,
                    x.nombre_comercial
                })
                .Where(
                x => x.Key.nombre_comercial.Contains(textoBusqueda) ||
                x.Key.codigo.Contains(textoBusqueda))
                .Take(5)
                .Select(x => new
                {
                    id = x.Key.codigo,
                    text = x.Key.nombre_comercial
                })
                .ToList(), JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}