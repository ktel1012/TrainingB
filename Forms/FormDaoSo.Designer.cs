namespace TrainingB.Forms
{
    partial class FormDaoSo
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
            btn22 = new Button();
            button4 = new Button();
            btn34 = new Button();
            btn33 = new Button();
            btn35 = new Button();
            btn44 = new Button();
            txtSoDao = new TextBox();
            txtKQ = new TextBox();
            lblStatus = new Label();
            btn23 = new Button();
            btn24 = new Button();
            SuspendLayout();
            // 
            // btn22
            // 
            btn22.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn22.Location = new Point(23, 26);
            btn22.Name = "btn22";
            btn22.Size = new Size(96, 30);
            btn22.TabIndex = 0;
            btn22.Text = "Dao 2/2";
            btn22.UseVisualStyleBackColor = true;
            btn22.Click += btn22_Click;
            // 
            // button4
            // 
            button4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button4.Location = new Point(125, 26);
            button4.Name = "button4";
            button4.Size = new Size(96, 30);
            button4.TabIndex = 0;
            button4.Text = "button1";
            button4.UseVisualStyleBackColor = true;
            // 
            // btn34
            // 
            btn34.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn34.Location = new Point(125, 60);
            btn34.Name = "btn34";
            btn34.Size = new Size(96, 30);
            btn34.TabIndex = 0;
            btn34.Text = "Dao 3/4";
            btn34.UseVisualStyleBackColor = true;
            btn34.Click += btn34_Click;
            // 
            // btn33
            // 
            btn33.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn33.Location = new Point(125, 26);
            btn33.Name = "btn33";
            btn33.Size = new Size(96, 30);
            btn33.TabIndex = 0;
            btn33.Text = "Dao 3/3";
            btn33.UseVisualStyleBackColor = true;
            btn33.Click += btn33_Click;
            // 
            // btn35
            // 
            btn35.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn35.Location = new Point(125, 93);
            btn35.Name = "btn35";
            btn35.Size = new Size(96, 30);
            btn35.TabIndex = 0;
            btn35.Text = "Dao 3/5";
            btn35.UseVisualStyleBackColor = true;
            btn35.Click += btn35_Click;
            // 
            // btn44
            // 
            btn44.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn44.Location = new Point(227, 26);
            btn44.Name = "btn44";
            btn44.Size = new Size(96, 30);
            btn44.TabIndex = 0;
            btn44.Text = "Dao 4/4";
            btn44.UseVisualStyleBackColor = true;
            btn44.Click += btn44_Click;
            // 
            // txtSoDao
            // 
            txtSoDao.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            txtSoDao.ForeColor = SystemColors.MenuHighlight;
            txtSoDao.Location = new Point(227, 91);
            txtSoDao.Name = "txtSoDao";
            txtSoDao.Size = new Size(96, 29);
            txtSoDao.TabIndex = 1;
            txtSoDao.Text = "12345";
            // 
            // txtKQ
            // 
            txtKQ.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            txtKQ.ForeColor = Color.Red;
            txtKQ.Location = new Point(23, 129);
            txtKQ.Multiline = true;
            txtKQ.Name = "txtKQ";
            txtKQ.ScrollBars = ScrollBars.Both;
            txtKQ.Size = new Size(507, 214);
            txtKQ.TabIndex = 3;
            txtKQ.Text = "12345";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.ForeColor = SystemColors.MenuHighlight;
            lblStatus.Location = new Point(338, 105);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(50, 15);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "? so dao";
            // 
            // btn23
            // 
            btn23.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn23.Location = new Point(23, 60);
            btn23.Name = "btn23";
            btn23.Size = new Size(96, 30);
            btn23.TabIndex = 5;
            btn23.Text = "Dao 2/3";
            btn23.UseVisualStyleBackColor = true;
            btn23.Click += btn23_Click;
            // 
            // btn24
            // 
            btn24.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn24.Location = new Point(23, 93);
            btn24.Name = "btn24";
            btn24.Size = new Size(96, 29);
            btn24.TabIndex = 6;
            btn24.Text = "Dao 2/4";
            btn24.UseVisualStyleBackColor = true;
            btn24.Click += btn24_Click;
            // 
            // FormDaoSo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(549, 359);
            Controls.Add(btn24);
            Controls.Add(btn23);
            Controls.Add(lblStatus);
            Controls.Add(txtKQ);
            Controls.Add(txtSoDao);
            Controls.Add(btn35);
            Controls.Add(btn44);
            Controls.Add(btn33);
            Controls.Add(btn34);
            Controls.Add(button4);
            Controls.Add(btn22);
            MaximizeBox = false;
            Name = "FormDaoSo";
            Text = "FromDaoSo";
            Load += FromDaoSo_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn22;
        private Button button4;
        private Button btn34;
        private Button btn33;
        private Button btn35;
        private Button btn44;
        private TextBox txtSoDao;
        private TextBox txtKQ;
        private Label lblStatus;
        private Button btn23;
        private Button btn24;
    }
}