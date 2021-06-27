using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Segmento
    {
        public int id { get; set; }
        public String descripcion { get; set; }
        public String nombre { get; set; }
        public List<Regla> reglas { get; set; }
    }
}
