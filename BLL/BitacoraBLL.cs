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

        private MP_Bitacora mp = new MP_Bitacora();
        public List<Bitacora> Listar() {
            return mp.Listar();
        }

        public void Grabar(Bitacora bitacora)
        {
             mp.Insertar(bitacora);
        }
    }
}
