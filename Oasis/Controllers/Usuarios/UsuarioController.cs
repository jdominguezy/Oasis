using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Oasis.Models;
using Oasis.Models.Login;

namespace Oasis.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            usuarioOasis usuario_login = new usuarioOasis();
            return View(usuario_login);
        }

        // GET: Usuario/Details/5
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Usuario/Create
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Crear()
        {
            usuarioOasis usuario_login = new usuarioOasis();
            return View(usuario_login);
        }

        [CustomAuthorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Crear(usuarioOasis user)
        {
            try
            {

                var pwd_parser = new LoginRequest();
                using (as2oasis oasis = new as2oasis())
                {
                    pwd_parser.Password = user.password;
                    user.password = pwd_parser.PasswordEncriptada();
                    user.activo = 1;
                    oasis.usuario.Add(user);
                    oasis.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("Roles");
                }
            }
            catch (Exception)
            {
                return View();
            }


        }

        [CustomAuthorize(Roles = "Admin")]
        [HttpGet]
        public JsonResult ObtenerRoles()
        {
            using(var db = new as2oasis())
            {
                return Json(db.roles.Select(x => new
                {
                    id = x.id_rol,
                    text = x.nombre
                }).ToList(), JsonRequestBehavior.AllowGet);
            }
        }


        [CustomAuthorize(Roles = "Admin")]
        [HttpGet]
        public JsonResult ObtenerEmpresas()
        {
            using (var db = new as2oasis())
            {
                return Json(db.empresaOasis.Select(x => new
                {
                    id = x.id_empresa,
                    text = x.nombre
                }).ToList(), JsonRequestBehavior.AllowGet);
            }
        }


        [CustomAuthorize(Roles = "Admin")]
        public JsonResult EmpresasPorUsuario(string id_usuario)
        {
            var id = Convert.ToInt32(id_usuario);
            using (var db = new as2oasis())
            {

                return Json(db.detalle_usuario_empresa_sucursal
                    .Where(x => x.id_usuario == id)
                    .Select(x => new
                    {
                        id = x.empresaOasis.id_empresa,
                        text = x.empresaOasis.nombre
                    }).ToList(), JsonRequestBehavior.AllowGet);
            }
        }


        [CustomAuthorize(Roles = "Admin")]
        [HttpGet]
        public JsonResult ObtenerSucursales()
        {
            using (var db = new as2oasis())
            {
                return Json(db.sucursalOasis.Select(x => new
                {
                    id = x.id_sucursal,
                    text = x.nombre
                }).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorize(Roles = "Admin")]
        public JsonResult SucursalesPorUsuario(string id_usuario,string id_empresa)
        {

            var idUsuario = Convert.ToInt32(id_usuario);
            var idEmpresa = Convert.ToInt32(id_empresa);
            using (var db = new as2oasis())
            {
                return Json(db.detalle_usuario_empresa_sucursal
                    .Where(x => x.id_usuario == idUsuario && x.id_empresa==idEmpresa )
                    .Select(x => new
                    {
                        id = x.sucursalOasis.id_sucursal,
                        text = x.sucursalOasis.nombre
                    }).ToList(), JsonRequestBehavior.AllowGet);
            }
        }



        public class UsuarioSucursalesJSON
        {
            public int id_empresa { get; set; }
            public int id_usuario { get; set; }
            public List<SucursalJson> sucursales { get; set; }
        }

        public class SucursalJson
        {
            public int id { get; set; }
        }

        public class RolesUsuarioJSON
        {
            public int id_usuario { get; set; }
            public List<RolesJson> roles { get; set; }
        }

        public class RolesJson
        {
            public int id { get; set; }
        }

        [HttpPost]
        public ActionResult ModificarPassword(string id_usuario, string password)
        {
            try
            {
                var _id_usuario = Convert.ToInt32(id_usuario);
                using (var db = new as2oasis())
                {
                    var pwd = new LoginRequest();
                    pwd.Password = password;
                    var usuario = db.usuario.Where(x=>x.id_usuario==_id_usuario).FirstOrDefault();
                    usuario.password = pwd.PasswordEncriptada();
                    db.SaveChanges();
                    return new HttpStatusCodeResult(200);
                }
            } catch
            {
                return new HttpStatusCodeResult(503);
            }
            
        }


        [CustomAuthorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult GuardarSucursales(UsuarioSucursalesJSON json)
        {
            try
            {
                if(
                    json.id_usuario !=0 && 
                    json.id_empresa !=0 )
                {

                using (var db = new as2oasis())
                {
                    var sucursalesABorrar = db.detalle_usuario_empresa_sucursal
                        .Where(x =>
                        x.id_empresa == json.id_empresa &&
                        x.id_usuario == json.id_usuario);
                    db.detalle_usuario_empresa_sucursal.RemoveRange(sucursalesABorrar);
                    db.SaveChanges();
                    if (json.sucursales!=null)
                    {
                        foreach (var sucursalJ in json.sucursales)
                        {   
                            var id_sucursal = Convert.ToInt32(sucursalJ.id);
                            var sucursal = new detalle_usuario_empresa_sucursal()
                            {
                                id_usuario = json.id_usuario,
                                id_empresa = json.id_empresa,
                                id_sucursal = id_sucursal
                            };
                            db.detalle_usuario_empresa_sucursal.Add(sucursal);
                            db.SaveChanges();
                        }
                    }
                    return new HttpStatusCodeResult(200);
                    }
                }
                return new HttpStatusCodeResult(500);
            }
            catch 
            {
                return new HttpStatusCodeResult(503);
            }
        }


        [CustomAuthorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult GuardarRoles(RolesUsuarioJSON json)
        {
            try
            {
                if (
                    json.id_usuario != 0)
                {

                    using (var db = new as2oasis())
                    {
                        var rolesABorrar = db.detalle_roles_usuario
                            .Where(x =>
                            x.id_usuario == json.id_usuario);
                        db.detalle_roles_usuario.RemoveRange(rolesABorrar);
                        db.SaveChanges();
                        if (json.roles != null)
                        {
                            foreach (var rolJ in json.roles)
                            { 
                                var rol = new detalle_roles_usuario()
                                {
                                    id_usuario = json.id_usuario,
                                    id_rol = rolJ.id
                                };
                                db.detalle_roles_usuario.Add(rol);
                                db.SaveChanges();
                            }
                        }
                        return new HttpStatusCodeResult(200);
                    }
                }
                return new HttpStatusCodeResult(500);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(503,e.Message);
            }
        }

        [CustomAuthorize(Roles = "Admin")]
        public JsonResult RolesPorUsuario(string id_usuario)
        {
            var id = Convert.ToInt32(id_usuario);
            using(var db = new as2oasis())
            {

                return Json(db.detalle_roles_usuario
                    .Include("roles")
                    .Where(x => x.id_usuario == id)
                    .Select(x => new
                    {
                        id = x.roles.id_rol,
                        text = x.roles.nombre
                    }).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorize(Roles = "Admin")]
        public JsonResult ObtenerUsuarios(string textoBusqueda)
        {
            using(var db = new as2oasis())
            {
                return  Json(db.usuario.Where(x => x.username.Contains(textoBusqueda) || x.nombre.Contains(textoBusqueda)
                || x.cedula.Contains(textoBusqueda)).Take(5).Select(x => new
                {
                   x.id_usuario,
                   x.username
                }).ToList(),JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Roles()
        {
            return View();
        }

        // POST: Usuario/Create
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

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Usuario/Edit/5
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

        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuario/Delete/5
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

        //[CustomAuthorize(Roles = "Admin")]
        public JsonResult ObtenerUsuariosAS2(string textoBusqueda)
        {
            using (var db = new as2oasis())
            {

                return Json(db.usuario.Where(x => x.nombre.Contains(textoBusqueda) || x.apellido.Contains(textoBusqueda)
               || x.cedula.Contains(textoBusqueda)).Take(5).Select(x => new
               {
                   nombre = x.nombre + " " +x.apellido,
                   x.id_usuario,
                   
               }).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

    }
}
