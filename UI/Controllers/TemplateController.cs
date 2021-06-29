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
    public class Templatecontroller : GenericController
    {
        private const string ROLES = "Datos";

        private BLL_Template templateBll = new BLL_Template();

        private UsuarioBLL usuarioBll = new UsuarioBLL();

        // GET: template
        public ActionResult Index(int? page)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            IEnumerable<Template> templatees = templateBll.Listar();
            
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(templatees.ToPagedList(pageNumber, pageSize));
        }

        // GET: template/Details/5
        public ActionResult Details(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            return View(templateBll.Listar(id));
        }

        // GET: template/Create
        public ActionResult Create()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            int[] temp = new int[5] { 1, 2, 3, 4, 5 };

            ViewBag.temp = new SelectList(temp, "id");


            return View();
        }

        // POST: template/Create
        [HttpPost]
        public ActionResult Create(Template template)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                templateBll.Grabar(template);

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

            return View(templateBll.Listar(id));
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(Template template)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                if (template != null)
                {

                    templateBll.Grabar(template);

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.mensaje = "template inexistente";
                    return View(template);
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
            return View(templateBll.Listar(id));
        }

        // POST: Vendedor/Delete/5
        [HttpPost]
        public ActionResult Delete(Template template)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                templateBll.Borrar(template);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public String VistaPrevia(Template template, int nroTemplate)
        {
            try
            {
                template.nroTemplate = nroTemplate;

                String p = templateBll.generateHTML(template);

                return p;
            }
            catch
            {
                ViewBag.mensaje = "Error";
                return "";
            }


        } 
     
    }
}