using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public abstract class MAPPER<T>
    {
        protected void Abrir()
        {
            if (accesopropio)
            {
                acceso.Abrir();
            }
        }
        protected void Cerrar()
        {
            if (accesopropio)
            {
                acceso.Cerrar();
            }
        }


        internal ACCESO acceso;

        protected bool accesopropio;

        public abstract int Insertar(T objeto);
        public abstract int Editar(T objeto);
        public abstract int Borrar(T objeto);

        public abstract List<T> Listar();

        

    }
}
