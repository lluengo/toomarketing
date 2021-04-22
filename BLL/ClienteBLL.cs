using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ClienteBLL: AbstractBLL<Cliente>
    {
        private MP_Cliente mp = new MP_Cliente();

        public override int Borrar(Cliente e)
        {
            return mp.Borrar(e);
        }

        public override int Grabar(Cliente e)
        {
            int resultado;
            if (e.Id == 0)
            {
                resultado = mp.Insertar(e);
            }
            else
            {
                resultado = mp.Editar(e);
            }


            return resultado;
        }

        public override List<Cliente> Listar()
        {
            return mp.Listar();
        }

        public override Cliente Listar(int id)
        {
            return mp.Listar(id);
        }

        public Cliente ListarPorUsuario(int id)
        {
            return mp.ListarPorUsuario(id);
        }


    }
}
