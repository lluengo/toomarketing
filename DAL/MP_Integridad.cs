using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MP_Integridad
    {

        private ACCESO acceso;
        public MP_Integridad()
        {
            acceso = new ACCESO();
            
        }

        internal MP_Integridad(ACCESO ac)
        {
            acceso = ac;
            
        }

        

    }
}
