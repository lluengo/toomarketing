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
    public class CondicionController : GenericController
    {
        private const string ROLES = "Datos";

        private BLL_Condicion condicionBll = new BLL_Condicion();

        private BLL_Usuario usuarioBll = new BLL_Usuario();

        // GET: Condicion
        public ActionResult Index(int? page)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            // return View(clienteBll.Listar());
            IEnumerable<Condicion> condiciones = condicionBll.Listar();
            
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(condiciones.ToPagedList(pageNumber, pageSize));
        }

        // GET: Condicion/Details/5
        public ActionResult Details(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            return View(condicionBll.Listar(id));
        }

        // GET: Condicion/Create
        public ActionResult Create()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            return View();
        }

        // POST: Condicion/Create
        [HttpPost]
        public ActionResult Create(Condicion condicion)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                condicionBll.Grabar(condicion);

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

            return View(condicionBll.Listar(id));
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(Condicion condicion)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                if (condicion != null)
                {

                    condicionBll.Grabar(condicion);

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.mensaje = "condicion inexistente";
                    return View(condicion);
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
            return View(condicionBll.Listar(id));
        }

        // POST: Vendedor/Delete/5
        [HttpPost]
        public ActionResult Delete(Condicion condicion)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                condicionBll.Borrar(condicion);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}