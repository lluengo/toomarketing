using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Familia : Componente
    {
        private List<Componente> lista_hijos;

        public Familia()
        {
            lista_hijos = new List<Componente>();
        }


        public override void AgregarHijos(Componente componente)
        {
            lista_hijos.Add(componente);
        }

        public override List<Componente> ObtenerHijos()
        {
            return lista_hijos;
        }
    }
}
