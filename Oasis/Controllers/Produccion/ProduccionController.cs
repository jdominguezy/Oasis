using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Oasis.Models;
using Oasis.Models.Login;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static Oasis.Reporte;

namespace Oasis.Controllers
{


    public class ProduccionController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        //Reporte mp lotes JD I
        public ActionResult Mplotes()
        {
            return View();
        }

        public ActionResult ConsolidadoProducto()
        {
            return View();
        }

        public ActionResult ImprimirOrdenProduccion(int id_orden_fabricacion)
        {

            using (var db = new as2oasis())
            {
                var cabecera = db.Orden_Produccion_Cabecera
                    .Where(x => x.id_orden_fabricacion == id_orden_fabricacion)
                    .FirstOrDefault();

                var costos_mp_me = db.Costos_MP_ME
                    .Where(x => x.id_orden_fabricacion == id_orden_fabricacion);

                var costos_mod = db.Costos_MOD
                    .Where(x => x.id_orden_fabricacion == id_orden_fabricacion);

                var costos_indirectos = db.Costos_Indirectos
                    .Where(x => x.id_orden_fabricacion == id_orden_fabricacion);

                using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Reporte R = new Reporte();
                R.MemoryStream = myMemoryStream;
                R.Empresa = "LABOVIDA S.A.";
                R.tipo_reporte = Reporte.TipoReporte.Reporte;
                float[] margenes = new float[] { 0f, 0f, 20f, 10f };
                var doc = R.CrearDocA4(margenes);
                var pdf = R.CrearPDF();
                var hoy = DateTime.Now;
                var _costos_mod = 0.0;
                var suma_horas_mod = 0.0;

                var fuente_cabecera = R.CrearFuente("georgia", 10, 1, BaseColor.BLACK);
                var fuente_cabecera_regular = R.CrearFuente("georgia", 10, 0, BaseColor.BLACK);
                var fuente_tabla_detalle = R.CrearFuente("georgia", 8, 0, BaseColor.BLACK);
                var fuente_tabla_cabecera = R.CrearFuente("georgia", 8, 1, BaseColor.BLACK);

                var _standardFont = FontFactory.GetFont("SEGOE UI", 15, Font.BOLD, BaseColor.BLACK);
                var subtitulo = FontFactory.GetFont("SEGOE UI", 7, Font.BOLD, BaseColor.BLACK);
                var encabezado_tabla = FontFactory.GetFont("SEGOE UI", 8, Font.BOLD, BaseColor.BLACK);
                var detalle = FontFactory.GetFont("SEGOE UI", 7, Font.NORMAL, BaseColor.BLACK);

                var MP = costos_mp_me.Where(x => x.codigo_categoria_producto.Replace("", "") == "GRP01" ||
                        x.codigo_categoria_producto.Replace("", "") == "GRP05" || x.codigo_categoria_producto.Replace("", "") == "GRP04"
                        || x.codigo_categoria_producto.Replace("", "") == "GRP07");
              
                var ME = costos_mp_me.Where(x => x.codigo_categoria_producto.Replace("", "") == "GRP02" ||
                        x.codigo_categoria_producto.Replace("", "") == "GRP03");

                var suma_mp = MP.Sum(x => x.costo_total == null ? 0 : x.costo_total) ?? 0;
                var suma_me = ME.Sum(x => x.costo_total == null ? 0 : x.costo_total) ?? 0;

                if (costos_mod.Count() > 0)
                {
                    _costos_mod = Convert.ToDouble(costos_mod.Select(x => x.costo_total).First() ?? 0);
                }
                    var suma_mod = _costos_mod;
                var suma_otros_costos = costos_indirectos.Sum(x => x.costo_total == null ? 0 : x.costo_total) ?? 0;


                var suma_costos_total =
                    suma_mp + suma_me + Convert.ToDecimal(suma_mod) + suma_otros_costos;
                var pnd_mp = string.Format("{0:0.00%}", ((suma_mp) / suma_costos_total));
                var pnd_me = string.Format("{0:0.00%}", ((suma_me) / suma_costos_total));
                var pnd_mod = string.Format("{0:0.00%}", ((Convert.ToDecimal(suma_mod)) / suma_costos_total));

                var pnd_otros_costos = string.Format("{0:0.00%}", ((suma_otros_costos) / suma_costos_total));
                
                if (costos_mod.Count() > 0)
                {
                    suma_horas_mod = Convert.ToDouble(costos_mod.Sum(x => x.horas_hombre));
                }

                doc.AddTitle($"Hoja de produccion #{cabecera.OP}");
                doc.Open();

                var encabezado = new PdfPTable(6)
                {
                    LockedWidth = true,
                    TotalWidth = 500f,
                    SpacingBefore = 5f,
                    SpacingAfter = 20f
                };

                encabezado.SetWidths(new float[] { 66f, 100f, 66f, 150f, 66f, 52f });
                //encabezado.SetWidths(new float[] { 66f, 70f, 66f, 100f, 66f, 52f, 66f, 50f });

                var table1 = new PdfPTable(3)
                {
                    LockedWidth = true,
                    TotalWidth = 500f,
                    SpacingBefore = 5f
                };

                var _Titulo = new Chunk($"ANÁLISIS DE PRODUCCION \n LOTE: {cabecera.lote} \n ",
                    _standardFont);
                Paragraph __Titulo = new Paragraph(_Titulo);
                __Titulo.Alignment = Element.ALIGN_CENTER;

                table1.SetWidths(new float[] { 100f, 200f, 100f });

                PdfPCell cell1 = new PdfPCell();
                cell1 = new PdfPCell();

                cell1.Padding = 0;
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(_Titulo));
                cell1.Padding = 0;
                cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cell1.Border = PdfPCell.NO_BORDER;

                table1.AddCell(cell1);

                //cell1 = new PdfPCell(new Phrase("USUARIO: " + User.Identity.GetUserName() +" \n "));
                var _Usuario = new Chunk($"Usuario Imprime: " + User.Identity.GetUserName() + " \n "+ "Sucursal: "+cabecera.planta,detalle);
                cell1 = new PdfPCell(new Phrase(_Usuario));
                cell1.Padding = 0;
                cell1.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                cell1.Border = PdfPCell.NO_BORDER;

                table1.AddCell(cell1);

                doc.Add(table1);

                doc.Add(Chunk.NEWLINE);

                cell1 = new PdfPCell(new Phrase("Cod. PT.:", subtitulo));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(cabecera.codigo, detalle));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase("PT.:", subtitulo));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(cabecera.nombre, detalle));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase("O.P.:", subtitulo));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(cabecera.OP, detalle));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase("Und. Elaboradas.:", subtitulo));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(String.Format("{0:n0}", cabecera.cantidad_fabricada), detalle));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase("Und. a elaborar:", subtitulo));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(String.Format("{0:n0}", cabecera.cantidad), detalle));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase("Rendimiento:", subtitulo));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(string.Format("{0:n2}", cabecera.rendimiento * 100) + "%", detalle));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase("U.M.:", subtitulo));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(cabecera.unidad_medida_op, detalle));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase("Fecha inicio O.P.:", subtitulo));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(cabecera.fecha_creacion_of, detalle));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase("Fecha cierre O.P.:", subtitulo));
                cell1.Border = PdfPCell.NO_BORDER;
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                encabezado.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(cabecera.fecha_cierre.ToString(), detalle));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                cell1.Border = PdfPCell.NO_BORDER;
                encabezado.AddCell(cell1);

                //cell1 = new PdfPCell(new Phrase("Sucursal:", subtitulo));
                //cell1.Border = PdfPCell.NO_BORDER;
                //cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //encabezado.AddCell(cell1);

                //cell1 = new PdfPCell(new Phrase(cabecera.planta, detalle));
                //cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //cell1.Border = PdfPCell.NO_BORDER;
                //encabezado.AddCell(cell1);


                doc.Add(encabezado);

                #region detalle
                table1.FlushContent();
                table1.SetWidths(new float[] { 150f, 150f, 100f });

                var detalle_mp = new PdfPTable(6);
                detalle_mp.LockedWidth = true;
                detalle_mp.TotalWidth = 500f;
                detalle_mp.SpacingBefore = 5f;
                detalle_mp.SetWidths(new float[] { 60f, 210f, 20f,
                    50f,50f,50f});

                PdfPCell cell_detalle = new PdfPCell();
                cell_detalle.Padding = 0;
                cell_detalle.Border = PdfPCell.NO_BORDER;


                cell1 = new PdfPCell(new Phrase("MATERIA PRIMA"));
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

                if (suma_mp > 0)
                    doc.Add(table1);

                detalle_mp.AddCell(new Phrase("Cod.", subtitulo));
                detalle_mp.AddCell(new Phrase("Materia prima", subtitulo));
                detalle_mp.AddCell(new Phrase("UM", subtitulo));
                detalle_mp.AddCell(new Phrase("Receta", subtitulo));
                detalle_mp.AddCell(new Phrase("Consumo", subtitulo));
                detalle_mp.AddCell(new Phrase("Desperdicio", subtitulo));

                foreach (var item in MP)
                {
                    detalle_mp.AddCell(new Phrase(
                        item.codigo_producto.ToString(), detalle));
                    detalle_mp.AddCell(new Phrase(item.producto, detalle));
                    detalle_mp.AddCell(new Phrase(item.unidad_medida, detalle));
                    detalle_mp.AddCell(new Phrase(string.Format("{0:n4}", item.receta), detalle));
                    detalle_mp.AddCell(new Phrase(string.Format("{0:n4}", item.consumo), detalle));
                    detalle_mp.AddCell(new Phrase(string.Format("{0:0.00%}", item.desperdicio), detalle));
                   
                }

                if (suma_mp > 0)
                    doc.Add(detalle_mp);
                detalle_mp.FlushContent();
                table1.FlushContent();

                cell1 = new PdfPCell(new Phrase(""));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_mp.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase(""));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_mp.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase("Pnd.:", subtitulo));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_mp.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase(pnd_mp, detalle));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_mp.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase("Total M.P.:", subtitulo));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_mp.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase(string.Format("{0:n4}", suma_mp), detalle));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_mp.AddCell(cell1);

                if (suma_mp > 0)
                    doc.Add(detalle_mp);
                detalle_mp.FlushContent();


                cell1 = new PdfPCell(new Phrase("MATERIAL EMPAQUE"));
                cell1.Padding = 0;
                cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cell1.PaddingBottom = 4f;
                cell1.CellEvent = new RoundedBorder();
                cell1.Border = PdfPCell.NO_BORDER;

                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(""));
                cell1.Padding = 0;
                cell1.Border = PdfPCell.NO_BORDER;

                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(""));
                cell1.Padding = 0;
                cell1.Border = PdfPCell.NO_BORDER;

                table1.AddCell(cell1);

                if (suma_me > 0)
                    doc.Add(table1);

                detalle_mp.AddCell(new Phrase("Cod.", subtitulo));
                detalle_mp.AddCell(new Phrase("Materia empaque", subtitulo));
                detalle_mp.AddCell(new Phrase("UM", subtitulo));
                detalle_mp.AddCell(new Phrase("Receta", subtitulo));
                detalle_mp.AddCell(new Phrase("Consumo", subtitulo));
                detalle_mp.AddCell(new Phrase("Desperdicio", subtitulo));

                foreach (var item in ME)
                {
                    detalle_mp.AddCell(new Phrase(item.codigo_producto, detalle));
                    detalle_mp.AddCell(new Phrase(item.producto, detalle));
                    detalle_mp.AddCell(new Phrase(item.unidad_medida, detalle));
                    detalle_mp.AddCell(new Phrase(string.Format("{0:n4}", item.receta), detalle));
                    detalle_mp.AddCell(new Phrase(string.Format("{0:n4}", item.consumo), detalle));
                    detalle_mp.AddCell(new Phrase(string.Format("{0:0.00%}", item.desperdicio), detalle));
                   
                }

                if (suma_me > 0)
                    doc.Add(detalle_mp);
                detalle_mp.FlushContent();
                table1.FlushContent();

                cell1 = new PdfPCell(new Phrase(""));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_mp.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase(""));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_mp.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase("Pnd.:", subtitulo));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_mp.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase(pnd_me, detalle));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_mp.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase("Total M.E.:", subtitulo));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_mp.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase(string.Format("{0:n4}", suma_me), detalle));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_mp.AddCell(cell1);

                if (suma_me > 0)
                    doc.Add(detalle_mp);
                detalle_mp.FlushContent();

                var detalle_datos = new PdfPTable(5);
                detalle_datos.LockedWidth = true;
                detalle_datos.TotalWidth = 500f;
                detalle_datos.SpacingBefore = 5f;
                detalle_datos.SetWidths(new float[] { 60f, 210f, 20f,
                    50f,50f});

                cell1 = new PdfPCell(new Phrase("MANO DE OBRA DIRECTA"));
                cell1.Padding = 0;
                cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cell1.PaddingBottom = 4f;
                cell1.CellEvent = new RoundedBorder();
                cell1.Border = PdfPCell.NO_BORDER;

                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(""));
                cell1.Padding = 0;
                cell1.Border = PdfPCell.NO_BORDER;

                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(""));
                cell1.Padding = 0;
                cell1.Border = PdfPCell.NO_BORDER;

                table1.AddCell(cell1);

                if (costos_mod.Count() > 0)
                    doc.Add(table1);

                detalle_datos.AddCell(new Phrase("Cod. ", subtitulo));
                detalle_datos.AddCell(new Phrase("Descripción", subtitulo));
                detalle_datos.AddCell(new Phrase("UM", subtitulo));
                detalle_datos.AddCell(new Phrase("Tiempo Est.", subtitulo));
                detalle_datos.AddCell(new Phrase("Consumo", subtitulo));

                foreach (var item in costos_mod)
                {
                    var costo_total_mod = (item.costo_total / Convert.ToDecimal(suma_horas_mod)) * item.horas_hombre;
                    var costo_unitario_mod = (item.costo_total / Convert.ToDecimal(suma_horas_mod));
                    detalle_datos.AddCell(new Phrase(item.operacion, detalle));
                    detalle_datos.AddCell(new Phrase(item.nombre_maquina, detalle));
                    detalle_datos.AddCell(new Phrase("HH", detalle));
                    detalle_datos.AddCell(new Phrase(string.Format("{0:n4}", item.horas_standard), detalle));
                    detalle_datos.AddCell(new Phrase(string.Format("{0:n4}", item.horas_hombre), detalle));
                   
                }

                if (costos_mod.Count() > 0)
                    doc.Add(detalle_datos);

                detalle_datos.FlushContent();
                table1.FlushContent();


                cell1 = new PdfPCell(new Phrase(""));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_datos.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase("Pnd.:", subtitulo));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_datos.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase(pnd_mod, detalle));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_datos.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase("Total MOD:", subtitulo));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_datos.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase(string.Format("{0:n4}", _costos_mod), detalle));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_datos.AddCell(cell1);


                if (costos_mod.Count() > 0)
                    doc.Add(detalle_datos);
                    detalle_datos.FlushContent();

                var detalle_otros = new PdfPTable(4);
                detalle_otros.LockedWidth = true;
                detalle_otros.TotalWidth = 500f;
                detalle_otros.SpacingBefore = 5f;
                detalle_otros.SetWidths(new float[] { 60f, 210f, 20f,
                    50f});

                cell1 = new PdfPCell(new Phrase("OTROS COSTOS"));
                cell1.Padding = 0;
                cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cell1.PaddingBottom = 4f;
                cell1.CellEvent = new RoundedBorder();
                cell1.Border = PdfPCell.NO_BORDER;

                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(""));
                cell1.Padding = 0;
                cell1.Border = PdfPCell.NO_BORDER;

                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(""));
                cell1.Padding = 0;
                cell1.Border = PdfPCell.NO_BORDER;

                table1.AddCell(cell1);

                doc.Add(table1);

                detalle_otros.AddCell(new Phrase("Cod. ", subtitulo));
                detalle_otros.AddCell(new Phrase("Descripción", subtitulo));
                detalle_otros.AddCell(new Phrase("UM", subtitulo));
                detalle_otros.AddCell(new Phrase("Consumo", subtitulo));

                foreach (var item in costos_indirectos)
                {
                    detalle_otros.AddCell(new Phrase("CIF", detalle));
                    detalle_otros.AddCell(new Phrase("COSTOS INDIRECTOS", detalle));
                    detalle_otros.AddCell(new Phrase("UP", detalle));
                    detalle_otros.AddCell(new Phrase(string.Format("{0:n2}", item.cantidad), detalle));
                   
                }

                doc.Add(detalle_otros);
                detalle_otros.FlushContent();
                table1.FlushContent();


                cell1 = new PdfPCell(new Phrase("Pnd.:", subtitulo));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_otros.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase(pnd_otros_costos, detalle));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_otros.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase("Total Otros:", subtitulo));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_otros.AddCell(cell1);
                cell1 = new PdfPCell(new Phrase(string.Format("{0:n4}", suma_otros_costos), detalle));
                cell1.Border = PdfPCell.NO_BORDER;
                detalle_otros.AddCell(cell1);

                doc.Add(detalle_otros);
                detalle_otros.FlushContent();
                #endregion

                #region NOTA     

                var tabla_nota = new PdfPTable(1);
                tabla_nota.LockedWidth = true;
                tabla_nota.TotalWidth = 500f;
                tabla_nota.SpacingBefore = 25f;
                tabla_nota.SetWidths(new float[] { 250f });

                cell1 = new PdfPCell(new Phrase("NOTA:"));
                cell1.Padding = 0;
                cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cell1.PaddingBottom = 4f;
                cell1.CellEvent = new RoundedBorder();
                cell1.Border = PdfPCell.NO_BORDER;

                table1.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase(""));
                cell1.Padding = 0;
                cell1.Border = PdfPCell.NO_BORDER;

                table1.AddCell(cell1);

                doc.Add(table1);

                tabla_nota.AddCell(new Phrase("Nota ", subtitulo));
                tabla_nota.AddCell(new Phrase(cabecera.descripcion, detalle));
                doc.Add(tabla_nota);
                tabla_nota.FlushContent();
                table1.FlushContent();
                #endregion

                #region FIRMAS 
                var tabla_firma = new PdfPTable(2);
                tabla_firma.LockedWidth = true;
                tabla_firma.TotalWidth = 500f;
                tabla_firma.SpacingBefore = 25f;
                tabla_firma.SetWidths(new float[] { 250f, 250f });

                cell1 = new PdfPCell(new Phrase("________________"));
                cell1.Padding = 0;
                cell1.Border = PdfPCell.NO_BORDER;
                cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla_firma.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase("________________"));
                cell1.Padding = 0;
                cell1.Border = PdfPCell.NO_BORDER;
                cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla_firma.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase("REVISADO \n Departamento de Producción"));
                cell1.Padding = 5f;
                cell1.Border = PdfPCell.NO_BORDER;
                cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla_firma.AddCell(cell1);

                cell1 = new PdfPCell(new Phrase("APROBADO"));
                cell1.Padding = 5f;
                cell1.Border = PdfPCell.NO_BORDER;
                cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla_firma.AddCell(cell1);

                doc.Add(tabla_firma);
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

        
        //[HttpPost]
        public ActionResult ObtenerOP(
            string fecha_desde,
            string fecha_hasta)
        {
            DateTime fecha_desde_ = DateTime.Parse(fecha_desde);
            DateTime fecha_hasta_ = DateTime.Parse(fecha_hasta);
 
            using (var context = new as2oasis())
            {
                var op =
                    context.Orden_Produccion_Cabecera
                    .Where(x => x.fecha_cierre >= fecha_desde_ &&
                                x.fecha_cierre <= fecha_hasta_ 
                                && x.fecha_cierre != null
                                //x.cod__producto_material== "PRODUCTOS TERMINADOS"
                            )
                    .ToList()
                    .Select(x => new
                    {
                        x.OP,
                        x.lote,
                        fecha =  Convert.ToString(x.fecha_cierre),
                        x.codigo,
                        x.nombre,
                        sucursal = x.planta,
                        x.cantidad,
                        x.cantidad_fabricada, 
                        rendimiento = Math.Round((double)((x.rendimiento) * 100), 2) + "%",
                        x.id_orden_fabricacion
                    });

                return Json(op, JsonRequestBehavior.AllowGet);
            }

        }

        //JASON OBTENCION CONSULTAMP LOTES JD I
        //[HttpPost]
        public JsonResult ObtenerLote(int id_producto)
        {
            as2oasis oasis = new as2oasis();
            var lotes = oasis.MateriaPrima_Lote
                .AsNoTracking()                
                .Where(x=>x.id_producto==id_producto)
            .ToList()
                    .Select(x => new
                    {
                        x.codigo,
                        x.nombre,
                        x.unidad,
                        x.nombre_proveedor,
                        //fecha = x.fecha.Value.ToShortDateString(),
                        //x.fecha,
                        fecha = x.fecha.HasValue?
                        x.fecha.Value.ToShortDateString() : "",
                        x.numero_analisis,
                        x.bodega,
                        x.bodega_liberar,
                        x.cantidad,
                        x.lote,
                        fecha_fabricacion = x.fecha_fabricacion.Value.ToShortDateString(),
                        fecha_caducidad = x.fecha_caducidad.ToShortDateString(),
                        fecha_reanalisis = x.fecha_reanalisis.HasValue? 
                        x.fecha_reanalisis.Value.ToShortDateString():"",
                        x.id_producto,
                        //rendimiento = Math.Round((double)((x.rendimiento) * 100), 2) + "%",
                    });


            return Json(lotes, JsonRequestBehavior.AllowGet);


        }
        //JASON OBTENCION CONSULTAMP LOTES JD F


        //JASON OBTENCION PRODUCTOS JD I
        public JsonResult ObtenerProductos(string textoBusqueda)
        {
            as2oasis oasis = new as2oasis();
            var productos = oasis.ProductosMP
                .AsEnumerable()
                .Where(
                x =>
                (x.codigo.ToLower().Contains(textoBusqueda.ToLower()) == true
                || x.nombre.ToLower().Contains(textoBusqueda.ToLower()) == true))
                .GroupBy(x => new { x.id_producto, x.nombre })
                .Select(x => new
                {
                    id_producto = x.Key.id_producto,
                    nombre = x.Key.nombre,
                })
                .Take(5)
                ;
            return Json(productos, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerConsolidadoProducto(
           string fecha_desde,
           string fecha_hasta)
        {
            DateTime fecha_desde_ = DateTime.Parse(fecha_desde);
            DateTime fecha_hasta_ = DateTime.Parse(fecha_hasta);

            using (var context = new as2oasis())
            {
                var op =
                    context.Orden_Produccion_Cabecera
                    .Where(x => x.fecha_cierre >= fecha_desde_ &&
                                x.fecha_cierre <= fecha_hasta_
                                && x.fecha_cierre != null
                            //x.cod__producto_material== "PRODUCTOS TERMINADOS"
                            )
                    .ToList()
                    .Select(x => new
                    {
                        x.OP,
                        x.lote,
                        fecha = Convert.ToString(x.fecha_cierre),
                        x.codigo,
                        x.nombre,
                        sucursal = x.planta,
                        x.cantidad,
                        x.cantidad_fabricada,
                        rendimiento = Math.Round((double)((x.rendimiento) * 100), 2) + "%",
                        x.id_orden_fabricacion
                    });

                return Json(op, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ImprimirConsolidado(int id_orden_fabricacion)
        {

            using (var db = new as2oasis())
            {
                var cabecera = db.Orden_Produccion_Cabecera
                    .Where(x => x.id_orden_fabricacion == id_orden_fabricacion)
                    .FirstOrDefault();

                var costos_mp_me = db.Costos_MP_ME
                    .Where(x => x.id_orden_fabricacion == id_orden_fabricacion);

                var costos_mod = db.Costos_MOD
                    .Where(x => x.id_orden_fabricacion == id_orden_fabricacion);

                var costos_indirectos = db.Costos_Indirectos
                    .Where(x => x.id_orden_fabricacion == id_orden_fabricacion);

                using (MemoryStream myMemoryStream = new MemoryStream())
                {
                    Reporte R = new Reporte();
                    R.MemoryStream = myMemoryStream;
                    R.Empresa = "LABOVIDA S.A.";
                    R.tipo_reporte = Reporte.TipoReporte.Reporte;
                    float[] margenes = new float[] { 0f, 0f, 20f, 10f };
                    var doc = R.CrearDocA4(margenes);
                    var pdf = R.CrearPDF();
                    var hoy = DateTime.Now;

                    var fuente_cabecera = R.CrearFuente("georgia", 10, 1, BaseColor.BLACK);
                    var fuente_cabecera_regular = R.CrearFuente("georgia", 10, 0, BaseColor.BLACK);
                    var fuente_tabla_detalle = R.CrearFuente("georgia", 8, 0, BaseColor.BLACK);
                    var fuente_tabla_cabecera = R.CrearFuente("georgia", 8, 1, BaseColor.BLACK);

                    var _standardFont = FontFactory.GetFont("SEGOE UI", 13, Font.BOLD, BaseColor.BLACK);
                    var subtitulo = FontFactory.GetFont("SEGOE UI", 7, Font.BOLD, BaseColor.BLACK);
                    var encabezado_tabla = FontFactory.GetFont("SEGOE UI", 8, Font.BOLD, BaseColor.BLACK);
                    var detalle = FontFactory.GetFont("SEGOE UI", 7, Font.NORMAL, BaseColor.BLACK);

                    var MP = costos_mp_me.Where(x => x.codigo_categoria_producto.Replace("", "") == "GRP01" ||
                            x.codigo_categoria_producto.Replace("", "") == "GRP05" || x.codigo_categoria_producto.Replace("", "") == "GRP04"
                            || x.codigo_categoria_producto.Replace("", "") == "GRP07");


                    doc.AddTitle($"Consolidado Produccion #{cabecera.OP}");
                    doc.Open();

                    var table1 = new PdfPTable(3)
                    {
                        LockedWidth = true,
                        TotalWidth = 500f,
                        SpacingBefore = 5f
                    };

                    var _Titulo = new Chunk($"DEPARTAMENTO DE PRODUCCIÓN \n CONCILIACIÓN DEL PRODUCTO \n {cabecera.tipo_pr_producto} ",
                        _standardFont);
                    Paragraph __Titulo = new Paragraph(_Titulo);
                    __Titulo.Alignment = Element.ALIGN_CENTER;

                    //table1.SetWidths(new float[] { 200f, 200f, 200f });

                    PdfPCell cell1 = new PdfPCell();
                    cell1 = new PdfPCell();

                    cell1.Padding = 0;
                    cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    cell1.Border = PdfPCell.NO_BORDER;
                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(_Titulo));
                    cell1.Padding = 0;
                    cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell1.Border = PdfPCell.NO_BORDER;

                    table1.AddCell(cell1);

                    //var _Usuario = new Chunk($"Usuario Imprime: " + User.Identity.GetUserName() + " \n " + "Sucursal: " + cabecera.planta, detalle);
                    //cell1 = new PdfPCell(new Phrase(_Usuario));
                    //cell1.Padding = 0;
                    //cell1.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    //cell1.Border = PdfPCell.NO_BORDER;

                    //table1.AddCell(cell1);

                    //doc.Add(table1);

                    doc.Add(Chunk.NEWLINE);

                    #region detalle
                    table1.FlushContent();
                    table1.SetWidths(new float[] { 50f, 150f, 100f });

                    var detalle_ = new PdfPTable(1);
                    detalle_.LockedWidth = true;
                    detalle_.TotalWidth = 500f;
                    detalle_.SpacingBefore = 5f;
                    detalle_.SetWidths(new float[] { 500f });

                    PdfPCell cell_detalle = new PdfPCell();
                    cell_detalle.Padding = 0;
                    cell_detalle.Border = PdfPCell.NO_BORDER;

                    cell1 = new PdfPCell(new Phrase(""));
                    cell1.Padding = 0;
                    cell1.Border = PdfPCell.NO_BORDER;

                    table1.AddCell(cell1);

                    doc.Add(table1);

                    var _cabecera = new Chunk($"PRODUCTO: " + cabecera.nombre + " \n \n " + "LOTE: " + cabecera.lote + " \n \n " + 
                        "FECHA: "+ cabecera.fecha + " \n \n " + "ORDEN DE FABRICACION: " + cabecera.OP  + "\n", detalle);

                    detalle_.AddCell(new Phrase(_cabecera.ToString(), detalle));

                    doc.Add(detalle_);
                    detalle_.FlushContent();
                    table1.FlushContent();

                    cell1 = new PdfPCell(new Phrase(""));
                    cell1.Padding = 0;
                    cell1.Border = PdfPCell.NO_BORDER;

                    table1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(""));
                    cell1.Padding = 0;
                    cell1.Border = PdfPCell.NO_BORDER;

                    table1.AddCell(cell1);


                    var detalle_datos = new PdfPTable(10);
                    detalle_datos.LockedWidth = true;
                    detalle_datos.TotalWidth = 500f;
                    detalle_datos.SpacingBefore = 5f;
                    detalle_datos.SetWidths(new float[] { 50f, 40f, 40f,
                    40f,40f, 40f, 40f, 40f, 40f, 40f});
                   
                    cell1 = new PdfPCell(new Phrase(""));
                    cell1.Padding = 0;
                    cell1.Border = PdfPCell.NO_BORDER;

                    table1.AddCell(cell1);

                    if (MP.Count() > 0)
                        doc.Add(table1);

                    detalle_datos.AddCell(new Phrase("Descripción ", subtitulo));
                    detalle_datos.AddCell(new Phrase("Recibido", subtitulo));
                    detalle_datos.AddCell(new Phrase("Requisición", subtitulo));
                    detalle_datos.AddCell(new Phrase("Total Recibido", subtitulo));
                    detalle_datos.AddCell(new Phrase("Producto Terminado", subtitulo));
                    detalle_datos.AddCell(new Phrase("Muestras a C.C.", subtitulo));
                    detalle_datos.AddCell(new Phrase("Rechazo", subtitulo));
                    detalle_datos.AddCell(new Phrase("Devolución", subtitulo));
                    detalle_datos.AddCell(new Phrase("Total Utilizado", subtitulo));
                    detalle_datos.AddCell(new Phrase("Diferencia", subtitulo));

                    foreach (var item in costos_mod)
                    {
                        
                        detalle_datos.AddCell(new Phrase(item.operacion, detalle));
                        detalle_datos.AddCell(new Phrase(item.nombre_maquina, detalle));
                        detalle_datos.AddCell(new Phrase("HH", detalle));
                        detalle_datos.AddCell(new Phrase(string.Format("{0:n4}", item.horas_standard), detalle));
                        detalle_datos.AddCell(new Phrase(string.Format("{0:n4}", item.horas_hombre), detalle));
                        detalle_datos.AddCell(new Phrase(string.Format("{0:n4}", item.horas_standard), detalle));
                        detalle_datos.AddCell(new Phrase(string.Format("{0:n4}", item.horas_hombre), detalle));
                        detalle_datos.AddCell(new Phrase(string.Format("{0:n4}", item.horas_standard), detalle));
                        detalle_datos.AddCell(new Phrase(string.Format("{0:n4}", item.horas_hombre), detalle));
                        detalle_datos.AddCell(new Phrase(string.Format("{0:n4}", item.horas_standard), detalle));

                    }

                    if (costos_mod.Count() > 0)
                        doc.Add(detalle_datos);

                    detalle_datos.FlushContent();
                    table1.FlushContent();

                    #endregion


                    #region FIRMAS 
                    var tabla_firma = new PdfPTable(1);
                    tabla_firma.LockedWidth = true;
                    tabla_firma.TotalWidth = 500f;
                    tabla_firma.SpacingBefore = 25f;
                    tabla_firma.SetWidths(new float[] { 250f});

                    cell1 = new PdfPCell(new Phrase("Fecha:"));
                    cell1.Padding = 0;
                    cell1.Border = PdfPCell.NO_BORDER;
                    cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    tabla_firma.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase("Revisado Por:"));
                    cell1.Padding = 0;
                    cell1.Border = PdfPCell.NO_BORDER;
                    cell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    tabla_firma.AddCell(cell1);

                    doc.Add(tabla_firma);
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


    }
}
