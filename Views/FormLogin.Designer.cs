
namespace SistemadeClinica.Views
{
    partial class FormLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BtnIngresar = new MaterialSkin.Controls.MaterialButton();
            this.BtnSalir = new MaterialSkin.Controls.MaterialButton();
            this.TxtContrasena = new MaterialSkin.Controls.MaterialTextBox();
            this.TxtUsuario = new MaterialSkin.Controls.MaterialTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox1.Image = global::SistemadeClinica.Properties.Resources.Doc;
            this.pictureBox1.Location = new System.Drawing.Point(2, 63);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(538, 411);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // BtnIngresar
            // 
            this.BtnIngresar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnIngresar.Depth = 0;
            this.BtnIngresar.DrawShadows = true;
            this.BtnIngresar.HighEmphasis = true;
            this.BtnIngresar.Icon = null;
            this.BtnIngresar.Location = new System.Drawing.Point(73, 303);
            this.BtnIngresar.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.BtnIngresar.MouseState = MaterialSkin.MouseState.HOVER;
            this.BtnIngresar.Name = "BtnIngresar";
            this.BtnIngresar.Size = new System.Drawing.Size(141, 36);
            this.BtnIngresar.TabIndex = 5;
            this.BtnIngresar.Text = "              Ingresar               ";
            this.BtnIngresar.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.BtnIngresar.UseAccentColor = false;
            this.BtnIngresar.UseVisualStyleBackColor = true;
            this.BtnIngresar.Click += new System.EventHandler(this.BtnIngresar_Click);
            // 
            // BtnSalir
            // 
            this.BtnSalir.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnSalir.Depth = 0;
            this.BtnSalir.DrawShadows = true;
            this.BtnSalir.HighEmphasis = true;
            this.BtnSalir.Icon = null;
            this.BtnSalir.Location = new System.Drawing.Point(73, 366);
            this.BtnSalir.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.BtnSalir.MouseState = MaterialSkin.MouseState.HOVER;
            this.BtnSalir.Name = "BtnSalir";
            this.BtnSalir.Size = new System.Drawing.Size(143, 36);
            this.BtnSalir.TabIndex = 6;
            this.BtnSalir.Text = "                       Salir                         ";
            this.BtnSalir.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.BtnSalir.UseAccentColor = false;
            this.BtnSalir.UseVisualStyleBackColor = true;
            this.BtnSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            // 
            // TxtContrasena
            // 
            this.TxtContrasena.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtContrasena.Depth = 0;
            this.TxtContrasena.Font = new System.Drawing.Font("Roboto", 12F);
            this.TxtContrasena.Hint = "Contraseña";
            this.TxtContrasena.Location = new System.Drawing.Point(42, 201);
            this.TxtContrasena.MaxLength = 50;
            this.TxtContrasena.MouseState = MaterialSkin.MouseState.OUT;
            this.TxtContrasena.Multiline = false;
            this.TxtContrasena.Name = "TxtContrasena";
            this.TxtContrasena.Size = new System.Drawing.Size(218, 50);
            this.TxtContrasena.TabIndex = 8;
            this.TxtContrasena.Text = "";
            this.TxtContrasena.Enter += new System.EventHandler(this.TxtContrasena_Enter);
            this.TxtContrasena.Leave += new System.EventHandler(this.TxtContrasena_Leave);
            // 
            // TxtUsuario
            // 
            this.TxtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtUsuario.Depth = 0;
            this.TxtUsuario.Font = new System.Drawing.Font("Roboto", 12F);
            this.TxtUsuario.Hint = "Usuario";
            this.TxtUsuario.Location = new System.Drawing.Point(42, 128);
            this.TxtUsuario.MaxLength = 50;
            this.TxtUsuario.MouseState = MaterialSkin.MouseState.OUT;
            this.TxtUsuario.Multiline = false;
            this.TxtUsuario.Name = "TxtUsuario";
            this.TxtUsuario.Size = new System.Drawing.Size(218, 50);
            this.TxtUsuario.TabIndex = 7;
            this.TxtUsuario.Text = "";
            this.TxtUsuario.TextChanged += new System.EventHandler(this.TxtUsuario_TextChanged);
            this.TxtUsuario.Enter += new System.EventHandler(this.TxtUsuario_Enter);
            this.TxtUsuario.Leave += new System.EventHandler(this.TxtUsuario_Leave);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 477);
            this.Controls.Add(this.TxtContrasena);
            this.Controls.Add(this.TxtUsuario);
            this.Controls.Add(this.BtnSalir);
            this.Controls.Add(this.BtnIngresar);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inicio de Sesión";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private MaterialSkin.Controls.MaterialButton BtnIngresar;
        private MaterialSkin.Controls.MaterialButton BtnSalir;
        private MaterialSkin.Controls.MaterialTextBox TxtContrasena;
        private MaterialSkin.Controls.MaterialTextBox TxtUsuario;
    }
}