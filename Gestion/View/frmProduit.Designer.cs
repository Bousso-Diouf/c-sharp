namespace Gestion.View
{
    partial class frmProduit
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
            this.dgProduit = new System.Windows.Forms.DataGridView();
            this.lblId = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtDesignation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtQuantiteMinimale = new System.Windows.Forms.TextBox();
            this.lblQuantiteMinimale = new System.Windows.Forms.Label();
            this.txtPU = new System.Windows.Forms.TextBox();
            this.lblPU = new System.Windows.Forms.Label();
            this.btnAjouter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgProduit)).BeginInit();
            this.SuspendLayout();
            // 
            // dgProduit
            // 
            this.dgProduit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProduit.Location = new System.Drawing.Point(458, 37);
            this.dgProduit.Name = "dgProduit";
            this.dgProduit.RowHeadersWidth = 62;
            this.dgProduit.RowTemplate.Height = 28;
            this.dgProduit.Size = new System.Drawing.Size(669, 618);
            this.dgProduit.TabIndex = 0;
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(26, 46);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(23, 20);
            this.lblId.TabIndex = 1;
            this.lblId.Text = "Id";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(30, 91);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(271, 26);
            this.txtId.TabIndex = 2;
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(30, 205);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(271, 26);
            this.txtCode.TabIndex = 4;
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(26, 150);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(47, 20);
            this.lblCode.TabIndex = 3;
            this.lblCode.Text = "Code";
            // 
            // txtDesignation
            // 
            this.txtDesignation.Location = new System.Drawing.Point(30, 280);
            this.txtDesignation.Name = "txtDesignation";
            this.txtDesignation.Size = new System.Drawing.Size(271, 26);
            this.txtDesignation.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 245);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Designation";
            // 
            // txtQuantiteMinimale
            // 
            this.txtQuantiteMinimale.Location = new System.Drawing.Point(30, 369);
            this.txtQuantiteMinimale.Name = "txtQuantiteMinimale";
            this.txtQuantiteMinimale.Size = new System.Drawing.Size(271, 26);
            this.txtQuantiteMinimale.TabIndex = 8;
            // 
            // lblQuantiteMinimale
            // 
            this.lblQuantiteMinimale.AutoSize = true;
            this.lblQuantiteMinimale.Location = new System.Drawing.Point(26, 324);
            this.lblQuantiteMinimale.Name = "lblQuantiteMinimale";
            this.lblQuantiteMinimale.Size = new System.Drawing.Size(136, 20);
            this.lblQuantiteMinimale.TabIndex = 7;
            this.lblQuantiteMinimale.Text = "Quantité minimale";
            // 
            // txtPU
            // 
            this.txtPU.Location = new System.Drawing.Point(30, 483);
            this.txtPU.Name = "txtPU";
            this.txtPU.Size = new System.Drawing.Size(271, 26);
            this.txtPU.TabIndex = 10;
            // 
            // lblPU
            // 
            this.lblPU.AutoSize = true;
            this.lblPU.Location = new System.Drawing.Point(26, 426);
            this.lblPU.Name = "lblPU";
            this.lblPU.Size = new System.Drawing.Size(31, 20);
            this.lblPU.TabIndex = 9;
            this.lblPU.Text = "PU";
            // 
            // btnAjouter
            // 
            this.btnAjouter.Location = new System.Drawing.Point(214, 539);
            this.btnAjouter.Name = "btnAjouter";
            this.btnAjouter.Size = new System.Drawing.Size(139, 66);
            this.btnAjouter.TabIndex = 11;
            this.btnAjouter.Text = "Ajouter";
            this.btnAjouter.UseVisualStyleBackColor = true;
            this.btnAjouter.Click += new System.EventHandler(this.btnAjouter_Click);
            // 
            // frmProduit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1156, 680);
            this.ControlBox = false;
            this.Controls.Add(this.btnAjouter);
            this.Controls.Add(this.txtPU);
            this.Controls.Add(this.lblPU);
            this.Controls.Add(this.txtQuantiteMinimale);
            this.Controls.Add(this.lblQuantiteMinimale);
            this.Controls.Add(this.txtDesignation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.dgProduit);
            this.Name = "frmProduit";
            this.Text = "Produit";
            this.Load += new System.EventHandler(this.frmProduit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgProduit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgProduit;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TextBox txtDesignation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtQuantiteMinimale;
        private System.Windows.Forms.Label lblQuantiteMinimale;
        private System.Windows.Forms.TextBox txtPU;
        private System.Windows.Forms.Label lblPU;
        private System.Windows.Forms.Button btnAjouter;
    }
}