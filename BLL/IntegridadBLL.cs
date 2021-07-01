using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class IntegridadBLL
    {
        private DAL_Usuario mpUsuario = new DAL_Usuario();
        public bool RecalcularDigitos()
        {
            return mpUsuario.RecalcularDigitos();
            
        }
    }
}
