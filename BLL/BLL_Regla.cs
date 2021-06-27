using BE;
using DAL;
using System.Collections.Generic;


namespace BLL
{

    public class BLL_Regla : AbstractBLL<Regla>
    {
        private DAL_Regla dal = new DAL_Regla();

        public override int Borrar(Regla e)
        {
            return dal.Borrar(e);
        }

        public override int Grabar(Regla e)
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

        public override List<Regla> Listar()
        {
            return dal.Listar();
        }

        public override Regla Listar(int id)
        {
            return dal.Listar(id);
        }
    }

   
}
