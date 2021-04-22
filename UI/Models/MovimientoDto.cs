using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class MovimientoDto
    {
        [DataType(DataType.Currency)]
        public double Monto { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }

        public String Descripcion { get; set; }

        public String Tipo { get; set; }

        public int Cuota { get; set; }

        public int CuotaTotal { get; set; }
    }
}