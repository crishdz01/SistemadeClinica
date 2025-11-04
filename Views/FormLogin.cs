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



        //Metodo para que al hacer click en usuario, el texto usuario desaparezca 
        private void TxtUsuario_Enter(object sender, EventArgs e)
        {
            if(TxtUsuario.Text == "Usuario")
            {
                TxtUsuario.Text = "";
            }
        }

        //Metodo para que al hacer click en Contraseña, el texto contraseña desaparezca 
        private void TxtContrasena_Enter(object sender, EventArgs e)
        {
            if (TxtContrasena.Text == "Contraseña")
            {
                TxtContrasena.Text = "";
            }
        }

        //Metodo para que usuario vuelva escribirse
        private void TxtUsuario_Leave(object sender, EventArgs e)
        {
            if(TxtUsuario.Text == "")
            {
                TxtUsuario.Text = "Usuario";
            }
        }

        //Metodo para que contraseña vuelva escribirse
        private void TxtContrasena_Leave(object sender, EventArgs e)
        {
            if(TxtContrasena.Text == "")
            {
                TxtContrasena.Text = "Contraseña";
            }
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

       

        

    }
}
