using BE;
using DAL;
using DAL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_Resguardo : AbstractBLL<Resguardo>
    {
        DAL_Resguardo mp = new DAL_Resguardo();
        public override int Borrar(Resguardo objeto)
        {
            throw new NotImplementedException();
        }

        public override int Grabar(Resguardo objeto)
        {
           return mp.Insertar(objeto);
        }

        public override List<Resguardo> Listar()
        {
            try
            {
                return mp.Listar();
            }
            catch (DvhException e) {
                throw new BLL.Exceptions.DvhException(e.Message);
            }
        }

        public override Resguardo Listar(int id)
        {
            return mp.Listar(id);
        }

        public int Restore(Resguardo r)
        {
            
           int resultado = mp.Restore(r);
            return resultado;
        }
    }
}
