using BE;
using DAL;
using System.Collections.Generic;


namespace BLL
{

    public class BLL_Mensaje : AbstractBLL<Mensaje>
    {
        private DAL_Mensaje dal = new DAL_Mensaje();

        public override int Borrar(Mensaje e)
        {
            return dal.Borrar(e);
        }

        public override int Grabar(Mensaje e)
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

        public override List<Mensaje> Listar()
        {
            return dal.Listar();
        }

        public override Mensaje Listar(int id)
        {
            return dal.Listar(id);
        }
    }

   
}
