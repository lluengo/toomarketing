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
    public class ReglaController : GenericController
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

        private IdiomaBLL idiomaBll = new IdiomaBLL();

        // GET: Idioma
        public ActionResult Index()
        {
            if (!verificarPermiso(ROLES, (Usuario)Session["usuario"]))
                return View(@"~\Views\Shared\AccessDenied.cshtml"); 

            var lista = idiomaBll.ListarIdiomas();
            return View(lista);
        }

    
    }
}