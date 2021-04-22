using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Permiso
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
    }
    public enum Tipo_Permiso
    {
        familia,
        patente
    }
}
