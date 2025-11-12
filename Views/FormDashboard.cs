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
using Microsoft.Reporting.WinForms;
using MaterialSkin;
using MaterialSkin.Controls;
using ClosedXML.Excel;

namespace SistemadeClinica.Views
{
    public partial class FormDashboard : MaterialForm
    {
        //Metodos para usuarios
        private int usuarioSelecionado = 0; //Variable Id del usuario
        private UsuarioControllers usuarioController;
        private DataTable dtUsuarios;
        private DataTable dtRoles;

        //Metodos para producto
        private int medicamentoSeleccionado = 0;
        private MedicamentosController medicamentosController;
        private DataTable dtMedicamentos;

        //Metodos para citas
        private int citaSeleccionada = 0;
        private CitaController citaController;
        private DataTable dtCitas;

        //Metodos para pacientes
        private int pacienteSeleccionado = 0;
        private PacientesController pacientesController;
        private DataTable dtPacientes;

        //Metodos para expediente
        private int expedienteSeleccionado = 0;
        private ExpedienteController expedienteController;
        private DataTable dtExpediente;

        public FormDashboard()
        {
            
            
            InitializeComponent();

            medicamentosController = new MedicamentosController();
            usuarioController = new UsuarioControllers();
            citaController = new CitaController();
            pacientesController = new PacientesController();
            expedienteController = new ExpedienteController();

            //Usuarios
            PersonalizarFormulario();
            CargarRoles();
            CargarUsuarios();
            CargarSexo();
            ConfigurarModoInicial();
            ConfigurarBotonesSegunRol();

            //Medicamentos
            ConfigurarModoInicial2();
            CargarMedicamentos();

            //Citas
            ConfigurarModoInicial3();
            ConfigurarModoInicial4();
            CargarCitas();
            CargarHora();
            CargarPacientes2();

            //Pacientes
            CargarPacientes();
            //Sondiferentes
            CargarParientes();


            //Expediente
            CargarPacientes3();
            ConfigurarModoInicial5();

            //Historial
            CargarPacientes4();
            CargarExpedientes();

        }

        private void Inicio_Load(object sender, EventArgs e)
        {

            
        }


        //Codigo que ocula gestion de usuarios si no es admin
        private void ConfigurarBotonesSegunRol()
        {
            try
            {
                // Verificar si hay sesión activa
                if (Sesion.RolId <= 0)
                {
                    MessageBox.Show("No se detectó el rol del usuario. Revise el inicio de sesión.");
                    return;
                }

                // Ocultar pestaña según rol
                if (Sesion.RolId != 1) 
                {
                    if (MaterialControl.TabPages.ContainsKey("GestionUsu"))
                    {
                        MaterialControl.TabPages.RemoveByKey("GestionUsu");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al configurar pestañas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /*
          ************************************************************************************************************************************
          ---------------------------------------PESTAÑA GESTION DE USUARIOS------------------------------------------------------------------
          ************************************************************************************************************************************
         */
        

        //Metodo para colorear los texbox de la pestaña gestion de usuarios
        private void PersonalizarFormulario()
        {
            //Usuarios
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

            //Medicamentos
            XtxNombreMedi.Enter += (s, e) => { XtxNombreMedi.BackColor = Color.LightCyan; };
            XtxNombreMedi.Leave += (s, e) => { XtxNombreMedi.BackColor = Color.White; };

            XtxPrecioMedi.Enter += (s, e) => { XtxPrecioMedi.BackColor = Color.LightCyan; };
            XtxPrecioMedi.Leave += (s, e) => { XtxPrecioMedi.BackColor = Color.White; };

            XtxDescripMedi.Enter += (s, e) => { XtxDescripMedi.BackColor = Color.LightCyan; };
            XtxDescripMedi.Leave += (s, e) => { XtxDescripMedi.BackColor = Color.White; };

            XtxStockMedi.Enter += (s, e) => { XtxStockMedi.BackColor = Color.LightCyan; };
            XtxStockMedi.Leave += (s, e) => { XtxStockMedi.BackColor = Color.White; };
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
        private void BtnInsertar_Click_1(object sender, EventArgs e)
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
            }
            catch (Exception ex)
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
                    condicion += $" AND Id != {idExcluir}";
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
        private void BtnActualizar_Click_1(object sender, EventArgs e)
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
                    CargarUsuarios();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el usuario, Verifique los datos.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo actualizar el usuario: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
   
        //Metodo limpiar formulario
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
        private void BtnLimpiar_Click_1(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }


        //Boton eliminar en este caso esconder un registro
        private void BtnEliminar_Click_1(object sender, EventArgs e)
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

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }


        //Boton buscar un registro
        private void BtnBuscar_Click_1(object sender, EventArgs e)
        {
            try
            {
                string filtro = TxtUsuarioGestion.Text.Trim();

                if (!Validaciones.ValidarNoVacio(TxtUsuarioGestion, "Usuario a buscar"))
                    return;

                DataTable dt = usuarioController.BuscarUsuariosPorNombreUsuario(filtro);
                LimpiarFormulario();

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show($"No se encontro el usuario '{filtro}'.",
                        "Sin resultados",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DgvUsuarios.DataSource = null;
                    return;
                }

                if (!dt.Columns.Contains("Rol") && dt.Columns.Contains("IdRol"))
                {
                    dt.Columns.Add("Rol", typeof(string));

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["IdRol"] == DBNull.Value)
                        {
                            row["IdRol"] = string.Empty;
                            continue;

                        }

                        int idRol;
                        if (int.TryParse(row["IdRol"].ToString(), out idRol))
                        {
                            string nombreRol = ObtenerNombreRolDesdeCombo(idRol);
                            if (!string.IsNullOrEmpty(nombreRol))
                            {
                                row["Rol"] = nombreRol;
                            }
                            else
                            {
                                row["Rol"] = idRol.ToString();
                            }
                        }
                    }
                }

                if (!dt.Columns.Contains("Correo") && dt.Columns.Contains("CorreoElectronico"))
                {
                    dt.Columns.Add("Correo", typeof(string));
                    foreach (DataRow row in dt.Rows)
                        row["Correo"] = row["CorreoElectronico"];
                }

                DgvUsuarios.DataSource = dt;

                MessageBox.Show($"Se encontraron {dt.Rows.Count} usuario(s).",
                    "Busqueda exitosa",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la busqueda: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        //Metodo que busca el ROl en el combo box Rol, devuelve null si no lo encuentra
        private string ObtenerNombreRolDesdeCombo (int idRol)
        {
            try
            {
                if (ComboBoxRol.DataSource == null) return null;

                if (ComboBoxRol.DataSource is DataTable dtRoles)
                {
                    DataRow[] rows = dtRoles.Select($"Id = {idRol}");
                    if (rows.Length > 0) return rows[0]["Nombre"].ToString();
                }

                if (ComboBoxRol.DataSource is BindingSource bs)
                {
                    if (bs.DataSource is DataTable dt2)
                    {
                        DataRow[] rows = dt2.Select($"Id = {idRol}");
                        if (rows.Length > 0) return rows[0]["Nombre"].ToString();
                    }

                    else if (bs.DataSource is IEnumerable<object> listObj)
                    {
                        foreach (var item in listObj)
                        {
                            var propId = item.GetType().GetProperty("Id");
                            var propNombre = item.GetType().GetProperty("Nombre");
                            if (propId != null && propNombre != null)
                            {
                                var val = propId.GetValue(item);
                                if (val != null && Convert.ToInt32(val) == idRol)
                                    return propNombre.GetValue(item)?.ToString();
                            }
                        }
                    }
                }

                if (ComboBoxRol.Items.Count > 0)
                {
                    foreach (var it in ComboBoxRol.Items)
                    {
                        var propId = it.GetType().GetProperty("Id");
                        var propNombre = it.GetType().GetProperty("Nombre");
                        if (propId != null && propNombre != null)
                        {
                            var val = propId.GetValue(it);
                            if (val != null && Convert.ToInt32(val) == idRol)
                                return propNombre.GetValue(it)?.ToString();
                        }
                    }
                }

                return null;
            }catch
            {
                return null;
            }
        }

        //Boton buscar todos los registros
        private void BtnVerTodos_Click_1(object sender, EventArgs e)
        {

            try
            {
                TxtUsuarioGestion.Clear();

                CargarUsuarios();
                LimpiarFormulario();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar usuarios: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        

        /*
          **********************************************************************************************************************************************************************
          ---------------------------------------FIN DE PESTAÑA GESTION DE USUARIOS---------------------------------------------------------------------------------------------
          **********************************************************************************************************************************************************************
         */



        /*
          **********************************************************************************************************************************************************************
          ---------------------------------------PESTAÑA MEDICAMENTOS---------------------------------------------------------------------------------------------
          **********************************************************************************************************************************************************************
         */

        //Ocultar botones indispensables
        private void ConfigurarModoInicial2()
        {
            MaterialBtnActualizarMedi.Enabled = false;
            MaterialBtnEliminarMedi.Enabled = false;
        }

        //Cargar medicamentos en datagriwview
        private void CargarMedicamentos()
        {
            try
            {
                dtMedicamentos = medicamentosController.ObtenerTodosMedicamentos();

                if (dtMedicamentos != null && dtMedicamentos.Rows.Count > 0)
                {
                    
                    DgvMedicamentos.DataSource = dtMedicamentos;
                    ConfigurarDataGridView2();
                }
                else
                {
                    DgvMedicamentos.DataSource = null;
                    MessageBox.Show("No se encontraron medicamentos registrados.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar medicamentos:  {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        //Configuracion del datagriwview
        private void ConfigurarDataGridView2()
        {
            try
            {
                DgvMedicamentos.SuspendLayout();

                DgvMedicamentos.AutoGenerateColumns = false;
                DgvMedicamentos.Columns.Clear();

                DgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Nombre",
                    HeaderText = "Nombre del medicamento",
                    DataPropertyName = "Nombre",
                    ReadOnly = true
                });

                DgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Precio",
                    HeaderText = "Precio",
                    DataPropertyName = "Precio",
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "C2",//DataGridContentAlignment
                        Alignment = DataGridViewContentAlignment.MiddleRight
                        
                    }
                });

                DgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Stock",
                    HeaderText = "Stock",
                    DataPropertyName = "Stock",
                    ReadOnly = true,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Alignment = DataGridViewContentAlignment.MiddleRight

                    }
                });

                //Columna ids no visible
                DgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Id",
                    HeaderText = "Id",
                    DataPropertyName = "Id",
                    ReadOnly = true,
                    Visible = false
                });

                DgvMedicamentos.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Descripcion",
                    HeaderText = "Descripcion",
                    DataPropertyName = "Descripcion",
                    ReadOnly = true
                });

                

                //Propiedades visuales del DataView
                DgvMedicamentos.ReadOnly = true;
                DgvMedicamentos.AllowUserToAddRows = false;
                DgvMedicamentos.AllowUserToDeleteRows = false;

                DgvMedicamentos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DgvMedicamentos.MultiSelect = false;

                DgvMedicamentos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                DgvMedicamentos.BackgroundColor = Color.White;
                DgvMedicamentos.BorderStyle = BorderStyle.None;
                DgvMedicamentos.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
                DgvMedicamentos.RowHeadersVisible = false;
                DgvMedicamentos.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                DgvMedicamentos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                DgvMedicamentos.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                DgvMedicamentos.EnableHeadersVisualStyles = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error  al configurar DataGridView2: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DgvMedicamentos.ResumeLayout();
            }

        }

        //Evento para cambiar las views al hacer clic
        private void DgvMedicamentos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                DataGridViewRow fila = DgvMedicamentos.Rows[e.RowIndex];

                if (fila.Cells["Id"].Value != null)
                {
                    medicamentoSeleccionado = Convert.ToInt32(fila.Cells["Id"].Value);
                }

                XtxNombreMedi.Text = fila.Cells["Nombre"].Value?.ToString() ?? "";
                XtxPrecioMedi.Text = fila.Cells["Precio"].Value?.ToString() ?? "";
                XtxStockMedi.Text = fila.Cells["Stock"].Value?.ToString() ?? "";
                XtxDescripMedi.Text = fila.Cells["Descripcion"].Value?.ToString() ?? "";


                CambiarColorFormularioEdicion2();
                MaterialBtnActualizarMedi.Enabled = true;
                MaterialBtnEliminarMedi.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del medicamento: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Metodo para cambiar el color cuando se edita
        private void CambiarColorFormularioEdicion2()
        {
            Color colorEdicion = Color.LightYellow;
            XtxNombreMedi.BackColor = colorEdicion;
            XtxPrecioMedi.BackColor = colorEdicion;
            XtxStockMedi.BackColor = colorEdicion;
            XtxDescripMedi.BackColor = colorEdicion;
        }

        //Metodo para cambiar el color cuando no se edita
        private void RestaurarColorFormulario2()
        {
            Color colorNormal = Color.White;
            XtxNombreMedi.BackColor = colorNormal;
            XtxPrecioMedi.BackColor = colorNormal;
            XtxStockMedi.BackColor = colorNormal;
            XtxDescripMedi.BackColor = colorNormal;

        }

        //Boton agregar
        private void MaterialBtnInsertarMedi_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (medicamentoSeleccionado > 0)
                {
                    MessageBox.Show("Esta en modo edicion, Para crear un nuevo medicamento, " +
                        "Primero llene el formulario con los datos correctos.", "Modo edicion",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario2();
                    return;
                }

                //Validaciones Simples

                if (!Validaciones.ValidarNoVacio(XtxNombreMedi, "Nombre del medicamento")) return;


                if (!Validaciones.ValidarNoVacio(XtxPrecioMedi, "Precio")) return;
                if (!Validaciones.ValidarNumeroDecimal(XtxPrecioMedi.Text, "Precio")) return;

                if (!Validaciones.ValidarNoVacio(XtxStockMedi, "Stock")) return;
                if (!Validaciones.ValidarSoloNumeros(XtxStockMedi.Text, "Stock")) return;

                if (!Validaciones.ValidarNoVacio(XtxDescripMedi, "Descripcion")) return;


                double precio = Convert.ToDouble(XtxPrecioMedi.Text);
                if (precio <= 0)
                {
                    MessageBox.Show("El precio debe ser mayor que 0", "Error en la validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    XtxPrecioMedi.Focus();
                    return;
                }

                int stock = Convert.ToInt32(XtxStockMedi.Text);

                if (stock < 0)
                {
                    MessageBox.Show("El stock no puede ser negativo",
                        "Error de validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    XtxStockMedi.Focus();
                    return;
                }

                if (!ValidarMedicamentoUnico(XtxNombreMedi.Text)) return;

                Medicamentos medicamento = new Medicamentos
                {
                    Nombre = XtxNombreMedi.Text,
                    Precio = precio,
                    Stock = stock,
                    Descripcion = XtxDescripMedi.Text

                };

                bool resultado = medicamentosController.CrearMedicamento(medicamento);

                if (resultado)
                {
                    MessageBox.Show("Medicamento creado exitosamente.", "Exito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario2();
                    CargarMedicamentos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar Medicamento: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        //Limpiar formulario medicamentos
        private void LimpiarFormulario2()
        {
            medicamentoSeleccionado = 0;

            XtxNombreMedi.Clear();
            XtxPrecioMedi.Clear();
            XtxStockMedi.Clear();
            XtxDescripMedi.Clear();
            

            RestaurarColorFormulario2();

            MaterialBtnActualizarMedi.Enabled = false;
            MaterialBtnEliminarMedi.Enabled = false;
        }

        //Validaciones
        private bool ValidarMedicamentoUnico (string nombre, int idExcluir = 0)
        {

            try
            {
                string condicion = $"Nombre = '{nombre.Replace("'", "''")}' AND Activo = 1";

                if (idExcluir > 0)
                {
                    condicion += $" AND Id != {idExcluir}";
                }

                DataTable medicamentosExistentes = medicamentosController.ObtenerMedicamentoPorCondicion(condicion);

                if (medicamentosExistentes.Rows.Count > 0)
                {
                    MessageBox.Show("Ya existe un medicamento con ese nombre. Por favor elija otro.",
                        "Usuario duplicado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    XtxNombreMedi.Focus();
                    XtxNombreMedi.SelectAll();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al validar medicamento unico: {ex.Message}", "Error");
                return false;
            }
        }

        //Boton Actualizar
        private void MaterialBtnActualizarMedi_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (medicamentoSeleccionado == 0)
                {
                    MessageBox.Show("Debe seleccionar un medicamento de la lista para actualizar.",
                        "Seleccion Requerida",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                //Validaciones Simples


                if (!Validaciones.ValidarNoVacio(XtxNombreMedi, "Nombre del medicamento")) return;


                if (!Validaciones.ValidarNoVacio(XtxPrecioMedi, "Precio")) return;
                if (!Validaciones.ValidarNumeroDecimal(XtxPrecioMedi.Text, "Precio")) return;


                if (!Validaciones.ValidarNoVacio(XtxStockMedi, "Stock")) return;
                if (!Validaciones.ValidarSoloNumeros(XtxStockMedi.Text, "Stock")) return;

                if (!Validaciones.ValidarNoVacio(XtxDescripMedi, "Descripcion")) return;



                double precio = Convert.ToDouble(XtxPrecioMedi.Text);

                if (precio <= 0)
                {
                    MessageBox.Show("El precio debe ser mayor que 0", "Error en la validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    XtxPrecioMedi.Focus();
                    return;
                }

                int stock = Convert.ToInt32(XtxStockMedi.Text);

                if (stock < 0)
                {
                    MessageBox.Show("El stock no puede ser negativo",
                        "Error de validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    XtxStockMedi.Focus();
                    return;
                }

                if (!ValidarMedicamentoUnico(XtxNombreMedi.Text, medicamentoSeleccionado)) return;


                var confirmacion = MessageBox.Show(
                    $"¿Esta seguro de actualizar los datos del medicamento: {XtxNombreMedi.Text}?",
                    "Confirmar Actualización",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);



                if (confirmacion != DialogResult.Yes) return;

                Medicamentos medicamento = new Medicamentos
                {
                    Id = medicamentoSeleccionado,
                    Nombre = XtxNombreMedi.Text,
                    Precio = precio,
                    Stock = stock,
                    Descripcion = XtxDescripMedi.Text

                };

                bool resultado = medicamentosController.ActualizarMedicamento(medicamento);

                if (resultado)
                {
                    MessageBox.Show("Medicamento actualizado exitosamente.", "Exito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario2();
                    CargarMedicamentos();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el medicamento, Verifique los datos.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo actualizar el medicamento: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //BotonEliminar
        private void MaterialBtnEliminarMedi_Click_1(object sender, EventArgs e)
        {

            try
            {
                if (medicamentoSeleccionado == 0)
                {
                    MessageBox.Show("Seleccione un medicamento para eliminar.");
                    return;
                }

                var confirmacion = MessageBox.Show(
                    $"¿Eliminar el medicamento? {XtxNombreMedi.Text}?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes) return;

                bool resultado = medicamentosController.EliminarMedicamento(medicamentoSeleccionado);

                if (resultado)
                {
                    MessageBox.Show("Usuario medicamento Exitosamente.");
                    LimpiarFormulario2();
                    CargarMedicamentos();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        //Boton buscar
        private void MaterialBtnBuscarMedi_Click_1(object sender, EventArgs e)
        {
            try
            {
                string filtro = XtxNombreMedi.Text.Trim();

                if (!Validaciones.ValidarNoVacio(XtxNombreMedi, "Medicamento a buscar"))
                    return;

                DataTable dt = medicamentosController.BuscarMedicamentoPorNombreUsuario(filtro);
                LimpiarFormulario2();

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show($"No se encontro el medicamento '{filtro}'.",
                        "Sin resultados",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DgvMedicamentos.DataSource = null;
                    LimpiarFormulario2();
                    return;
                }

                DgvMedicamentos.DataSource = dt;
                LimpiarFormulario2();

                MessageBox.Show($"Se encontraron {dt.Rows.Count} medicamento(s).",
                    "Busqueda exitosa",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la busqueda: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Boton ver todos
        private void MaterialBtnVerTodosMedi_Click_1(object sender, EventArgs e)
        {
            try
            {
                XtxNombreMedi.Clear();

                CargarMedicamentos();
                LimpiarFormulario2();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar medicamentos: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Boton Limpiar
        private void MaterialBtnLimpiarMedi_Click(object sender, EventArgs e)
        {
            LimpiarFormulario2();
        }

        //Exportar a excell
        private void materialButton1_Click(object sender, EventArgs e)
        {
            if (DgvMedicamentos.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para exportar", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DataTable dt = new DataTable();

                foreach (DataGridViewColumn columna in DgvMedicamentos.Columns)
                {
                    if (columna.HeaderText != "" && columna.Visible)
                        dt.Columns.Add(columna.HeaderText, typeof(String));
                }

                foreach (DataGridViewRow row in DgvMedicamentos.Rows)
                {
                    if (row.Visible)
                        dt.Rows.Add(new object[] {
                            row.Cells[0].Value.ToString(),
                            row.Cells[1].Value.ToString(),
                            row.Cells[2].Value.ToString(),
                            row.Cells[4].Value.ToString(),
                            


                            });
                }

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = string.Format("ReporteMedicamentos_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
                saveFile.Filter = "Excel Files | *.xlsx";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add(dt, "ReporteMedicamentos");
                        hoja.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(saveFile.FileName);

                        MessageBox.Show("Repore Medicamentos Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    catch 
                    {
                        MessageBox.Show("Error al generar el reporte", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

            }
        }



        /*
          **********************************************************************************************************************************************************************
          ------------------------------------------------------------PESTAÑA CITAS---------------------------------------------------------------------------------------------
          **********************************************************************************************************************************************************************
         */

        //Ocultar botones indispensables
        private void ConfigurarModoInicial3()
        {
            
            BtnCitasInsertar.Enabled = false;
            BtnCitasActualizar.Enabled = false;
            BtnCitasEli.Enabled = false;
            
        }

        //Cambio de color
        private void CambiarColorFormularioEdicion5()
        {
            Color colorEdicion = Color.LightYellow;
            TxtCitaCodigo.BackColor = colorEdicion;
            TxtCitaFecha.BackColor = colorEdicion;

        }




        //DataGridView Pacientes
        //Cargar Pacientes en DgvCitas
        private void CargarPacientes2()
        {
            try
            {
                dtPacientes = pacientesController.ObtenerTodosPacientes();

                if (dtPacientes != null && dtPacientes.Rows.Count > 0)
                {

                    DgvCitas.DataSource = dtPacientes;
                    ConfigurarDataGridView3();
                }
                else
                {
                    DgvCitas.DataSource = null;
                    MessageBox.Show("No se encontraron pacientes registrados.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pacientes:  {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Configuracion DataGridView
        private void ConfigurarDataGridView3()
        {
            try
            {
                DgvCitas.SuspendLayout();

                DgvCitas.AutoGenerateColumns = false;
                DgvCitas.Columns.Clear();

                DgvCitas.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Nombre",
                    HeaderText = "Nombre",
                    DataPropertyName = "Nombre",
                    ReadOnly = true
                });

                DgvCitas.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Edad",
                    HeaderText = "Edad",
                    DataPropertyName = "Edad",
                    ReadOnly = true,

                });

                DgvCitas.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "FechaDeNacimiento",
                    HeaderText = "FechaDeNacimiento",
                    DataPropertyName = "FechaDeNacimiento",
                    ReadOnly = true,

                });

                DgvCitas.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "FechaDeIngreso",
                    HeaderText = "FechaDeIngreso",
                    DataPropertyName = "FechaDeIngreso",
                    ReadOnly = true,

                });

                DgvCitas.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Dui",
                    HeaderText = "Dui",
                    DataPropertyName = "Dui",
                    ReadOnly = true,

                });

                DgvCitas.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Telefono",
                    HeaderText = "Telefono",
                    DataPropertyName = "Telefono",
                    ReadOnly = true,

                });

                DgvCitas.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Domicilio",
                    HeaderText = "Domicilio",
                    DataPropertyName = "Domicilio",
                    ReadOnly = true,

                });

                DgvCitas.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Tutor",
                    HeaderText = "Tutor",
                    DataPropertyName = "Tutor",
                    ReadOnly = true,

                });

                DgvCitas.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Parentesco",
                    HeaderText = "Parentesco",
                    DataPropertyName = "Parentesco",
                    ReadOnly = true,

                });

                DgvCitas.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "DomicilioTutor",
                    HeaderText = "DomicilioTutor",
                    DataPropertyName = "DomicilioTutor",
                    ReadOnly = true,

                });

                DgvCitas.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "CodigoPacientes",
                    HeaderText = "CodigoPacientes",
                    DataPropertyName = "CodigoPacientes",
                    ReadOnly = true,

                });

                //Columna ids no visible
                DgvCitas.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Id",
                    HeaderText = "Id",
                    DataPropertyName = "Id",
                    ReadOnly = true,
                    Visible = false
                });

                //Columna Activo no visible
                DgvCitas.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Activo",
                    HeaderText = "Activo",
                    DataPropertyName = "Activo",
                    ReadOnly = true,
                    Visible = false
                });


                //Propiedades visuales del DataView
                DgvCitas.ReadOnly = true;
                DgvCitas.AllowUserToAddRows = false;
                DgvCitas.AllowUserToDeleteRows = false;

                DgvCitas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DgvCitas.MultiSelect = false;

                DgvCitas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                DgvCitas.BackgroundColor = Color.White;
                DgvCitas.BorderStyle = BorderStyle.None;
                DgvCitas.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
                DgvCitas.RowHeadersVisible = false;
                DgvCitas.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                DgvCitas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                DgvCitas.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                DgvCitas.EnableHeadersVisualStyles = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error  al configurar DataGridView: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DgvCitas.ResumeLayout();
            }

        }

        //Evento para cambiar las views al hacer clic
        private void DgvCitas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                LimpiarFormulario3();

                if (e.RowIndex < 0) return;

                DataGridViewRow fila = DgvCitas.Rows[e.RowIndex];

                
                LblNombreCitas.Text = fila.Cells["Nombre"].Value?.ToString() ?? "";
                


                


                CambiarColorFormularioEdicion5();
                BtnCitasInsertar.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del pacientes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Metodo para agregar los parientes al combo box
        private void CargarHora()
        {
            ComboBoxHoraCita.Items.Clear();

            ComboBoxHoraCita.Items.Add("8:00 AM - 12:00MD");
            ComboBoxHoraCita.Items.Add("1:00 PM - 4:00PM");
            

            ComboBoxHoraCita.SelectedIndex = 0;

            ComboBoxHoraCita.DropDownStyle = ComboBoxStyle.DropDownList;

        }





        //DataGridView Citas
        //Cargar citas
        private void CargarCitas()
        {
            try
            {
                
                dtCitas = citaController.ObtenerTodasCitas();

                //DgvCitasVerda.Columns.Clear();


                if (dtCitas != null && dtCitas.Rows.Count > 0)
                {
                    ConfigurarDataGridView5();
                    
                    DgvCitasVerda.DataSource = dtCitas;
                    
                }
                else
                {
                    DgvCitasVerda.DataSource = null;
                    MessageBox.Show("No se encontraron citas registrados.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar citas:  {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Configuracion DataGridView
        private void ConfigurarDataGridView5()
        {
            try
            {
                DgvCitasVerda.SuspendLayout();

                DgvCitasVerda.AutoGenerateColumns = false;
                DgvCitasVerda.Columns.Clear();


                //Columna ids no visible
                DgvCitasVerda.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Id",
                    HeaderText = "Id",
                    DataPropertyName = "Id",
                    ReadOnly = true,
                    Visible = false
                });

                DgvCitasVerda.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Nombre",
                    HeaderText = "Nombre",
                    DataPropertyName = "Nombre",
                    ReadOnly = true
                });

                DgvCitasVerda.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Fecha",
                    HeaderText = "Fecha",
                    DataPropertyName = "Fecha",
                    ReadOnly = true,

                });

                DgvCitasVerda.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Hora",
                    HeaderText = "Hora",
                    DataPropertyName = "Hora",
                    ReadOnly = true,

                });

                




                //Propiedades visuales del DataView
                DgvCitasVerda.ReadOnly = true;
                DgvCitasVerda.AllowUserToAddRows = false;
                DgvCitasVerda.AllowUserToDeleteRows = false;

                DgvCitasVerda.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DgvCitasVerda.MultiSelect = false;

                DgvCitasVerda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                DgvCitasVerda.BackgroundColor = Color.White;
                DgvCitasVerda.BorderStyle = BorderStyle.None;
                DgvCitasVerda.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
                DgvCitasVerda.RowHeadersVisible = false;
                DgvCitasVerda.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                DgvCitasVerda.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                DgvCitasVerda.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                DgvCitasVerda.EnableHeadersVisualStyles = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error  al configurar DataGridView: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DgvCitasVerda.ResumeLayout();
            }

        }

        //Evento para cambiar las views al hacer clic
        private void DgvCitasVerda_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                LimpiarFormulario3();
                if (e.RowIndex < 0) return;

                DataGridViewRow fila = DgvCitasVerda.Rows[e.RowIndex];

                if (fila.Cells["Id"].Value != null)
                {
                    citaSeleccionada = Convert.ToInt32(fila.Cells["Id"].Value);
                    

                }


                LblNombreCitas.Text = fila.Cells["Nombre"].Value?.ToString() ?? "";
                TxtCitaFecha.Text = fila.Cells["Fecha"].Value?.ToString() ?? "";

                string hora = fila.Cells["Hora"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(hora))
                {
                    ComboBoxHoraCita.Text = hora;
                }


                
                CambiarColorFormularioEdicion5();

                BtnCitasInsertar.Enabled = false;
                BtnCitasActualizar.Enabled = true;
                BtnCitasEli.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos de la cita: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        //Metodo para cambiar el color cuando no se edita
        private void RestaurarColorFormulario3()
        {
            Color colorNormal = Color.White;
            TxtCitaCodigo.BackColor = colorNormal;
            TxtCitaFecha.BackColor = colorNormal;
            

        }

        //LimpiarFormulario
        private void LimpiarFormulario3()
        {
            citaSeleccionada = 0;

            LblNombreCitas.Text = "N/A";
            TxtCitaCodigo.Clear();
            TxtCitaFecha.Clear();

            ComboBoxHoraCita.SelectedIndex = 0;
            


            RestaurarColorFormulario3();

            BtnCitasInsertar.Enabled = false;
            BtnCitasActualizar.Enabled = false;
            BtnCitasEli.Enabled = false;
        }

        //Boton Ingresar
        private void BtnCitasInsertar_Click(object sender, EventArgs e)
        {
            try
            {
                if (citaSeleccionada > 0)
                {
                    MessageBox.Show("Esta en modo edicion, Para crear un nuevo medicamento, " +
                        "Primero llene el formulario con los datos correctos.", "Modo edicion",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario3();
                    return;
                }

                //Validaciones Simples
                if (!Validaciones.ValidarNoVacio(TxtCitaFecha, "Cita Fecha")) return;
                if (!Validaciones.ValidarLongitud(TxtCitaFecha.Text, 10, 10, "Fecha de la cita")) return;

                //Validar ComboBox
                if (!Validaciones.ValidarComboBox2(ComboBoxHoraCita, "Hora cita")) return;


                var confirmacion = MessageBox.Show(
                    $"¿Esta seguro de crear los datos de la cita: {LblNombreCitas}?",
                    "Confirmar Actualización",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes) return;

                Cita cita = new Cita
                {
                    NombreCompleto = LblNombreCitas.Text,
                    fecha = TxtCitaFecha.Text,
                    Hora = ComboBoxHoraCita.SelectedItem.ToString(),
                };

                bool resultado = citaController.CrearCita(cita);

                if (resultado)
                {
                    MessageBox.Show("Cita creada exitosamente.", "Exito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario3();
                    CargarCitas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar Citas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Boton Actualizar
        private void BtnCitasActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (citaSeleccionada == 0)
                {
                    MessageBox.Show("Debe seleccionar una cita de la lista para actualizar.",
                        "Seleccion Requerida",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LimpiarFormulario3();
                    return;
                }

                //Validaciones Simples
                if (!Validaciones.ValidarNoVacio(TxtCitaFecha, "Cita Fecha")) return;
                if (!Validaciones.ValidarLongitud(TxtCitaFecha.Text, 10, 10, "Fecha de la cita")) return;

                //Validar ComboBox
                if (!Validaciones.ValidarComboBox2(ComboBoxHoraCita, "Hora cita")) return;


                var confirmacion = MessageBox.Show(
                    $"¿Esta seguro de actualizar los datos de la cita: {LblNombreCitas.Text}?",
                    "Confirmar Actualización",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);



                if (confirmacion != DialogResult.Yes) return;

                Cita cita = new Cita
                {
                    id = citaSeleccionada,
                    NombreCompleto = LblNombreCitas.Text,
                    fecha = TxtCitaFecha.Text,
                    Hora = ComboBoxHoraCita.SelectedItem.ToString(),
                };

                bool resultado = citaController.ActualizarCita(cita);


                if (resultado)
                {
                    MessageBox.Show("Cita actualizado exitosamente.", "Exito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario3();
                    CargarCitas();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar la cita, Verifique los datos.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo actualizar la cita: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Boton Eliminar
        private void BtnCitasEli_Click(object sender, EventArgs e)
        {
            try
            {
                if (citaSeleccionada == 0)
                {
                    MessageBox.Show("Seleccione una cita para eliminar.");
                    return;
                }

                var confirmacion = MessageBox.Show(
                    $"¿Eliminar la cita? {XtxNombreMedi.Text}?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes) return;

                bool resultado = citaController.EliminarCitas(citaSeleccionada);

                if (resultado)
                {
                    MessageBox.Show("Cita eliminada Exitosamente.");
                    LimpiarFormulario3();
                    CargarCitas();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        //Boton Buscar
        private void BtnCitasBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string filtro = TxtCitaCodigo.Text.Trim();

                if (!Validaciones.ValidarNoVacio(TxtCitaCodigo, "Paciente a buscar"))
                    return;

                DataTable dt = pacientesController.BuscarPacientePorNombrePaciente(filtro);
                LimpiarFormulario4();

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show($"No se encontro el paciente '{filtro}'.",
                        "Sin resultados",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DgvCitas.DataSource = null;
                    
                    return;
                }

                DgvCitas.DataSource = dt;
                

                MessageBox.Show($"Se encontraron {dt.Rows.Count} pacientes(s).",
                    "Busqueda exitosa",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la busqueda: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Boton ver todos
        private void BtnCitasVertodos_Click(object sender, EventArgs e)
        {
            try
            {
                LblNombreCitas.Text = "N/A";

                CargarCitas();
                CargarPacientes2();
                LimpiarFormulario3();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Boton Limpiar
        private void BtnCitasLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario3();
        }


        /*
         ************************************************************************************************************************************
         ----------------------------------------------FIN DE PESTAÑA CITAS------------------------------------------------------------------
         ************************************************************************************************************************************
        */

        /*
         ************************************************************************************************************************************
         -------------------------------------------------PESTAÑA PACIENTES------------------------------------------------------------------
         ************************************************************************************************************************************
        */

        //Ocultar botones al principio
        private void ConfigurarModoInicial4()
        {
            BtnPacienteActualizar.Enabled = false;
            BtnPacienteEliminar.Enabled = false;
        }

        //Cargar Pacientes
        private void CargarPacientes()
        {
            try
            {
                dtPacientes = pacientesController.ObtenerTodosPacientes();

                if (dtPacientes != null && dtPacientes.Rows.Count > 0)
                {

                    DgvPacientes.DataSource = dtPacientes;
                    ConfigurarDataGridView4();
                }
                else
                {
                    DgvPacientes.DataSource = null;
                    MessageBox.Show("No se encontraron pacientes registrados.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pacientes:  {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Configuracion del datagriwview
        private void ConfigurarDataGridView4()
        {
            try
            {
                DgvPacientes.SuspendLayout();

                DgvPacientes.AutoGenerateColumns = false;
                DgvPacientes.Columns.Clear();

                DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Nombre",
                    HeaderText = "Nombre",
                    DataPropertyName = "Nombre",
                    ReadOnly = true
                });

                DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Edad",
                    HeaderText = "Edad",
                    DataPropertyName = "Edad",
                    ReadOnly = true,
                    
                });

                DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "FechaDeNacimiento",
                    HeaderText = "FechaDeNacimiento",
                    DataPropertyName = "FechaDeNacimiento",
                    ReadOnly = true,
                    
                });

                DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "FechaDeIngreso",
                    HeaderText = "FechaDeIngreso",
                    DataPropertyName = "FechaDeIngreso",
                    ReadOnly = true,

                });

                DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Dui",
                    HeaderText = "Dui",
                    DataPropertyName = "Dui",
                    ReadOnly = true,

                });

                DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Telefono",
                    HeaderText = "Telefono",
                    DataPropertyName = "Telefono",
                    ReadOnly = true,

                });

                DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Domicilio",
                    HeaderText = "Domicilio",
                    DataPropertyName = "Domicilio",
                    ReadOnly = true,

                });

                DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Tutor",
                    HeaderText = "Tutor",
                    DataPropertyName = "Tutor",
                    ReadOnly = true,

                });

                DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Parentesco",
                    HeaderText = "Parentesco",
                    DataPropertyName = "Parentesco",
                    ReadOnly = true,

                });

                DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "DomicilioTutor",
                    HeaderText = "DomicilioTutor",
                    DataPropertyName = "DomicilioTutor",
                    ReadOnly = true,

                });

                DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "CodigoPacientes",
                    HeaderText = "CodigoPacientes",
                    DataPropertyName = "CodigoPacientes",
                    ReadOnly = true,

                });

                //Columna ids no visible
                DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Id",
                    HeaderText = "Id",
                    DataPropertyName = "Id",
                    ReadOnly = true,
                    Visible = false
                });

                //Columna Activo no visible
                DgvPacientes.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Activo",
                    HeaderText = "Activo",
                    DataPropertyName = "Activo",
                    ReadOnly = true,
                    Visible = false
                });


                //Propiedades visuales del DataView
                DgvPacientes.ReadOnly = true;
                DgvPacientes.AllowUserToAddRows = false;
                DgvPacientes.AllowUserToDeleteRows = false;

                DgvPacientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DgvPacientes.MultiSelect = false;

                DgvPacientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                DgvPacientes.BackgroundColor = Color.White;
                DgvPacientes.BorderStyle = BorderStyle.None;
                DgvPacientes.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
                DgvPacientes.RowHeadersVisible = false;
                DgvPacientes.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                DgvPacientes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                DgvPacientes.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                DgvPacientes.EnableHeadersVisualStyles = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error  al configurar DataGridView: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DgvPacientes.ResumeLayout();
            }

        }

        //Metodo para agregar los parientes al combo box
        private void CargarParientes()
        {
            ComboBoxParientes.Items.Clear();

            ComboBoxParientes.Items.Add("Padre");
            ComboBoxParientes.Items.Add("Madre");
            ComboBoxParientes.Items.Add("Otro");

            ComboBoxParientes.SelectedIndex = 0;

            ComboBoxParientes.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        //Evento para cambiar las views al hacer clic
        private void DgvPacientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                DataGridViewRow fila = DgvPacientes.Rows[e.RowIndex];

                if (fila.Cells["Id"].Value != null)
                {
                    pacienteSeleccionado = Convert.ToInt32(fila.Cells["Id"].Value);
                }

                TxtPacientesNombre.Text = fila.Cells["Nombre"].Value?.ToString() ?? "";
                TxtPacientesEdad.Text = fila.Cells["Edad"].Value?.ToString() ?? "";
                TxtPacientesFechaNacimiento.Text = fila.Cells["FechaDeNacimiento"].Value?.ToString() ?? "";
                TxtPacientesFechaIngreso.Text = fila.Cells["FechaDeIngreso"].Value?.ToString() ?? "";
                TxtPacientesDui.Text = fila.Cells["Dui"].Value?.ToString() ?? "";
                TxtPacientesTelefono.Text = fila.Cells["Telefono"].Value?.ToString() ?? "";
                TxtPacientesDomicilio.Text = fila.Cells["Domicilio"].Value?.ToString() ?? "";
                TxtPacientesNombreTutor.Text = fila.Cells["Tutor"].Value?.ToString() ?? "";
                TxtPacientesDomicilioTutor.Text = fila.Cells["DomicilioTutor"].Value?.ToString() ?? "";
                TxtPacientesCodigo.Text = fila.Cells["CodigoPacientes"].Value?.ToString() ?? "";


                string parientes = fila.Cells["Parentesco"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(parientes))
                {
                    ComboBoxParientes.Text = parientes;
                }


                CambiarColorFormularioEdicion4();
                BtnPacienteActualizar.Enabled = true;
                BtnPacienteEliminar.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del pacientes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Metodo para cambiar el color cuando se edita
        private void CambiarColorFormularioEdicion4()
        {
            Color colorEdicion = Color.LightYellow;
            TxtPacientesNombre.BackColor = colorEdicion;
            TxtPacientesEdad.BackColor = colorEdicion;
            TxtPacientesFechaNacimiento.BackColor = colorEdicion;
            TxtPacientesFechaIngreso.BackColor = colorEdicion;
            TxtPacientesDui.BackColor = colorEdicion;
            TxtPacientesTelefono.BackColor = colorEdicion;
            TxtPacientesDomicilio.BackColor = colorEdicion;
            TxtPacientesNombreTutor.BackColor = colorEdicion;
            TxtPacientesDomicilioTutor.BackColor = colorEdicion;
            TxtPacientesCodigo.BackColor = colorEdicion;
        }

        //Metodo para cambiar el color cuando no se edita
        private void RestaurarColorFormulario4()
        {
            Color colorEdicion2 = Color.White;
            TxtPacientesNombre.BackColor = colorEdicion2;
            TxtPacientesEdad.BackColor = colorEdicion2;
            TxtPacientesFechaNacimiento.BackColor = colorEdicion2;
            TxtPacientesFechaIngreso.BackColor = colorEdicion2;
            TxtPacientesDui.BackColor = colorEdicion2;
            TxtPacientesTelefono.BackColor = colorEdicion2;
            TxtPacientesDomicilio.BackColor = colorEdicion2;
            TxtPacientesNombreTutor.BackColor = colorEdicion2;
            TxtPacientesDomicilioTutor.BackColor = colorEdicion2;
            TxtPacientesCodigo.BackColor = colorEdicion2;

        }

        //Boton agregar
        private void BtnPacienteInsertar_Click(object sender, EventArgs e)
        {
            try
            {
                if (pacienteSeleccionado > 0)
                {
                    MessageBox.Show("Esta en modo edicion, Para crear un nuevo paciente, " +
                        "Primero llene el formulario con los datos correctos.", "Modo edicion",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario4();
                    return;
                }

                //Validaciones Simples
                if (!Validaciones.ValidarNoVacio(TxtPacientesNombre, "Nombre del pacientes")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesEdad, "Edad")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesFechaNacimiento, "Fecha de nacimiento")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesFechaIngreso, "Fecha de ingreso")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesDui, "Dui")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesTelefono, "Telefono")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesDomicilio, "Domicilio")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesNombreTutor, "Nombre Tutor")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesDomicilioTutor, "Domocilio tutor")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesCodigo, "Codigo pacientes")) return;

                
                if (!Validaciones.ValidarSoloNumeros(TxtPacientesTelefono.Text, "Telefono")) return;
                if (!Validaciones.ValidarSoloNumeros(TxtPacientesCodigo.Text, "Paciente Codigo")) return;

                if (!Validaciones.ValidarTelefono(TxtPacientesTelefono, "Teléfono")) return;
                if (!Validaciones.ValidarLongitud(TxtPacientesFechaNacimiento.Text, 10, 10, "Fecha de nacimiento")) return;
                if (!Validaciones.ValidarLongitud(TxtPacientesFechaIngreso.Text, 10, 10, "Fecha de ingreso")) return;
                if (!Validaciones.ValidarLongitud(TxtPacientesDui.Text, 10, 10, "Dui")) return;

                int edad = Convert.ToInt32(TxtPacientesEdad.Text);
                int telefono = Convert.ToInt32(TxtPacientesTelefono.Text);
                int codigopaciente = Convert.ToInt32(TxtPacientesCodigo.Text);

                if (!Validaciones.ValidarRango(codigopaciente, 0, 9999999999, "Codigo de paciente")) return;
                if (!Validaciones.ValidarComboBox2(ComboBoxParientes, "Parientes")) return;

                //Validacion Edad
                if (edad < 0 && edad > 200)
                {
                    MessageBox.Show("La edad no puede ser negativo",
                        "Error de validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtPacientesEdad.Focus();
                    return;
                }

                

                if (!ValidarPacienteUnico(TxtPacientesCodigo.Text)) return;

                Pacientes pacientes = new Pacientes()
                {
                    Nombre = TxtPacientesNombre.Text,
                    Edad = edad,
                    FechaNacimiento = TxtPacientesFechaNacimiento.Text,
                    FechaIngreso = TxtPacientesFechaNacimiento.Text,
                    DUI = TxtPacientesDui.Text,
                    Telefono = telefono,
                    Domicilio = TxtPacientesDomicilio.Text,
                    Tutor = TxtPacientesNombreTutor.Text,
                    Parentesco = ComboBoxParientes.SelectedItem.ToString(),
                    DomicilioTutor = TxtPacientesDomicilioTutor.Text,
                    CodigoPaciente = codigopaciente,
                };

                bool resultado = pacientesController.CrearPaciente(pacientes);

                if (resultado)
                {
                    MessageBox.Show("Paciente creado exitosamente.", "Exito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario4();
                    CargarPacientes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar Paciente: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Limpiar formulario medicamentos
        private void LimpiarFormulario4()
        {
            medicamentoSeleccionado = 0;

            TxtPacientesNombre.Clear();
            TxtPacientesEdad.Clear();
            TxtPacientesFechaNacimiento.Clear();
            TxtPacientesFechaIngreso.Clear();
            TxtPacientesDui.Clear();
            TxtPacientesTelefono.Clear();
            TxtPacientesDomicilio.Clear();
            TxtPacientesNombreTutor.Clear();
            TxtPacientesDomicilioTutor.Clear();
            TxtPacientesCodigo.Clear();


            RestaurarColorFormulario4();

            BtnPacienteActualizar.Enabled = false;
            BtnPacienteEliminar.Enabled = false;
        }

        //Validaciones
        private bool ValidarPacienteUnico(string nombre, int idExcluir = 0)
        {

            try
            {
                string condicion = $"CodigoPacientes = '{nombre.Replace("'", "''")}' AND Activo = 1";

                if (idExcluir > 0)
                {
                    condicion += $" AND Id != {idExcluir}";
                }

                DataTable pacientesExistentes = pacientesController.ObtenerPacientesPorCondicion(condicion);

                if (pacientesExistentes.Rows.Count > 0)
                {
                    MessageBox.Show("Ya existe un paciente con ese codigo. Por favor elija otro.",
                        "Codigo duplicado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    TxtPacientesCodigo.Focus();
                    TxtPacientesCodigo.SelectAll();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al validar paciente unico: {ex.Message}", "Error");
                return false;
            }
        }

        //Boton Actualizar
        private void BtnPacienteActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (pacienteSeleccionado == 0)
                {
                    MessageBox.Show("Debe seleccionar un paciente de la lista para actualizar.",
                        "Seleccion Requerida",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }

                //Validaciones Simples

                //Validaciones Simples
                if (!Validaciones.ValidarNoVacio(TxtPacientesNombre, "Nombre del pacientes")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesEdad, "Edad")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesFechaNacimiento, "Fecha de nacimiento")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesFechaIngreso, "Fecha de ingreso")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesDui, "Dui")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesTelefono, "Telefono")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesDomicilio, "Domicilio")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesNombreTutor, "Nombre Tutor")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesDomicilioTutor, "Domocilio tutor")) return;
                if (!Validaciones.ValidarNoVacio(TxtPacientesCodigo, "Codigo pacientes")) return;


                if (!Validaciones.ValidarSoloNumeros(TxtPacientesTelefono.Text, "Telefono")) return;
                if (!Validaciones.ValidarSoloNumeros(TxtPacientesCodigo.Text, "Paciente Codigo")) return;

                if (!Validaciones.ValidarTelefono(TxtPacientesTelefono, "Teléfono")) return;
                if (!Validaciones.ValidarLongitud(TxtPacientesFechaNacimiento.Text, 10, 10, "Fecha de nacimiento")) return;
                if (!Validaciones.ValidarLongitud(TxtPacientesFechaIngreso.Text, 10, 10, "Fecha de ingreso")) return;
                if (!Validaciones.ValidarLongitud(TxtPacientesDui.Text, 10, 10, "Dui")) return;

                int edad = Convert.ToInt32(TxtPacientesEdad.Text);
                int telefono = Convert.ToInt32(TxtPacientesTelefono.Text);
                int codigopaciente = Convert.ToInt32(TxtPacientesCodigo.Text);

                if (!Validaciones.ValidarRango(codigopaciente, 0, 9999999999, "Codigo de paciente")) return;
                if (!Validaciones.ValidarComboBox2(ComboBoxParientes, "Parientes")) return;

                

                //Validacion Edad
                if (edad < 0 && edad > 200)
                {
                    MessageBox.Show("La edad no puede ser negativo",
                        "Error de validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtPacientesEdad.Focus();
                    return;
                }


                


                var confirmacion = MessageBox.Show(
                    $"¿Esta seguro de actualizar los datos del paciente: {TxtPacientesNombre.Text}?",
                    "Confirmar Actualización",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);



                if (confirmacion != DialogResult.Yes) return;

                Pacientes pacientes = new Pacientes()
                {
                    Nombre = TxtPacientesNombre.Text,
                    Edad = edad,
                    FechaNacimiento = TxtPacientesFechaNacimiento.Text,
                    FechaIngreso = TxtPacientesFechaNacimiento.Text,
                    DUI = TxtPacientesDui.Text,
                    Telefono = telefono,
                    Domicilio = TxtPacientesDomicilio.Text,
                    Tutor = TxtPacientesNombreTutor.Text,
                    Parentesco = ComboBoxParientes.SelectedItem.ToString(),
                    DomicilioTutor = TxtPacientesDomicilioTutor.Text,
                    CodigoPaciente = codigopaciente,
                };

                bool resultado = pacientesController.ActualizarPaciente(pacientes);

                if (resultado)
                {
                    MessageBox.Show("Paciente actualizado exitosamente.", "Exito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario4();
                    CargarPacientes();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el paciente, Verifique los datos.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo actualizar el paciente: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Boton Eliminar
        private void BtnPacienteEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (pacienteSeleccionado == 0)
                {
                    MessageBox.Show("Seleccione un paciente para eliminar.");
                    return;
                }

                var confirmacion = MessageBox.Show(
                    $"¿Eliminar el paciente? {TxtPacientesNombre.Text}?",
                    "Confirmar",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacion != DialogResult.Yes) return;

                
                bool resultado = pacientesController.EliminarPaciente(pacienteSeleccionado);

                if (resultado)
                {
                    MessageBox.Show("paciente medicamento Exitosamente.");
                    LimpiarFormulario4();
                    CargarPacientes();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        //Boton Buscaar
        private void BtnPacienteBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string filtro = TxtPacientesCodigo.Text.Trim();

                if (!Validaciones.ValidarNoVacio(TxtPacientesCodigo, "Paciente a buscar"))
                    return;

                DataTable dt = pacientesController.BuscarPacientePorNombrePaciente(filtro);
                LimpiarFormulario4();

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show($"No se encontro el paciente '{filtro}'.",
                        "Sin resultados",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DgvPacientes.DataSource = null;
                    LimpiarFormulario4();
                    return;
                }

                DgvPacientes.DataSource = dt;
                LimpiarFormulario2();

                MessageBox.Show($"Se encontraron {dt.Rows.Count} pacientes(s).",
                    "Busqueda exitosa",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la busqueda: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Boton VerTodos
        private void BtnPacienteVerTodos_Click(object sender, EventArgs e)
        {
            try
            {
                TxtPacientesCodigo.Clear();

                CargarPacientes();
                LimpiarFormulario4();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pacientes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Limpiar
        private void BtnPacienteLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario4();
        }

        /*
         ************************************************************************************************************************************
         ------------------------------------------------FIN DE PESTAÑA PACIENTES------------------------------------------------------------
         ************************************************************************************************************************************
        */

        /*
          **********************************************************************************************************************************************************************
          ------------------------------------------------------------PESTAÑA EXPEDIENETE---------------------------------------------------------------------------------------
          **********************************************************************************************************************************************************************
         */

        //Ocultar botones indispensables
        private void ConfigurarModoInicial5()
        {

            BtnExpeCrear.Enabled = false;

        }

        //Metodo para cambiar el color cuando se edita
        private void CambiarColorFormularioEdicion6()
        {
            Color colorEdicion = Color.LightYellow;
            TxtExpeEnfermedadActual.BackColor = colorEdicion;
            TxtExpeMotivo.BackColor = colorEdicion;
            TxtExpePadece.BackColor = colorEdicion;
            TxtExpePadecido.BackColor = colorEdicion;
            TxtExpeTomarMedi.BackColor = colorEdicion;
            TxtExpeRecetado.BackColor = colorEdicion;
            TxtExpeFecha.BackColor = colorEdicion;
        }

        //DataGridView Pacientes
        //Cargar Pacientes en DgvCitas
        private void CargarPacientes3()
        {
            try
            {
                dtPacientes = pacientesController.ObtenerTodosPacientes();

                if (dtPacientes != null && dtPacientes.Rows.Count > 0)
                {

                    DgvExpedientePaci.DataSource = dtPacientes;
                    ConfigurarDataGridView6();
                }
                else
                {
                    DgvExpedientePaci.DataSource = null;
                    MessageBox.Show("No se encontraron pacientes registrados.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pacientes:  {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Configuracion DataGridView
        private void ConfigurarDataGridView6()
        {
            try
            {
                DgvExpedientePaci.SuspendLayout();

                DgvExpedientePaci.AutoGenerateColumns = false;
                DgvExpedientePaci.Columns.Clear();

                DgvExpedientePaci.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Nombre",
                    HeaderText = "Nombre",
                    DataPropertyName = "Nombre",
                    ReadOnly = true
                });

                DgvExpedientePaci.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Edad",
                    HeaderText = "Edad",
                    DataPropertyName = "Edad",
                    ReadOnly = true,

                });

                DgvExpedientePaci.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "FechaDeNacimiento",
                    HeaderText = "FechaDeNacimiento",
                    DataPropertyName = "FechaDeNacimiento",
                    ReadOnly = true,

                });

                DgvExpedientePaci.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "FechaDeIngreso",
                    HeaderText = "FechaDeIngreso",
                    DataPropertyName = "FechaDeIngreso",
                    ReadOnly = true,

                });

                DgvExpedientePaci.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Dui",
                    HeaderText = "Dui",
                    DataPropertyName = "Dui",
                    ReadOnly = true,

                });

                DgvExpedientePaci.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Telefono",
                    HeaderText = "Telefono",
                    DataPropertyName = "Telefono",
                    ReadOnly = true,

                });

                DgvExpedientePaci.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Domicilio",
                    HeaderText = "Domicilio",
                    DataPropertyName = "Domicilio",
                    ReadOnly = true,

                });

                DgvExpedientePaci.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Tutor",
                    HeaderText = "Tutor",
                    DataPropertyName = "Tutor",
                    ReadOnly = true,

                });

                DgvExpedientePaci.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Parentesco",
                    HeaderText = "Parentesco",
                    DataPropertyName = "Parentesco",
                    ReadOnly = true,

                });

                DgvExpedientePaci.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "DomicilioTutor",
                    HeaderText = "DomicilioTutor",
                    DataPropertyName = "DomicilioTutor",
                    ReadOnly = true,

                });

                DgvExpedientePaci.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "CodigoPacientes",
                    HeaderText = "CodigoPacientes",
                    DataPropertyName = "CodigoPacientes",
                    ReadOnly = true,

                });

                //Columna ids no visible
                DgvExpedientePaci.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Id",
                    HeaderText = "Id",
                    DataPropertyName = "Id",
                    ReadOnly = true,
                    Visible = false
                });

                //Columna Activo no visible
                DgvExpedientePaci.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Activo",
                    HeaderText = "Activo",
                    DataPropertyName = "Activo",
                    ReadOnly = true,
                    Visible = false
                });


                //Propiedades visuales del DataView
                DgvExpedientePaci.ReadOnly = true;
                DgvExpedientePaci.AllowUserToAddRows = false;
                DgvExpedientePaci.AllowUserToDeleteRows = false;

                DgvExpedientePaci.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DgvExpedientePaci.MultiSelect = false;

                DgvExpedientePaci.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                DgvExpedientePaci.BackgroundColor = Color.White;
                DgvExpedientePaci.BorderStyle = BorderStyle.None;
                DgvExpedientePaci.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
                DgvExpedientePaci.RowHeadersVisible = false;
                DgvExpedientePaci.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                DgvExpedientePaci.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                DgvExpedientePaci.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                DgvExpedientePaci.EnableHeadersVisualStyles = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error  al configurar DataGridView: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DgvExpedientePaci.ResumeLayout();
            }

        }

        //Evento para cambiar las views al hacer clic
        private void DgvExpedientePaci_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //LimpiarFormulario3();

                if (e.RowIndex < 0) return;

                DataGridViewRow fila = DgvExpedientePaci.Rows[e.RowIndex];


                LblExpeNombre.Text = fila.Cells["Nombre"].Value?.ToString() ?? "";
                LblExpeEdad.Text = fila.Cells["Edad"].Value?.ToString() ?? "";
                LblExpeCodigo.Text = fila.Cells["CodigoPacientes"].Value?.ToString() ?? "";




                CambiarColorFormularioEdicion6();
                BtnExpeCrear.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del pacientes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Limpiar Formulario
        private void LimpiarFormulario5()
        {
            expedienteSeleccionado = 0;

            TxtExpeMotivo.Clear();

            TxtExpePadece.Text = "N/A";
            TxtExpePadecido.Text = "N/A";
            TxtExpeTomarMedi.Text = "N/A";
            TxtExpeRecetado.Clear();
            TxtExpeFecha.Clear();

            TxtExpeEnfermedadActual.Clear();
            


            RestaurarColorFormulario5();

            BtnExpeCrear.Enabled = false;

        }

        //Metodo para cambiar el color cuando no se edita
        private void RestaurarColorFormulario5()
        {
            Color colorEdicion2 = Color.White;
            TxtExpeEnfermedadActual.BackColor = colorEdicion2;
            TxtExpeMotivo.BackColor = colorEdicion2;
            TxtExpePadece.BackColor = colorEdicion2;
            TxtExpePadecido.BackColor = colorEdicion2;
            TxtExpeTomarMedi.BackColor = colorEdicion2;
            TxtExpeRecetado.BackColor = colorEdicion2;
            TxtExpeFecha.BackColor = colorEdicion2;

        }
        
        //Boton Crear
        private void BtnExpeCrear_Click(object sender, EventArgs e)
        {
            try
            {
                if (expedienteSeleccionado > 0)
                {
                    MessageBox.Show("Esta en modo edicion, Para crear un nuevo paciente, " +
                        "Primero llene el formulario con los datos correctos.", "Modo edicion",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario5();
                    return;
                }

                //Validaciones Simples
                if (!Validaciones.ValidarNoVacio(TxtExpeMotivo, "Motivo")) return;
                if (!Validaciones.ValidarNoVacio(TxtExpeEnfermedadActual, "Enfermedad")) return;
                if (!Validaciones.ValidarNoVacio(TxtExpeFecha, "Fecha")) return;
                if (!Validaciones.ValidarNoVacio(TxtExpeRecetado, "Medicamento Recetado")) return;

                Expediente expediente = new Expediente()
                {
                    Motivo = TxtExpeMotivo.Text,
                    Padece = TxtExpePadece.Text,
                    Padecido = TxtExpePadecido.Text,
                    Enfermedad = TxtExpeTomarMedi.Text,
                    Nombre = LblExpeNombre.Text,
                    Edad = LblExpeEdad.Text,
                    Codigo = LblExpeCodigo.Text,
                    Medicamento = TxtExpeRecetado.Text,
                    Fecha = TxtExpeFecha.Text
                };

                bool resultado = expedienteController.CrearExpediente(expediente);

                

                if (resultado)
                {
                    MessageBox.Show("Expediente creado exitosamente.", "Exito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario4();
                    CargarPacientes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar expediente: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Boton Limpiar
        private void BtnExpeLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario5();
        }

        //Boton Buscar
        private void BtnExpeBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string filtro = TxtExpeBuscar.Text.Trim();

                if (!Validaciones.ValidarNoVacio(TxtExpeBuscar, "Paciente a buscar"))
                    return;

                DataTable dt = pacientesController.BuscarPacientePorNombrePaciente(filtro);
                

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show($"No se encontro el paciente '{filtro}'.",
                        "Sin resultados",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DgvExpedientePaci.DataSource = null;

                    return;
                }

                DgvExpedientePaci.DataSource = dt;


                MessageBox.Show($"Se encontraron {dt.Rows.Count} pacientes(s).",
                    "Busqueda exitosa",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la busqueda: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Boton VerTodos
        private void BtnExpeVerTodos_Click(object sender, EventArgs e)
        {
            try
            {
                TxtExpeBuscar.Clear();

                CargarPacientes3();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pacientes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        //Cargar Pacientes en DgvHistorial
        private void CargarPacientes4()
        {
            try
            {
                dtPacientes = pacientesController.ObtenerTodosPacientes();

                if (dtPacientes != null && dtPacientes.Rows.Count > 0)
                {

                    DgvHistorial.DataSource = dtPacientes;
                    ConfigurarDataGridView7();
                }
                else
                {
                    DgvHistorial.DataSource = null;
                    MessageBox.Show("No se encontraron pacientes registrados.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pacientes:  {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Configuracion DataGridView
        private void ConfigurarDataGridView7()
        {
            try
            {
                DgvHistorial.SuspendLayout();

                DgvHistorial.AutoGenerateColumns = false;
                DgvHistorial.Columns.Clear();

                DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Nombre",
                    HeaderText = "Nombre",
                    DataPropertyName = "Nombre",
                    ReadOnly = true
                });

                DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Edad",
                    HeaderText = "Edad",
                    DataPropertyName = "Edad",
                    ReadOnly = true,

                });

                DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "FechaDeNacimiento",
                    HeaderText = "FechaDeNacimiento",
                    DataPropertyName = "FechaDeNacimiento",
                    ReadOnly = true,

                });

                DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "FechaDeIngreso",
                    HeaderText = "FechaDeIngreso",
                    DataPropertyName = "FechaDeIngreso",
                    ReadOnly = true,

                });

                DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Dui",
                    HeaderText = "Dui",
                    DataPropertyName = "Dui",
                    ReadOnly = true,

                });

                DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Telefono",
                    HeaderText = "Telefono",
                    DataPropertyName = "Telefono",
                    ReadOnly = true,

                });

                DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Domicilio",
                    HeaderText = "Domicilio",
                    DataPropertyName = "Domicilio",
                    ReadOnly = true,

                });

                DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Tutor",
                    HeaderText = "Tutor",
                    DataPropertyName = "Tutor",
                    ReadOnly = true,

                });

                DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Parentesco",
                    HeaderText = "Parentesco",
                    DataPropertyName = "Parentesco",
                    ReadOnly = true,

                });

                DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "DomicilioTutor",
                    HeaderText = "DomicilioTutor",
                    DataPropertyName = "DomicilioTutor",
                    ReadOnly = true,

                });

                DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "CodigoPacientes",
                    HeaderText = "CodigoPacientes",
                    DataPropertyName = "CodigoPacientes",
                    ReadOnly = true,

                });

                //Columna ids no visible
                DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Id",
                    HeaderText = "Id",
                    DataPropertyName = "Id",
                    ReadOnly = true,
                    Visible = false
                });

                //Columna Activo no visible
                DgvHistorial.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Activo",
                    HeaderText = "Activo",
                    DataPropertyName = "Activo",
                    ReadOnly = true,
                    Visible = false
                });


                //Propiedades visuales del DataView
                DgvHistorial.ReadOnly = true;
                DgvHistorial.AllowUserToAddRows = false;
                DgvHistorial.AllowUserToDeleteRows = false;

                DgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DgvHistorial.MultiSelect = false;

                DgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                DgvHistorial.BackgroundColor = Color.White;
                DgvHistorial.BorderStyle = BorderStyle.None;
                DgvHistorial.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
                DgvHistorial.RowHeadersVisible = false;
                DgvHistorial.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                DgvHistorial.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                DgvHistorial.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                DgvHistorial.EnableHeadersVisualStyles = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error  al configurar DataGridView7: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DgvHistorial.ResumeLayout();
            }

        }

        //Boton BuscarHistoial
        private void BtnHistoBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string filtro = TxtHistoCodigo.Text.Trim();

                if (!Validaciones.ValidarNoVacio(TxtHistoCodigo, "Paciente a buscar"))
                    return;

                DataTable dt = pacientesController.BuscarPacientePorNombrePaciente(filtro);


                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show($"No se encontro el paciente '{filtro}'.",
                        "Sin resultados",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DgvHistorial.DataSource = null;

                    return;
                }

                DgvHistorial.DataSource = dt;


                MessageBox.Show($"Se encontraron {dt.Rows.Count} pacientes(s).",
                    "Busqueda exitosa",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la busqueda: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //string Codigo = fila.Cells["CodigoPacientes"].Value?.ToString() ?? "";

        //DataTable dt = expedienteController.ObtenerHistorialporCodigo(Codigo);

        //ConfigurarDataGridView8();
        //Evento para cambiar las views al hacer clic
        private void DgvHistorial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) 
                {
                    DataGridView dgv = sender as DataGridView;
                    DataGridViewRow fila = dgv.Rows[e.RowIndex];

                    string Codigo = fila.Cells["CodigoPacientes"].Value?.ToString() ?? "";

                    DataTable dt = expedienteController.ObtenerHistorialporCodigo(Codigo);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        MessageBox.Show($"No se encontró el paciente '{Codigo}'.", "Sin resultados",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DgvHistoVerdad.DataSource = null;
                        return;
                    }

                    DgvHistoVerdad.DataSource = dt;
                }




            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el historial del pacientes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void CargarExpedientes()
        {
            try
            {
                //dtPacientes = pacientesController.ObtenerTodosPacientes();
                dtExpediente = expedienteController.ObtenerTodosExpediente();

                if (dtExpediente != null && dtExpediente.Rows.Count > 0)
                {

                    DgvHistoVerdad.DataSource = dtExpediente;
                    ConfigurarDataGridView8();
                }
                else
                {
                    DgvHistorial.DataSource = null;
                    MessageBox.Show("No se encontraron expedientes registrados.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar expedientes:  {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //Configuracion DataGridView
        private void ConfigurarDataGridView8()
        {
            try
            {
                DgvHistoVerdad.SuspendLayout();

                DgvHistoVerdad.AutoGenerateColumns = false;
                DgvHistoVerdad.Columns.Clear();

                DgvHistoVerdad.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Nombre",
                    HeaderText = "Nombre",
                    DataPropertyName = "Nombre",
                    ReadOnly = true
                });

                DgvHistoVerdad.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Edad",
                    HeaderText = "Edad",
                    DataPropertyName = "Edad",
                    ReadOnly = true,

                });

                DgvHistoVerdad.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "FechaDeCreacionExpe",
                    HeaderText = "FechaDeCreacionExpe",
                    DataPropertyName = "Fecha",
                    ReadOnly = true,

                });

                DgvHistoVerdad.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "MotivoConsulta",
                    HeaderText = "MotivoConsulta",
                    DataPropertyName = "Motivo",
                    ReadOnly = true,

                });

                DgvHistoVerdad.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Padece alguna enfermedad diagnosticada",
                    HeaderText = "Padece alguna enfermedad diagnosticada",
                    DataPropertyName = "Padece",
                    ReadOnly = true,

                });

                DgvHistoVerdad.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Padecido alguna enfermedad importante",
                    HeaderText = "Padece alguna enfermedad importante",
                    DataPropertyName = "Padecido",
                    ReadOnly = true,

                });

                DgvHistoVerdad.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Medicamento recetado",
                    HeaderText = "Medicamento recetado",
                    DataPropertyName = "Medicamento",
                    ReadOnly = true,

                });

                DgvHistoVerdad.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Enfermedad actual",
                    HeaderText = "Enfermedad actual",
                    DataPropertyName = "Enfermedad",
                    ReadOnly = true,

                });

                //Columna ids no visible
                DgvHistoVerdad.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Id",
                    HeaderText = "Id",
                    DataPropertyName = "Id",
                    ReadOnly = true,
                    Visible = false
                });


                DgvHistoVerdad.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Codigo",
                    HeaderText = "Codigo",
                    DataPropertyName = "Codigo",
                    ReadOnly = true,
                   
                });




                //Propiedades visuales del DataView
                DgvHistoVerdad.ReadOnly = true;
                DgvHistoVerdad.AllowUserToAddRows = false;
                DgvHistoVerdad.AllowUserToDeleteRows = false;

                DgvHistoVerdad.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DgvHistoVerdad.MultiSelect = false;

                DgvHistoVerdad.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                DgvHistoVerdad.BackgroundColor = Color.White;
                DgvHistoVerdad.BorderStyle = BorderStyle.None;
                DgvHistoVerdad.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
                DgvHistoVerdad.RowHeadersVisible = false;
                DgvHistoVerdad.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                DgvHistoVerdad.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                DgvHistoVerdad.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                DgvHistoVerdad.EnableHeadersVisualStyles = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error  al configurar DataGridView7: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DgvHistoVerdad.ResumeLayout();
            }

        }

        //Boon ver todos
        private void BtnHistoVerTodos_Click(object sender, EventArgs e)
        {
            try
            {
                TxtHistoCodigo.Clear();

                CargarPacientes();
                CargarExpedientes();
                


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar pacientes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void materialButton5_Click(object sender, EventArgs e)
        {
            
        }



        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void PanelMedio_Paint(object sender, PaintEventArgs e)
        {

        }
        private void pictureBox13_Click(object sender, EventArgs e)
        {

        }
        private void materialTextBox7_TextChanged(object sender, EventArgs e)
        {

        }
        private void Expe_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        
    }
}
