
namespace AplicacionVentaLINQ
{
    partial class MateriaPrima
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MateriaPrima));
            this.cmbProductos = new System.Windows.Forms.ComboBox();
            this.clbMateriasPrimas = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // cmbProductos
            // 
            this.cmbProductos.FormattingEnabled = true;
            this.cmbProductos.Location = new System.Drawing.Point(12, 28);
            this.cmbProductos.Name = "cmbProductos";
            this.cmbProductos.Size = new System.Drawing.Size(179, 21);
            this.cmbProductos.TabIndex = 0;
            // 
            // clbMateriasPrimas
            // 
            this.clbMateriasPrimas.FormattingEnabled = true;
            this.clbMateriasPrimas.Location = new System.Drawing.Point(226, 28);
            this.clbMateriasPrimas.Name = "clbMateriasPrimas";
            this.clbMateriasPrimas.Size = new System.Drawing.Size(179, 154);
            this.clbMateriasPrimas.TabIndex = 1;
            // 
            // MateriaPrima
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(417, 303);
            this.Controls.Add(this.clbMateriasPrimas);
            this.Controls.Add(this.cmbProductos);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MateriaPrima";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestionar Materias Prima";
            this.Load += new System.EventHandler(this.MateriaPrima_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbProductos;
        private System.Windows.Forms.CheckedListBox clbMateriasPrimas;
    }
}