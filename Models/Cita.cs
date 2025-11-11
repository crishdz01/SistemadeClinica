using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeClinica.Models
{
    public class Cita
    {
        public int id { get; set; }
        public string NombreCompleto { get; set; }
        public string fecha { get; set; }
        public string Hora { get;set }
    }
}
