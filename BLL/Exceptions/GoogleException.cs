using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class GoogleException : Exception
    {
        public GoogleException() { }
        public GoogleException(string mensaje) : base(mensaje)
        {
            Console.WriteLine(mensaje);
        }
    }
}
