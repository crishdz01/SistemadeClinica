using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Clase para usar la base
using MySql.Data.MySqlClient;

namespace SistemadeClinica.Data
{
    //Clase para la base
    class ClaseCRUD
    {
        public string Cadena = "server=localhost;port=3306;uid=root;pwd='';database=sistema_de_clinica;";
        

        
        private MySqlConnection Conexion;
        private MySqlCommandBuilder cmb;
        public DataSet ds = new DataSet();
        public MySqlDataAdapter da;
        public MySqlCommand Comando;
         
        private void Conectar()
        {
            Conexion = new MySqlConnection(Cadena);
        }
        //Constructor para conectar, debe tener el mismo nombre de la Clase
        public ClaseCRUD()
        {
            Conectar();
        }
        //Método para insertar nuevo registros a una tabla
        public bool Insertar(string Tabla, string Campos, string Valores)
        {
            try
            {
                if (Conexion.State != ConnectionState.Open)
                    Conexion.Open();

                string querySQL = $"INSERT INTO {Tabla} ({Campos}) VALUES ({Valores});";
                Comando = new MySqlCommand(querySQL, Conexion);
                int i = Comando.ExecuteNonQuery();

                return i > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al insertar en la tabla {Tabla}: {ex.Message}");
            }
            finally
            {
                if (Conexion.State == ConnectionState.Open)
                    Conexion.Close();
            }
        }
        //Método para generar una consulta de una tabla en específica
        public DataTable Consultar(string Campos, string Tabla)
        {
            try
            {
                if (Conexion.State != ConnectionState.Open)
                    Conexion.Open();

                string querySQL = $"SELECT {Campos} FROM {Tabla}";
                da = new MySqlDataAdapter(querySQL, Conexion);
                DataSet dts = new DataSet();
                da.Fill(dts, Tabla);
                return dts.Tables[Tabla];
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consultar la tabla {Tabla}: {ex.Message}");
            }
            finally
            {
                if (Conexion.State == ConnectionState.Open)
                    Conexion.Close();
            }
        }
        //Método para realizar todo tipo de consulta
        public void ConsultarGral(string querySQL, string Tabla)
        {
            try
            {
                ds.Tables.Clear();

                if (Conexion.State != ConnectionState.Open)
                    Conexion.Open();

                da = new MySqlDataAdapter(querySQL, Conexion);
                cmb = new MySqlCommandBuilder(da);
                da.Fill(ds, Tabla);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consultar datos generales: {ex.Message}");
            }
            finally
            {
                if (Conexion.State == ConnectionState.Open)
                    Conexion.Close();
            }
        }
        //Método para eliminar registro de una tabla
        public bool Eliminar(string Tabla, string Condicion)
        {
            try
            {
                if (Conexion.State != ConnectionState.Open)
                    Conexion.Open();

                string querySQL = $"DELETE FROM {Tabla} WHERE {Condicion}";
                Comando = new MySqlCommand(querySQL, Conexion);
                int i = Comando.ExecuteNonQuery();

                return i > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar de la tabla {Tabla}: {ex.Message}");
            }
            finally
            {
                if (Conexion.State == ConnectionState.Open)
                    Conexion.Close();
            }
        }
        //Método para generar una consulta definiendo una condición
        public DataTable ConsultarCondicion(string Campos, string Tabla, string Condicion)
        {
            try
            {
                if (Conexion.State != ConnectionState.Open)
                    Conexion.Open();

                string querySQL = $"SELECT {Campos} FROM {Tabla} WHERE {Condicion}";
                da = new MySqlDataAdapter(querySQL, Conexion);
                DataSet dts = new DataSet();
                da.Fill(dts, Tabla);
                return dts.Tables[Tabla];
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al consultar con condición: {ex.Message}");
            }
            finally
            {
                if (Conexion.State == ConnectionState.Open)
                    Conexion.Close();
            }
        }
        //Método para actualizar registro en una tabla
        public bool Actualizar(string Tabla, string Campos, string Condicion)
        {
            try
            {
                if (Conexion.State != ConnectionState.Open)
                    Conexion.Open();

                string querySQL = $"UPDATE {Tabla} SET {Campos} WHERE {Condicion}";
                Comando = new MySqlCommand(querySQL, Conexion);
                int i = Comando.ExecuteNonQuery();

                return i > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar la tabla {Tabla}: {ex.Message}");
            }
            finally
            {
                if (Conexion.State == ConnectionState.Open)
                    Conexion.Close();
            }
        }


    }
}