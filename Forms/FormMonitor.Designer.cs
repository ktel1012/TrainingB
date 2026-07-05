namespace TrainingB.Forms
{
    partial class FormMonitor
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
            btnGo = new Button();
            button2 = new Button();
            btnWindow = new Button();
            txtURL = new TextBox();
            listBox1 = new ListBox();
            btnMN = new Button();
            btnHN = new Button();
            txt2D = new TextBox();
            txt3D = new TextBox();
            txt4D = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            btnDaoSo = new Button();
            chk4dlo = new CheckBox();
            btn4dlo = new Button();
            chk4dduoiMN = new CheckBox();
            chk4dloMN = new CheckBox();
            chk4DDuoiHN = new CheckBox();
            SuspendLayout();
            // 
            // btnGo
            // 
            btnGo.Location = new Point(745, 12);
            btnGo.Margin = new Padding(1);
            btnGo.Name = "btnGo";
            btnGo.Size = new Size(79, 23);
            btnGo.TabIndex = 0;
            btnGo.Text = "CLOSE";
            btnGo.UseVisualStyleBackColor = true;
            btnGo.Click += button1_Click;
            // 
            // button2
            // 
            button2.Enabled = false;
            button2.Location = new Point(12, 89);
            button2.Margin = new Padding(1);
            button2.Name = "button2";
            button2.Size = new Size(79, 21);
            button2.TabIndex = 1;
            button2.Text = "Get Change";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnWindow
            // 
            btnWindow.Enabled = false;
            btnWindow.Location = new Point(12, 52);
            btnWindow.Name = "btnWindow";
            btnWindow.Size = new Size(130, 23);
            btnWindow.TabIndex = 3;
            btnWindow.Text = "Switch Window/ Tab";
            btnWindow.UseVisualStyleBackColor = true;
            btnWindow.Click += btnRefresh_Click;
            // 
            // txtURL
            // 
            txtURL.Location = new Point(12, 12);
            txtURL.Name = "txtURL";
            txtURL.Size = new Size(729, 23);
            txtURL.TabIndex = 4;
            txtURL.Text = "https://b2one789.net";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 120);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(218, 199);
            listBox1.TabIndex = 5;
            // 
            // btnMN
            // 
            btnMN.Location = new Point(147, 52);
            btnMN.Name = "btnMN";
            btnMN.Size = new Size(83, 23);
            btnMN.TabIndex = 6;
            btnMN.Text = "MIEN NAM";
            btnMN.UseVisualStyleBackColor = true;
            btnMN.Click += btnMN_Click;
            // 
            // btnHN
            // 
            btnHN.Location = new Point(147, 81);
            btnHN.Name = "btnHN";
            btnHN.Size = new Size(83, 23);
            btnHN.TabIndex = 7;
            btnHN.Text = "MIEN BAC";
            btnHN.UseVisualStyleBackColor = true;
            btnHN.Click += btnHN_Click;
            // 
            // txt2D
            // 
            txt2D.Location = new Point(273, 56);
            txt2D.Multiline = true;
            txt2D.Name = "txt2D";
            txt2D.ScrollBars = ScrollBars.Both;
            txt2D.Size = new Size(549, 118);
            txt2D.TabIndex = 8;
            // 
            // txt3D
            // 
            txt3D.Location = new Point(12, 353);
            txt3D.Multiline = true;
            txt3D.Name = "txt3D";
            txt3D.ScrollBars = ScrollBars.Both;
            txt3D.Size = new Size(810, 150);
            txt3D.TabIndex = 8;
            // 
            // txt4D
            // 
            txt4D.Location = new Point(273, 180);
            txt4D.Multiline = true;
            txt4D.Name = "txt4D";
            txt4D.ScrollBars = ScrollBars.Both;
            txt4D.Size = new Size(549, 139);
            txt4D.TabIndex = 8;
            txt4D.TextChanged += txt4D_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(243, 86);
            label1.Name = "label1";
            label1.Size = new Size(24, 15);
            label1.TabIndex = 9;
            label1.Text = "2D:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 332);
            label2.Name = "label2";
            label2.Size = new Size(24, 15);
            label2.TabIndex = 9;
            label2.Text = "3D:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(243, 202);
            label3.Name = "label3";
            label3.Size = new Size(24, 15);
            label3.TabIndex = 9;
            label3.Text = "4D:";
            // 
            // btnDaoSo
            // 
            btnDaoSo.Location = new Point(733, 324);
            btnDaoSo.Name = "btnDaoSo";
            btnDaoSo.Size = new Size(87, 23);
            btnDaoSo.TabIndex = 10;
            btnDaoSo.Text = "Dao So";
            btnDaoSo.UseVisualStyleBackColor = true;
            btnDaoSo.Click += btnDaoSo_Click;
            // 
            // chk4dlo
            // 
            chk4dlo.AutoSize = true;
            chk4dlo.Location = new Point(273, 328);
            chk4dlo.Name = "chk4dlo";
            chk4dlo.Size = new Size(77, 19);
            chk4dlo.TabIndex = 11;
            chk4dlo.Text = "4D Lo HN";
            chk4dlo.UseVisualStyleBackColor = true;
            // 
            // btn4dlo
            // 
            btn4dlo.Location = new Point(635, 324);
            btn4dlo.Name = "btn4dlo";
            btn4dlo.Size = new Size(59, 23);
            btn4dlo.TabIndex = 12;
            btn4dlo.Text = "Run";
            btn4dlo.UseVisualStyleBackColor = true;
            btn4dlo.Click += btn4dlo_Click;
            // 
            // chk4dduoiMN
            // 
            chk4dduoiMN.AutoSize = true;
            chk4dduoiMN.Location = new Point(538, 327);
            chk4dduoiMN.Name = "chk4dduoiMN";
            chk4dduoiMN.Size = new Size(91, 19);
            chk4dduoiMN.TabIndex = 14;
            chk4dduoiMN.Text = "4D Duoi MN";
            chk4dduoiMN.UseVisualStyleBackColor = true;
            // 
            // chk4dloMN
            // 
            chk4dloMN.AutoSize = true;
            chk4dloMN.Location = new Point(456, 328);
            chk4dloMN.Name = "chk4dloMN";
            chk4dloMN.Size = new Size(79, 19);
            chk4dloMN.TabIndex = 13;
            chk4dloMN.Text = "4D Lo MN";
            chk4dloMN.UseVisualStyleBackColor = true;
            // 
            // chk4DDuoiHN
            // 
            chk4DDuoiHN.AutoSize = true;
            chk4DDuoiHN.Location = new Point(353, 328);
            chk4DDuoiHN.Name = "chk4DDuoiHN";
            chk4DDuoiHN.Size = new Size(89, 19);
            chk4DDuoiHN.TabIndex = 15;
            chk4DDuoiHN.Text = "4D Duoi HN";
            chk4DDuoiHN.UseVisualStyleBackColor = true;
            // 
            // FormMonitor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(834, 515);
            Controls.Add(chk4DDuoiHN);
            Controls.Add(chk4dduoiMN);
            Controls.Add(chk4dloMN);
            Controls.Add(btn4dlo);
            Controls.Add(chk4dlo);
            Controls.Add(btnDaoSo);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txt4D);
            Controls.Add(txt3D);
            Controls.Add(txt2D);
            Controls.Add(btnHN);
            Controls.Add(btnMN);
            Controls.Add(listBox1);
            Controls.Add(txtURL);
            Controls.Add(btnWindow);
            Controls.Add(button2);
            Controls.Add(btnGo);
            Name = "FormMonitor";
            Text = "Monitor";
            Load += FormMonitor_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnGo;
        private Button button2;
        private Button btnWindow;
        private TextBox txtURL;
        private ListBox listBox1;
        private Button btnMN;
        private Button btnHN;
        private TextBox txt2D;
        private TextBox txt3D;
        private TextBox txt4D;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button btnDaoSo;
        private CheckBox chk4dlo;
        private Button btn4dlo;
        private CheckBox chk4dduoiMN;
        private CheckBox chk4dloMN;
        private CheckBox chk4DDuoiHN;
    }
}