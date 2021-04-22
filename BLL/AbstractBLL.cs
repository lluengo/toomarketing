using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public abstract class AbstractBLL<T>
    {
        public abstract int Grabar(T objeto);

        public abstract int Borrar(T objeto);

        public abstract List<T> Listar();

        public abstract T Listar(int id);
    }
}
