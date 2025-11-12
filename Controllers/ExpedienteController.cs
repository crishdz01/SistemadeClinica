using SistemadeClinica.Models;
using SistemadeClinica.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeClinica.Controllers
{
    public class ExpedienteController
    {
        private ClaseCRUD crud = new ClaseCRUD();

        public bool CrearExpediente(Expediente expediente) //INSERTAR
        {
            try
            {
                string campos = "Motivo, Padece, Padecido, Medicamento, Enfermedad, Nombre, Edad, Codigo, Fecha";
                string valores = $"'{expediente.Motivo}', '{expediente.Padece}', '{expediente.Padecido}', '{expediente.Medicamento}', '{expediente.Enfermedad}', '{expediente.Nombre}', '{expediente.Edad}', '{expediente.Codigo}', '{expediente.Fecha}'";

                return crud.Insertar("expediente", campos, valores);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear expediente: {ex.Message}");
            }
        }
    }
}
