using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Etiqueta
    {
        public Etiqueta() { }

        public Etiqueta(int id, string nombre)
        {
            this.ID = id;
            this.Descripcion = nombre;
        }

        public Etiqueta(int id, string nombre, string traduccion)
        {
            this.ID = id;
            this.Descripcion = nombre;
            this.Traduccion = traduccion;
        }

        public int ID { get; set; }
        public string Descripcion { get; set; }
        public string Traduccion { get; set; }
    }
}
