using System;
using SistemadeClinica.Models;
using SistemadeClinica.Data;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeClinica.Controllers
{
    
    class CitaController
    {
        private ClaseCRUD crud = new ClaseCRUD();

        public DataTable ObtenerTodasCitas()
        {
            try
            {
                string query = "SELECT Id, Nombre, Fecha FROM citas";
                crud.ConsultarGral(query, "citas");
                return crud.ds.Tables["citas"];
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener citas: {ex.Message}");
            }
        }

        public bool CrearCita(Cita cita) //INSERTAR
        {
            try
            {
                string campos = "Nombre, Fecha";
                string valores = $"'{cita.NombreCompleto}', '{cita.fecha}'";

                return crud.Insertar("citas", campos, valores);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear citas: {ex.Message}");
            }
        }

        public bool ActualizarCita(Cita cita)
        {
            try
            {
                string campos = $"Nombre = '{cita.NombreCompleto}', Fecha = '{cita.fecha}'";
                string condicion = $"Id = {cita.id}";
                return crud.Actualizar("Citas", campos, condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar Citas: {ex.Message}");
            }
        }

        public bool EliminarCitas(int idcita)
        {
            try
            {

                string condicion = $"Id = {idcita}";
                return crud.Eliminar("Citas", condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar citas: {ex.Message}");
            }
        }

        public DataTable ObtenerCitasPorCondicion(string condicion)
        {
            try
            {
                string query = $"SELECT Id,Nombre FROM citas WHERE {condicion}";
                return crud.ConsultarCondicion("Id, Nombre", "Citas", condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Citas por condición: {ex.Message}");
            }
        }

        public DataTable BuscarCitaPorNombreUsuario(string filtro)
        {
            try
            {
                // Escapar comillas simples para evitar inyección SQL
                string filtroSeguro = filtro.Replace("'", "''");
                // Construir condición (solo usuarios activos)
                string condicion = $"LOWER(Cita) LIKE '%{filtroSeguro.ToLower()}'";
                // Seleccionar las columnas que quieres mostrar
                string columnas = "Id, Nombre, Fecha";
                return crud.ConsultarCondicion(columnas, "Citas", condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la búsqueda: {ex.Message}");
            }
        }
    }
    
}
