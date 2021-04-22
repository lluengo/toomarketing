using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Util
    {
        public Boolean verificarPermiso(string permiso, Usuario usuario)
        {
            PermisoBLL permisoBLL = new PermisoBLL();
            List<Permiso> permisos = new List<Permiso>();
            if (usuario == null)
                return false;
            permisos = permisoBLL.VerificarPermisosUsuario(usuario.ID);
            if (permisos.Count == 0)
                return false;
            else
            {
                foreach (Permiso perm in permisos)
                {
                    if (perm.Nombre.Equals(permiso))
                        return true;
                }

                return false;
            }
        }
    }
}
