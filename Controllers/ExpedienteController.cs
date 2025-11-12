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

        public DataTable ObtenerHistorialporCodigo(string codigo)
        {
            try
            {
                string condicion = $"Codigo = {codigo}";
                return crud.ConsultarCondicion("*", "expediente", condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener expedienes: {ex.Message}");
            }
        }

        public DataTable ObtenerTodosExpediente()
        {
            try
            {
                string query = "SELECT Id, Motivo, Padece, Padecido, Medicamento, Enfermedad, Nombre, Edad, Codigo, Fecha FROM expediente";
                crud.ConsultarGral(query, "expediente");
                return crud.ds.Tables["expediente"];
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener expedientes: {ex.Message}");
            }
        }
    }
}
