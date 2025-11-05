using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeClinica.Models
{
    public class Medicamentos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set;  }
        public string Descripcion { get; set; }
        public int Activo { get; set; }
    }
}
