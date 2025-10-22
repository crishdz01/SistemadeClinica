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
        public FormLogin()
        {
            InitializeComponent();
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

        private void TxtUsuario_Leave(object sender, EventArgs e)
        {
            if(TxtUsuario.Text == "")
            {
                TxtUsuario.Text = "Usuario";
            }
        }

        private void TxtContrasena_Leave(object sender, EventArgs e)
        {
            if(TxtContrasena.Text == "")
            {
                TxtContrasena.Text = "Contraseña";
            }
        }
    }
}
