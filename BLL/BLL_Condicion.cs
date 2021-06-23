using BE;
using DAL;
using System.Collections.Generic;


namespace BLL
{

    public class BLL_Condicion : AbstractBLL<Condicion>
    {
        private DAL_Condicion dal = new DAL_Condicion();

        public override int Borrar(Condicion e)
        {
            return dal.Borrar(e);
        }

        public override int Grabar(Condicion e)
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

        public override List<Condicion> Listar()
        {
            return dal.Listar();
        }

        public override Condicion Listar(int id)
        {
            return dal.Listar(id);
        }
    }

   
}
