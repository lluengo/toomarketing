using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;

namespace UI.Controllers
{
    public class BitacoraController : GenericController
    {

        private const string ROLES = "Administrador";

        private BitacoraBLL bll = new BitacoraBLL();

        private UsuarioBLL usuarioBll = new UsuarioBLL();
        

        public ActionResult Index(string currentFilter, string currentFilterFechaDesde, string currentFilterFechaHasta, string currentFilterMensaje, string filtroUsuario, int? page, string filtroFechaDesde, string filtroFechaHasta, string filtroMensaje)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            List<SelectListItem> lst = new List<SelectListItem>();

            lst.Add(new SelectListItem() { Text = "Seleccione", Value = "" });
            lst.Add(new SelectListItem() { Text = "INFO", Value = "INFO" });
            lst.Add(new SelectListItem() { Text = "ERROR", Value = "ERROR" });
            lst.Add(new SelectListItem() { Text = "WARNING", Value = "WARNING" });

            
            ViewBag.Msg = lst;

            if (filtroUsuario != null)
                page = 1;
            else
                filtroUsuario = currentFilter;

            if (filtroFechaDesde != null)
            {
                page = 1;
            }
            else
            {
                filtroFechaDesde = currentFilterFechaDesde;
            }

            if (filtroFechaHasta != null)
            {
                page = 1;
            }
            else
            {
                filtroFechaHasta = currentFilterFechaHasta;
            }

            if (filtroMensaje != null)
            {
                page = 1;
            }
            else
            {
                filtroMensaje = currentFilterMensaje;
            }

            ViewBag.CurrentFilter = filtroUsuario;
            ViewBag.CurrentFechaDesde = filtroFechaDesde;
            ViewBag.CurrentFechaHasta = filtroFechaHasta;
            ViewBag.CurrentMensaje = filtroMensaje;

            IEnumerable<Bitacora> bitacoras = bll.Listar();

            if (!string.IsNullOrEmpty(filtroUsuario))
            {
                bitacoras = bitacoras.Where(s => s.Usuario.usuario.ToLower().Contains(filtroUsuario.ToLower()));
            }
            if (!string.IsNullOrEmpty(filtroFechaDesde))
            {
                bitacoras = bitacoras.Where(s => s.fecha >= Convert.ToDateTime(filtroFechaDesde));
            }

            if (!string.IsNullOrEmpty(filtroFechaHasta))
            {
                bitacoras = bitacoras.Where(s => s.fecha <= Convert.ToDateTime(filtroFechaHasta));
            }

            if (!string.IsNullOrEmpty(filtroMensaje))
            {
                bitacoras = bitacoras.Where(s => s.Tipo.ToString().ToLower().Contains(filtroMensaje.ToLower()));
            }

            bitacoras = bitacoras.OrderByDescending(o => o.fecha);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(bitacoras.ToPagedList(pageNumber, pageSize));
        }

        public JsonResult GetUsuarios(string Areas, string term = "")
        {
            var lista = usuarioBll.Listar().Where(o => o.usuario.Contains(term)).Select(o => new { o.ID, o.usuario }).OrderBy(o => o.ID).ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}