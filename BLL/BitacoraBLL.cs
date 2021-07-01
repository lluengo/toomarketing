using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BitacoraBLL
    {

        private DAL_Bitacora mp = new DAL_Bitacora();
        public List<Bitacora> Listar() {
            return mp.Listar();
        }

        public void Grabar(Bitacora bitacora)
        {
             mp.Insertar(bitacora);
        }
    }
}
