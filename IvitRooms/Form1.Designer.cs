namespace IvitRooms
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.button3 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.менюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьПомещениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьЭвитToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьКуллерToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавтьПАСToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.грыфикиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.температура2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.влажностьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.перепадToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.таблициToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ивитыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пасыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button1.Location = new System.Drawing.Point(88, 56);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Red;
            this.button2.Location = new System.Drawing.Point(7, 56);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(345, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Location = new System.Drawing.Point(7, 85);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1286, 467);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 3;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(169, 56);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(186, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Сохранить конфигурацию";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.менюToolStripMenuItem,
            this.грыфикиToolStripMenuItem,
            this.таблициToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1300, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // менюToolStripMenuItem
            // 
            this.менюToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьПомещениеToolStripMenuItem,
            this.добавитьЭвитToolStripMenuItem,
            this.добавитьКуллерToolStripMenuItem,
            this.добавтьПАСToolStripMenuItem});
            this.менюToolStripMenuItem.Name = "менюToolStripMenuItem";
            this.менюToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.менюToolStripMenuItem.Text = "Меню";
            // 
            // добавитьПомещениеToolStripMenuItem
            // 
            this.добавитьПомещениеToolStripMenuItem.Name = "добавитьПомещениеToolStripMenuItem";
            this.добавитьПомещениеToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.добавитьПомещениеToolStripMenuItem.Text = "Добавить помещение";
            this.добавитьПомещениеToolStripMenuItem.Click += new System.EventHandler(this.добавитьПомещениеToolStripMenuItem_Click);
            // 
            // добавитьЭвитToolStripMenuItem
            // 
            this.добавитьЭвитToolStripMenuItem.Name = "добавитьЭвитToolStripMenuItem";
            this.добавитьЭвитToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.добавитьЭвитToolStripMenuItem.Text = "Добавить ИВИТ";
            this.добавитьЭвитToolStripMenuItem.Click += new System.EventHandler(this.добавитьЭвитToolStripMenuItem_Click);
            // 
            // добавитьКуллерToolStripMenuItem
            // 
            this.добавитьКуллерToolStripMenuItem.Name = "добавитьКуллерToolStripMenuItem";
            this.добавитьКуллерToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.добавитьКуллерToolStripMenuItem.Text = "Добавить куллер";
            this.добавитьКуллерToolStripMenuItem.Click += new System.EventHandler(this.добавитьКуллерToolStripMenuItem_Click);
            // 
            // добавтьПАСToolStripMenuItem
            // 
            this.добавтьПАСToolStripMenuItem.Name = "добавтьПАСToolStripMenuItem";
            this.добавтьПАСToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.добавтьПАСToolStripMenuItem.Text = "Добавть ПАС";
            this.добавтьПАСToolStripMenuItem.Click += new System.EventHandler(this.добавтьПАСToolStripMenuItem_Click);
            // 
            // грыфикиToolStripMenuItem
            // 
            this.грыфикиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.температура2ToolStripMenuItem,
            this.влажностьToolStripMenuItem,
            this.перепадToolStripMenuItem});
            this.грыфикиToolStripMenuItem.Name = "грыфикиToolStripMenuItem";
            this.грыфикиToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.грыфикиToolStripMenuItem.Text = "Графики";
            // 
            // температура2ToolStripMenuItem
            // 
            this.температура2ToolStripMenuItem.Name = "температура2ToolStripMenuItem";
            this.температура2ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.температура2ToolStripMenuItem.Text = "Температура";
            this.температура2ToolStripMenuItem.Click += new System.EventHandler(this.температура2ToolStripMenuItem_Click);
            // 
            // влажностьToolStripMenuItem
            // 
            this.влажностьToolStripMenuItem.Name = "влажностьToolStripMenuItem";
            this.влажностьToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.влажностьToolStripMenuItem.Text = "Влажность";
            this.влажностьToolStripMenuItem.Click += new System.EventHandler(this.влажностьToolStripMenuItem_Click);
            // 
            // перепадToolStripMenuItem
            // 
            this.перепадToolStripMenuItem.Name = "перепадToolStripMenuItem";
            this.перепадToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.перепадToolStripMenuItem.Text = "Перепад";
            this.перепадToolStripMenuItem.Click += new System.EventHandler(this.перепадToolStripMenuItem_Click);
            // 
            // таблициToolStripMenuItem
            // 
            this.таблициToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ивитыToolStripMenuItem,
            this.пасыToolStripMenuItem});
            this.таблициToolStripMenuItem.Name = "таблициToolStripMenuItem";
            this.таблициToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.таблициToolStripMenuItem.Text = "Таблици";
            // 
            // ивитыToolStripMenuItem
            // 
            this.ивитыToolStripMenuItem.Name = "ивитыToolStripMenuItem";
            this.ивитыToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.ивитыToolStripMenuItem.Text = "Ивиты";
            this.ивитыToolStripMenuItem.Click += new System.EventHandler(this.ивитыToolStripMenuItem_Click);
            // 
            // пасыToolStripMenuItem
            // 
            this.пасыToolStripMenuItem.Name = "пасыToolStripMenuItem";
            this.пасыToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.пасыToolStripMenuItem.Text = "Пасы";
            this.пасыToolStripMenuItem.Click += new System.EventHandler(this.пасыToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 556);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "ИВИТ монитор";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem менюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьПомещениеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьЭвитToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьКуллерToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавтьПАСToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem грыфикиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem температура2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem таблициToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ивитыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пасыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem влажностьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem перепадToolStripMenuItem;
    }
}

