using BE;
using BLL;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class UsuarioController : GenericController
    {
        // GET: Usuario
        private const string ROLES = "Administrador";

        private UsuarioBLL bll = new UsuarioBLL();

        public ActionResult Index(int? page)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            IEnumerable<Usuario> usuarios = bll.Listar();
            
            usuarios =usuarios.OrderBy(o => o.usuario);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(usuarios.ToPagedList(pageNumber, pageSize));
            
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            return View(bll.Listar(id));
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            IdiomaBLL idiomaBll = new IdiomaBLL();
            ViewBag.Idiomas = new SelectList(idiomaBll.ListarIdiomas(), "Id", "Descripcion");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(Usuario user, int Idioma)
        {
            IdiomaBLL idiomaBll = new IdiomaBLL();
            Idioma i = idiomaBll.ListarIdioma(Idioma);
            user.idioma = i;
            try
            {
                Usuario u = bll.Listar(user.usuario);
                if (u == null)
                {
                    int resultado = bll.Grabar(user);
                    if (Session["usuario"] == null || Session["usuario"].Equals(""))
                    {
                        return RedirectToAction("Index", "Login");

                    }
                    return RedirectToAction("Index");
                } else
                {
                    ViewBag.Mensaje = "El nombre de usuario ya existe";
                    ViewBag.Idiomas = new SelectList(idiomaBll.ListarIdiomas(), "Id", "Descripcion");
                    return View(user);
                }
            }
            catch
            {
                
                ViewBag.Idiomas = new SelectList(idiomaBll.ListarIdiomas(), "Id", "Descripcion");
                return View();
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            IdiomaBLL idiomaBll = new IdiomaBLL();
            ViewBag.Idiomas = new SelectList(idiomaBll.ListarIdiomas(), "Id", "Descripcion");
            return View(bll.Listar(id));
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(Usuario user)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            int result = bll.Grabar(user);
            
            if (result == 1) { return RedirectToAction("Index"); }
            else { return View(); }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            return View(bll.Listar(id));
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(Usuario user)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            int result = bll.Borrar(user);

            if (result == 1) { return RedirectToAction("Index"); }
            else { return View(); }
        }

        public ActionResult Activar(Usuario user)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");
            int result = bll.Activar(user);

            if (result == 1) { return RedirectToAction("Index"); }
            else { return View(); }
        }

        // Users/Inactivate/5
        public ActionResult Desactivar(Usuario user)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            int result = bll.Desactivar(user);

            if (result == 1) { return RedirectToAction("Index"); }
            else { return View(); }
        }

        public ActionResult CambiarIdioma(int id_idioma, int id_usuario)
        {

            Idioma idioma = new Idioma();
            idioma.Id = id_idioma;
            Usuario usuario = (Usuario)Session["usuario"];
            if (usuario == null)
            {
               return RedirectToAction("Index", "Login");
            }
            usuario.idioma = idioma;
            int result = bll.EditarIdioma(usuario);

            if (result == 1)
            {
                #region Idiomas
                IdiomaBLL idiomaBll = new IdiomaBLL();
                List<Etiqueta> etiquetas = new List<Etiqueta>();
                etiquetas = idiomaBll.ListarEtiquetasPorIdioma(idiomaBll.ListarIdioma(usuario.idioma.Id).Descripcion);
                Dictionary<object, object> list = new Dictionary<object, object>();
                foreach (Etiqueta etiqueta in etiquetas)
                {
                    list.Add(etiqueta.Descripcion, etiqueta.Traduccion);
                }
                Session["EtiquetasIDIOMA"] = list;
                #endregion

                return View(@"~\Views\Idioma\CambioIdioma.cshtml");
            }
            else { return View(); }
        }

        public ActionResult MisDatos()
        {
            return View();
        }


        public ActionResult CambiarContrasena()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CambiarContrasena(Usuario usuario)
        {


            Usuario user = bll.Listar(usuario.ID);
            user.password = usuario.password;
            try
            {
                bll.Grabar(user);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            return RedirectToAction("LogOut", "Login");
        }
    }
}