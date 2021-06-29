using BE;
using DAL;
using System.Collections.Generic;


namespace BLL
{

    public class BLL_Publicidad : AbstractBLL<Publicidad>
    {
        private DAL_Publicidad dal = new DAL_Publicidad();

        public override int Borrar(Publicidad e)
        {
            return dal.Borrar(e);
        }

        public override int Grabar(Publicidad e)
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

        public override List<Publicidad> Listar()
        {
            return dal.Listar();
        }

        public override Publicidad Listar(int id)
        {
            return dal.Listar(id);
        }
    }

   
}
