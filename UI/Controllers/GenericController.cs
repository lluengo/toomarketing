using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class GenericController : Controller
    {
        // GET: Generic
        public Boolean verificarPermiso(string permiso, Usuario usuario)
        {
            bool estado = false;
            BLL_Permiso gestor_permisos = new BLL_Permiso();

            List<Permiso> permisos = new List<Permiso>();
            if (usuario == null)
                return false;
            permisos = gestor_permisos.VerificarPermisosUsuario(usuario.ID);
            if (permisos.Count == 0)
                return false;
            else
            {
                foreach (Permiso perm in permisos)
                {
                    estado = gestor_permisos.BusquedaRecursiva(gestor_permisos.BuscarPermiso(perm.Nombre).ID.ToString(), permiso);
                    if (estado)
                        return estado;
                }
                return estado;
            }
        }
    }
}