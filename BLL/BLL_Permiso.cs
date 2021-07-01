using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_Permiso : AbstractBLL<Permiso>
    {
        DAL_Permiso mp = new DAL_Permiso();

        public List<Componente> Listar(string familia)
        {
            return mp.Listar(familia);
        }

        public List<Permiso> ListarTodos()
        {
            return mp.Listar();
        }

        public List<Permiso> VerificarPermisosUsuario(int id_usuario)
        {
            return mp.VerificarPermisosUsuario(id_usuario);
        }

        public List<Permiso> ListarPermisosDeFamilia(int id_permiso)
        {
            List<Permiso> return_permisos = new List<Permiso>();
            List<Permiso> permisos = mp.ListarPermisosDeFamilia(id_permiso);
            foreach (Permiso permiso in permisos)
            {
                return_permisos.Add(mp.BuscarPermiso(permiso.ID));
            }

            return return_permisos;
        }

        public List<Permiso> ListarFamilias()
        {
            return mp.ListarFamilias();
        }

        public List<Permiso> ListarPatentes()
        {
            return mp.ListarPatentes();
        }

        public Permiso BuscarPermiso(int id_permiso)
        {
            return mp.BuscarPermiso(id_permiso);
        }


        public bool AsignacionRoles(Permiso permiso, Permiso permisoPadre)
        {
            try
            {
                mp.AsignacionRoles(permiso, permisoPadre);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //return false;
            }
        }

        public bool BajaPermiso(Permiso permiso)
        {
            try
            {
                mp.EliminarPermisos(permiso);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //return false;
            }
        }

        public bool BajaRol(Permiso permiso)
        {
            try
            {
                mp.EliminarRoles(permiso);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //return false;
            }
        }

        public int Grabar(Permiso permiso, string rol)
        {
            permiso.ID = mp.Insertar(permiso);
            if (permiso.ID != -1)
            {

                if (permiso.Tipo == "familia")
                {
                    Permiso permiso_vacio = new Permiso();
                    permiso_vacio.ID = int.Parse(rol);
                    if (permiso_vacio.ID == 0)
                        permiso_vacio.Nombre = "NULL";
                    mp.AsignacionRoles(permiso, permiso_vacio);
                }
                else if (permiso.Tipo == "patente")
                {
                    Permiso permiso_rol = new Permiso();
                    permiso_rol = mp.BuscarPermiso(int.Parse(rol));
                    mp.AsignacionRoles(permiso, permiso_rol);
                }

                return 1;
            }
            else
            {
                return 0;
            }

        }

        public int AsignarPermisoUsuario(int id_permiso, int id_usuario)
        {
            int resultado = mp.AsignarPermisoUsuario(id_permiso, id_usuario);
            return resultado;
        }

        public int DesasignarPermisoUsuario(int id_permiso, int id_usuario)
        {
            int resultado = mp.DesasignarPermisoUsuario(id_permiso, id_usuario);
            return resultado;
        }

        public int Editar(Permiso permiso)
        {
            return mp.Editar(permiso);
        }

        public override int Borrar(Permiso objeto)
        {
            throw new NotImplementedException();
        }

        public override List<Permiso> Listar()
        {
            throw new NotImplementedException();
        }

        public override Permiso Listar(int id)
        {
            throw new NotImplementedException();
        }

        public override int Grabar(Permiso objeto)
        {
            throw new NotImplementedException();
        }

        public Permiso BuscarPermiso(string nombre)
        {
            return mp.BuscarPermiso(nombre);
        }

        public bool BusquedaRecursiva(string familia, string permiso)
        {
            return mp.BusquedaRecursiva(familia, permiso);
        }
    }
}
