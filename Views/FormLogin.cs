using SistemadeClinica.Models;
using SistemadeClinica.Data;
using SistemadeClinica.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;


namespace SistemadeClinica.Views
{
    public partial class FormLogin : MaterialForm
    {
        private SistemadeClinica.Controllers.AuthControllers authController;
        public FormLogin()
        {
            InitializeComponent();
            authController = new SistemadeClinica.Controllers.AuthControllers();
            
            
        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }


        
        
        private void TxtUsuario_Enter(object sender, EventArgs e)
        {
            
        }

        
        private void TxtContrasena_Enter(object sender, EventArgs e)
        {
            
        }

        
        private void TxtUsuario_Leave(object sender, EventArgs e)
        {
            
        }

        
        private void TxtContrasena_Leave(object sender, EventArgs e)
        {
            
        }

        

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            try
            {

            
                if (authController == null)
                {
                MessageBox.Show("Error: Controlador de autentificación no iniciado", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
                }
                if (string.IsNullOrWhiteSpace(TxtUsuario.Text) || string.IsNullOrWhiteSpace(TxtContrasena.Text))
                {
                    MessageBox.Show("Por favor, Complete todos los campos.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                if (authController.Autenticar(TxtUsuario.Text, TxtContrasena.Text))
                {
                    MessageBox.Show($"Bienvenido {SistemadeClinica.Models.Sesion.NombreCompleto} ({SistemadeClinica.Models.Sesion.RolNombre})",
                        "Login Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SistemadeClinica.Views.FormDashboard dashboard = new FormDashboard();
                    dashboard.Show();
                    this.Hide();
              
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error de login",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error durante el login: {ex.Message}\n\nDealles: {ex.InnerException?.Message}", "Error");
            }


        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TxtUsuario_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
