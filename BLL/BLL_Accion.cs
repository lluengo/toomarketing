using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{

    public class BLL_Accion : AbstractBLL<Accion>
    {
        private DAL_Accion dal = new DAL_Accion();

        public override int Borrar(Accion e)
        {
            return dal.Borrar(e);
        }

        public override int Grabar(Accion e)
        {
            int resultado;
            if (e.id == 0)
            {
                resultado = dal.Insertar(e);
            }
            else
            {
                resultado = dal.Editar(e);
            }


            return resultado;
        }

        public override List<Accion> Listar()
        {
            return dal.Listar();
        }

        public override Accion Listar(int id)
        {
            return dal.Listar(id);
        }
    }


}