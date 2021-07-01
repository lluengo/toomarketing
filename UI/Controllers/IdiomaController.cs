using BE;
using BLL;
using BLL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class IdiomaController : GenericController
    {
        private const string ROLES = "Administrador";

        private Dictionary<string, string> idiomas_disponibles = new Dictionary<string, string>() {
            { "Chino", "zh" },
            { "Holandés", "nl" },
            { "Inglés", "en" },
            { "Francés", "fr" },
            { "Alemán", "de" },
            { "Griego", "el" },
            { "Húngaro", "hu" },
            { "Italiano", "it" },
            { "Latín", "la" },
            { "Noruego", "no" },
            { "Polaco", "pl" },
            { "Portugués", "pt" },
            { "Ruso", "ru" },
            { "Sueco", "sv" },
            { "Tailandés", "th" },
        };

        private BLL_Idioma idiomaBll = new BLL_Idioma();

        // GET: Idioma
        public ActionResult Index()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml"); 

            var lista = idiomaBll.ListarIdiomas();
            return View(lista);
        }

        // GET: Idioma/Details/5
        public ActionResult Details(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            var model = idiomaBll.ListarEtiquetasPorIdioma(idiomaBll.ListarIdioma(id).Descripcion);
            return View(model);
        }

        // GET: Idioma/Create
        public ActionResult Create()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            List<string> idiomas = new List<string>();
            foreach (string idioma in idiomas_disponibles.Keys)
            {
                idiomas.Add(idioma);
            }

            ViewBag.idiomas = idiomas;
            return View();
        }

        // POST: Idioma/Create
        [HttpPost]
        public ActionResult Create(Idioma idioma)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            string codigo_idioma = idiomas_disponibles[idioma.Descripcion];
            try
            {
                int id_idioma = idiomaBll.AltaIdioma(idioma);
                //Si se creo correctamente el idioma busco todas las etiquetas y realizo la traduccion con GOOGLE API
                if (id_idioma > 0)
                {
                    List<Etiqueta> etiquetas = new List<Etiqueta>();
                    etiquetas = idiomaBll.ListarEtiquetas();

                    foreach (Etiqueta etiqueta in etiquetas)
                    {
                        etiqueta.Traduccion = idiomaBll.TranslateText(etiqueta.Descripcion, codigo_idioma);
                        idiomaBll.AltaEtiqueta(etiqueta, id_idioma);
                    }

                }

                List<Idioma> idiomas = new List<Idioma>();
                idiomas = idiomaBll.ListarIdiomas();
                Session["Idiomas"] = idiomas;

                return RedirectToAction("Index");
            }
            catch (GoogleException e)
            {
                return View(@"~\Views\Idioma\TranslationError.cshtml");
            }
            catch (Exception ex)
            {
                List<string> idiomas = new List<string>();
                foreach (string idioma1 in idiomas_disponibles.Keys)
                {
                    idiomas.Add(idioma1);
                }

                ViewBag.idiomas = idiomas;
                Console.WriteLine(ex.Message);
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            var model = idiomaBll.ListarIdioma(id);
            return View(model);
        }

        // POST: Idioma/Edit/5
        [HttpPost]
        public ActionResult Edit(Idioma idioma)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            int result = idiomaBll.EditarIdioma(idioma);

            if (result == 1) { return RedirectToAction("Index"); }
            else { return View(); }
        }

        // GET: Idioma/Edit/5 ---- Editar etiqueta
        public ActionResult EditarEtiqueta(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            var model = idiomaBll.ListarEtiqueta(id);
            return View(model);
        }

        // POST: Idioma/Edit/5 ---- Editar etiqueta
        [HttpPost]
        public ActionResult EditarEtiqueta(Etiqueta etiqueta)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            int result = idiomaBll.EditarEtiqueta(etiqueta);

            if (result == 1) { return RedirectToAction("Index"); }
            else { return View(); }
        }

        // GET: Idioma/Delete/5
        public ActionResult Delete(int id)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");

            var model = idiomaBll.ListarIdioma(id);
            return View(model);
        }

        // POST: Idioma/Delete/5
        [HttpPost]
        public ActionResult Delete(Idioma idioma)
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml");


            Usuario usuario = (Usuario)Session["usuario"];
         //   if (idioma.Id == usuario.Idioma.Id)
         //   {
         //       ViewBag.message = "El idioma no puede ser eliminado ya que esta siendo utilizado por los usuarios";
         //       return View();
         //   }

            try
            {
                int result = idiomaBll.BorrarIdioma(idioma);
                if (result == 1)
                {
                    List<Idioma> idiomas = new List<Idioma>();
                    idiomas = idiomaBll.ListarIdiomas();
                    Session["Idiomas"] = idiomas;

                    return RedirectToAction("Index");
                }
                else { return View(); }
            }
            catch
            {
                return View();
            }
        }
    }
}