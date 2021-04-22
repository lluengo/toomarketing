using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Resguardo
    {
        public int Id { get; set; }

        public string Tipo { get; set; }

        public DateTime Fecha { get; set; }

        public Usuario Usuario { get; set; }

        public string Nombre { get; set; }

        public string Path { get; set; }
    }
}
