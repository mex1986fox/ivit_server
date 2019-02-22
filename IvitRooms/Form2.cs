using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.IO;

namespace IvitRooms
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                string pubPath = Environment.CurrentDirectory;
                string imgsPath = pubPath + "\\..\\..\\public\\img";
                openFileDialog.InitialDirectory = imgsPath;
                openFileDialog.Filter = "txt files (*.jpg)|*.jpg";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;
                    FileInfo test = new FileInfo(@filePath);
                    string name = test.Name;
                    label5.Text = name;

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //проверить одинаковые имена комнат
            bool flRoom = false;
            foreach (Form1.Room room in Form1._form._newRoot.rooms)
            {
                if (textBox2.Text == room.name)
                {
                    flRoom = true;
                }
            }
            //Проверить заполнение эементов формы
            if (textBox2.Text=="" || label5.Text == "")
            {
                MessageBox.Show(
                    "Заполните все элементы формы",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
            else if (flRoom == true)
            {
                MessageBox.Show(
                   "Такое название помещения уже существует. Измените название!",
                   "Сообщение",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information,
                   MessageBoxDefaultButton.Button1,
                   MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                int id=1;
                // выкрутить id для формы
                foreach (Form1.Room room in Form1._form._newRoot.rooms)
                {
                    if (id <= room.id)
                    {
                        id = room.id + 1;
                    }
                }
                // заполнить объект и создать
                Form1.Room newRoom = new Form1.Room();
                newRoom.name = textBox2.Text;
                newRoom.id = id;
                newRoom.fon = label5.Text;
                Form1._form._newRoot.rooms.Add(newRoom);
                Form1._form.saveJConfig();
                this.Hide();
            }
        
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
