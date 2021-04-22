using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public class DvhException : Exception
    {

        public DvhException(string mensaje) : base(mensaje){ }
    }
}
