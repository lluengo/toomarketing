using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_Integridad
    {

        private ACCESO acceso;
        public DAL_Integridad()
        {
            acceso = new ACCESO();
            
        }

        internal DAL_Integridad(ACCESO ac)
        {
            acceso = ac;
            
        }

        

    }
}
