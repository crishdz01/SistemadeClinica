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
    public class PacientesController
    {
        private ClaseCRUD crud = new ClaseCRUD();

        public DataTable ObtenerTodosPacientes()
        {
            try
            {
                string query = "SELECT Id, Nombre, Edad, FechaDeNacimiento, FechaDeIngreso, Dui, Telefono, Domicilio, Tutor, Parentesco, DomicilioTutor, CodigoPacientes FROM pacientes WHERE Activo = 1";
                crud.ConsultarGral(query, "pacientes");
                return crud.ds.Tables["pacientes"];
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener pacientes: {ex.Message}");
            }
        }

        public bool CrearPaciente(Pacientes pacientes)
        {
            try
            {
                string campos = "Nombre, Edad, FechaDeNacimiento, FechaDeIngreso, Dui, Telefono, Domicilio, Tutor, Parentesco, DomicilioTutor, CodigoPacientes";
                string valores = $"'{pacientes.Nombre}', '{pacientes.Edad}', '{pacientes.FechaNacimiento}', '{pacientes.FechaIngreso}', '{pacientes.DUI}', '{pacientes.Telefono}', '{pacientes.Domicilio}', '{pacientes.Tutor}', '{pacientes.Parentesco}', '{pacientes.DomicilioTutor}', '{pacientes.CodigoPaciente}'";

                return crud.Insertar("pacientes", campos, valores);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear pacientes: {ex.Message}");
            }
        }

        public bool ActualizarPaciente(Pacientes pacientes)
        {
            try
            {
                string campos = $"Nombre='{pacientes.Nombre}', " +
                                $"Edad='{pacientes.Edad}', " +
                                $"FechaDeNacimiento='{pacientes.FechaNacimiento}', " +
                                $"FechaDeIngreso='{pacientes.FechaIngreso}', " +
                                $"Dui='{pacientes.DUI}', " +
                                $"Telefono='{pacientes.Telefono}', " +
                                $"Domicilio='{pacientes.Domicilio}', " +
                                $"Tutor='{pacientes.Tutor}', " +
                                $"Parentesco='{pacientes.Parentesco}', " +
                                $"DomicilioTutor='{pacientes.DomicilioTutor}'";

                
                string condicion = $"CodigoPacientes = {pacientes.CodigoPaciente}";

                return crud.Actualizar("pacientes", campos, condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar pacientes: {ex.Message}");
            }
        }




        public DataTable ObtenerPacientesPorCondicion(string condicion)
        {
            try
            {
                return crud.ConsultarCondicion("Id, CodigoPacientes", "pacientes", condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener pacientes por condición: {ex.Message}");
            }
        }

        public bool EliminarPaciente(int idPaciente)
        {
            try
            {
                // ELIMINACIÓN LÓGICA (recomendado) - Cambiar Activo = 0
                string campos = "Activo = 0";
                string condicion = $"Id = {idPaciente}";
                return crud.Actualizar("pacientes", campos, condicion);
            }
            
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar pacientes: {ex.Message}");
            }
        }
        public DataTable ObtenerPacientePorId(int idPacientes)
        {
            try
            {
                string condicion = $"Id = {idPacientes} AND Activo = 1";
                return crud.ConsultarCondicion("*", "pacientes", condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener pacientes por ID: {ex.Message}");
            }
        }

        public DataTable BuscarPacientePorNombrePaciente(string filtro)
        {
            try
            {
                // Escapar comillas simples para evitar inyección SQL
                string filtroSeguro = filtro.Replace("'", "''");
                // Construir condición (solo usuarios activos)
                string condicion = $"LOWER(CodigoPacientes) LIKE '%{filtroSeguro.ToLower()}%' AND Activo = 1";
                // Seleccionar las columnas que quieres mostrar
                string columnas = "Id, Nombre, Edad, FechaDeNacimiento, FechaDeIngreso, Dui, Telefono, Domicilio, Tutor, Parentesco, DomicilioTutor, CodigoPacientes";
                return crud.ConsultarCondicion(columnas, "pacientes", condicion);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la búsqueda: {ex.Message}");
            }
        }
    }
}
