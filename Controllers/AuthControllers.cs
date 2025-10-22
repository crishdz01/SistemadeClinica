
//Usando la clase models. La clase models nos dara los que usaran el programa 
using SistemadeClinica.Models;
using SistemadeClinica.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemadeClinica.Controllers
{
    //Clase para el login
    public class AuthControllers
    {
        /*
        private ClaseCRUD crud = new ClaseCRUD();
        public bool Autenticar(string usuario, string password)
        {
            try
            {
                string passwordHash = GenerarHashSHA256(password);
                string condicion = $"Usuario = '{usuario}' AND PasswordHash = '{passwordHash}' AND Activo = 1";
                DataTable dt = crud.ConsultarCondicion("u.Id, u.Usuario, u.IdRol, r.Nombre as RolNombre, u.Nombre, u.Apellido",
                "Usuarios u INNER JOIN Roles r ON u.IdRol = r.Id", condicion);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    Sesion.UsuarioId = Convert.ToInt32(row["Id"]);
                    Sesion.Username = row["Usuario"].ToString();
                    Sesion.RolId = Convert.ToInt32(row["IdRol"]);
                    Sesion.RolNombre = row["RolNombre"].ToString();
                    Sesion.NombreCompleto = $"{row["Nombre"]} {row["Apellido"]}";
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en autenticación: {ex.Message}");
            }
        }
        */
    }
}
