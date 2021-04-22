using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Bitacora
    {
        public Bitacora() { }

        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime fecha { get; set; }
        public String mensaje { get; set; }
        public TipoLog Tipo { get; set; }

    }
}
