using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class IntegridadController : GenericController
    {

        private const string ROLES = "Administrador";

        private IntegridadBLL integridadBll = new IntegridadBLL();
        // GET: Integridad
        public ActionResult Index()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            return View();
        }

        public ActionResult Recalcular()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            bool resultado = integridadBll.RecalcularDigitos();
            if (resultado)
            {
                ViewBag.mensaje = "Digitos recalculados";
            }
            else
            {
                ViewBag.mensaje = "Error al recalcular";
            }
            return View("Index");
        }
    }
}
