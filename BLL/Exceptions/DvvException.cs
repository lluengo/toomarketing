using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class DvvException : Exception
    {
        public DvvException() { }
        public DvvException(string mensaje) : base(mensaje)
        {
            
        }
    }
}
