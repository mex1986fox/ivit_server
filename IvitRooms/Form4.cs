using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IvitRooms
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            //сформировать список
            int listI = 0;
            foreach (Form1.Room room in Form1._form._newRoot.rooms)
            {
                listBox1.Items.Insert(listI, room.name);
                listI++;
            };
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //Проверить заполнение эементов формы
            if (textBox5.Text == ""
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
                foreach (Form1.Cooler cooler in Form1._form._newRoot.coolers)
                {
                    if (id <= cooler.id)
                    {
                        id = cooler.id + 1;
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
                Form1.Cooler newCool = new Form1.Cooler();
                newCool.id = id;
                newCool.status = "";
                newCool.roomID = roomId;
                newCool.name = textBox5.Text;
                Form1._form._newRoot.coolers.Add(newCool);
                Form1._form.saveJConfig();
                this.Hide();
            }
        }
    }
}
