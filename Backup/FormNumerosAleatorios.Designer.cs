namespace SIM
{
    partial class FormNumerosAleatorios
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNumerosAleatorios));
            this.button4 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(14, 12);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(87, 27);
            this.button4.TabIndex = 72;
            this.button4.Text = "Atras";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(615, 15);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(218, 26);
            this.label13.TabIndex = 71;
            this.label13.Text = "SIMULADOR MONTE CARLO PARA RAMS\r\nIng. Blas J. Galván, Phd (www.bgalvan.com)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(13, 56);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(797, 48);
            this.label12.TabIndex = 70;
            this.label12.Text = resources.GetString("label12.Text");
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(287, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(183, 16);
            this.label11.TabIndex = 69;
            this.label11.Text = "NÚMEROS ALEATORIOS";
            // 
            // chart1
            // 
            this.chart1.Location = new System.Drawing.Point(437, 124);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(397, 278);
            this.chart1.TabIndex = 68;
            this.chart1.Text = "chart1";
            // 
            // textBox10
            // 
            this.textBox10.Enabled = false;
            this.textBox10.Location = new System.Drawing.Point(256, 253);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(150, 20);
            this.textBox10.TabIndex = 66;
            this.textBox10.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumerosSinComa);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(13, 260);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(242, 13);
            this.label17.TabIndex = 65;
            this.label17.Text = "Cantidad de Números Aleatorios a generar...........:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 226);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 53;
            this.label5.Text = "Maximo Admisible ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 52;
            this.label4.Text = "Mínimo Admisible ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 51;
            this.label3.Text = "Parámetro 2 ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 50;
            this.label2.Text = "Parámetro 1 ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(110, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 13);
            this.label1.TabIndex = 49;
            this.label1.Text = "DISTRIBUCION DE PROBABILIDAD";
            // 
            // textBox5
            // 
            this.textBox5.Enabled = false;
            this.textBox5.Location = new System.Drawing.Point(14, 343);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox5.Size = new System.Drawing.Size(392, 59);
            this.textBox5.TabIndex = 48;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 310);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(393, 25);
            this.progressBar1.TabIndex = 47;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(336, 279);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(71, 25);
            this.button3.TabIndex = 46;
            this.button3.Text = "Ejemplo";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(256, 279);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(76, 25);
            this.button2.TabIndex = 45;
            this.button2.Text = "Reset";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 279);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(235, 25);
            this.button1.TabIndex = 44;
            this.button1.Text = "Generar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Ninguna Ley",
            "Exponencial",
            "Weibull",
            "Normal",
            "Uniforme"});
            this.comboBox1.Location = new System.Drawing.Point(14, 124);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(393, 21);
            this.comboBox1.TabIndex = 43;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(256, 226);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(150, 20);
            this.textBox4.TabIndex = 42;
            this.textBox4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumerosConComa);
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(256, 202);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(150, 20);
            this.textBox3.TabIndex = 41;
            this.textBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumerosConComa);
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(256, 176);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(150, 20);
            this.textBox2.TabIndex = 40;
            this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumerosConComa);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(256, 151);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(150, 20);
            this.textBox1.TabIndex = 39;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumerosConComa);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(199, 408);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(463, 65);
            this.pictureBox1.TabIndex = 73;
            this.pictureBox1.TabStop = false;
            // 
            // FormNumerosAleatorios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 475);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "FormNumerosAleatorios";
            this.Text = "Numeros Aleatorios";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}