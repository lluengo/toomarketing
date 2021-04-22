using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Idioma
    {
        public Idioma() { }

        public Idioma(int id, string nombre)
        {
            this.Id = id;
            this.Descripcion = nombre;
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
}
