using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Condicion
    {
        public int id { get; set; }
        public String atributo { get; set; }
        public String valor { get; set; }
        public Entidad entidad { get; set; }
        public Operacion operacion { get; set; }
        public TipoValor tipoValor { get; set; }

    }
}
