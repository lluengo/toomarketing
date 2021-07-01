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
    public class ReglaController : GenericController
    {
        private const string ROLES = "Datos";

        private BLL_Regla reglaBll = new BLL_Regla();

        private BLL_Condicion condicionBll = new BLL_Condicion();

        private BLL_Accion accionBll = new BLL_Accion();

        private BLL_Usuario usuarioBll = new BLL_Usuario();

        // GET: Regla
        public ActionResult Index(int? page)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            // return View(clienteBll.Listar());
            IEnumerable<Regla> reglaes = reglaBll.Listar();

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(reglaes.ToPagedList(pageNumber, pageSize));
        }

        // GET: Regla/Details/5
        public ActionResult Details(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            return View(reglaBll.Listar(id));
        }

        // GET: Regla/Create
        public ActionResult Create()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            
            ViewBag.acciones = new SelectList(accionBll.Listar(),"id","nombre");  
            ViewBag.condiciones = new SelectList(condicionBll.Listar(), "id", "nombre");

            return View();
        }

        // POST: Regla/Create
        [HttpPost]
        public ActionResult Create(Regla regla)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                reglaBll.Grabar(regla);

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

            ViewBag.acciones = new SelectList(accionBll.Listar(), "id", "nombre");
            ViewBag.condiciones = new SelectList(condicionBll.Listar(), "id", "nombre");

            return View(reglaBll.Listar(id));
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(Regla regla)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                if (regla != null)
                {

                    reglaBll.Grabar(regla);

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.mensaje = "regla inexistente";
                    return View(regla);
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
            return View(reglaBll.Listar(id));
        }

        // POST: Vendedor/Delete/5
        [HttpPost]
        public ActionResult Delete(Regla regla)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                reglaBll.Borrar(regla);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}