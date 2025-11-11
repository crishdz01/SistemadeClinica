using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeClinica.Models
{
    public class Pacientes
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string FechaNacimiento { get; set; }
        public string FechaIngreso { get; set; }
        public string DUI { get; set; }
        public int Telefono { get; set; }
        public string Domicilio { get; set; }
        public string Tutor { get; set; }
        public string Parentesco { get; set; }
        public string DomicilioTutor { get; set; }
        public int CodigoPaciente { get; set; }
    }
}
