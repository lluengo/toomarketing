using BE;
using BLL.Exceptions;
using DAL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_Usuario : AbstractBLL<Usuario>
    {
        private DAL.DAL_Usuario mp = new DAL.DAL_Usuario();

        public override int Borrar(Usuario objeto)
        {
            return mp.Borrar(objeto);
        }

        public override int Grabar(Usuario objeto)
        {
            int resultado;
            objeto.password = Encriptor.encrypt(objeto.password);
            if (objeto.ID == 0)
            {
                resultado = mp.Insertar(objeto);
            }
            else
            {
                resultado = mp.Editar(objeto);
            }
            string valor = mp.calcularDVV();
            mp.actualizarDVV(valor);

            return resultado;
        }

        public override List<Usuario> Listar()
        {
            try
            {
                return mp.Listar();
            }
            catch (DAL.Exceptions.DvhException e)
            {
                throw new Exceptions.DvhException(e.Message);
            }
        }

        public override Usuario Listar(int id)
        {
            try
            { 
                return mp.Listar(id);
            }
            catch (DAL.Exceptions.DvhException e)
            {
                throw new Exceptions.DvhException(e.Message);
            }
        }

        public Usuario Consulta_login(string Usuario, string password)
        {
            Usuario u = mp.Consulta_login(Usuario, Encriptor.encrypt(password));
          
            return u;
        }

        public int Activar(Usuario objeto)
        {
            return mp.Activar(objeto);
        }

        public int Desactivar(Usuario objeto)
        {
            return mp.Desactivar(objeto);
        }

        public int Bloquear(string objeto)
        {
            return mp.Bloquear(objeto);
        }

        public Usuario Listar(string usuario)
        {
            return mp.Listar(usuario);
        }

        public int EditarIdioma(Usuario usuario)
        {
            return mp.EditarIdioma(usuario);
        }

        public string calcularDVV() {
            return mp.calcularDVV();
        }

        public string getDVV()
        {
            return mp.getDVV("Usuario");
        }
    }
}
