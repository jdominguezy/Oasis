using Newtonsoft.Json;
using Oasis.Models;
using Oasis.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Oasis.Controllers.Instituciones
{
    public class InventarioController : Controller
    {

        [CustomAuthorize(Roles = "Instituciones")]
        // GET: Inventario
        public ActionResult Index()
        {
            return RedirectToAction("Producto");
        }

        #region Producto
        public ActionResult Producto()
        {
            as2oasis oasis = new as2oasis();
            return View(oasis.producto_instituciones);
        }

        [HttpGet]
        public ActionResult CrearProducto()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CrearProducto(
            string nombre_generico,
            int id_cpc,
            int id_forma_farmaceutica,
            int id_concentracion,
            int id_presentacion)
        {
            try
            {
                using (as2oasis oasis = new as2oasis())
                {
                    var nueva_prod = new producto_instituciones();
                    nueva_prod.nombre_generico = nombre_generico;
                    nueva_prod.cpc = oasis.cpc.Find(id_cpc);
                    nueva_prod.forma_farmaceutica = oasis.forma_farmaceutica.Find(id_forma_farmaceutica);
                    nueva_prod.concentracion = oasis.concentracion.Find(id_concentracion);
                    nueva_prod.presentacion = oasis.presentacion.Find(id_presentacion);
                    oasis.producto_instituciones.Add(nueva_prod);
                    oasis.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("Producto");
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditarProducto(int id)
        {
            as2oasis oasis = new as2oasis();
            var ff = oasis.producto_instituciones
                .Where(x => x.id_producto_inst == id)
                .FirstOrDefault();
            return View(ff);
        }

        [HttpPost]
        public ActionResult EditarProducto(
            int id, 
            string nombre_generico, 
            int id_cpc, 
            int id_forma_farmaceutica,
            int id_concentracion,
            int id_presentacion
            )
        {
            try
            {
                using (var db = new as2oasis())
                {
                    var prod = db.producto_instituciones.Find(id);
                    prod.nombre_generico = nombre_generico;
                    prod.id_cpc = id_cpc;
                    prod.id_forma_farmaceutica = id_forma_farmaceutica;
                    prod.id_concentracion = id_concentracion;
                    prod.id_presentacion = id_presentacion;
                    db.SaveChanges();
                    return RedirectToAction("Producto");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult ObtenerProductosInstituciones(string textoBusqueda = "")
        {
            as2oasis oasis = new as2oasis();
            var productos = oasis.producto_instituciones;
            IQueryable<producto_instituciones> datos_lista = productos;

            if (textoBusqueda != "")
            {
                var data = productos
                   .Where(x => x.nombre_generico.Contains(textoBusqueda))
                   .Take(5)
                   .Select(x => new
                   {
                       id = x.id_producto_inst,
                       text = x.nombre_generico,
                       precio_mayor = x.detalle_lista_precio.FirstOrDefault().precio_mayor,
                       precio_menor = x.detalle_lista_precio.FirstOrDefault().precio_menor,
                       cpc = x.cpc.nombre
                   }).ToList();
                JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
                var result = JsonConvert.SerializeObject(data, Formatting.Indented, jss);
                return Content(result, "application/json"); 
            } else
            {
                var datos = productos.Select(x => new {
                    id = x.id_producto_inst,
                    text = x.nombre_generico,
                    precio_mayor = x.detalle_lista_precio.FirstOrDefault().precio_mayor,
                    precio_menor = x.detalle_lista_precio.FirstOrDefault().precio_menor,
                    cpc = x.cpc
                }).ToList();
                return Json(datos, JsonRequestBehavior.AllowGet);

            }
        }
        
        #endregion

        #region Forma farmaceutica
        public ActionResult FormaFarmaceutica()
        {
            as2oasis oasis = new as2oasis();
            return View(oasis.forma_farmaceutica);
        }
        
        [HttpGet]
        public ActionResult CrearFormaFarmaceutica()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CrearFormaFarmaceutica(forma_farmaceutica ff)
        {
            try
            {
                using (as2oasis oasis = new as2oasis())
                {
                    var nueva_ff = new forma_farmaceutica();
                    oasis.forma_farmaceutica.Add(ff);
                    oasis.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("FormaFarmaceutica");
                }
            }
            catch (Exception)
            {
                return View();
            }           
        }


        [HttpGet]
        public ActionResult EditarFormaFarmaceutica(int id)
        {
            as2oasis oasis = new as2oasis();
            var ff = oasis.forma_farmaceutica
                .Where(x => x.id_forma_farmaceutica == id)
                .FirstOrDefault();
            return View(ff);
        }

        [HttpPost]
        public ActionResult EditarFormaFarmaceutica(int id,string nombre)
        {
            try
            {
                using (var db = new as2oasis())
                {
                    forma_farmaceutica forma_f = db.forma_farmaceutica.Find(id);
                    forma_f.nombre = nombre;
                    db.SaveChanges();
                    return RedirectToAction("FormaFarmaceutica");
                }
            }
            catch (Exception)
            {
                throw;
            }                       
        }

        [HttpGet]
        public ActionResult ObtenerFormaFarmaceutica()
        {
            as2oasis oasis = new as2oasis();
            var datos_lista = oasis.forma_farmaceutica
                .Select(x=>new { 
                    id = x.id_forma_farmaceutica,
                    text = x.nombre
                }).ToList();
            return Json(datos_lista,JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Presentacion 
        public ActionResult Presentacion()
        {
            as2oasis oasis = new as2oasis();
            return View(oasis.presentacion);
        }

        [HttpGet]
        public ActionResult CrearPresentacion()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult CrearPresentacion(presentacion pst)
        {
            try
            {
                using (as2oasis oasis = new as2oasis())
                {
                    oasis.presentacion.Add(pst);
                    oasis.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("Presentacion");
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditarPresentacion(int id)
        {
            as2oasis oasis = new as2oasis();
            var dato_a_editar = oasis.presentacion
                .Where(x => x.id_presentacion == id)
                .FirstOrDefault();
            return View(dato_a_editar);
        }

        [HttpPost]
        public ActionResult EditarPresentacion(int id, string nombre)
        {
            try
            {
                using (var db = new as2oasis())
                {
                    var dato= db.presentacion.Find(id);
                    dato.nombre = nombre;
                    db.SaveChanges();
                    return RedirectToAction("Presentacion");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ObtenerPresentacion()
        {
            as2oasis oasis = new as2oasis();
            var datos_lista = oasis.presentacion
                .Select(x => new {
                    id = x.id_presentacion,
                    text = x.nombre
                }).ToList();
            return Json(datos_lista, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CPC
        public ActionResult CPC()
        {
            as2oasis oasis = new as2oasis();
            return View(oasis.cpc);
        }

        [HttpGet]
        public ActionResult CrearCPC()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CrearCPC(cpc cpc)
        {
            try
            {
                using (as2oasis oasis = new as2oasis())
                {
                    oasis.cpc.Add(cpc);
                    oasis.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("CPC");
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditarCPC(int id)
        {
            as2oasis oasis = new as2oasis();
            var dato_a_editar = oasis.cpc
                .Where(x => x.id_cpc == id)
                .FirstOrDefault();
            return View(dato_a_editar);
        }

        [HttpPost]
        public ActionResult EditarCPC(int id, string nombre)
        {
            try
            {
                using (var db = new as2oasis())
                {
                    var dato = db.cpc.Find(id);
                    dato.nombre = nombre;
                    db.SaveChanges();
                    return RedirectToAction("CPC");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ObtenerCPC()
        {
            as2oasis oasis = new as2oasis();
            var datos_lista = oasis.cpc
                .Select(x => new {
                    id = x.id_cpc,
                    text = x.nombre
                }).ToList();
            return Json(datos_lista, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Concentracion
        public ActionResult Concentracion()
        {
            as2oasis oasis = new as2oasis();
            return View(oasis.concentracion);
        }

        [HttpGet]
        public ActionResult CrearConcentracion()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CrearConcentracion(concentracion concentracion)
        {
            try
            {
                using (as2oasis oasis = new as2oasis())
                {
                    oasis.concentracion.Add(concentracion);
                    oasis.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("Concentracion");
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditarConcentracion(int id)
        {
            as2oasis oasis = new as2oasis();
            var dato_a_editar = oasis.concentracion
                .Where(x => x.id_concentracion == id)
                .FirstOrDefault();
            return View(dato_a_editar);
        }

        [HttpPost]
        public ActionResult EditarConcentracion(int id, string nombre)
        {
            try
            {
                using (var db = new as2oasis())
                {
                    var dato = db.concentracion.Find(id);
                    dato.nombre = nombre;
                    db.SaveChanges();
                    return RedirectToAction("Concentracion");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ObtenerConcentracion()
        {
            as2oasis oasis = new as2oasis();
            var datos_lista = oasis.concentracion
                .Select(x => new {
                    id = x.id_concentracion,
                    text = x.nombre
                }).ToList();
            return Json(datos_lista, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Lista de precios

        public ActionResult ListaPrecio()
        {
            as2oasis listaPrecio = new as2oasis();
            return View(listaPrecio.detalle_lista_precio);
        }

        [HttpGet]
        public ActionResult EditarListaPrecio(int id)
        {
            as2oasis oasis = new as2oasis();
            var dato_a_editar = oasis.detalle_lista_precio
                .Where(x => x.id_detalle_lista_precio == id)
                .FirstOrDefault();
            return View(dato_a_editar);
        }

        [HttpPost]
        public ActionResult EditarListaPrecio(int id, 
            decimal precio_mayor,
            decimal precio_menor,
            decimal porcentaje)
        {
            try
            {
                using (var db = new as2oasis())
                {
                    var dato = db.detalle_lista_precio.Find(id);
                    dato.precio_mayor = precio_mayor;
                    dato.precio_menor = precio_menor;
                    dato.porcentaje= porcentaje;
                    db.SaveChanges();
                    return RedirectToAction("ListaPrecio");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Detalle lista de precios 
        [HttpGet]
        public ActionResult CrearDetalleLista()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CrearDetalleLista(concentracion concentracion)
        {
            try
            {
                using (as2oasis oasis = new as2oasis())
                {
                    oasis.concentracion.Add(concentracion);
                    oasis.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("Concentracion");
                }
            }
            catch (Exception)
            {
                return View();
            }
        }
        #endregion

    }
}