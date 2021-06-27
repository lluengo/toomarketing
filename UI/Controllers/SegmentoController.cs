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
    public class SegmentoController : GenericController
    {
        private const string ROLES = "Datos";

        private BLL_Segmento segmentoBll = new BLL_Segmento();

        private BLL_Regla reglaBll = new BLL_Regla();

        private List<String> reglas = new List<string>();


        // GET: Segmento
        public ActionResult Index(int? page)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            // return View(clienteBll.Listar());
            IEnumerable<Segmento> segmento = segmentoBll.Listar();

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(segmento.ToPagedList(pageNumber, pageSize));
        }

        // GET: Segmento/Details/5
        public ActionResult Details(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            return View(segmentoBll.Listar(id));
        }

        // GET: Segmento/Create
        public ActionResult Create()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            ViewBag.reglas = reglaBll.Listar();

            return View();
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            ViewBag.reglas = reglaBll.Listar();

            return View(segmentoBll.Listar(id));
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(string[] ids, Segmento segmento)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                if (segmento.reglas == null)
                    segmento.reglas = new List<Regla>();

                if (ids != null && ids.Length > 0) { 
                    foreach (var id in ids)
                    {
                        var r = new Regla();
                        r.id = int.Parse(id);
                        segmento.reglas.Add(r);
                    }
                }

                segmentoBll.Grabar(segmento);

                return RedirectToAction("Index");
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
            return View(segmentoBll.Listar(id));
        }

        // POST: Vendedor/Delete/5
        [HttpPost]
        public ActionResult Delete(Segmento segmento)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                segmentoBll.Borrar(segmento);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: segmento/create
        [HttpPost]
        public ActionResult create(string[] ids, Segmento segmento)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                if (segmento.reglas == null)
                    segmento.reglas = new List<Regla>();

                if (ids == null || ids.Length == 0)
                {
                    foreach (var id in ids)
                    {
                        var r = new Regla();
                        r.id = int.Parse(id);
                        segmento.reglas.Add(r);
                    }
                }

                segmentoBll.Grabar(segmento);

                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.mensaje = "Error";
                return View();
            }

        }
    }
}