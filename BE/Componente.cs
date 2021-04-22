using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public abstract class Componente
    {
        public int ID { get; set; }
        public string Nombre { get; set; }

        public abstract List<Componente> ObtenerHijos();
        public abstract void AgregarHijos(Componente componente);

    }
}
