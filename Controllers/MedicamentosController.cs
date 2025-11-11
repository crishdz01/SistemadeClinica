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

        public DataTable ObtenerTodosMedicamentos()
        {
            try
            {
                string query = "SELECT Id, Nombre, Precio, Stock, Descripcion FROM Medicamentos WHERE Activo = 1";
                crud.ConsultarGral(query, "Medicamentos");
                return crud.ds.Tables["Medicamentos"];
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Medicamentos: {ex.Message}");
            }
        }

        public bool CrearMedicamento(Medicamentos medicamento) //INSERTAR
        {
            try
            {
                string campos = "Nombre, Precio, Stock, Descripcion";
                string valores = $"'{medicamento.Nombre}', '{medicamento.Precio}', '{medicamento.Stock}', '{medicamento.Descripcion}'";

                return crud.Insertar("Medicamentos", campos, valores);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear Medicamentos: {ex.Message}");
            }
        }

        public bool ActualizarMedicamento(Medicamentos medicamentos)
        {
            try
            {
                string campos = $"Nombre='{medicamentos.Nombre}', Precio='{medicamentos.Precio}', Stock='{medicamentos.Stock}', Descripcion='{medicamentos.Descripcion}'";
                string condicion = $"Id = {medicamentos.Id}";
                return crud.Actualizar("Medicamentos", campos, condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar Medicamentos: {ex.Message}");
            }
        }

        public bool EliminarMedicamento(int id)
        {
            try
            {
                string condicion = $"Id = {id}";
                string campos = "Activo = 0"; // Eliminación lógica
                return crud.Actualizar("Medicamentos", campos, condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar Medicamentos: {ex.Message}");
            }
        }

        public DataTable ObtenerMedicamentoPorCondicion(string condicion)
        {
            try
            {
                string query = $"SELECT Id, Nombre FROM medicamentos WHERE {condicion}";
                return crud.ConsultarCondicion("Id, Nombre", "Medicamentos", condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Medicamento por condición: {ex.Message}");
            }
        }

        public DataTable BuscarMedicamentoPorNombreUsuario(string filtro)
        {
            try
            {
                // Escapar comillas simples para evitar inyección SQL
                string filtroSeguro = filtro.Replace("'", "''");
                // Construir condición (solo usuarios activos)
                string condicion = $"LOWER(Medicamento) LIKE '%{filtroSeguro.ToLower()}%' AND Activo = 1";
                // Seleccionar las columnas que quieres mostrar
                string columnas = "Id, Nombre, Precio, Stock, Descripcion";
                return crud.ConsultarCondicion(columnas, "Medicamento", condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la búsqueda: {ex.Message}");
            }
        }

    }
}
