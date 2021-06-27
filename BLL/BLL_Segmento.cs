using BE;
using DAL;
using System.Collections.Generic;


namespace BLL
{

    public class BLL_Segmento : AbstractBLL<Segmento>
    {
        private DAL_Segmento dal = new DAL_Segmento();

        public override int Borrar(Segmento e)
        {
            return dal.Borrar(e);
        }

        public override int Grabar(Segmento e)
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

        public override List<Segmento> Listar()
        {
            return dal.Listar();
        }

        public override Segmento Listar(int id)
        {
            return dal.Listar(id);
        }
    }

   
}
