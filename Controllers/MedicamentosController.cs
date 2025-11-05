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
    public class MedicamentosController
    {
        private ClaseCRUD crud = new ClaseCRUD();

        public DataTable ObtenerTodosMedicamentos ()
        {
            try
            {
                string query = "SELECT Id, Nombre, Precio, Stock, Descripcion FROM medicamentos WHERE Activo = 1";
                crud.ConsultarGral(query, "medicamentoss");
                return crud.ds.Tables["medicamentos"];
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener medicamentos: {ex.Message}");
            }
        }

        public bool CrearMedicamento(Medicamentos medicamento) //INSERTAR
        {
            try
            {
                string campos = "Nombre, Precio, Stock, Descripcion";
                string valores = $"'{medicamento.Nombre}', {medicamento.Precio}, {medicamento.Stock}, {medicamento.Descripcion}";
                return crud.Insertar("medicamentos", campos, valores);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear medicamentos: {ex.Message}");
            }
        }

        public bool ActualizarProducto(Medicamentos medicamentos)
        {
            try
            {
                string campos = $"Nombre='{medicamentos.Nombre}', Precio={medicamentos.Precio}, Stock={medicamentos.Stock}";
                string condicion = $"Id = {medicamentos.Id}";
                return crud.Actualizar("Productos", campos, condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar producto: {ex.Message}");
            }
        }

        public bool EliminarMedicamento(int id)
        {
            try
            {
                string condicion = $"Id = {id}";
                string campos = "Activo = 0"; // Eliminación lógica
                return crud.Actualizar("medicamentos", campos, condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar medicamentos: {ex.Message}");
            }
        }
    }
}
