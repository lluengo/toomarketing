using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Usuario
    {
        public Usuario()
        { }

        public Usuario(int id, string usuario, string estado, string nombre, string apellido, string correo, string dvh, Idioma idioma)
        {
            this.ID = id;
            this.usuario = usuario;
            this.estado = estado;
            this.nombre = nombre;
            this.apellido = apellido;
            this.correo = correo;
            this.dvh = dvh;
            this.idioma = idioma;
        }

        public int ID { get; set; }

        [Required]
        public string usuario { get; set; }
        public string estado { get; set; }

        [Required]
        public string password { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public string apellido { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string correo { get; set; }
        public string dvh { get; set; }
        public Idioma idioma { get; set; }

        public string CalcularDigito() {

            string cadena = nombre + apellido + correo;
            double resultado = 0;
            foreach (char c in cadena) {
                resultado += char.GetNumericValue(c);
            }
            return resultado.ToString();
        }
    }
}
