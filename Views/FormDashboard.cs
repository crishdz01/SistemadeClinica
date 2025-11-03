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
            PersonalizarFormulario();
            CargarRoles();
            CargarUsuarios();
            CargarSexo();
            ConfigurarModoInicial();
        }



        /*
          ************************************************************************************************************************************
          ---------------------------------------PESTAÑA GESTION DE USUARIOS------------------------------------------------------------------
          ************************************************************************************************************************************
         */
        
        //Metodo para colorear los texbox de la pestaña gestion de usuarios
        private void PersonalizarFormulario()
        {
            TxtUsuarioGestion.Enter += (s, e) => { TxtUsuarioGestion.BackColor = Color.LightCyan; };
            TxtUsuarioGestion.Leave += (s, e) => { TxtUsuarioGestion.BackColor = Color.White; };

            TxtPaswordGestion.Enter += (s, e) => { TxtPaswordGestion.BackColor = Color.LightCyan; };
            TxtPaswordGestion.Leave += (s, e) => { TxtPaswordGestion.BackColor = Color.White; };

            TxtNombreGestion.Enter += (s, e) => { TxtNombreGestion.BackColor = Color.LightCyan; };
            TxtNombreGestion.Leave += (s, e) => { TxtNombreGestion.BackColor = Color.White; };

            TxtApellidoGestion.Enter += (s, e) => { TxtApellidoGestion.BackColor = Color.LightCyan; };
            TxtApellidoGestion.Leave += (s, e) => { TxtApellidoGestion.BackColor = Color.White; };

            TxtTelefonoGestion.Enter += (s, e) => { TxtTelefonoGestion.BackColor = Color.LightCyan; };
            TxtTelefonoGestion.Leave += (s, e) => { TxtTelefonoGestion.BackColor = Color.White; };

            TxtCorreoGestion.Enter += (s, e) => { TxtCorreoGestion.BackColor = Color.LightCyan; };
            TxtCorreoGestion.Leave += (s, e) => { TxtCorreoGestion.BackColor = Color.White; };
        }
        

        private void Inicio_Load(object sender, EventArgs e)
        {
            
        }


        //Metodo para configurar la pestaña gestion de usuarios, que no aparezca el boton actualizar ni el eliminar porque no ha nada auin
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

        //Metodo para obtener los roles del combo box
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

        //Metodo para cargar los usuarios del DataView
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

        //Metodo para configurar la vista del DataView
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

                //Propiedades visuales del DataView
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

        //Metodo y Evento para colocar los datos de una celda a los texboxs
        private void DgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                DataGridViewRow fila = DgvUsuarios.Rows[e.RowIndex];

                if (fila.Cells["Id"].Value != null)
                {
                    usuarioSelecionado = Convert.ToInt32(fila.Cells["Id"].Value);
                }

                TxtUsuarioGestion.Text = fila.Cells["Usuario"].Value?.ToString() ?? "";
                TxtNombreGestion.Text = fila.Cells["Nombre"].Value?.ToString() ?? "";
                TxtApellidoGestion.Text = fila.Cells["Apellido"].Value?.ToString() ?? "";
                TxtCorreoGestion.Text = fila.Cells["Correo"].Value?.ToString() ?? "";
                TxtTelefonoGestion.Text = fila.Cells["Telefono"].Value?.ToString() ?? "";

                string sexo = fila.Cells["Sexo"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(sexo))
                {
                    ComboBoxSexo.Text = sexo;
                }

                string nombreRol = fila.Cells["Rol"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(nombreRol))
                {
                    foreach (DataRowView item in ComboBoxRol.Items)
                    {
                        if (item["Nombre"].ToString() == nombreRol)
                        {
                            ComboBoxRol.SelectedValue = item["Id"];
                            break;
                        }
                    }
                }

                TxtPaswordGestion.Text = "********";
                TxtPaswordGestion.Tag = "edicion";

                CambiarColorFormularioEdicion();
                BtnActualizar.Enabled = true;
                BtnEliminar.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Metodo para cambiar el color cuando se edita
        private void CambiarColorFormularioEdicion()
        {
            Color colorEdicion = Color.LightYellow;
            TxtUsuarioGestion.BackColor = colorEdicion;
            TxtNombreGestion.BackColor = colorEdicion;
            TxtApellidoGestion.BackColor = colorEdicion;

        }

        //Metodo para cambiar el color cuando no se edita
        private void RestaurarColorFormulario()
        {
            Color colorNormal = Color.White;
            TxtUsuarioGestion.BackColor = colorNormal;
            TxtNombreGestion.BackColor = colorNormal;
            TxtApellidoGestion.BackColor = colorNormal;

        }

        //Metodo y Evento para colocar los datos de una celda a los texboxs este sirve para volverlos a cargar
        private void DgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (DgvUsuarios.SelectedRows.Count > 0)
            {
                DataGridViewRow row = DgvUsuarios.SelectedRows[0];
            }
        }

        //Metodo y boton para agregar usuarios
        private void BtnInsertar_Click(object sender, EventArgs e)
        {
            try
            {
                if (usuarioSelecionado > 0)
                {
                    MessageBox.Show("Esta en modo edicion, Para crear un nuevo usuario, " +
                        "Primero llene el formulario con los datos correctos.", "Modo edicion",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    return;
                }

                //Validaciones Simples

                if (!Validaciones.ValidarNoVacio(TxtNombreGestion, "Nombre")) return;
                if (!Validaciones.ValidarSoloLetras(TxtNombreGestion.Text, "Nombre")) return;

                if (!Validaciones.ValidarNoVacio(TxtApellidoGestion, "Apellido")) return;
                if (!Validaciones.ValidarSoloLetras(TxtApellidoGestion.Text, "Apellido")) return;

                if (!Validaciones.ValidarNoVacio(TxtTelefonoGestion, "Telefono")) return;
                if (!Validaciones.ValidarSoloNumeros(TxtTelefonoGestion.Text, "Apellido")) return;

                if (!Validaciones.ValidarNoVacio(TxtCorreoGestion, "Correo electronico")) return;
                if (!Validaciones.ValidarEmail(TxtCorreoGestion.Text, "Correo electronico")) return;

                //Validacion del combo box sexo
                if (!Validaciones.ValidarComboBox2(ComboBoxSexo, "Sexo")) return;

                //Validacion de pasword solo usuarios nuevos
                if (!Validaciones.ValidarNoVacio(TxtUsuarioGestion, "Usuario")) return;

                //Validaciones especificas
                if (!Validaciones.ValidarNoVacio(TxtPaswordGestion, "Contraseña")) return;
                if (!Validaciones.ValidarFortalezaPassword(TxtPaswordGestion.Text)) return;

                if (!Validaciones.ValidarComboBox(ComboBoxRol, "Rol")) return;

                Usuarios usuarios = new Usuarios
                {
                    Username = TxtUsuarioGestion.Text,
                    PasswordHash = AuthControllers.GenerarHashSHA256(TxtPaswordGestion.Text),
                    IdRol = Convert.ToInt32(ComboBoxRol.SelectedValue),
                    Nombre = TxtNombreGestion.Text,
                    Apellido = TxtApellidoGestion.Text,
                    Sexo = ComboBoxSexo.SelectedItem.ToString(),
                    Telefono = TxtTelefonoGestion.Text,
                    Correo = TxtCorreoGestion.Text
                };

                bool resultado = usuarioController.CrearUsuario(usuarios);

                if (resultado)
                {
                    MessageBox.Show("Usuario creado exitosamente.", "Exito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    CargarUsuarios();
                }
            }catch(Exception ex)
            {
                MessageBox.Show($"Error al guardar usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Metodo uilizado en agregar para verificar que se repita usuarios
        private bool ValidarUsuarioUnico (string usuarios, int idExcluir = 0)
        {
            try
            {
                string condicion = $"Usuario = '{usuarios.Replace("'", "''")}' AND Activo = 1";
                
                if (idExcluir > 0)
                {
                    condicion += $"AND Id != {idExcluir}";
                }

                DataTable usuariosExistentes = usuarioController.ObtenerUsuariosPorCondicion(condicion);

                if (usuariosExistentes.Rows.Count > 0)
                {
                    MessageBox.Show("Ya existe un usuario con ese nombre de usuario. Por favor elija otro.",
                        "Usuario duplicado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    TxtUsuarioGestion.Focus();
                    TxtUsuarioGestion.SelectAll();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al validar usuario unico: {ex.Message}", "Error");
                    return false;
            }

        }

        //Metodo y boton para actualizar usuarios
        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (usuarioSelecionado == 0)
                {
                    MessageBox.Show("Debe seleccionar un usuario de la lista para actualizar.",
                        "Seleccion Requerida",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                //Validaciones Simples

                if (!Validaciones.ValidarNoVacio(TxtNombreGestion, "Nombre")) return;
                if (!Validaciones.ValidarSoloLetras(TxtNombreGestion.Text, "Nombre")) return;

                if (!Validaciones.ValidarNoVacio(TxtApellidoGestion, "Apellido")) return;
                if (!Validaciones.ValidarSoloLetras(TxtApellidoGestion.Text, "Apellido")) return;

                if (!Validaciones.ValidarNoVacio(TxtTelefonoGestion, "Telefono")) return;
                if (!Validaciones.ValidarSoloNumeros(TxtTelefonoGestion.Text, "Apellido")) return;

                if (!Validaciones.ValidarNoVacio(TxtCorreoGestion, "Correo electronico")) return;
                if (!Validaciones.ValidarEmail(TxtCorreoGestion.Text, "Correo electronico")) return;

                //Validaciones Combo box
                if (!Validaciones.ValidarComboBox2(ComboBoxSexo, "Sexo")) return;
                if (!Validaciones.ValidarComboBox(ComboBoxRol, "Rol")) return;

                //Validaciones Especificas
                if (!Validaciones.ValidarNoVacio(TxtUsuarioGestion, "Usuario")) return;

                //Validacion de usuario Unico
                if (!ValidarUsuarioUnico(TxtUsuarioGestion.Text, usuarioSelecionado)) return;

                //Validacion de contraseña
                string nuevoPasswordHash = null;
                if (!string.IsNullOrEmpty(TxtPaswordGestion.Text) && TxtPaswordGestion.Text != "********")
                {
                    if (!Validaciones.ValidarFortalezaPassword(TxtPaswordGestion.Text)) return;
                    nuevoPasswordHash = AuthControllers.GenerarHashSHA256(TxtPaswordGestion.Text);
                }

                var confirmacion = MessageBox.Show(
                    $"¿Esta seguro de actualizar los datos del usuario: {TxtNombreGestion.Text} {TxtApellidoGestion.Text}?",
                    "Confirmar Actualización",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes) return;

                Usuarios usuarios = new Usuarios
                {
                    Id = usuarioSelecionado,
                    Username = TxtUsuarioGestion.Text,
                    PasswordHash = nuevoPasswordHash,
                    IdRol = Convert.ToInt32(ComboBoxRol.SelectedValue),
                    Nombre = TxtNombreGestion.Text,
                    Apellido = TxtApellidoGestion.Text,
                    Sexo = ComboBoxSexo.SelectedItem.ToString(),
                    Telefono = TxtTelefonoGestion.Text,
                    Correo = TxtCorreoGestion.Text
                };

                bool resultado = usuarioController.ActualizarUsuario(usuarios);

                if (resultado)
                {
                    MessageBox.Show("Usuario actualizado exitosamente.", "Exito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el usuario, Verifique los datos.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }catch (Exception ex)
            {
                MessageBox.Show($"No se pudo actualizar el usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void LimpiarFormulario()
        {
            usuarioSelecionado = 0;

            TxtUsuarioGestion.Clear();
            TxtNombreGestion.Clear();
            TxtApellidoGestion.Clear();
            TxtCorreoGestion.Clear();
            TxtTelefonoGestion.Clear();
            TxtPaswordGestion.Clear();
            TxtPaswordGestion.Tag = null;
            ComboBoxSexo.SelectedIndex = 0;
            ComboBoxRol.SelectedIndex = 0;

            RestaurarColorFormulario();

            BtnActualizar.Enabled = false;
            BtnEliminar.Enabled = false;
        }

        //Boton para limpiar el formulario
        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        //Boton eliminar en este caso esconder un registro
        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (usuarioSelecionado == 0)
                {
                    MessageBox.Show("Seleccione un usuario para eliminar.");
                    return;
                }

                var confirmacion = MessageBox.Show(
                    $"¿Eliminar al usuario {TxtNombreGestion.Text} {TxtApellidoGestion.Text}?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes) return;

                bool resultado = usuarioController.EliminarUsuario(usuarioSelecionado);

                if (resultado)
                {
                    MessageBox.Show("Usuario Eliminado Exitosamente.");
                    LimpiarFormulario();
                    CargarUsuarios();
                }
                
            }catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

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

        private void materialButton1_Click(object sender, EventArgs e)
        {
            tabPage2 = MaterialControl.TabPages["tabPage2"];
            MaterialControl.TabPages.Remove(tabPage2);
        }

        
    }
}
