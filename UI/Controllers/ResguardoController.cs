using BE;
using BLL;
using BLL.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class ResguardoController : GenericController
    {
        private const string ROLES = "Administrador";

        private BLL_Resguardo bll = new BLL_Resguardo();
        // lista los backups
        public ActionResult Index()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                return View(bll.Listar());
            }
            catch (DvhException ex) {
                return View(@"~\Views\Shared\Dvh.cshtml");
            }
        }

        // GET: Resguardo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Resguardo/Crea
        public ActionResult Create()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            try
            {
                Resguardo resg = new Resguardo();
                resg.Usuario = (Usuario) Session["usuario"];
                resg.Fecha = DateTime.Now;
                resg.Tipo = "BackUP";

                string path = Server.MapPath("~/BackUps/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string nombre = "backUp-" + DateTime.Now.ToString("dd-MM-yyyy-hhmmss") + ".bak";
                resg.Nombre = nombre;                
                resg.Path = path;
                bll.Grabar(resg);
                ViewBag.Mensaje = "BackUp Creado";
                return View();
            }
            catch(Exception e)
            {
               var o = e.Message;
                return View();
            }
        }

        
        // GET: Resguardo/Delete/5
        public ActionResult Delete(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            //lamar al listar para sobrescribir el usuario que lo hace o pasarle el usuario

            Resguardo resguardo = bll.Listar(id);

            Resguardo r = new Resguardo();
            r.Fecha = DateTime.Now;
            r.Usuario = (Usuario)Session["usuario"];
            r.Tipo = "Restore";
            r.Path = Server.MapPath("~/BackUps/");
            r.Nombre = resguardo.Nombre;
            int re = bll.Restore(r);
            if (re == 1) {
                Session["usuario"] = null;
                return RedirectToAction("Index", "Login");
            }
            return RedirectToAction("Index");
        }

        
    }
}
