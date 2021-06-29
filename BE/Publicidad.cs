using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Publicidad
    {
        public int id { get; set; }
        public String nombre { get; set; }
        public String descripcion { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public Template template { get; set; }
        public Mensaje mensaje { get; set; }
        public List<Segmento> segmentos { get; set; }
    }
}
