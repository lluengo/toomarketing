using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Regla
    {
        public int id { get; set; }
        public int nombre { get; set; }
        public Condicion condicion { get; set; }
        public Accion accion { get; set; }
    }
}
