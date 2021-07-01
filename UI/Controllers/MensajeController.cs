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
    public class MensajeController : GenericController
    {
        private const string ROLES = "Datos";

        private BLL_Mensaje mensajeBll = new BLL_Mensaje();

        private BLL_Usuario usuarioBll = new BLL_Usuario();

        // GET: mensaje
        public ActionResult Index(int? page)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            IEnumerable<Mensaje> mensajees = mensajeBll.Listar();
            
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(mensajees.ToPagedList(pageNumber, pageSize));
        }

        // GET: mensaje/Details/5
        public ActionResult Details(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            return View(mensajeBll.Listar(id));
        }

        // GET: mensaje/Create
        public ActionResult Create()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            return View();
        }

        // POST: mensaje/Create
        [HttpPost]
        public ActionResult Create(Mensaje mensaje)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                mensajeBll.Grabar(mensaje);

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

            return View(mensajeBll.Listar(id));
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(Mensaje mensaje)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                if (mensaje != null)
                {

                    mensajeBll.Grabar(mensaje);

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.mensaje = "mensaje inexistente";
                    return View(mensaje);
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
            return View(mensajeBll.Listar(id));
        }

        // POST: Vendedor/Delete/5
        [HttpPost]
        public ActionResult Delete(Mensaje mensaje)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                mensajeBll.Borrar(mensaje);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}