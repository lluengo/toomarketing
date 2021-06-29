using BE;
using BLL;
using PagedList;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace UI.Controllers
{
    public class PublicidadController : GenericController
    {
        private const string ROLES = "Datos";

        private BLL_Publicidad publicidadBll = new BLL_Publicidad();

        private BLL_Segmento segmentoBll = new BLL_Segmento();
        private BLL_Template templateBll = new BLL_Template();
        private BLL_Mensaje  mensajeBll  = new BLL_Mensaje();



        // GET: Publicidad
        public ActionResult Index(int? page)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            // return View(clienteBll.Listar());
            IEnumerable<Publicidad> publicidad = publicidadBll.Listar();

            int pageSize = 10;
            int pageNumber = (page ?? 1);   

            return View(publicidad.ToPagedList(pageNumber, pageSize));
        }

        // GET: Publicidad/Details/5
        public ActionResult Details(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            return View(publicidadBll.Listar(id));
        }

        // GET: Publicidad/Create
        public ActionResult Create()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");


            ViewBag.mensajes = new SelectList(mensajeBll.Listar(), "id", "nombre");
            ViewBag.templates = new SelectList(templateBll.Listar(), "id", "nombre");
            ViewBag.segmentos = segmentoBll.Listar();

            return View();
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            ViewBag.mensajes  = new SelectList(mensajeBll.Listar(), "id", "nombre"); 
            ViewBag.templates = new SelectList(templateBll.Listar(), "id", "nombre");
            ViewBag.segmentos = segmentoBll.Listar();

            return View(publicidadBll.Listar(id));
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(string[] ids, Publicidad publicidad, int? template, int? mensaje)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                if (publicidad.segmentos == null)
                    publicidad.segmentos = new List<Segmento>();

                if (template != null)
                {
                    Template t = new Template();
                    t.id = template.Value;
                    publicidad.template = t;
                }
                if (mensaje != null)
                {
                    Mensaje m = new Mensaje();
                    m.id = mensaje.Value;
                    publicidad.mensaje = m;
                }

                if (ids != null && ids.Length > 0) { 
                    foreach (var id in ids)
                    {
                        var s = new Segmento();
                        s.id = int.Parse(id);
                        publicidad.segmentos.Add(s);
                    }
                }

                publicidadBll.Grabar(publicidad);

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
            return View(publicidadBll.Listar(id));
        }

        // POST: Vendedor/Delete/5
        [HttpPost]
        public ActionResult Delete(Publicidad publicidad)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                publicidadBll.Borrar(publicidad);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: publicidad/create
        [HttpPost]
        public ActionResult create(string[] ids, Publicidad publicidad, int? template, int? mensaje)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                if (publicidad.segmentos == null)
                    publicidad.segmentos = new List<Segmento>();

                if (template != null)
                {
                    Template t = new Template();
                    t.id = template.Value;
                    publicidad.template = t;
                }
                if (mensaje != null)
                {
                    Mensaje m = new Mensaje();
                    m.id = mensaje.Value;
                    publicidad.mensaje = m;
                }

                if (ids != null && ids.Length > 0)
                {
                    foreach (var id in ids)
                    {
                        var s = new Segmento();
                        s.id = int.Parse(id);
                        publicidad.segmentos.Add(s);
                    }
                }

                publicidadBll.Grabar(publicidad);

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