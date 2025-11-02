using SistemadeClinica.Views;
using SistemadeClinica.Controllers;
using SistemadeClinica.Models;
using SistemadeClinica.Helpers;
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
    public partial class FormDashboard : MaterialForm
    {
        private int usuarioSelecionado = 0; //Variable Id del usuario
        private UsuarioControllers usuarioController;
        private DataTable dtUsuarios;
        private DataTable dtRoles;
        public FormDashboard()
        {
            InitializeComponent();
            usuarioController = new UsuarioControllers();
            CargarRoles();
            CargarUsuarios();
            CargarSexo();
            ConfigurarModoInicial();
        }


        /*
         tabPage2 = MaterialControl.TabPages["tabPage2"];
            MaterialControl.TabPages.Remove(tabPage2);
         */

        private void Inicio_Load(object sender, EventArgs e)
        {
            
        }


        private void ConfigurarModoInicial()
        {
            BtnActualizar.Enabled = false;
            BtnEliminar.Enabled = false;
        }

        //Metodo para agregar los sexos al combo box
        private void CargarSexo()
        {
            ComboBoxSexo.Items.Clear();

            ComboBoxSexo.Items.Add("Masculino");
            ComboBoxSexo.Items.Add("Femenino");

            ComboBoxSexo.SelectedIndex = 0;

            ComboBoxSexo.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void CargarRoles()
        {
            try
            {
                RolController rolController = new RolController();
                dtRoles = rolController.ObtenerRolesParaComboBox();

                ComboBoxRol.DataSource = dtRoles;
                ComboBoxRol.DisplayMember = "Nombre";
                ComboBoxRol.ValueMember = "Id";

                if (ComboBoxRol.Items.Count > 0)
                {
                    ComboBoxRol.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar roles: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarUsuarios()
        {
            try
            {
                dtUsuarios = usuarioController.ObtenerTodosUsuarios();

                if (dtUsuarios != null && dtUsuarios.Rows.Count > 0)
                {
                    DgvUsuarios.DataSource = dtUsuarios;
                    ConfigurarDataGriwView();
                }
                else
                {
                    DgvUsuarios.DataSource = null;
                    MessageBox.Show("No se encontraron usuarios registrados.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios:  {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGriwView()
        {
            try
            {
                DgvUsuarios.SuspendLayout();

                DgvUsuarios.AutoGenerateColumns = false;
                DgvUsuarios.Columns.Clear();

                DgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Usuario",
                    HeaderText = "Usuario",
                    DataPropertyName = "Usuario",
                    ReadOnly = true
                });

                DgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Rol",
                    HeaderText = "Rol",
                    DataPropertyName = "Rol",
                    ReadOnly = true
                });

                DgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Nombre",
                    HeaderText = "Nombre",
                    DataPropertyName = "Nombre",
                    ReadOnly = true
                });

                DgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Apellido",
                    HeaderText = "Apellido",
                    DataPropertyName = "Apellido",
                    ReadOnly = true
                });

                DgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Sexo",
                    HeaderText = "Sexo",
                    DataPropertyName = "Sexo",
                    ReadOnly = true
                });

                DgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Telefono",
                    HeaderText = "Telefono",
                    DataPropertyName = "Telefono",
                    ReadOnly = true
                });

                DgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Correo",
                    HeaderText = "Correo Electrónico",
                    DataPropertyName = "Correo",
                    ReadOnly = true
                });

                DgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Id",
                    HeaderText = "Id",
                    DataPropertyName = "Id",
                    ReadOnly = true,
                    Visible = false
                });

                //Propiedades visuales
                DgvUsuarios.ReadOnly = true;
                DgvUsuarios.AllowUserToAddRows = false;
                DgvUsuarios.AllowUserToDeleteRows = false;
                DgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DgvUsuarios.MultiSelect = false;
                DgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                DgvUsuarios.BackgroundColor = Color.White;
                DgvUsuarios.BorderStyle = BorderStyle.None;
                DgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
                DgvUsuarios.RowHeadersVisible = false;
                DgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                DgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                DgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                DgvUsuarios.EnableHeadersVisualStyles = false;


            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error  al configurar DataGridView: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DgvUsuarios.ResumeLayout();
            }

        }










        private void BtnInsertar_Click(object sender, EventArgs e)
        {

        }




























        private void tabPage2_Click(object sender, EventArgs e)
        {
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void materialButton11_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void materialButton12_Click(object sender, EventArgs e)
        {
            


        }

        private void PanelMedio_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        
    }
}
