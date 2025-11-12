using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeClinica.Models
{
    public class Expediente
    {
        public string Nombre { get; set; }
        public string Edad { get; set; }
        public string Codigo { get; set; }
        public string Motivo { get; set; }
        public string Padece { get; set; }
        public string Padecido { get; set; }
        public string Medicamento { get; set; }
        public string Enfermedad { get; set; }

        public string Fecha { get; set; }
    }
}
