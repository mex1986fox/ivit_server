using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace IvitRooms
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            //сформировать список
            int listI = 0;
            foreach (Form1.Room room in Form1._form._newRoot.rooms)
            {
                listBox1.Items.Insert(listI, room.name);
                listI++;
            };
            maskedTextBox1.Text = "010.020.002.";
            maskedTextBox2.Text = "502";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Проверить заполнение эементов формы
            if (maskedTextBox1.Text == ""
                || maskedTextBox2.Text == ""
                || maskedTextBox3.Text == ""
                || textBox5.Text == ""
                || listBox1.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Заполните все элементы формы",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                int id = 1;
                // выкрутить id для формы
                foreach (Form1.Pas pas in Form1._form._newRoot.pases)
                {
                    if (id <= pas.id)
                    {
                        id = pas.id + 1;
                    }
                }
                int roomId = 1;
                foreach (Form1.Room room in Form1._form._newRoot.rooms)
                {
                    if (listBox1.Items[listBox1.SelectedIndex].ToString() == room.name)
                    {
                        roomId = room.id;
                    }
                }
                // заполнить объект и создать
                Form1.Pas newPas = new Form1.Pas();
                newPas.id = id;
                newPas.status = "";
                newPas.roomID = roomId;
                newPas.ip = Regex.Replace(maskedTextBox1.Text.Replace(",", "."), "0*([0-9]+)", "${1}");
                newPas.port = Convert.ToInt32(maskedTextBox2.Text);
                newPas.sirialID = Convert.ToInt32(maskedTextBox3.Text);
                newPas.name = textBox5.Text;
                newPas.pressure = "0";
                Form1._form._newRoot.pases.Add(newPas);
                Form1._form.saveJConfig();
                this.Hide();
            }
        }
    }
}
