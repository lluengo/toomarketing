using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    
    public class PermisoController : GenericController
    {
        private const string ROLES = "Administrador";

        BLL_Permiso permisoBLL = new BLL_Permiso();
        // GET: Permiso
        public ActionResult Index()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            try
            {
                var lista = permisoBLL.Listar("");
                return View(lista);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View();
            }
        }

        public ActionResult Create()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {

                var model = permisoBLL.ListarFamilias();
                Permiso ultimo = new Permiso();
                ultimo.ID = 0;
                ultimo.Nombre = "Sin rol asociado";
                model.Add(ultimo);
                ViewData["permisos"] = model;
                ViewBag.lista_permisos = new SelectList(model, "ID", "Nombre");
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }

        // POST: Permiso/Create
        [HttpPost]
        public ActionResult Create(Permiso permiso, FormCollection collection)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            try
            {
               
                string rol = Convert.ToString(collection["rol"]);

                if (permiso.Tipo == "patente" && rol == "0")
                {
                    ViewBag.error = "Los permisos de tipo PATENTE deben tener una FAMILIA asociada";
                    var models = permisoBLL.ListarFamilias();
                    Permiso ultimo = new Permiso();
                    ultimo.ID = 0;
                    ultimo.Nombre = "Sin rol asociado";
                    models.Add(ultimo);
                    ViewData["permisos"] = models;
                    ViewBag.lista_permisos = new SelectList(models, "ID", "Nombre");
                    return View();
                }

                var model = permisoBLL.Grabar(permiso, rol);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ActionResult Patentes()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            var lista = permisoBLL.ListarPatentes();
            return View(lista);
        }

        public ActionResult Details(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            try
            {
                var lista = permisoBLL.ListarPermisosDeFamilia(id);
                return View(lista);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            try { 
                var lista = permisoBLL.BuscarPermiso(id);
                return View(lista);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }

        // POST: Permiso/Edit/5
        [HttpPost]
        public ActionResult Edit(Permiso permiso)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");


            int result = permisoBLL.Editar(permiso);

            if (result == 1) { return RedirectToAction("Index"); }
            else { return View(); }

        }

        // GET: Permiso/Delete/5
        public ActionResult Delete(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            try
            {
                var lista = permisoBLL.BuscarPermiso(id);
                return View(lista);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return View();
            }
        }

        // POST: Permiso/Delete/5
        [HttpPost]
        public ActionResult Delete(Permiso permiso)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            try
            {
            
                bool result;
                result = permisoBLL.BajaRol(permiso);
                if (result)
                {
                    permisoBLL.BajaPermiso(permiso);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Usuario(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            var lista = permisoBLL.VerificarPermisosUsuario(id);
            ViewData["usuario"] = id;
            return View(lista);
        }

        public ActionResult Asignar(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");


            List<Permiso> permisos_todos = new List<Permiso>();
            permisos_todos = permisoBLL.ListarTodos();

            
            List<Permiso> permisos_usuario = new List<Permiso>();
            permisos_usuario = permisoBLL.VerificarPermisosUsuario(id);

            foreach (Permiso permi in permisos_usuario)
                permisos_todos.RemoveAll(r => r.ID == permi.ID);

            BLL_Usuario usuarioBLL = new BLL_Usuario();
            ViewData["usuario"] = usuarioBLL.Listar(id);
            ViewBag.lista_permisos = new SelectList(permisos_todos, "ID", "Nombre");
            return View(permisos_todos);
        }

        public ActionResult DesasignarPermiso(int id, int id_usuario)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");


            permisoBLL.DesasignarPermisoUsuario(id, id_usuario);
            string redirect = "Usuario/" + id_usuario.ToString();
            return RedirectToAction(redirect);
        }

        [HttpPost]
        public ActionResult AsignarPermiso(List<Permiso> permisos, int usuario)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");


            foreach (Permiso permiso in permisos)
            {
                permisoBLL.AsignarPermisoUsuario(permiso.ID, usuario);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetPermiso(string permiso)
        {           
            Permiso model = permisoBLL.BuscarPermiso(int.Parse(permiso));
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}