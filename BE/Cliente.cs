using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public long Dni { get; set; }

        public Usuario Usuario { get; set; }

        [Required]
       
        public DateTime FechaNacimiento { get; set; }

        public string Direccion { get; set; }

        public int Edad
        {
            get
            {
                DateTime now = DateTime.Today;
                int edad = DateTime.Today.Year - FechaNacimiento.Year;

                if (DateTime.Today < FechaNacimiento.AddYears(edad))
                    return --edad;
                else
                    return edad;
            }
        }

    }
}

