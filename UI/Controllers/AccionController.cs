using BE;
using BLL;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Models;
using UI.Reportes;
using UI.Excel;

namespace UI.Controllers
{
    public class AccionController : GenericController
    {
        private const string ROLES = "Datos";

        private BLL_Accion accionBll = new BLL_Accion();

        private UsuarioBLL usuarioBll = new UsuarioBLL();

        // GET: accion
        public ActionResult Index(int? page)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            IEnumerable<Accion> acciones = accionBll.Listar();
            
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(acciones.ToPagedList(pageNumber, pageSize));
        }

        // GET: accion/Details/5
        public ActionResult Details(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            return View(accionBll.Listar(id));
        }

        // GET: accion/Create
        public ActionResult Create()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            return View();
        }

        // POST: accion/Create
        [HttpPost]
        public ActionResult Create(Accion accion)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                accionBll.Grabar(accion);

                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.mensaje = "Error";
                return View();
            }
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            return View(accionBll.Listar(id));
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(Accion accion)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                if (accion != null)
                {

                    accionBll.Grabar(accion);

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.mensaje = "accion inexistente";
                    return View(accion);
                }
            }
            catch
            {
                ViewBag.mensaje = "Error";
                return View();
            }
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            return View(accionBll.Listar(id));
        }

        // POST: Vendedor/Delete/5
        [HttpPost]
        public ActionResult Delete(Accion accion)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                accionBll.Borrar(accion);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}