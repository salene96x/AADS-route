
namespace AADS.Views.Route
{
    partial class rightPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel4 = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.rdbLineRouteSF = new System.Windows.Forms.RadioButton();
            this.rdbLineRouteAir = new System.Windows.Forms.RadioButton();
            this.rdbLineRouteL = new System.Windows.Forms.RadioButton();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.panel4.Controls.Add(this.listBox1);
            this.panel4.Controls.Add(this.rdbLineRouteSF);
            this.panel4.Controls.Add(this.rdbLineRouteAir);
            this.panel4.Controls.Add(this.rdbLineRouteL);
            this.panel4.Controls.Add(this.label27);
            this.panel4.Controls.Add(this.label26);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Font = new System.Drawing.Font("TH SarabunPSK", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(544, 737);
            this.panel4.TabIndex = 20;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 28;
            this.listBox1.Location = new System.Drawing.Point(23, 79);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(333, 284);
            this.listBox1.TabIndex = 56;
            // 
            // rdbLineRouteSF
            // 
            this.rdbLineRouteSF.AutoSize = true;
            this.rdbLineRouteSF.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.rdbLineRouteSF.Location = new System.Drawing.Point(280, 48);
            this.rdbLineRouteSF.Name = "rdbLineRouteSF";
            this.rdbLineRouteSF.Size = new System.Drawing.Size(76, 32);
            this.rdbLineRouteSF.TabIndex = 20;
            this.rdbLineRouteSF.Text = "Surface";
            this.rdbLineRouteSF.UseVisualStyleBackColor = true;
            this.rdbLineRouteSF.CheckedChanged += new System.EventHandler(this.rdbLineRoute_CheckedChanged);
            // 
            // rdbLineRouteAir
            // 
            this.rdbLineRouteAir.AutoSize = true;
            this.rdbLineRouteAir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.rdbLineRouteAir.Location = new System.Drawing.Point(219, 48);
            this.rdbLineRouteAir.Name = "rdbLineRouteAir";
            this.rdbLineRouteAir.Size = new System.Drawing.Size(46, 32);
            this.rdbLineRouteAir.TabIndex = 19;
            this.rdbLineRouteAir.Text = "Air";
            this.rdbLineRouteAir.UseVisualStyleBackColor = true;
            this.rdbLineRouteAir.CheckedChanged += new System.EventHandler(this.rdbLineRoute_CheckedChanged);
            // 
            // rdbLineRouteL
            // 
            this.rdbLineRouteL.AutoSize = true;
            this.rdbLineRouteL.Checked = true;
            this.rdbLineRouteL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.rdbLineRouteL.Location = new System.Drawing.Point(144, 48);
            this.rdbLineRouteL.Name = "rdbLineRouteL";
            this.rdbLineRouteL.Size = new System.Drawing.Size(60, 32);
            this.rdbLineRouteL.TabIndex = 18;
            this.rdbLineRouteL.TabStop = true;
            this.rdbLineRouteL.Text = "Land";
            this.rdbLineRouteL.UseVisualStyleBackColor = true;
            this.rdbLineRouteL.CheckedChanged += new System.EventHandler(this.rdbLineRoute_CheckedChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label27.Location = new System.Drawing.Point(43, 48);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(90, 28);
            this.label27.TabIndex = 1;
            this.label27.Text = "Route type :";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("TH SarabunPSK", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label26.Location = new System.Drawing.Point(17, 25);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(180, 36);
            this.label26.TabIndex = 0;
            this.label26.Text = "Route control panel";
            // 
            // rightPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel4);
            this.Name = "rightPanel";
            this.Size = new System.Drawing.Size(544, 737);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton rdbLineRouteAir;
        private System.Windows.Forms.RadioButton rdbLineRouteL;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.RadioButton rdbLineRouteSF;
    }
}
