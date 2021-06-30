using BE;
using DAL;
using System.Collections.Generic;


namespace BLL
{

    public class BLL_Servicio : AbstractBLL<Servicio>
    {
        private DAL_Servicio dal = new DAL_Servicio();

        public override int Borrar(Servicio e)
        {
            return dal.Borrar(e);
        }

        public override int Grabar(Servicio e)
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

        public override List<Servicio> Listar()
        {
            return dal.Listar();
        }

        public override Servicio Listar(int id)
        {
            return dal.Listar(id);
        }
    }

   
}
